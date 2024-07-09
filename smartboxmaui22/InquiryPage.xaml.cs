using Google.Cloud.Firestore;
using static Microsoft.Maui.ApplicationModel.Permissions;
using FirestoreDocumentReference = Google.Cloud.Firestore.DocumentReference;

namespace smartboxmaui2;

public partial class InquiryPage : ContentPage
{
    private List<FirebaseProperty> dataList;
    FirestoreDb db;


    public InquiryPage(List<FirebaseProperty> data)
	{
		InitializeComponent();
        dataList = data;
    }
    private async void BtnSet_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage());
    }

    private async void ImgBtnMenu_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MenuPage(dataList));
    }


    private async void BtnMyinfo_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MyInfoPage());
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        try
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"test-325e6-firebase-adminsdk-rnle1-972e67f70a.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("test-325e6");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "initializing Firestore: ","ok");
        }
    }

    async void Question(string question)
    { 
        
        FirestoreDocumentReference DOC = db.Collection("Questions").Document($"{Question}");
        Dictionary<string, object> data1 = new Dictionary<string, object>()
        {
            {"Question", question },
        };
        await DOC.SetAsync(data1);

        await DisplayAlert("저장", "질문이 저장되었습니다.", "확인");

    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var question = QuestionEntry.Text;
        Question(question);
        await DisplayAlert("저장", "질문이 저장되었습니다.", "확인");
    }
}