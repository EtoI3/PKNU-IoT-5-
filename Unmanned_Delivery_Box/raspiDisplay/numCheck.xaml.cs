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

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace raspiDisplay
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class numCheck : Page
    {
        int boxNumber;
        private FirestoreHelper firestoreHelper;
        public numCheck()
        {
            this.InitializeComponent();
            firestoreHelper = new FirestoreHelper();
        }

        private async void restartBtn_Click(object sender, RoutedEventArgs e)
        {
            bool deleteSuccess = await firestoreHelper.DeleteBoxDataAsync(boxNumber.ToString());
            firestoreHelper.ShowMessage("알림", $"{boxNumber}번 박스의 데이터가 삭제되었습니다.");

            Frame.Navigate(typeof(MainPage));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            boxNumber = (int)e.Parameter;
            TextN.Text = boxNumber.ToString();
            TextN.FontSize = 30;

        }

    }
}
