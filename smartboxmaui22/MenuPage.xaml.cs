namespace smartboxmaui2;

public partial class MenuPage : ContentPage
{
    private smartboxmaui2.FirebaseProperty receivedData;

    public MenuPage(smartboxmaui2.FirebaseProperty data)
    {
        InitializeComponent();
        receivedData = data;
    }

    private async void BtnCode_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CodeNumberPage(receivedData));
    }
}