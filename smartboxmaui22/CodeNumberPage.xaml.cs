namespace smartboxmaui2;

public partial class CodeNumberPage : ContentPage
{
    private FirebaseProperty receivedData;

    public CodeNumberPage(FirebaseProperty data)
    {
        InitializeComponent();
        receivedData = data;

        // UI�� �ʱ�ȭ�� �Ŀ� Label�� �ؽ�Ʈ�� �����մϴ�.
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
