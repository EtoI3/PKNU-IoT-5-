using System.Collections.ObjectModel;

namespace smartboxmaui2;

public partial class MapPage : ContentPage
{
    private List<FirebaseProperty> dataList;
    // 즐겨찾기 목록을 위한 ObservableCollection
    private ObservableCollection<string> favoriteAddresses;

    public MapPage(List<FirebaseProperty> data)
	{
		InitializeComponent();
        dataList = data;

        favoriteAddresses = new ObservableCollection<string>();
        //FavoritesListView.ItemsSource = favoriteAddresses;

        var mapsUrl = "https://www.google.co.kr/maps/?hl=ko&entry=ttu";
        webView.Source = mapsUrl;
    }

    private void BtnSerch_Clicked(object sender, EventArgs e)
    {
       // string address = AddressEntry.Text;
        //if (!string.IsNullOrWhiteSpace(address))
        //{
        //    // 주소 검색 및 지도 업데이트 로직 구현
        //    //DisplayAlert("검색", $"주소 '{address}'로 검색합니다.", "확인");
        //    //var mapsUrl = $"https://maps.google.com/?q={Uri.EscapeDataString(address)}";
        //   // webView.Source = mapsUrl;
        //}
        //else
        //{
        //    DisplayAlert("오류", "주소를 입력하세요.", "확인");
        //}
    }

  
    private async void ImgBtnSet_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage());
    }

    private async void ImgBtnMenu_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MenuPage(dataList));
    }


    private async void ImgBtnMyinfo_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MyInfoPage());
    }

    private void RadioButtonA_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var mapsUrl = "https://www.google.co.kr/maps/search/%EB%B6%80%EA%B2%BD%EB%8C%80+%ED%96%89%EB%B3%B5%EA%B8%B0%EC%88%99%EC%82%AC/data=!3m1!4b1?hl=ko&entry=ttu";
        webView.Source = mapsUrl;
    }

    private void RadioButtonB_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var mapsUrl = "https://www.google.co.kr/maps/search/%EA%B2%BD%EC%84%B1%EB%8C%80%EB%B6%80%EA%B2%BD%EB%8C%80%EC%97%AD/data=!3m1!4b1?hl=ko&entry=ttu";
        webView.Source = mapsUrl;
    }

    private void RadioButtonC_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var mapsUrl = "https://www.google.co.kr/maps/search/%EB%B6%80%EA%B2%BD%EB%8C%80+%EC%9A%A9%EB%8B%B9%EC%BA%A0%ED%8E%8C%EC%8A%A4/data=!3m1!4b1?hl=ko&entry=ttu";
        webView.Source = mapsUrl;
    }
}