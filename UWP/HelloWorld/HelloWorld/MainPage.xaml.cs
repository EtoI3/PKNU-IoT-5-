using System;
using System.Data;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x412에 나와 있습니다.

namespace HelloWorld
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class MainPage : Page
    {
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

        public MainPage()
        {
            this.InitializeComponent();
            InitializeSerialPort();
        }
        
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
                    serialText.Text = serialPort1.ToString();
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
                    statusText.Text = "Serial ports initialized successfully.";
                }
                else
                {
                    statusText.Text = "No serial devices found.";
                }
            }
            catch (Exception ex)
            {
                statusText.Text = "Error initializing serial port: " + ex.Message;
            }
        }

        private void ConfigureSerialPort(SerialDevice serialPort)
        {
            serialPort.BaudRate = 9600;
            serialPort.DataBits = 8;
            serialPort.Parity = SerialParity.None;
            serialPort.StopBits = SerialStopBitCount.One;
            serialPort.Handshake = SerialHandshake.None;
        }

        private async void Button1_Click(object sender, RoutedEventArgs e)
        {
            await SendSerialData1("1");
            serialText.Text = "1번 문이 닫혔습니다!";
        }

        private async void Button2_Click(object sender, RoutedEventArgs e)
        {
            await SendSerialData1("2");
            serialText.Text = "1번 문이 열렸습니다!";
        }

        private async void Button3_Click(object sender, RoutedEventArgs e)
        {
            await SendSerialData2("1");
            serialText.Text = "2번 문이 닫혔습니다!";
        }

        private async void Button4_Click(object sender, RoutedEventArgs e)
        {
            await SendSerialData2("2");
            serialText.Text = "2번 문이 열렸습니다!";
        }

        private async void Button5_Click(object sender, RoutedEventArgs e)
        {
            await SendSerialData3("1");
            serialText.Text = "3번 문이 닫혔습니다!";
        }

        private async void Button6_Click(object sender, RoutedEventArgs e)
        {
            await SendSerialData3("2");
            serialText.Text = "3번 문이 열렸습니다!";
        }

        private async void Button7_Click(object sender, RoutedEventArgs e)
        {
            await SendSerialData4("1");
            serialText.Text = "4번 문이 닫혔습니다!";
        }

        private async void Button8_Click(object sender, RoutedEventArgs e)
        {
            await SendSerialData4("2");
            serialText.Text = "4번 문이 열렸습니다!";
        }

        private async Task SendSerialData1(string data)
        {
            if (serialPort1 != null)
            {
                try
                {
                    dataWriter1.WriteString(data);
                    await dataWriter1.StoreAsync();
                    statusText.Text = "Received from Arduino 1: " + data;
                }
                catch (Exception ex)
                {
                    statusText.Text = "Error communicating with Arduino 1: " + ex.Message;
                }
            }
            else
            {
                statusText.Text = "Serial port 1 is not initialized.";
            }
        }

        private async Task SendSerialData2(string data)
        {
            if (serialPort2 != null)
            {
                try
                {
                    dataWriter2.WriteString(data);
                    await dataWriter2.StoreAsync();
                    statusText.Text = "Received from Arduino 2: " + data;
                }
                catch (Exception ex)
                {
                    statusText.Text = "Error communicating with Arduino 2: " + ex.Message;
                }
            }
            else
            {
                statusText.Text = "Serial port 2 is not initialized.";
            }
        }
        private async Task SendSerialData3(string data)
        {
            if (serialPort1 != null)
            {
                try
                {
                    dataWriter3.WriteString(data);
                    await dataWriter3.StoreAsync();
                    statusText.Text = "Received from Arduino 3: " + data;
                }
                catch (Exception ex)
                {
                    statusText.Text = "Error communicating with Arduino 3: " + ex.Message;
                }
            }
            else
            {
                statusText.Text = "Serial port 3 is not initialized.";
            }
        }

        private async Task SendSerialData4(string data)
        {
            if (serialPort1 != null)
            {
                try
                {
                    dataWriter4.WriteString(data);
                    await dataWriter4.StoreAsync();
                    statusText.Text = "Received from Arduino 4: " + data;
                }
                catch (Exception ex)
                {
                    statusText.Text = "Error communicating with Arduino 4: " + ex.Message;
                }
            }
            else
            {
                statusText.Text = "Serial port 4 is not initialized.";
            }
        }

        protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            // DataWriter가 null이 아니면 stream 연결 해제
            dataWriter1?.DetachStream();
            dataWriter1 = null;
            dataWriter2?.DetachStream();
            dataWriter2 = null;
            dataWriter3?.DetachStream();
            dataWriter3 = null;
            dataWriter4?.DetachStream();
            dataWriter4 = null;

            // SerialPort가 null이 아니면 시리얼 포트 해제 및 null로 설정
            serialPort1?.Dispose();
            serialPort1 = null;
            serialPort2?.Dispose();
            serialPort2 = null;
            serialPort3?.Dispose();
            serialPort3 = null;
            serialPort4?.Dispose();
            serialPort4 = null;

            // 기본 OnNavigatedFrom 메서드 호출
            base.OnNavigatedFrom(e);
        }

    }
}
