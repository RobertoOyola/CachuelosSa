import { AdvancedImage } from '@cloudinary/react';
import './PerfilInfo.css';
import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import UserInfoModal from '../modals/UpdateUserModal.js';
import { obtenerFoto } from '../../sevices/apis/docuServ';
import { obtenerInfoToken, obtenerOtrosUser } from '../../sevices/apis/userServ.js';
import IframeModal from '../modals/IframeModal.js';

export default function PerfilInfo() {
    const { id } = useParams(); // ← ID del usuario en la URL
    const [showModal, setShowModal] = useState(false);
    const [showDocumentModal, setShowDocumentModal] = useState(false);
    const [ownPerfil, setOwnPerfil] = useState(false);
    const [infoDto, setInfoDto] = useState(null);
    const [imagen, setImagen] = useState(null);

    useEffect(() => {
        const cargarPerfil = async () => {
            try {
                const response = await obtenerOtrosUser({ id: parseInt(id) });
                const userData = response?.body;

                if (!userData) {
                    console.error('No se recibió data del usuario');
                    return;
                }

                setInfoDto(userData);
                
                const urlImg = userData.usuarioInfoDto?.urlImg;
                const imagenFinal = urlImg && urlImg.trim() !== ''
                    ? obtenerFoto(urlImg, 'orig')
                    : obtenerFoto('default_zwneaa', 'orig');

                setImagen(imagenFinal);

                const token = await obtenerInfoToken();
                if (token && userData.usuarioDto?.id === parseInt(token.Id)) {
                    setOwnPerfil(true);
                }
            } catch (error) {
                console.error('Error al cargar perfil:', error);
            }
        };

        cargarPerfil();
    }, [id]);
    const handleOpenModal = () => setShowModal(true);
    const handleCloseModal = () => setShowModal(false);
    const handleOpenDocumentModal = () => setShowDocumentModal(true);
    const handleCloseDocumentModal = () => setShowDocumentModal(false);

    const pdfUrl = 'https://res.cloudinary.com/dkbtvl4oz/raw/upload/v1754587194/nx48iixlm2atcarw3stz.pdf';

    if (!infoDto) return <div className="container mt-4">Cargando perfil...</div>;

    return (
        <>
            <div className="container perfil-container mt-3 p-3">
                {ownPerfil && (
                    <div className="row justify-content-between d-flex flex-md-row flex-column align-items-start gap-2">
                        <div className='col-auto'>
                            <button className='btn btn-secondary btn-sm btn-actualizar' onClick={handleOpenModal}>
                                Actualizar Perfil
                            </button>
                        </div>
                        <div className='col-auto'>
                            <button className='btn btn-secondary btn-sm btn-actualizar'>
                                Actualizar Foto
                            </button>
                        </div>
                    </div>
                )}

                <div className="row align-items-center text-center p-2">
                    <div className="col">
                        <h1 className="title mt-md-0 mt-3">{infoDto.usuarioInfoDto.nombre} {infoDto.usuarioInfoDto.apellido}</h1>
                    </div>
                </div>

                <div className="row">
                    <div className="col-lg-5 col-md-12 p-2">
                        <h3 className="section-title">Descripción</h3>
                        <h5 className="description">{infoDto.usuarioInfoDto?.descripcion || 'Sin descripción'}</h5>
                        <div className='container text-center'>
                            <div className='container text-center'>
                                <div className='row justify-content-center'>
                                    <button className='col-sm-4 col-md-3 col-lg-2 btn btn-secondary btn-sm btn-actualizar m-1' onClick={handleOpenDocumentModal}>
                                        Abrir Titulo
                                    </button>
                                    <button className='col-sm-4 col-md-3 col-lg-2 btn btn-secondary btn-sm btn-actualizar m-1' onClick={handleOpenDocumentModal}>
                                        Abrir Curriculum
                                    </button>
                                    <button className='col-sm-4 col-md-3 col-lg-2 btn btn-secondary btn-sm btn-actualizar m-1' onClick={handleOpenDocumentModal}>
                                        Abrir Historial Policial
                                    </button>
                                </div>
                            </div>

                        </div>
                    </div>

                    <div className="col-lg-3 col-md-6 text-center p-2">
                        {imagen ? (
                            <AdvancedImage cldImg={imagen} className="Fperfil" />
                        ) : (
                            <div className="Fperfil-placeholder">Sin imagen</div>
                        )}
                    </div>


                    <div className="col-lg-4 col-md-6 p-2">
                        <h4 className="section-title">Información Personal</h4>
                        <div className="InfoPerfil">
                            <h5 className='text-break'>Nombre de Usuario: {infoDto.usuarioDto?.nombreUsuario}</h5>
                            <h5 className='text-break'>Correo: {infoDto.usuarioDto?.correo}</h5>
                            <h5 className='text-break'>Ubicación: {infoDto?.ubicacion || 'No especificado'}</h5>
                            <h5 className='text-break'>Teléfono: {infoDto.usuarioInfoDto?.telefono || 'No especificado'}</h5>
                            <h5 className='text-break'>Edad: {infoDto?.edad}</h5>
                        </div>
                    </div>
                </div>
            </div>

            <UserInfoModal show={showModal} onClose={handleCloseModal} />
            <IframeModal
                show={showDocumentModal}
                onClose={handleCloseDocumentModal}
                title={'Documento 1'}
                src={pdfUrl} />
        </>
    );
}
