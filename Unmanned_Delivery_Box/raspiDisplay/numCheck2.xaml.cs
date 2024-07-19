using raspiDisplay.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Storage.Streams;

namespace raspiDisplay
{
    public sealed partial class numCheck2 : Page
    {
        private FirestoreHelper firestoreHelper; // 파이어베이스
        string BoxNum;

        // 시리얼 통신 전역변수
        private SerialDevice serialPort = null;
        private DataWriter dataWriter = null;
        private DataReader dataReader = null;

        // 문 상태 전역 변수
        private bool isDoorOpen = false;

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

                        // 데이터 읽기 시작
                        ReadSerialDataContinuously();
                    }
                }
                else
                {
                    firestoreHelper.ShowMessage("ERROR", "No serial devices found.");
                }
            }
            catch (Exception ex)
            {
                firestoreHelper.ShowMessage("ERROR", "Error initializing serial port: " + ex.Message);
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

        private async void restartBtn2_Click(object sender, RoutedEventArgs e)
        {
            if (isDoorOpen)  // 문이 열려있으면
            {
                firestoreHelper.ShowMessage("알림", "문이 열려 있습니다. 제발 꼭 문을 닫으셔야합니다.");
                await firestoreHelper.DeleteBoxDataAsync(BoxNum.ToString());
            }
            else  // 문이 닫혀있으면
            {
                await SendSerialData("2");
                firestoreHelper.ShowMessage("감사합니다", "문을 닫아주셨군요. 안녕히 가세요!");
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
                    //firestoreHelper.ShowMessage("SUCCESS", "Data sent: " + data);
                }
                catch (Exception ex)
                {
                    firestoreHelper.ShowMessage("ERROR", "Failed to send data: " + ex.Message);
                }
            }
            else
            {
                firestoreHelper.ShowMessage("ERROR", "Serial port is not initialized.");
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
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
