using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using raspiDisplay.Helpers;
using System.Security.Cryptography;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;
using Windows.Devices.Enumeration;
using System.IO.Ports;
using System.Threading;

namespace raspiDisplay
{
    // 사물함 번호 선택하는 화면
    public sealed partial class numChoice : Page
    {
        private Button selectedButton = null;
        private string phoneNumber;
        private string selectedButtonNumber;
        private FirestoreHelper firestoreHelper;
        private static Random random = new Random();

        // 시리얼 통신 전역변수
        private SerialDevice serialPort1;
        private SerialDevice serialPort2;
        private SerialDevice serialPort3;
        private SerialDevice serialPort4;

        private DataWriter dataWriter1;
        private DataWriter dataWriter2;
        private DataWriter dataWriter3;
        private DataWriter dataWriter4;

        private DataReader dataReader1;
        private DataReader dataReader2;
        private DataReader dataReader3;
        private DataReader dataReader4;

        public numChoice()
        {
            this.InitializeComponent();
            firestoreHelper = new FirestoreHelper();
            InitializeSerialPort();
            LoadButtonStates();
        }

        //public static string GenerateRandomNumber()
        //{
        //    int randomNumber = random.Next(100000, 999999);
        //    return randomNumber.ToString();
        //}

        private async void ShowMessage(string title, string content)
        {
            var dialog = new MessageDialog(content, title);
            dialog.Commands.Add(new UICommand("OK"));
            await dialog.ShowAsync();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            phoneNumber = e.Parameter as string;
        }

