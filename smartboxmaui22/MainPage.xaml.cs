using Microsoft.Maui.Controls;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using FirestoreDocumentReference = Google.Cloud.Firestore.DocumentReference;
using System.IO;
using System.Reflection;

namespace smartboxmaui2
{
    public partial class MainPage : ContentPage
    {

        // 파이어베이스 db 사용 위한 전역변수 선언
        FirestoreDb db;


        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            Loaded += ContentPage_Loaded;
        }



        private async void BtnSubmit_Clicked(object sender, EventArgs e)
        {
            // 번호 입력을 받는 Entry 컨트롤의 텍스트를 가져옵니다.
            var phoneNumber = PhoneNumberEntry.Text;

            // 전화번호 유효성 검사를 수행합니다.
            if (IsValidPhoneNumber(phoneNumber))
            {
                if (db != null)
                {
                    await SearchByPhoneNumber(phoneNumber); // 비동기 메서드 호출 시 await 사용
                    
                }
                else
                {
                    await DisplayAlert("Error", "Firebase not initialized", "OK");
                }
            }
            else
            {
                // 경고창을 표시합니다.
                await DisplayAlert("유효성 검사 실패", "올바른 전화번호 형식이 아닙니다. 010-XXXX-XXXX 형식으로 입력해주세요.", "확인");
            }
        }

        // 전화번호 유효성 검사 함수
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // 전화번호 형식을 검증하는 정규 표현식
            var regex = new Regex(@"^010-\d{4}-\d{4}$");
            return regex.IsMatch(phoneNumber);
        }

        //-------------------------Firebase DB--------------------------------------//
        private async void ContentPage_Loaded(object sender, EventArgs e)
        {
            await InitializeFirestoreAsync();
        }

        

        private async Task InitializeFirestoreAsync()
        {
            try
            {
                string filename = "test-325e6-firebase-adminsdk-rnle1-972e67f70a.json";
                string resourceName = $"{typeof(MainPage).Namespace}.{filename}";

                var assembly = typeof(MainPage).GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream(resourceName);

                if (stream == null)
                {
                    await DisplayAlert("Error", $"Firebase config file not found in assembly. Expected: {resourceName}", "OK");
                    return;
                }

                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), filename);

                using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    stream.CopyTo(fileStream);
                }

                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
                db = FirestoreDb.Create("test-325e6");

                await DisplayAlert("Success", "Firebase Initialized", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Firebase Initialization Error: {ex.Message}", "OK");
            }
        }

        // document 저장 db (App에서 필요 X)
        //private async Task Join(string phone)
        //{
        //    try
        //    {
        //        FirestoreDocumentReference docRef = db.Collection("PhonNuber").Document(phone);
        //        Dictionary<string, object> data1 = new Dictionary<string, object>()
        //        {
        //            {"PhoneNumber", phone},
        //        };
        //        await docRef.SetAsync(data1);
        //        await DisplayAlert("Success", "Document successfully written!", "OK");

        //        await DisplayAlert("문 열림", "보관함 문이 열렸습니다", "확인");
        //    }
        //    catch (Exception ex)
        //    {
        //        await DisplayAlert("Error", $"Error writing document: {ex.Message}", "OK");
        //    }
        //}

        // 전화번호로 사용여부 확인 검색 메서드
        async Task SearchByPhoneNumber(string phone)
        {
            try
            {
                Query qref = db.Collection("box").WhereEqualTo("PhoneNumber", phone);
                QuerySnapshot snap = await qref.GetSnapshotAsync();

                if (snap.Count > 0)
                {
                    var document = snap.Documents.FirstOrDefault();
                    if (document != null)
                    {
                        var data = document.ConvertTo<FirebaseProperty>();
                        await DisplayAlert("Success", "보관중인 회원입니다.", "확인");
                        await Navigation.PushAsync(new MenuPage(data));
                        return;
                    }
                }
                else
                {
                    await DisplayAlert("Fail", "보관된 물건이 없습니다.", "확인");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An unexpected error occurred: {ex.Message}", "OK");
            }
        }




    }
}
 