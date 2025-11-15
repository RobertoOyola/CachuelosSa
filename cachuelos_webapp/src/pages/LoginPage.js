import { useState } from "react";
import { toast } from "react-toastify";
import { login } from "../sevices/apis/authServ";
import OtpModal from "./modals/OtpModal";
import { useNavigate } from "react-router-dom";

export default function LoginPage({ onLogin }) {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [showOtpModal, setShowOtpModal] = useState(false);
  const [showOtpModalV, setShowOtpModalV] = useState(false);

  const navigate = useNavigate();

  const handleSuccess = (response) => {
    navigate(`/cambiar-contrasena`, { state: { user: response.body } });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!email.trim()) return toast.error("Ingrese su email");
    if (!password.trim()) return toast.error("Ingrese su contraseña");

    try {
      const data = await login({ email, password });
      if (data.body?.datos) {
        toast.success("¡Login exitoso!");
        onLogin(data.body.token);
      }
      else if (data.header.codigo === 206) {
        setShowOtpModalV(true)
      }
      else {
        toast.error(data.header?.mensaje || "Credenciales inválidas");
      }
    } catch (error){
      const mensaje = error?.header?.mensaje || "Error al iniciar sesión";
      toast.error(mensaje);
    }
  };

  const verify_login = async () => {
    try {
      const data = await login({ email, password });
      if (data.body?.datos) {
        toast.success("¡Login exitoso!");
        onLogin(data.body.token);
      }
      else {
        toast.error(data.header?.mensaje || "Credenciales inválidas");
      }
    } catch {
      toast.error("Error al iniciar sesión");
    }
  }

  return (
    <div className="d-flex vh-90 container mt-4">
      <div className="d-none d-md-flex flex-column justify-content-center align-items-center flex-grow-1 bg-light position-relative">
        <img
          src="img/login img.png"
          alt="Login illustration"
          className="img-fluid p-5"
          style={{ maxHeight: "600px" }}
        />
      </div>

      <div className="d-flex flex-column justify-content-center align-items-center p-5 bg-white shadow w-100" style={{ maxWidth: 480 }}>
        <div className="w-100">
          <h3 className="mb-3 text-center">Bienvenido a Cachuelos SA</h3>
          <p className="text-muted text-center mb-4">Ingresa a tu cuenta para continuar</p>

          <form onSubmit={handleSubmit} className="d-grid gap-3">
            <div className="form-group">
              <label>Correo</label>
              <input
                type="email"
                className="form-control"
                placeholder="correo@ejemplo.com"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
              />
            </div>

            <div className="form-group">
              <label>Contraseña</label>
              <input
                type="password"
                className="form-control"
                placeholder="••••••••"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
            </div>

            <button type="submit" className="btn btn-primary w-100">
              Iniciar sesión
            </button>

            <div className="text-center mt-3 mb-0">
              <span>¿Nuevo aquí? </span>
              <a href="/register" className="text-decoration-none">Crea una cuenta</a>
            </div>
            <div className="text-center">
              <span>¿Olvidaste la contaseña? </span>
              <a href="#!" onClick={() => setShowOtpModal(true)} className="text-decoration-none text-primary">
                Recuperar Contraseña
              </a>
            </div>
          </form>
        </div>
      </div>
      {showOtpModal && (
        <OtpModal
          onClose={() => setShowOtpModal(false)}
          actionType="CAMBIO_CONTRA"
          email=""
          onSuccess={handleSuccess}
        />
      )}
      {showOtpModalV && (
        <OtpModal
          onClose={() => setShowOtpModalV(false)}
          actionType="VERIFICAR_USU"
          email=""
          onSuccess={verify_login}
        />
      )}
    </div>
  );
}
