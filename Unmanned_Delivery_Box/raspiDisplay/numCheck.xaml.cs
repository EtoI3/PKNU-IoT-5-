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
        string boxNum;
        private FirestoreHelper firestoreHelper;

        // 시리얼 통신 전역변수
        private SerialDevice serialPort = null;
        private DataWriter dataWriter = null;
        private DataReader dataReader = null;

        private bool isDoorOpen = false;


        public numCheck()
        {
            this.InitializeComponent();
            firestoreHelper = new FirestoreHelper();
            InitializeSerialPort();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            boxNum = e.Parameter as string;
            TextN.Text = boxNum;
            TextN.FontSize = 30;

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
                        dataReader = new DataReader(serialPort.InputStream);
                        //string title = "SUCCESS";
                        //string text = "Serial port initialized successfully.";
                        //firestoreHelper.ShowMessage(title, text);

                        ReadSerialDataContinuously();

                    }
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

        private async void ReadSerialDataContinuously()
        {
            while (serialPort != null)
            {
                string sensorStatus = await ReadSerialData();
                if (sensorStatus == "0")  // 문이 닫혔을 때
                {
                    isDoorOpen = false;
                }
                else if (sensorStatus == "1")  // 문이 열렸을 때
                {
                    isDoorOpen = true;
                }

                // 디버깅 메시지
                //firestoreHelper.ShowMessage("DEBUG", "Sensor Status: " + sensorStatus);

                // 잠시 대기 (예: 500ms)
                await Task.Delay(500);
            }
        }
        private async Task<string> ReadSerialData()
        {
            try
            {
                uint size = await dataReader.LoadAsync(1);
                if (size > 0)
                {
                    return dataReader.ReadString(size);
                }
            }
            catch (Exception ex)
            {
                //firestoreHelper.ShowMessage("ERROR", "Error reading serial data: " + ex.Message);
            }
            return null;
        }

        private async void restartBtn_Click(object sender, RoutedEventArgs e)
        {

            if (isDoorOpen)  // 문이 열려있으면
            {
                firestoreHelper.ShowMessage("알림", "문이 열려 있습니다. 제발 꼭 문을 닫으셔야합니다.");
                //await firestoreHelper.DeleteBoxDataAsync(boxNum.ToString());
            }
            else
            {
                await SendSerialData("2");
                string title = "감사합니다";
                string text = "문을 닫아주셨군요. 안녕히 가세요!";
                firestoreHelper.ShowMessage(title, text);

                await firestoreHelper.DeleteBoxDataAsync(boxNum.ToString());
                //firestoreHelper.ShowMessage("알림", $"{boxNum}번 박스의 데이터가 삭제되었습니다.");
                Frame.Navigate(typeof(MainPage));
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
                    //firestoreHelper.ShowMessage(title, text);
                }
                catch (Exception ex)
                {
                    string title = "ERROR";
                    string text = "Failed to send data: " + ex.Message;
                    firestoreHelper.ShowMessage(title, text);
                }
            }
            else
            {
                string title = "ERROR";
                string text = "Serial port is not initialized.";
                firestoreHelper.ShowMessage(title, text);
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
