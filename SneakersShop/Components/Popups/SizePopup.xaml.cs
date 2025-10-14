using SneakersShop.Models;

namespace SneakersShop.Components.Popups;

public partial class SizePopup
{
	public IEnumerable<SizeModel> Sizes { get; }
    public SizeModel SelectedSize { get; set; }
    public SizePopup(IEnumerable<SizeModel> sizes, SizeModel selectedSize)
	{
		InitializeComponent();
		Sizes = sizes;
        SelectedSize = selectedSize;
        BindingContext = this;
    }
    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is SizeModel selected)
        {
            SelectedSize = selected;
            Close(selected);
        }
    }
    private void OnCancelTapped(object sender, TappedEventArgs e)
    {
        Close();
    }
}