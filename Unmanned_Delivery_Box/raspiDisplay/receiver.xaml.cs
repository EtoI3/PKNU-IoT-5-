using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using raspiDisplay.Helpers;


// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace raspiDisplay
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class receiver : Page
    {
        private FirestoreHelper firestoreHelper = new FirestoreHelper();
        
        
        public receiver()
        {
            this.InitializeComponent();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(userType));
        }

        private void homeBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                NumTxtBox.Text += button.Content.ToString();
                NumTxtBox.SelectionStart = NumTxtBox.Text.Length;
            }
        }

        private void NumTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Text.Length > 6)
            {
                textBox.Text = textBox.Text.Substring(0, 6);
                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        private void delBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.NumTxtBox.Text))
            {
                this.NumTxtBox.Text = this.NumTxtBox.Text.Substring(0, this.NumTxtBox.Text.Length - 1);
                NumTxtBox.SelectionStart = NumTxtBox.Text.Length;
            }

        }

        private async void okBtn_Click(object sender, RoutedEventArgs e)
        {
            //Frame.Navigate(typeof(numCheck));

            string enteredPassword = NumTxtBox.Text.Trim();
            for (int boxNumber = 1; boxNumber <= 4; boxNumber++)
            {
                bool isPasswordCorrect = await firestoreHelper.CheckPasswordAsync(boxNumber.ToString(), enteredPassword);

                if (isPasswordCorrect)
                {
                    OpenBox(boxNumber.ToString());
                    Frame.Navigate(typeof(numCheck), boxNumber);
                    return; // 비밀번호가 맞으면 더 이상 확인하지 않고 종료
                }
             
            }

            firestoreHelper.ShowMessage("알림", "비밀번호가 올바르지 않습니다.");
            
        }

        private void OpenBox(string boxNumber)
        {
            // 여기에 박스를 열거나 관련 작업을 수행하는 코드 추가
            firestoreHelper.ShowMessage("알림", $"{boxNumber}번 박스가 열렸습니다.");
        }
    }
}
