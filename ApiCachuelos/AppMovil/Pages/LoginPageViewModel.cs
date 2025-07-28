using Entitys.CachuelosSA;
using Entitys.Entitys;
using Entitys.Entitys.Auth;
using Services.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMovil.Pages
{
    public class LoginPageViewModel : INotifyPropertyChanged
    {
        private readonly IAuthService _authService;
		public LoginPageViewModel(IAuthService authService)
		{
			_authService = authService;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		// Método para llamar a Login en el servicio
		public async Task<ServiceResult<Usuario>> Login(Login loginData)
		{
			ServiceResult<Usuario> result = await _authService.Login(loginData);
			return result;
		}

		// Método para generar el token a partir del servicio
		public string GenerarToken(Usuario usuario)
		{
			string token = _authService.GenerarToken(usuario);
			return token;
		}
	}
}
