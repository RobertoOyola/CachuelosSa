using AppMovil.Pages;
using Services.Auth;

namespace AppMovil
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //IAuthService authService = MauiProgram.CreateMauiApp().Services.GetRequiredService<IAuthService>();

            // Pasar el servicio manualmente al constructor de LoginPage
            MainPage = new NavigationPage(new LoginPage());
        }

        //protected override Window CreateWindow(IActivationState? activationState)
        //{
        //    return new Window(new AppShell());
        //}
    }
}