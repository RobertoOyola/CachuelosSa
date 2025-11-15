import './App.css';
import { Navigate, Route, Routes, useNavigate } from 'react-router-dom';
import { useCallback, useEffect, useState } from 'react';
import LoginPage from './pages/LoginPage';
import HomePage from './pages/HomePage';
import { checkAuth, estaTokenExpirado } from './sevices/apis/authServ';
import RegisterPage from './pages/RegisterPage';
import CambiarContrasena from './pages/utils/CambiarContrasena';
import Layout from './pages/utils/Layout ';
import { obtenerInfoToken } from './sevices/apis/userServ';
import { UploadImage } from './pages/utils/UploadImage ';
import PerfilPage from './pages/PerfilPage';
import UploadFile from './pages/utils/UploadFile';
import { toast } from 'react-toastify';

export default function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();


  const handleLogout = useCallback(() => {
    setIsAuthenticated(false);
    navigate("/login");
  }, [navigate]);

  useEffect(() => {
    const verificarToken = async () => {
      try {
        const token = await obtenerInfoToken();

        if (token && !estaTokenExpirado(token)) {
          setIsAuthenticated(true);
          // 🚨 redirige a home si estás en /login
          if (window.location.pathname === '/login') {
            navigate("/home");
          }
        } else {
          const resultado = await checkAuth();
          setIsAuthenticated(resultado);
        }
      } catch (error) {
        console.warn('Error al verificar autenticación', error);
        if (error?.response?.status === 401) {
          toast.warning('Sesión expirada. Inicia sesión otra vez.');
          handleLogout();
        }
      } finally {
        setLoading(false);
      }
    };

    verificarToken();
  }, [navigate, handleLogout]);


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
