using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// receiver.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class receiver : Window
    {
        public receiver()
        {
            InitializeComponent();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            raspiDisplay.userType usertype = new raspiDisplay.userType();
            usertype.Show();
            this.Close();
        }

        private void homeBtn_Click(object sender, RoutedEventArgs e)
        {
            raspiDisplay.MainWindow main = new raspiDisplay.MainWindow();
            main.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string number)
            {
                NumTxtBox.Text += number;
                NumTxtBox.Focus();
                NumTxtBox.CaretIndex = NumTxtBox.Text.Length; // 포커스 뒤로 가져오기
            }
        }

        private void Btn10_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.NumTxtBox.Text))
            {
                this.NumTxtBox.Text = this.NumTxtBox.Text.Substring(0, this.NumTxtBox.Text.Length - 1);
                NumTxtBox.Focus();
                NumTxtBox.CaretIndex = NumTxtBox.Text.Length;
            }
        }
    }
}
