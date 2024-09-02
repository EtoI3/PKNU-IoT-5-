using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using raspiDisplay.Helpers;
using System.IO.Ports;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace raspiDisplay
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class numCheck : Page
    {
        int boxNumber;
        private FirestoreHelper firestoreHelper;

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

        public numCheck()
        {
            this.InitializeComponent();
            firestoreHelper = new FirestoreHelper();
            InitializeSerialPort();
        }      

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            boxNumber = (int)e.Parameter;
            TextN.Text = boxNumber.ToString();
            TextN.FontSize = 30;

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

        private async void restartBtn_Click(object sender, RoutedEventArgs e)
        {
            // 몇번 사물함을 선택했는지 모르니 전부 잠금
            //await SendSerialData1("2");
            //await SendSerialData2("2");
            //await SendSerialData3("2");
            //await SendSerialData4("2");

            string selectedButtonNumber = boxNumber.ToString();
            if (selectedButtonNumber == "1")
                await SendSerialData1("1");
            else if (selectedButtonNumber == "2")
                await SendSerialData2("1");
            else if (selectedButtonNumber == "3")
                await SendSerialData3("1");
            else if (selectedButtonNumber == "4")
                await SendSerialData4("1");
            string title = "SUCCESS";
            string text = "문이 닫혔습니다!";
            firestoreHelper.ShowMessage(title, text);

            bool deleteSuccess = await firestoreHelper.DeleteBoxDataAsync(boxNumber.ToString());
            firestoreHelper.ShowMessage("알림", $"{boxNumber}번 박스의 데이터가 삭제되었습니다.");
            Frame.Navigate(typeof(MainPage));
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

            firestoreHelper.ShowMessage("성공", "시리얼 포트가 성공적으로 닫혔습니다.");
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
