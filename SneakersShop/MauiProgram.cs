using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SneakersShop.Helpers;
using SneakersShop.Services.Implementations;
using SneakersShop.Services.Interfaces;
using UraniumUI;
using SneakersShop.ViewModels;
using SneakersShop.Validators.Interfaces;
using SneakersShop.Views;
using SneakersShop.Validators.Implementations;

namespace SneakersShop
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseUraniumUI()
                .UseUraniumUIMaterial()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            //DI

            #region Services
            builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
            {
                client.BaseAddress = new Uri(AppConstants.API_URL);
            });
            builder.Services.AddHttpClient<IBrandService, BrandService>(client =>
            {
                client.BaseAddress = new Uri(AppConstants.API_URL);
            });
            builder.Services.AddHttpClient<IProductsService, ProductService>(client =>
            {
                client.BaseAddress = new Uri(AppConstants.API_URL);
            });
            builder.Services.AddHttpClient<IReviewService, ReviewService>(client =>
            {
                client.BaseAddress = new Uri(AppConstants.API_URL);
            });
            builder.Services.AddHttpClient<IFavoriteService, FavoriteService>(client =>
            {
                client.BaseAddress = new Uri(AppConstants.API_URL);
            });
            builder.Services.AddHttpClient<ICategoryService, CategoryService>(client =>
            {
                client.BaseAddress = new Uri(AppConstants.API_URL);
            });
            builder.Services.AddHttpClient<IColorService, ColorService>(client =>
            {
                client.BaseAddress = new Uri(AppConstants.API_URL);
            });
            builder.Services.AddHttpClient<ICityService, CityService>(client =>
            {
                client.BaseAddress = new Uri(AppConstants.API_URL);
            });
            builder.Services.AddHttpClient<IUserService, UserService>(client =>
            {
                client.BaseAddress = new Uri(AppConstants.API_URL);
            });
            builder.Services.AddHttpClient<IAddressService, AddressService>(client =>
            {
                client.BaseAddress = new Uri(AppConstants.API_URL);
            });
            builder.Services.AddHttpClient<IOrderService, OrderService>(client =>
            {
                client.BaseAddress = new Uri(AppConstants.API_URL);
            });
            builder.Services.AddHttpClient<ICartService, CartService>(client =>
            {
                client.BaseAddress = new Uri(AppConstants.API_URL);
            });
            builder.Services.AddHttpClient<IPasswordUpdateService, PasswordUpdateService>(client =>
            {
                client.BaseAddress = new Uri(AppConstants.API_URL);
            });
            #endregion

            #region ViewModels
            builder.Services.AddTransient<AuthViewModel>();
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<ProductViewModel>();
            builder.Services.AddTransient<ProductsViewModel>();
            builder.Services.AddTransient<FiltersViewModel>();
            builder.Services.AddTransient<RegisterViewModel>();
            builder.Services.AddTransient<ProfileViewModel>();
            builder.Services.AddTransient<ProfileUpdateViewModel>();
            builder.Services.AddTransient<AddressesViewModel>();
            builder.Services.AddTransient<AddressUpdateViewModel>();
            builder.Services.AddTransient<AddressCreateViewModel>();
            builder.Services.AddTransient<ReviewsViewModel>();
            builder.Services.AddTransient<ReviewCreateViewModel>();
            builder.Services.AddTransient<FavoritesViewModel>();
            builder.Services.AddTransient<OrdersViewModel>();
            builder.Services.AddTransient<OrderViewModel>();
            builder.Services.AddTransient<CartViewModel>();
            builder.Services.AddTransient<CheckoutViewModel>();
            builder.Services.AddTransient<PasswordUpdateViewModel>();
            #endregion

            #region Validators
            builder.Services.AddSingleton<IAuthViewModelValidator, AuthViewModelValidator>();
            builder.Services.AddSingleton<IRegisterViewModelValidator, RegisterViewModelValidator>();
            builder.Services.AddSingleton<IUpdateUserViewModelValidator, UpdateUserViewModelValidator>();
            builder.Services.AddSingleton<ICheckoutViewModelValidator, CheckoutViewModelValidator>();
            builder.Services.AddSingleton<IPasswordUpdateViewModelValidator, PasswordUpdateViewModelValidator>();
            #endregion

            #region Views
            builder.Services.AddTransient<WelcomePage>();

            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegisterPage>();

            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<CartPage>();
            builder.Services.AddTransient<ProductsPage>();
            builder.Services.AddTransient<FavoritesPage>();
            builder.Services.AddTransient<ProfilePage>();

            builder.Services.AddTransient<ProductPage>();
            builder.Services.AddTransient<FiltersPage>();
            builder.Services.AddTransient<ProfileUpdatePage>();
            builder.Services.AddTransient<AddressesPage>();
            builder.Services.AddTransient<OrdersPage>();
            builder.Services.AddTransient<OrderPage>();
            builder.Services.AddTransient<PasswordUpdatePage>();
            builder.Services.AddTransient<ContactPage>();
            builder.Services.AddTransient<PolicyAndPrivacyPage>();
            builder.Services.AddTransient<AddressUpdatePage>();
            builder.Services.AddTransient<AddressCreatePage>();
            builder.Services.AddTransient<ReviewsPage>();
            builder.Services.AddTransient<ReviewCreatePage>();
            builder.Services.AddTransient<CheckoutPage>();
            builder.Services.AddTransient<OrderSuccessPage>();
            #endregion

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
