import { useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import { RecuperarContrasena } from '../../sevices/apis/authServ'
import './CambiarContrasena.css';

export default function CambiarContrasena() {
    const navigate = useNavigate();
    const location = useLocation();
    const user = location.state?.user;
    const [newPassword, setNewPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        if (!user) {
            toast.error('Realice todo el formulario por favor');
            navigate('/login');
        }
    }, [user, navigate]);

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (newPassword !== confirmPassword) {
            return toast.error('Las contraseñas no coinciden');
        }

        if (newPassword.length <= 6) {
            return toast.error('La contraseña debe tener mas de 6 caracteres');
        }

        try {
            setLoading(true);
            const dataToSend = {
                id: user.id,
                mail: user.correo,
                password: newPassword,
            };
            const response = await RecuperarContrasena(dataToSend);

            if (response?.header?.codigo === 200) {
                toast.success('Contraseña cambiada exitosamente');
                navigate('/login');
            } else {
                toast.error(response?.header?.mensaje);
            }
        } catch (error) {
            toast.error(error?.header?.mensaje);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="cambiar-contrasena-container">
            <h3>Cambiar Contraseña</h3>
            <form onSubmit={handleSubmit} className="cambiar-contrasena-form">
                <div className="form-group">
                    <label htmlFor="newPassword">Nueva Contraseña</label>
                    <input
                        type="password"
                        id="newPassword"
                        className="form-control"
                        value={newPassword}
                        onChange={(e) => setNewPassword(e.target.value)}
                        placeholder="••••••••"
                        required
                    />
                </div>

                <div className="form-group">
                    <label htmlFor="confirmPassword">Confirmar Contraseña</label>
                    <input
                        type="password"
                        id="confirmPassword"
                        className="form-control"
                        value={confirmPassword}
                        onChange={(e) => setConfirmPassword(e.target.value)}
                        placeholder="••••••••"
                        required
                    />
                </div>

                <div className="text-center">
                    <button type="submit" className="btn btn-primary" disabled={loading}>
                        {loading ? 'Cambiando...' : 'Cambiar Contraseña'}
                    </button>
                </div>
            </form>
        </div>
    );
}
