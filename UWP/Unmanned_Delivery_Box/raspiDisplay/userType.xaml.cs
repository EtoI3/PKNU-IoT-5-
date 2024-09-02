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
using Windows.UI.Popups;

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace raspiDisplay
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class userType : Page
    {
        private string phoneNumber;
        private FirestoreHelper firestoreHelper;

        public userType()
        {
            this.InitializeComponent();
            firestoreHelper = new FirestoreHelper();
        }

        private void receiverBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(receiver));
        }

        private async void senderBtn_Click(object sender, RoutedEventArgs e)
        {
            bool allBoxesFilled = await firestoreHelper.AreAllBoxesFilledAsync(4);
            if (allBoxesFilled)
            {
                firestoreHelper.ShowMessage("알림", "모든 박스가 이미 저장되었습니다.");
            }
            else
            {
                Frame.Navigate(typeof(sender));

            }

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            phoneNumber = e.Parameter as string;  // 이전 페이지에서 넘어온 전화번호
        }

        
    }
}