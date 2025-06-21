using CommunityToolkit.Maui.Views;

namespace SneakersShop.Components;

public partial class MessagePopup
{
	public string Message { get; }
    public string Title { get; }
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