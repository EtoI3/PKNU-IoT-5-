namespace smartboxmaui2;

public partial class MenuPage : ContentPage
{
    private List<FirebaseProperty> dataList;

    public MenuPage(List<FirebaseProperty> data)
    {
        InitializeComponent();
        dataList = data;
    }

    private async void BtnCode_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CodeNumberPage(dataList));
    }


    private async void BtnInquery_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InquiryPage(dataList));
    }

    private async void BtnMap_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MapPage(dataList));
    }

    private void BtnCctv_Clicked(object sender, EventArgs e)
    {

    }

    private async void ImgBtnSet_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage());
    }

    private async void ImgBtnMyinfo_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MyInfoPage());
    }
}