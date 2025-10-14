using SneakersShop.Helpers;

namespace SneakersShop.Controls
{
    public class RatingControl : HorizontalStackLayout
    {
        public static readonly BindableProperty CurrentValueProperty =
             BindableProperty.Create(
                 nameof(CurrentValue),
                 typeof(double),
                 typeof(RatingControl),
                 defaultValue: 0d,
                 propertyChanged: OnRefreshControl);

        public double CurrentValue
        {
            get => (double)GetValue(CurrentValueProperty);
            set => SetValue(CurrentValueProperty, value);
        }

        public static readonly BindableProperty AmountProperty =
          BindableProperty.Create(
              nameof(Amount),
              typeof(int),
              typeof(RatingControl),
              defaultValue: 10,
              propertyChanged: OnRefreshControl);

        public int Amount
        {
            get => (int)GetValue(AmountProperty);
            set => SetValue(AmountProperty, value);
        }

        public static readonly BindableProperty StarSizeProperty =
          BindableProperty.Create(
              nameof(StarSize),
              typeof(double),
              typeof(RatingControl),
              defaultValue: 24d,
              propertyChanged: OnRefreshControl);

        public double StarSize
        {
            get => (double)GetValue(StarSizeProperty);
            set => SetValue(StarSizeProperty, value);
        }

        public static readonly BindableProperty AccentColorProperty =
            BindableProperty.Create(
                nameof(AccentColor),
                typeof(Color),
                typeof(RatingControl),
                defaultValue: Colors.Red,
                propertyChanged: OnRefreshControl);

        public Color AccentColor
        {
            get => (Color)GetValue(AccentColorProperty);
            set => SetValue(AccentColorProperty, value);
        }


        private static void OnRefreshControl(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RatingControl ratingControl)
                ratingControl.UpdateLayout();
        }


        public RatingControl()
        {
            Spacing = -8;
            UpdateLayout();
        }


        private void UpdateLayout()
        {
            Children.Clear();

            var intValue = (int)ClampValue(CurrentValue);
            var decimalPart = CurrentValue - intValue;
            var isHalfStarNeeded = false;

            if (decimalPart > .25)
            {
                if (decimalPart > 0.25 && decimalPart <= .75)
                    isHalfStarNeeded = true;
                else
                    intValue++;
            }

            for (int i = 0; i < Amount; i++)
            {
                if (intValue > i)
                {
                    Add(CreateStarImage(StarState.Full));
                }
                else if (intValue <= i && isHalfStarNeeded)
                {
                    Add(CreateStarImage(StarState.Half));
                    isHalfStarNeeded = false;
                }
                else
                {
                    Add(CreateStarImage(StarState.Empty));
                }
            }
        }

        private Image CreateStarImage(StarState state)
        {
            var imageSource = state switch
            {
                StarState.Empty => "star_outlined.svg",
                StarState.Half => "star_half_stroke.svg",
                StarState.Full => "star_filled.svg",
                _ => "star_outlined.svg",
            };

            return new Image
            {
                Source = imageSource,
                WidthRequest = StarSize,
                HeightRequest = StarSize,
                Margin = new Thickness(2),
            };
        }

        private double ClampValue(double value)
        {
            if (value < 0)
                return 0;

            if (value > Amount)
                return Amount;

            return value;
        }
    }
}
