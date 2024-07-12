using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace raspiDisplay.Helpers
{
    public class FirestoreHelper
    {
        private FirestoreDb db;

        public FirestoreHelper()
        {
            InitializeFirebase();
        }

        private void InitializeFirebase()
        {
            string pathToCredentialFile = @"test-325e6-firebase-adminsdk-rnle1-972e67f70a.json";

            try
            {
                // Firestore 초기화
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", pathToCredentialFile);
                db = FirestoreDb.Create("test-325e6");
            }
            catch (Exception ex)
            {
                ShowMessage("오류", $"Error initializing Firestore: {ex.Message}");
            }
        }

        public async Task<bool> AreAllBoxesFilledAsync(int numberOfBoxes)
        {
            for (int i = 1; i <= numberOfBoxes; i++)
            {
                string boxNumber = i.ToString();
                DocumentReference docRef = db.Collection("box").Document(boxNumber);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                if (!snapshot.Exists)
                {
                    return false;
                }
            }
            return true;
        }
        public async Task<bool> IsBoxFilledAsync(string boxNumber)
        {
            DocumentReference docRef = db.Collection("box").Document(boxNumber);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            return snapshot.Exists;
        }

        public async Task SaveDataAsync(string phoneNumber, string selectedButtonNumber, string randomNumber)
        {
            try
            {
                // 6자리 숫자 비밀번호 생성
                Random random = new Random();
                int password = random.Next(100000, 1000000);

                DocumentReference docRef = db.Collection("box").Document(selectedButtonNumber);
                Dictionary<string, object> data = new Dictionary<string, object>()
                {
                    { "phone_number", phoneNumber },
                    { "box_number", selectedButtonNumber },
                    { "password", password } // 비밀번호 필드 추가
                };
                await docRef.SetAsync(data, SetOptions.MergeAll);
                ShowMessage("성공", "데이터가 성공적으로 저장되었습니다.");
            }
            catch (Exception ex)
            {
                ShowMessage("오류", $"데이터 저장 중 오류 발생: {ex.Message}");
            }
        }

        public async void ShowMessage(string title, string content)
        {
            var dialog = new MessageDialog(content, title);
            dialog.Commands.Add(new UICommand("OK"));
            await dialog.ShowAsync();
        }

        public async Task<bool> CheckPasswordAsync(string boxNumber, string enteredPassword)
        {
            try
            {
                DocumentReference docRef = db.Collection("box").Document(boxNumber);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    // Firestore에 저장된 비밀번호 가져오기
                    int storedPassword = snapshot.GetValue<int>("password");

                    // 비밀번호 확인
                    if (enteredPassword == storedPassword.ToString())
                    {
                        return true; // 비밀번호 일치
                    }
                }

                return false; // 비밀번호 불일치 또는 문서가 없음
            }
            catch (Exception ex)
            {
                ShowMessage("오류", $"비밀번호 확인 중 오류 발생: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteBoxDataAsync(string boxNumber)
        {
            try
            {
                DocumentReference docRef = db.Collection("box").Document(boxNumber);
                await docRef.DeleteAsync();
                return true; // 삭제 성공
            }
            catch (Exception ex)
            {
                // 삭제 중 오류 발생 시 예외 처리
                throw ex;
            }
        }
    }
}
