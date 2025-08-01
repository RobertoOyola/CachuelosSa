import { useNavigate } from 'react-router-dom'; // Importa el hook para navegar
import { toast } from 'react-toastify';
import './Header.css';
import { obtenerInfoToken } from '../../sevices/apis/userServ';
import { useEffect, useState, useCallback } from 'react'; // Importa useCallback
import { logout } from '../../sevices/apis/authServ';
import { cld } from '../../sevices/apis/docuServ';
import { fill } from '@cloudinary/url-gen/actions/resize';
import { AdvancedImage } from '@cloudinary/react';

export default function Header({ onLogout }) {
    const navigate = useNavigate();

    const [userInfo, setUserInfo] = useState({
        nombre_usuario: 'A',
        imagen_url: ''
    });
    
    const setInfo = useCallback(async () => {
        try {
            const data = await obtenerInfoToken();
            if (data != null) {
                setUserInfo({
                    nombre_usuario: data.UserName,
                    imagen_url: data.ImgPerfil
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
            }
        };

    useEffect(() => {
        setInfo();
    }, [setInfo]);

    const myImage = cld.image(userInfo.imagen_url); 
    myImage.resize(fill().width(100).height(100));

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
                    <div style={{ display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
                        <div className="avatar" style={{ width: '32px', height: '32px', borderRadius: '50%', backgroundColor: '#ddd' }}>
                            {userInfo.imagen_url ? (
                                <AdvancedImage cldImg={myImage} style={{ width: '100%', height: '100%', borderRadius: '50%' }}/>
                            ) : (
                                userInfo.nombre_usuario[0].toUpperCase()  // Usamos la primera letra del nombre si no hay imagen
                            )}
                        </div>
                        <span>{userInfo.nombre_usuario}</span>  {/* Muestra el nombre de usuario */}
                    </div>
                </div>

                <div>
                    <button 
                        className='btn'
                        onClick={() => {handleLogout()}}>
                        Cerrar Session
                    </button>
                </div>
            </div>
        </header>
    );
}
