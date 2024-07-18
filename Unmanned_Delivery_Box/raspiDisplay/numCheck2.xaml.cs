using raspiDisplay.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace raspiDisplay
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class numCheck2 : Page
    {
        private FirestoreHelper firestoreHelper; // 파이어베이스
        string BoxNum;

        // 시리얼 통신 전역변수
        private SerialDevice serialPort = null;
        private DataWriter dataWriter = null;
        public numCheck2()
        {
            this.InitializeComponent();
            firestoreHelper = new FirestoreHelper();
            InitializeSerialPort();
        }

        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            BoxNum = e.Parameter as string;
            TextN.Text = BoxNum;
            TextN.FontSize = 30;

        }

        #region "시리얼 통신"
        private async void InitializeSerialPort()
        {
            try
            {
                string selector = SerialDevice.GetDeviceSelector("COM3");
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
                        string title = "SUCCESS";
                        string text = "Serial port initialized successfully.";
                        firestoreHelper.ShowMessage(title, text);
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

        private async void restartBtn2_Click(object sender, RoutedEventArgs e)
        {
            await SendSerialData("2");
            string title = "SUCCESS";
            string text = "문이 닫혔습니다!";
            firestoreHelper.ShowMessage(title, text);

            Frame.Navigate(typeof(MainPage));

        }

        private async Task SendSerialData(string data)
        {
            if (serialPort != null)
            {
                try
                {
                    dataWriter.WriteString(data);
                    await dataWriter.StoreAsync();
                    string title = "SUCCESS";
                    string text = "Data sent: " + data;
                    firestoreHelper.ShowMessage(title, text);
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
