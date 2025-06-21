using SneakersShop.MVVM.Models;

namespace SneakersShop.Components;

public partial class SizePopup
{
	public IEnumerable<SizeModel> Sizes { get; set; }
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
            SelectedSize.IsSelected = false;
            Close(selected);
        }
    }

    private void OnCancelTapped(object sender, TappedEventArgs e)
    {
        Close();
    }
}