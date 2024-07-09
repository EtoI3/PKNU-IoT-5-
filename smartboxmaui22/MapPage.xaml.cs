using System.Collections.ObjectModel;

namespace smartboxmaui2;

public partial class MapPage : ContentPage
{
    private List<FirebaseProperty> dataList;
    // ���ã�� ����� ���� ObservableCollection
    private ObservableCollection<string> favoriteAddresses;

    public MapPage(List<FirebaseProperty> data)
	{
		InitializeComponent();
        dataList = data;

        favoriteAddresses = new ObservableCollection<string>();
        FavoritesListView.ItemsSource = favoriteAddresses;

        var mapsUrl = "https://www.google.co.kr/maps/?hl=ko&entry=ttu";
        webView.Source = mapsUrl;
    }

    private void BtnSerch_Clicked(object sender, EventArgs e)
    {
        string address = AddressEntry.Text;
        if (!string.IsNullOrWhiteSpace(address))
        {
            // �ּ� �˻� �� ���� ������Ʈ ���� ����
            DisplayAlert("�˻�", $"�ּ� '{address}'�� �˻��մϴ�.", "Ȯ��");
            var mapsUrl = $"https://maps.google.com/?q={Uri.EscapeDataString(address)}";
            webView.Source = mapsUrl;
        }
        else
        {
            DisplayAlert("����", "�ּҸ� �Է��ϼ���.", "Ȯ��");
        }
    }

    private void BtnFavorite_Clicked(object sender, EventArgs e)
    {
        string address = AddressEntry.Text;
        if (!string.IsNullOrWhiteSpace(address))
        {
            favoriteAddresses.Add(address);
            DisplayAlert("�߰���", $"�ּ� '{address}'��(��) ���ã�⿡ �߰��Ǿ����ϴ�.", "Ȯ��");
            AddressEntry.Text = string.Empty; // �Է� â ����
        }
        else
        {
            DisplayAlert("����", "�ּҸ� �Է��ϼ���.", "Ȯ��");
        }
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
}