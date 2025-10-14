namespace SneakersShop.Components.Popups;

public partial class MessagePopup
{
    public string Title { get; set; }
    public string Message { get; set; }

    public MessagePopup(string title, string message)
	{
		InitializeComponent();

        Message = message;
        Title = title;
        BindingContext = this;
    }

    private void Cancel_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        Close();
    }
}