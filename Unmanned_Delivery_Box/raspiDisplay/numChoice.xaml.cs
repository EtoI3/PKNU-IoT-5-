using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class numChoice : Page
    {
        private Button selectedButton = null;

        public numChoice()
        {
            this.InitializeComponent();
        }

        private void backBtn3_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(sender));
        }

        private void homeBtn3_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;

            clickedButton.Background = new SolidColorBrush(Color.FromArgb(255, 169, 169, 169));

            if (selectedButton != null && selectedButton != clickedButton)
            {
                selectedButton.Background = new SolidColorBrush(Color.FromArgb(255, 63, 81, 181));
            }

            selectedButton = clickedButton;
        }

        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(numCheck2));
        }
    }
}
