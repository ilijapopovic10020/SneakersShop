using SneakersShop.Helpers;

namespace SneakersShop.Components.Popups;

public partial class SortPopup
{
    public IEnumerable<FilterOption> Filters { get; }
    public FilterOption SelectedFilter { get; set; }
    public SortPopup(IEnumerable<FilterOption> filters, FilterOption selectedFilter)
    {
        InitializeComponent();
        Filters = filters;
        SelectedFilter = selectedFilter;
        BindingContext = this;

    }

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is FilterOption selected)
        {
            SelectedFilter.IsSelected = false;
            Close(selected);
        }
    }

    private void OnCancelTapped(object sender, TappedEventArgs e)
    {
        Close();
    }
}