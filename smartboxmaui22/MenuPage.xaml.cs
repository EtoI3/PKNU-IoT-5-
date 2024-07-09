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
}