using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace raspiDisplay
{
    /// <summary>
    /// userType.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class userType : Window
    {
        public userType()
        {
            InitializeComponent();
        }

        private void recvBtn_Click(object sender, RoutedEventArgs e)
        {
            raspiDisplay.receiver recv = new raspiDisplay.receiver();
            recv.Show();
            this.Close();
        }

        private void sendBtn_Click(object sender, RoutedEventArgs e)
        {
            raspiDisplay.sender send = new raspiDisplay.sender();
            send.Show();
            this.Close();
        }
    }
}
