using SneakersShop.Models;

namespace SneakersShop.Components.Popups;

public partial class CategoryPopup
{
	public IEnumerable<CategoriesModel> Categories { get; }
    public CategoriesModel SelectedCategory { get; set; }

    public CategoryPopup(IEnumerable<CategoriesModel> categories, CategoriesModel selectedCategory)
	{
		InitializeComponent();

        Categories = categories;
        SelectedCategory = selectedCategory;
        BindingContext = this;
    }

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is CategoriesModel selected)
        {
            SelectedCategory.IsSelected = false;
            Close(selected);
        }
    }

    private void OnCancelTapped(object sender, TappedEventArgs e)
    {
        Close();
    }
}