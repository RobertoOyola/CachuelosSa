using Azure;
using Entitys.CachuelosSA;
using Entitys.Entitys;
using Entitys.Entitys.Auth;
using Newtonsoft.Json;
using Services.Auth;
using System.Text;
using System.Threading.Tasks;

namespace AppMovil.Pages;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
	}

    private void OnLabelTapped(object sender, EventArgs e)
    {

    }

    private async void LogginButton(object sender, EventArgs e)
    {
        //if (_authService == null)
        //{
        //    await DisplayAlert("Error", "Servicio de autenticación no disponible", "Ok");
        //    return;
        //}

        if (correoLogin.Text == null || correoLogin.Text == "")
        {
            DisplayAlert("Falta de Informacion", "Falta de ingresar el correo", "Ok");
        }
        else if (contraLogin.Text == null || contraLogin.Text == "")
        {
            DisplayAlert("Falta de Informacion", "Falta de ingresar la contraseña", "Ok");
        }
        else
        {
            Login loginInfo = new Login()
            {
                email = correoLogin.Text,
                password = contraLogin.Text,
            };

            try
            {
                //ServiceResult<Usuario> result = await _authService.Login(loginInfo);

                //if (result.Exitoso)
                //{
                //    string token = _authService.GenerarToken(result.Datos);
                //    await DisplayAlert("Bienvenido", $"Login con Éxito. Bienvenido: {token}", "Ok");
                //}
                //else
                //{
                //    await DisplayAlert("Error", $"Algo ocurrió al iniciar sesión: {result.Mensaje}", "Ok");
                //}
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }

        }

    }
}