namespace smartboxmaui2;

public partial class CodeNumberPage : ContentPage
{
    private FirebaseProperty receivedData;

    public CodeNumberPage(FirebaseProperty data)
    {
        InitializeComponent();
        receivedData = data;

        // UI가 초기화된 후에 Label의 텍스트를 설정합니다.
        if (receivedData.BoxNumber.Equals("1") == true) {
            CodeNumber1.Text = receivedData.Password;
        }
        else if (receivedData.BoxNumber.Equals("2") == true){
            CodeNumber2.Text = receivedData.Password;
        }
        else if(receivedData.BoxNumber.Equals("3") == true) {
            CodeNumber3.Text = receivedData.Password;
        }
        else
        {
            CodeNumber4.Text = receivedData.Password;
        }
    }
}
