using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Windows.UI.Popups;

namespace raspiDisplay.Helpers
{
    public class FirestoreHelper
    {
        private static readonly HttpClient client = new HttpClient();
        private const string projectId = "test-325e6";
        private const string baseUrl = "https://firestore.googleapis.com/v1/projects/" + projectId + "/databases/(default)/documents/";

        public FirestoreHelper()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> SaveBoxDataAsync(string collection, string documentId, Dictionary<string, object> data)
        {
            try
            {
                string url = $"{baseUrl}{collection}/{documentId}";
                var firestoreData = ConvertToFirestoreFormat(data);
                var json = JsonConvert.SerializeObject(firestoreData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PatchAsync(url, content);
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception ex)
            {
                await ShowMessage("오류", $"데이터 저장 중 오류 발생: {ex.Message}");
                return false;
            }
        }

        private Dictionary<string, object> ConvertToFirestoreFormat(Dictionary<string, object> data)
        {
            var firestoreData = new Dictionary<string, object>();
            foreach (var kvp in data)
            {
                if (kvp.Value is string)
                {
                    firestoreData[kvp.Key] = new { stringValue = kvp.Value };
                }
                else if (kvp.Value is int)
                {
                    firestoreData[kvp.Key] = new { integerValue = kvp.Value };
                }
                else if (kvp.Value is bool)
                {
                    firestoreData[kvp.Key] = new { booleanValue = kvp.Value };
                }
                else if (kvp.Value is double)
                {
                    firestoreData[kvp.Key] = new { doubleValue = kvp.Value };
                }
                else if (kvp.Value is Dictionary<string, object> nestedDictionary)
                {
                    firestoreData[kvp.Key] = new { mapValue = new { fields = ConvertToFirestoreFormat(nestedDictionary) } };
                }
                else
                {
                    throw new ArgumentException($"Unsupported data type: {kvp.Value.GetType().Name}");
                }
            }
            return new Dictionary<string, object> { { "fields", firestoreData } };
        }

        public async Task<T> GetDataAsync<T>(string collection, string documentId)
        {
            try
            {
                string url = baseUrl + collection + "/" + documentId;
                var response = await client.GetAsync(url);

                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var document = JsonConvert.DeserializeObject<T>(jsonResponse);

                return document;
            }
            catch (Exception ex)
            {
                await ShowMessage("오류", $"데이터 가져오기 중 오류 발생: {ex.Message}");
                return default(T);
            }
        }

        public async Task<bool> AreAllBoxesFilledAsync(int numberOfBoxes)
        {
            for (int i = 1; i <= numberOfBoxes; i++)
            {
                string boxNumber = i.ToString();
                string url = $"{baseUrl}box/{boxNumber}";
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> IsBoxFilledAsync(string boxNumber)
        {
            try
            {
                string url = $"{baseUrl}box/{boxNumber}";
                var response = await client.GetAsync(url);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                await ShowMessage("오류", $"데이터 확인 중 오류 발생: {ex.Message}");
                return false;
            }
        }

        public async Task SaveDataAsync(string phoneNumber, string selectedButtonNumber)
        {
            try
            {
                // 6자리 숫자 비밀번호 생성
                Random random = new Random();
                int pass_word = random.Next(100000, 1000000);

                string url = $"{baseUrl}box/{selectedButtonNumber}";
                Dictionary<string, object> data = new Dictionary<string, object>()
                {
                    { "phone_number", phoneNumber },
                    { "box_number", selectedButtonNumber },
                    { "password", pass_word } // 비밀번호 필드 추가
                };

                await SaveBoxDataAsync("box", selectedButtonNumber, data);
                await ShowMessage("성공", "데이터가 성공적으로 저장되었습니다.");
            }
            catch (Exception ex)
            {
                await ShowMessage("오류", $"데이터 저장 중 오류 발생: {ex.Message}");
            }
        }

        public async Task<bool> CheckPasswordAsync(string boxNumber, string enteredPassword)
        {
            try
            {
                string url = $"{baseUrl}box/{boxNumber}";
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var document = JObject.Parse(jsonResponse);

                    if (document["fields"] != null && document["fields"]["password"] != null)
                    {
                        int storedPassword = (int)document["fields"]["password"]["integerValue"];
                        if (enteredPassword == storedPassword.ToString())
                        {
                            return true; // 비밀번호 일치
                        }
                    }
                }

                return false; // 비밀번호 불일치 또는 문서가 없음
            }
            catch (Exception ex)
            {
                await ShowMessage("오류", $"비밀번호 확인 중 오류 발생: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteBoxDataAsync(string boxNumber)
        {
            try
            {
                string url = $"{baseUrl}box/{boxNumber}";
                var response = await client.DeleteAsync(url);
                return response.IsSuccessStatusCode; // 삭제 성공
            }
            catch (Exception ex)
            {
                await ShowMessage("오류", $"데이터 삭제 중 오류 발생: {ex.Message}");
                return false;
            }
        }

        public async Task ShowMessage(string title, string content)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                {
                    var dialog = new MessageDialog(content, title);
                    dialog.Commands.Add(new UICommand("OK"));
                    await dialog.ShowAsync();
                });
        }
    }
}
