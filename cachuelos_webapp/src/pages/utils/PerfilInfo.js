import { AdvancedImage } from '@cloudinary/react';
import './PerfilInfo.css';
import { obtenerFoto } from '../../sevices/apis/docuServ';

export default function PerfilInfo() {
    const img = obtenerFoto('eqqgrxwezptgzq1skfni', 'orig');
    const pdfUrl = 'https://console.cloudinary.com/app/c-c2c84172e34dcf6e9e42c3112a5875/assets/media_library/folders/cc32f0479f0942a372858ccbed9db3e80b/asset/8c111fb2b225d3b41715d06ccf988fa6/manage/summary?view_mode=list&context=manage';

    return (
        <div className="container perfil-container mt-3 p-3">
            <div className="row">
                <div className="col">
                    <h1 className="title">Roberto Oyola Vera</h1>
                </div>
            </div>
            <div className="row">
                <div className="col-lg-6 col-md-12">
                    <h3 className="section-title">Descripción</h3>
                    <h5 className="description">Estudiante de software probando para su tesis</h5>
                    <div style={{ height: '80vh' }}>
                        <a href={pdfUrl} download target="_blank" rel="noopener noreferrer">
                            Descargar PDF
                        </a>
                    </div>
                </div>
                <div className="col-lg-3 col-md-6">
                    <AdvancedImage cldImg={img} className="Fperfil" />
                </div>
                <div className="col-lg-3 col-md-6">
                    <h4 className="section-title">Información Personal</h4>
                    <div className="InfoPerfil">
                        <h5>Nombre de Usuario: royolas</h5>
                        <h5>Correo: royolavera@gmail.com</h5>
                        <h5>Ubicación: Ecuador, Guayas, Guayaquil</h5>
                        <h5>Teléfono: 0992940360</h5>
                        <h5>Edad: 23</h5>
                    </div>
                </div>
            </div>
        </div>
    );
}
