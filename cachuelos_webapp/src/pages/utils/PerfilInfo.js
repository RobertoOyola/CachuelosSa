import { AdvancedImage } from '@cloudinary/react';
import './PerfilInfo.css';
import { obtenerFoto } from '../../sevices/apis/docuServ';

export default function PerfilInfo() {
    const img = obtenerFoto('eqqgrxwezptgzq1skfni', 'orig');
    const pdfUrl = 'https://console.cloudinary.com/app/c-c2c84172e34dcf6e9e42c3112a5875/assets/media_library/folders/cc32f0479f0942a372858ccbed9db3e80b/asset/8c111fb2b225d3b41715d06ccf988fa6/manage/summary?view_mode=list&context=manage';

    return (
        <div className="container perfil-container mt-3 p-3">
            <div className="row justify-content-between d-flex flex-md-row flex-column align-items-start gap-2">
                <div className='col-auto'>
                    <button className='btn btn-secondary btn-sm btn-actualizar'>
                        Actualizar Perfil
                    </button>
                </div>
                <div className='col-auto'>
                    <button className='btn btn-secondary btn-sm btn-actualizar'>
                        Actualizar Foto
                    </button>
                </div>
            </div>

            <div className="row align-items-center text-center p-2">
                <div className="col">
                    <h1 className="title mt-md-0 mt-3">Roberto Donato Oyola Vera</h1>
                </div>
            </div>
            <div className="row">
                <div className="col-lg-5 col-md-12 p-2">
                    <h3 className="section-title">Descripción</h3>
                    <h5 className="description">Dolor esse pariatur eu et sunt</h5>
                    <div style={{}}>
                        <a href={pdfUrl} download target="_blank" rel="noopener noreferrer">
                            Descargar PDF
                        </a>
                    </div>
                </div>
                <div className="col-lg-3 col-md-6 text-center p-2">
                    <AdvancedImage cldImg={img} className="Fperfil" />
                </div>
                <div className="col-lg-4 col-md-6 p-2">
                    <h4 className="section-title">Información Personal</h4>
                    <div className="InfoPerfil">
                        <h5 className='text-break'>Nombre de Usuario: royolas</h5>
                        <h5 className='text-break'>Correo: royolavera@gmail.com</h5>
                        <h5 className='text-break'>Ubicación: Ecuador, Guayas, Guayaquil</h5>
                        <h5 className='text-break'>Teléfono: 0992940360</h5>
                        <h5 className='text-break'>Edad: 23</h5>
                    </div>
                </div>
            </div>
        </div>
    );
}
