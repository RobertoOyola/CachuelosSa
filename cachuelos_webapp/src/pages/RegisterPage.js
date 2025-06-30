import { useState } from "react";
import { toast } from "react-toastify";
import { Link, useNavigate } from "react-router-dom";
import { Register } from "../sevices/apis/authServ";

export default function RegisterPage() {
  const [form, setForm] = useState({
    nombreUsuario: "",
    correo: "",
    contrasenaHash: "",
  });

  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    const { nombreUsuario, correo, contrasenaHash } = form;
    if (!nombreUsuario.trim() || !correo.trim() || !contrasenaHash.trim()) {
      toast.error("Completa todos los campos");
      return;
    }
    
    try {
            const data = await Register(form);
            if (data.body) {
                toast.success("Registro exitoso!");
                navigate('/login');
            } else {
                toast.error(data.header?.mensaje);
            }
        } catch (error) {
            toast.error("Error al registrar" || error.mensaje );
        }
    };

  return (
    <div className="d-flex vh-95">
      {/* Lado izquierdo con imagen (oculto en móviles) */}
      <div className="d-none d-md-flex flex-column justify-content-center align-items-center flex-grow-1 bg-light position-relative">
        <img
          src="img/register img.png"
          alt="register-illustration"
          className="img-fluid p-5"
          style={{ maxHeight: "700px" }}
        />
      </div>

      {/* Lado derecho con formulario */}
      <div className="d-flex flex-column justify-content-center align-items-center p-5 bg-white shadow w-100" style={{ maxWidth: 480 }}>
        <div className="w-100" style={{ maxWidth: "400px" }}>
          <h4 className="mb-2">🚀 Comienza a trabajar con nosotros</h4>
          <p className="mb-4 text-muted">Make your app management easy and fun!</p>

          <form onSubmit={handleSubmit} className="needs-validation" noValidate>
            <div className="mb-3">
              <label className="form-label">Nombre de usuario</label>
              <input
                type="text"
                className="form-control"
                value={form.nombreUsuario}
                onChange={(e) => setForm({ ...form, nombreUsuario: e.target.value })}
                required
              />
            </div>

            <div className="mb-3">
              <label className="form-label">Correo</label>
              <input
                type="correo"
                className="form-control"
                value={form.correo}
                onChange={(e) => setForm({ ...form, correo: e.target.value })}
                required
              />
            </div>

            <div className="mb-3">
              <label className="form-label">Contraseña</label>
              <input
                type="contrasenaHash"
                className="form-control"
                value={form.contrasenaHash}
                onChange={(e) => setForm({ ...form, contrasenaHash: e.target.value })}
                required
              />
            </div>

            <button type="submit" className="btn btn-primary w-100 mb-3">
              Registrarse
            </button>

            <p className="text-center">
              ¿Ya tienes cuenta? <Link to="/login">Inicia sesión</Link>
            </p>
          </form>

        </div>
      </div>
    </div>
  );
}
