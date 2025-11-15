import { useNavigate } from 'react-router-dom'; // Importa el hook para navegar
import { toast } from 'react-toastify';
import './Header.css';
import { obtenerInfoToken } from '../../sevices/apis/userServ';
import { useEffect, useState, useCallback } from 'react'; // Importa useCallback
import { logout } from '../../sevices/apis/authServ';
import { obtenerFoto } from '../../sevices/apis/docuServ';
import { AdvancedImage } from '@cloudinary/react';

export default function Header({ onLogout }) {
    const navigate = useNavigate();

    const [userInfo, setUserInfo] = useState({
        nombre_usuario: 'A',
        imagen_url: '',
        Id: 0
    });

    const setInfo = useCallback(async () => {
        try {
            const data = await obtenerInfoToken();
            if (data != null) {
                setUserInfo({
                    nombre_usuario: data.UserName,
                    imagen_url: data.ImgPerfil,
                    Id: data.Id
                });
            } else {
                navigate('/login');
            }
        } catch (error) {
            toast.error("Error al obtener la información del usuario");
        }
    }, [navigate]);

    const handleLogout = async () => {
        try {
            const data = await logout();
            if (data.header.codigo === 200) {
                toast.success("Logout Exitoso!");
                onLogout();
                navigate('/login');
            } else {
                toast.error(data.header.mensaje);
            }
        } catch (error) {
            toast.error("Error al cerrar sesión");
            navigate('/login');
        }
    };

    const Perfil = () => {
        navigate(`/perfil/${userInfo.Id}`)
    }

    useEffect(() => {
        setInfo();
    }, [setInfo]);

    const myImage = obtenerFoto(userInfo.imagen_url, 'icon');

    const handleHomeClick = () => {
        navigate('/home');
    };

    return (
        <header className="header">
            <div className="header-content">
                {/* Logo */}
                <div className="logo-section">
                    <div className="logo" onClick={handleHomeClick}>
                        <span className="logo-text">Cachuelos</span>
                        <span className="logo-accent">SA</span>
                    </div>
                </div>

                {/* Auth/User */}
                <div className="auth-buttons">
                    <div
                        style={{ display: 'flex', alignItems: 'center', gap: '0.5rem', cursor: 'pointer' }}
                        onClick={Perfil}
                        title="Ver perfil"
                    >
                        <div
                            className="avatar"
                            style={{ width: '32px', height: '32px', borderRadius: '50%', backgroundColor: '#ddd' }}
                        >
                            {userInfo.imagen_url ? (
                                <AdvancedImage cldImg={myImage} style={{ width: '100%', height: '100%', borderRadius: '50%' }} />
                            ) : (
                                userInfo.nombre_usuario[0].toUpperCase()
                            )}
                        </div>
                        <span style={{ fontWeight: '500', color: '#333' }}>{userInfo.nombre_usuario}</span>
                    </div>
                </div>


                <div>
                    <button
                        className='btn'
                        onClick={() => { handleLogout() }}>
                        Cerrar Session
                    </button>
                </div>
            </div>
        </header>
    );
}
