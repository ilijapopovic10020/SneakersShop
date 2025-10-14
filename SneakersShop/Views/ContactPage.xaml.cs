using Microsoft.Maui.Platform;

namespace SneakersShop.Views;

public partial class ContactPage : ContentPage
{
    public ContactPage()
	{
		InitializeComponent();

        Map.Source = new HtmlWebViewSource
        {
            Html = @"
<!DOCTYPE html>
<html>
<head>
<meta name='viewport' content='width=device-width, initial-scale=1.0'>
<style>html, body { height: 100%; margin: 0; padding: 0; } iframe { width: 100%; height: 100%; border: 0; }</style>
</head>
<body>
<iframe src='https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d2830.5785308359914!2d20.41406921553529!3d44.8238465790986!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x475a6570419ea04f%3A0xa9ceffa4d601e40c!2sBulevar%20Mihajla%20Pupina%2010b%2C%20Beograd!5e0!3m2!1sen!2srs!4v1696690369995!5m2!1sen!2srs' allowfullscreen='' loading='lazy'></iframe>
</body>
</html>"
        };
    }

    //private string LoadHtmlFromResource(string filename)
    //{
    //    using var stream = FileSystem.OpenAppPackageFileAsync(filename).Result;
    //    using var reader = new StreamReader(stream);
    //    return reader.ReadToEnd();
    //}
}