        private void backBtn3_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(sender));
        }

        private void homeBtn3_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        //async Task<bool> FindBoxNum(string selectedButtonNumber)
        //{
        //    Query qref = db.Collection("Box").WhereEqualTo("BoxNum", selectedButtonNumber);
        //    QuerySnapshot snap = await qref.GetSnapshotAsync();

        //    foreach (DocumentSnapshot docsnap in snap)
        //    {
        //        if (docsnap.Exists)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        private async Task LoadButtonStates()
        {
            for (int i = 1; i <= 4; i++)
            {
                string buttonNumber = i.ToString();
                bool isFilled = await firestoreHelper.IsBoxFilledAsync(buttonNumber);

                Button button = FindName($"box{buttonNumber}") as Button;
                if (button != null)
                {
                    if (isFilled)
                    {
                        // 선택한 박스는 회색
                        button.IsEnabled = false;
                        button.Background = new SolidColorBrush(Windows.UI.Colors.Gray);
                    }
                    else
                    {
                        // 선택하지 않은 박스는 파란색
                        button.Background = new SolidColorBrush(Windows.UI.Colors.Blue);
                        button.Click += Button_Click; // 클릭이 가능하게 활성화
                    }
                }
            }
        }

        // 사물함 번호 선택 버튼 함수
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            var textBlock = clickedButton.Content as TextBlock;
            if (textBlock != null)
            {
                // 선택된 박스 번호 저장, 파이어베이스 입력하기위함
                selectedButtonNumber = textBlock.Text;
            }

            // 클릭한 버튼 비활성화
            clickedButton.IsEnabled = false;

            if (selectedButton != null && selectedButton != clickedButton)
            {
                selectedButton.Background = new SolidColorBrush(Windows.UI.Colors.Blue);
                selectedButton.IsEnabled = true;
            }

            selectedButton = clickedButton; ;
        }

        // 파이어베이스에 입력하는 함수
        private async void SaveDataToFirestore()
        {
            //string RandNum = GenerateRandomNumber();
            await firestoreHelper.SaveDataAsync(phoneNumber, selectedButtonNumber);
        }

        

        #region "시리얼 통신"
        private async void InitializeSerialPort()
        {
            try
            {
                string serialDeviceSelector = SerialDevice.GetDeviceSelector();
                var devices = await DeviceInformation.FindAllAsync(serialDeviceSelector);

                if (devices.Count > 0)
                {
                    serialPort1 = await SerialDevice.FromIdAsync(devices[0].Id);
                    serialPort2 = await SerialDevice.FromIdAsync(devices[1].Id);
                    serialPort3 = await SerialDevice.FromIdAsync(devices[2].Id);
                    serialPort4 = await SerialDevice.FromIdAsync(devices[3].Id);

                    if (serialPort1 != null)
                    {
                        ConfigureSerialPort(serialPort1);
                        dataWriter1 = new DataWriter(serialPort1.OutputStream);
                        dataReader1 = new DataReader(serialPort1.InputStream);
                    }

                    if (serialPort2 != null)
                    {
                        ConfigureSerialPort(serialPort2);
                        dataWriter2 = new DataWriter(serialPort2.OutputStream);
                        dataReader2 = new DataReader(serialPort2.InputStream);
                    }

                    if (serialPort3 != null)
                    {
                        ConfigureSerialPort(serialPort3);
                        dataWriter3 = new DataWriter(serialPort3.OutputStream);
                        dataReader3 = new DataReader(serialPort3.InputStream);
                    }

                    if (serialPort4 != null)
                    {
                        ConfigureSerialPort(serialPort4);
                        dataWriter4 = new DataWriter(serialPort4.OutputStream);
                        dataReader4 = new DataReader(serialPort4.InputStream);
                    }
                    string title = "SUCCESS";
                    string text = "Serial port initialized successfully.";
                    firestoreHelper.ShowMessage(title, text);
                }
                else
                {
                    string title = "ERROR";
                    string text = "No serial devices found.";
                    firestoreHelper.ShowMessage(title, text);
                }
            }
            catch (Exception ex)
            {
                string title = "ERROR";
                string text = "Error initializing serial port: " + ex.Message;
                firestoreHelper.ShowMessage(title, text);
            }
        }

        // 시리얼 포트 매칭
        private void ConfigureSerialPort(SerialDevice serialPort)
        {
            serialPort.BaudRate = 9600;
            serialPort.DataBits = 8;
            serialPort.Parity = SerialParity.None;
            serialPort.StopBits = SerialStopBitCount.One;
            serialPort.Handshake = SerialHandshake.None;
        }

        // 사물함 선택완료 버튼
        private async void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedButtonNumber))
            {
                //await SendSerialData1("1");
                if (selectedButtonNumber == "1")
                    await SendSerialData1("2");
                else if (selectedButtonNumber == "2")
                    await SendSerialData2("2");
                else if (selectedButtonNumber == "3")
                    await SendSerialData3("2");
                else if (selectedButtonNumber == "4")
                    await SendSerialData4("2");
                string title = "SUCCESS";
                string text = "문이 열렸습니다!";
                ShowMessage(title, text);
                SaveDataToFirestore();
                Frame.Navigate(typeof(numCheck2), selectedButtonNumber);
            }
            else
            {
                ShowMessage("오류", "박스 번호를 선택해주세요.");
            }
        }
        // 시리얼 포트 사용 후 닫아주는 함수
        public void CloseSerialPort()
        {
            if (serialPort1 != null)
            {
                serialPort1.Dispose();
                serialPort1 = null;
            }
            if (serialPort2 != null)
            {
                serialPort2.Dispose();
                serialPort2 = null;
            }
            if (serialPort3 != null)
            {
                serialPort3.Dispose();
                serialPort3 = null;
            }
            if (serialPort4 != null)
            {
                serialPort4.Dispose();
                serialPort4 = null;
            }

            ShowMessage("성공", "시리얼 포트가 성공적으로 닫혔습니다.");
        }

        // 아두이노 1번 연결
        private async Task SendSerialData1(string data)
        {
            if (serialPort1 != null)
            {
                try
                {
                    dataWriter1.WriteString(data);
                    await dataWriter1.StoreAsync();
                    string title = "SUCCESS";
                    string text = "Received from Arduino 1: " + data;
                    firestoreHelper.ShowMessage(title, text);
                }
                catch (Exception ex)
                {
                    string title = "ERROR";
                    string text = "Error communicating with Arduino 1: " + ex.Message;
                    firestoreHelper.ShowMessage(title, text);
                }
            }
            else
            {
                string title = "ERROR";
                string text = "Serial port 1 is not initialized.";
                firestoreHelper.ShowMessage(title, text);
            }
            // 작업이 끝나면 시리얼 포트를 닫음
            CloseSerialPort();
        }
        // 아두이노 2번 연결
        private async Task SendSerialData2(string data)
        {
            if (serialPort2 != null)
            {
                try
                {
                    dataWriter2.WriteString(data);
                    await dataWriter2.StoreAsync();
                    string title = "SUCCESS";
                    string text = "Received from Arduino 2: " + data;
                    firestoreHelper.ShowMessage(title, text);
                }
                catch (Exception ex)
                {
                    string title = "ERROR";
                    string text = "Error communicating with Arduino 2: " + ex.Message;
                    firestoreHelper.ShowMessage(title, text);
                }
            }
            else
            {
                string title = "ERROR";
                string text = "Serial port 2 is not initialized.";
                firestoreHelper.ShowMessage(title, text);
            }
            // 작업이 끝나면 시리얼 포트를 닫음
            CloseSerialPort();
        }
        // 아두이노 3번 연결
        private async Task SendSerialData3(string data)
        {
            if (serialPort3 != null)
            {
                try
                {
                    dataWriter3.WriteString(data);
                    await dataWriter3.StoreAsync();
                    string title = "SUCCESS";
                    string text = "Received from Arduino 3: " + data;
                    firestoreHelper.ShowMessage(title, text);
                }
                catch (Exception ex)
                {
                    string title = "ERROR";
                    string text = "Error communicating with Arduino 3: " + ex.Message;
                    firestoreHelper.ShowMessage(title, text);
                }
            }
            else
            {
                string title = "ERROR";
                string text = "Serial port 3 is not initialized.";
                firestoreHelper.ShowMessage(title, text);
            }
            // 작업이 끝나면 시리얼 포트를 닫음
            CloseSerialPort();
        }
        // 아두이노 4번 연결
        private async Task SendSerialData4(string data)
        {
            if (serialPort4 != null)
            {
                try
                {
                    dataWriter4.WriteString(data);
                    await dataWriter4.StoreAsync();
                    string title = "SUCCESS";
                    string text = "Received from Arduino 4: " + data;
                    firestoreHelper.ShowMessage(title, text);
                }
                catch (Exception ex)
                {
                    string title = "ERROR";
                    string text = "Error communicating with Arduino 4: " + ex.Message;
                    firestoreHelper.ShowMessage(title, text);
                }
            }
            else
            {
                string title = "ERROR";
                string text = "Serial port 4 is not initialized.";
                firestoreHelper.ShowMessage(title, text);
            }
            // 작업이 끝나면 시리얼 포트를 닫음
            CloseSerialPort();
        }
        #endregion


    }
}
