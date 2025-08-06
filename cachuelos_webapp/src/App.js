import './App.css';
import { Navigate, Route, Routes, useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import LoginPage from './pages/LoginPage';
import HomePage from './pages/HomePage';
import { checkAuth } from './sevices/apis/authServ';
import RegisterPage from './pages/RegisterPage';
import CambiarContrasena from './pages/utils/CambiarContrasena';
import Layout from './pages/utils/Layout ';
import { obtenerInfoToken } from './sevices/apis/userServ';
import { UploadImage } from './pages/utils/UploadImage ';
import PerfilPage from './pages/PerfilPage';
import UploadFile from './pages/utils/UploadFile';

export default function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    const verificarToken = async () => {
      const token = await obtenerInfoToken();
      if (token != null) {
        setIsAuthenticated(true);
        setLoading(false);
      } else {
        const resultado = await checkAuth();
        setIsAuthenticated(resultado);
        setLoading(false);
      }
    };
    verificarToken();
  }, [navigate]);

  const ProtectedRoute = ({ children }) => {
    if (loading) return <div>Cargando...</div>;

    if (!isAuthenticated) {
      return <Navigate to="/login" replace />;
    }
    return children;
  };

  const handleLogin = () => {
    setIsAuthenticated(true);
    navigate("/home");
  };

  const handleLogout = () => {
    setIsAuthenticated(false);
    navigate("/login");
  };

  return (
    <div>
      <Routes>
        <Route path="/login" element={<LoginPage onLogin={handleLogin} />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/cambiar-contrasena" element={<CambiarContrasena />} />
        <Route path="/prueba-img" element={<UploadImage />} />
        <Route path="/prueba-file" element={<UploadFile />} />
        {/* Ruta protegida */}
        <Route element={<ProtectedRoute><Layout onLogout={handleLogout} /></ProtectedRoute>}>
          <Route path="/home" element={<HomePage onLogout={() => setIsAuthenticated(false)} />} />
          <Route path="/perfil/:id" element={<PerfilPage />} />
        </Route>  

        {/* Redirección para cualquier ruta no definida */}
        <Route path="*" element={<Navigate to={isAuthenticated ? "/home" : "/login"} replace />} />
      </Routes>
    </div>
  );
}
