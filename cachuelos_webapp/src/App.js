import './App.css';
import { Navigate, Route, Routes, useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import LoginPage from './pages/LoginPage';
import HomePage from './pages/HomePage';
import { checkAuth } from './sevices/apis/authServ'; // ✅ tu función nueva
import RegisterPage from './pages/RegisterPage';

export default function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [loading, setLoading] = useState(true); // para esperar la verificación
  const navigate = useNavigate();

  useEffect(() => {
    const verificarToken = async () => {
      const resultado = await checkAuth();
      setIsAuthenticated(resultado);
      setLoading(false);
    };
    verificarToken();
  }, []);

  const ProtectedRoute = ({ children }) => {
    if (loading) return <div>Cargando...</div>; // evitar parpadeo

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
    <div className="container mt-4">
      <Routes>
        <Route path="/login" element={<LoginPage onLogin={handleLogin} />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/home"
          element={
            <ProtectedRoute>
              <HomePage onLogout={() => setIsAuthenticated(false)} />
            </ProtectedRoute>
          } />
        <Route path="*" element={<Navigate to={isAuthenticated ? "/home" : "/login"} replace />} />
      </Routes>
    </div>
  );
}
