//using Android.Graphics.Drawables;
//using Android.Views;
//using Google.Android.Material.BottomNavigation;
//using Microsoft.Maui.Controls.Handlers.Compatibility;
//using Microsoft.Maui.Controls.Platform;
//using Microsoft.Maui.Controls.Platform.Compatibility;
//using Microsoft.Maui.Platform;
//using static Android.Views.ViewGroup;

//namespace SneakersShop.Platforms.Android;

//internal class RoundedFloatingTabbarHandler : ShellRenderer
//{
//    protected override IShellBottomNavViewAppearanceTracker CreateBottomNavViewAppearanceTracker(ShellItem shellItem)
//    {
//        return new RoundedFloatingBottomNavViewAppearanceTracker(this, shellItem);
//    }
//}

//internal class RoundedFloatingBottomNavViewAppearanceTracker : ShellBottomNavViewAppearanceTracker
//{
//    public RoundedFloatingBottomNavViewAppearanceTracker(IShellContext shellContext, ShellItem shellItem) : base(shellContext, shellItem)
//    {
//    }


//    public override void SetAppearance(BottomNavigationView bottomView, IShellAppearanceElement appearance)
//    {
//        //base.SetAppearance(bottomView, appearance);

//        //var tabbarDrawable = new GradientDrawable();

//        ////tabbarDrawable.SetCornerRadii([50, 50, 50, 50, 0, 0, 0, 0]);
//        ////tabbarDrawable.SetColor(Colors.Blue.ToPlatform());

//        //tabbarDrawable.SetCornerRadius(50);

//        //tabbarDrawable.SetColor(appearance.EffectiveTabBarBackgroundColor.ToPlatform());

//        //bottomView.SetBackground(tabbarDrawable);

//        //ViewGroup.LayoutParams layaoutParams = bottomView.LayoutParameters;

//        //if (layaoutParams is MarginLayoutParams marginLayoutParams)
//        //{
//        //    marginLayoutParams.SetMargins(40, 0, 40, 40);
//        //    bottomView.LayoutParameters = marginLayoutParams;
//        //}
//    }
//}