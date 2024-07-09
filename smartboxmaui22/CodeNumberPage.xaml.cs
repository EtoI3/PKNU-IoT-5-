
namespace smartboxmaui2;

public partial class CodeNumberPage : ContentPage
{
    private List<FirebaseProperty> receivedDataList;

    public CodeNumberPage(List<FirebaseProperty> dataList)
    {
        InitializeComponent();
        receivedDataList = dataList;


        Device.BeginInvokeOnMainThread(() =>
        {
            foreach (var data in receivedDataList)
            {
                switch (data.BoxNumber)
                {
                    case "1":
                        CodeNumber1.Text = data.Password;
                        break;
                    case "2":
                        CodeNumber2.Text = data.Password;
                        break;
                    case "3":
                        CodeNumber3.Text = data.Password;
                        break;
                    case "4":
                        CodeNumber4.Text = data.Password;
                        break;
                    default:
                        // 예외 처리 또는 로그 추가
                        break;
                }
            }
        });

    }
}
