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

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace raspiDisplay
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class sender : Page
    {
        public sender()
        {
            this.InitializeComponent();
        }

        private void backBtn2_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(userType));
        }
        private void homeBtn2_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                NumTxtBox2.Text += button.Content.ToString();
                NumTxtBox2.Focus(FocusState.Programmatic);
                NumTxtBox2.SelectionStart = NumTxtBox2.Text.Length;
            }
        }

        private void NumTxtBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            string currentText = NumTxtBox2.Text.Replace("-", "");
            
            if (currentText.Length > 11)
            {
                currentText = currentText.Substring(0, 11);
            }
            if (currentText.Length >= 4)
            {
                currentText = currentText.Insert(3, "-");
            }
            if (currentText.Length >= 9)
            {
                currentText = currentText.Insert(8, "-");
            }

            NumTxtBox2.Text = currentText;
            NumTxtBox2.Focus(FocusState.Programmatic);
            NumTxtBox2.SelectionStart = NumTxtBox2.Text.Length;
        }

        private void delBtn2_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.NumTxtBox2.Text))
            {
                this.NumTxtBox2.Text = this.NumTxtBox2.Text.Substring(0, this.NumTxtBox2.Text.Length - 1);
                NumTxtBox2.Focus(FocusState.Programmatic);
                NumTxtBox2.SelectionStart = NumTxtBox2.Text.Length;
            }
        }

        private void okBtn2_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(numChoice));
        }

    }
}
