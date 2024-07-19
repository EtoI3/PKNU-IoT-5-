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

namespace raspiDisplay
{
    public sealed partial class numChoice : Page
    {
        private Button selectedButton = null;
        private string phoneNumber;
        private string selectedButtonNumber;
        private FirestoreHelper firestoreHelper;
        private static Random random = new Random();

        // 시리얼 통신 전역변수
        private SerialDevice serialPort = null;
        private DataWriter dataWriter = null;

        public numChoice()
        {
            this.InitializeComponent();
            firestoreHelper = new FirestoreHelper();
            LoadButtonStates();
            InitializeSerialPort();
        }

        public static string GenerateRandomNumber()
        {
            int randomNumber = random.Next(100000, 999999);
            return randomNumber.ToString();
        }

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
                        button.IsEnabled = false;
                        button.Background = new SolidColorBrush(Windows.UI.Colors.Gray);
                    }
                    else
                    {
                        button.Background = new SolidColorBrush(Windows.UI.Colors.Blue);
                        button.Click += Button_Click;
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            var textBlock = clickedButton.Content as TextBlock;
            if (textBlock != null)
            {
                selectedButtonNumber = textBlock.Text;
            }

            clickedButton.IsEnabled = false;

            if (selectedButton != null && selectedButton != clickedButton)
            {
                selectedButton.Background = new SolidColorBrush(Windows.UI.Colors.Blue);
                selectedButton.IsEnabled = true;
            }

            selectedButton = clickedButton; ;
        }

        private async void SaveDataToFirestore()
        {
            string RandNum = GenerateRandomNumber();
            await firestoreHelper.SaveDataAsync(phoneNumber, selectedButtonNumber, RandNum);
        }

        

        #region "시리얼 통신"
        private async void InitializeSerialPort()
        {
            try
            {
                string selector = SerialDevice.GetDeviceSelector("COM5");
                var devices = await DeviceInformation.FindAllAsync(selector);
                if (devices.Count > 0)
                {
                    serialPort = await SerialDevice.FromIdAsync(devices[0].Id);
                    if (serialPort != null)
                    {
                        serialPort.BaudRate = 9600;
                        serialPort.DataBits = 8;
                        serialPort.Parity = SerialParity.None;
                        serialPort.StopBits = SerialStopBitCount.One;
                        serialPort.Handshake = SerialHandshake.None;

                        dataWriter = new DataWriter(serialPort.OutputStream);
                        //string title = "SUCCESS";
                        //string text = "Serial port initialized successfully.";
                        //ShowMessage(title, text);
                    }
                }
                else
                {
                    string title = "ERROR";
                    string text = "No serial devices found.";
                    ShowMessage(title, text);
                }
            }
            catch (Exception ex)
            {
                string title = "ERROR";
                string text = "Error initializing serial port: " + ex.Message;
                ShowMessage(title, text);
            }
        }

        private async void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedButtonNumber))
            {
                await SendSerialData("1");
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

        private async Task SendSerialData(string data)
        {
            if (serialPort != null)
            {
                try
                {
                    dataWriter.WriteString(data);
                    await dataWriter.StoreAsync();
                    //string title = "SUCCESS";
                    //string text = "Data sent: " + data;
                    //ShowMessage(title, text);
                }
                catch (Exception ex)
                {
                    string title = "ERROR";
                    string text = "Failed to send data: " + ex.Message;
                    ShowMessage(title, text);
                }
            }
            else
            {
                string title = "ERROR";
                string text = "Serial port is not initialized.";
                ShowMessage(title, text);
            }
        }

        protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            dataWriter?.DetachStream();
            dataWriter = null;

            serialPort?.Dispose();
            serialPort = null;

            base.OnNavigatedFrom(e);
        }
        #endregion


    }
}
