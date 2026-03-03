import { AdvancedImage } from "@cloudinary/react";
import "./PerfilInfo.css";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import UserInfoModal from "../modals/UpdateUserModal.js";
import {
    ObtenerDocumentosUsuario,
    ObtenerDocumentosUsuarioxId,
    obtenerFoto,
    uploadPhoto,
} from "../../sevices/apis/docuServ";
import {
    CambiarFotoUsuario,
    obtenerInfoToken,
    obtenerOtrosUser,
} from "../../sevices/apis/userServ.js";
import IframeModal from "../modals/IframeModal.js";
import { toast } from "react-toastify";

export default function PerfilInfo() {
    const { id } = useParams(); // ← ID del usuario en la URL
    const [showModal, setShowModal] = useState(false);
    const [showDocumentModal, setShowDocumentModal] = useState(false);
    const [ownPerfil, setOwnPerfil] = useState(false);
    const [infoDto, setInfoDto] = useState(null);
    const [imagen, setImagen] = useState(null);
    const [documentos, setDocumentos] = useState(null);
    const [documentoSeleccionado, setDocumentoSeleccionado] = useState(null);
    const [uploading, setUploading] = useState(false);

    useEffect(() => {
        cargarPerfil();
    }, [id]);

    const handleOpenModal = () => setShowModal(true);
    const handleCloseModal = async () => {
        setShowModal(false);
        await cargarPerfil();
    };

    const handleCloseDocumentModal = () => setShowDocumentModal(false);

    const cargarPerfil = async () => {
        try {
            const response = await obtenerOtrosUser({ id: parseInt(id) });
            const userData = response?.body;

            if (!userData) {
                console.error("No se recibió data del usuario");
                return;
            }

            const responseDocs = await ObtenerDocumentosUsuarioxId({ id: parseInt(id) });

            if (responseDocs?.header?.codigo === 200) {
                setDocumentos(responseDocs.body);
            }

            setInfoDto(userData);

            const urlImg = userData.usuarioInfoDto?.urlImg;
            const imagenFinal =
                urlImg && urlImg.trim() !== ""
                    ? obtenerFoto(urlImg, "orig")
                    : obtenerFoto("default_zwneaa", "orig");

            setImagen(imagenFinal);

            const token = await obtenerInfoToken();
            if (token && userData.usuarioDto?.id === parseInt(token.Id)) {
                setOwnPerfil(true);
            }
        } catch (error) {
            console.error("Error al cargar perfil:", error);
        }
    };

    const handleImageChange = async (e) => {
        const file = e.target.files[0];
        if (!file) return;

        try {
            setUploading(true);
            const imgId = await uploadPhoto(file);

            if (!imgId) {
                toast.error("Error al subir imagen");
                return;
            }
            const response = await CambiarFotoUsuario(imgId);

            if (response?.header?.codigo === 201) {
                toast.success("Foto actualizada correctamente");
                const nuevaImagen = obtenerFoto(imgId, "orig");
                setImagen(nuevaImagen);
            } else {
                toast.error("No se pudo actualizar la foto");
            }
        } catch (error) {
            toast.error("Error al actualizar foto");
        } finally {
            setUploading(false);
        }
    };

    const handleOpenDocumentModal = (url) => {
        if (!url) return;
        setDocumentoSeleccionado(url);
        setShowDocumentModal(true);
    };

    if (!infoDto) return <div className="container mt-4">Cargando perfil...</div>;

    return (
        <>
            <div className="container perfil-container mt-3 p-3">
                {ownPerfil && (
                    <div className="row justify-content-between align-items-center mb-3">
                        <div className="col-auto">
                            <button
                                className="btn btn-secondary btn-sm btn-actualizar"
                                onClick={handleOpenModal}
                            >
                                Actualizar Perfil
                            </button>
                        </div>
                        <div className="col-auto">
                            <div className="col-auto">
                                <input
                                    type="file"
                                    accept="image/*"
                                    onChange={handleImageChange}
                                    style={{ display: "none" }}
                                    id="inputFoto"
                                />

                                <label
                                    htmlFor="inputFoto"
                                    className="btn btn-success btn-sm"
                                >
                                    {uploading ? "Actualizando..." : "Actualizar Foto"}
                                </label>
                            </div>
                        </div>
                    </div>
                )}

                {/* NOMBRE */}
                <div className="row text-center mb-4">
                    <div className="col">
                        <h1 className="title">
                            {infoDto.usuarioInfoDto?.nombre}{" "}
                            {infoDto.usuarioInfoDto?.apellido}
                        </h1>
                    </div>
                </div>

                <div className="row">
                    <div className="col-lg-5 col-md-12 p-2">
                        <h3 className="section-title mb-2">Descripción</h3>

                        <p className="description mb-3 text-justify">
                            {" "}
                            {infoDto.usuarioInfoDto?.descripcion || "Sin descripción"}
                        </p>

                        <div className="row justify-content-center">
                            {/* TITULO */}
                            <button
                                className="col-sm-4 col-md-3 col-lg-2 btn btn-secondary btn-sm btn-actualizar m-1"
                                disabled={!documentos?.titulos?.length}
                                onClick={() =>
                                    handleOpenDocumentModal(
                                        documentos?.titulos?.[0]?.urlDocumento,
                                    )
                                }
                            >
                                Abrir Título
                            </button>

                            {/* CURRICULUM */}
                            <button
                                className="col-sm-4 col-md-3 col-lg-2 btn btn-secondary btn-sm btn-actualizar m-1"
                                disabled={!documentos?.curriculum?.urlDocumento}
                                onClick={() =>
                                    handleOpenDocumentModal(documentos?.curriculum?.urlDocumento)
                                }
                            >
                                Abrir Curriculum
                            </button>

                            {/* HISTORIAL POLICIAL */}
                            <button
                                className="col-sm-4 col-md-3 col-lg-2 btn btn-secondary btn-sm btn-actualizar m-1"
                                disabled={!documentos?.historialPolicial?.urlDocumento}
                                onClick={() =>
                                    handleOpenDocumentModal(
                                        documentos?.historialPolicial?.urlDocumento,
                                    )
                                }
                            >
                                Abrir Historial Policial
                            </button>
                        </div>
                    </div>

                    {/* FOTO */}
                    <div className="col-lg-3 col-md-6 text-center p-2">
                        {imagen ? (
                            <AdvancedImage
                                cldImg={imagen}
                                className="Fperfil"
                                alt="Foto de perfil"
                            />
                        ) : (
                            <div className="Fperfil-placeholder">Sin imagen</div>
                        )}
                    </div>

                    {/* INFORMACIÓN GENERAL */}
                    <div className="col-lg-4 col-md-6 p-2">
                        <h4 className="section-title mb-3">Información General</h4>

                        <div className="InfoPerfil card shadow-sm border-0 p-3">
                            <div className="d-flex justify-content-between mb-2">
                                <span className="text-muted">Usuario</span>
                                <span className="fw-semibold text-break">
                                    {infoDto.usuarioDto?.nombreUsuario}
                                </span>
                            </div>

                            <div className="d-flex justify-content-between mb-2">
                                <span className="text-muted">Correo </span>
                                <span className="fw-semibold text-break">
                                    {infoDto.usuarioDto?.correo}
                                </span>
                            </div>

                            <div className="d-flex justify-content-between mb-2">
                                <span className="text-muted">Ubicación</span>
                                <span className="fw-semibold">
                                    {infoDto?.ubicacion || "No especificado"}
                                </span>
                            </div>

                            <div className="d-flex justify-content-between mb-2">
                                <span className="text-muted">Teléfono</span>
                                <span className="fw-semibold">
                                    {infoDto.usuarioInfoDto?.telefono || "No especificado"}
                                </span>
                            </div>

                            <div className="d-flex justify-content-between">
                                <span className="text-muted">Edad</span>
                                <span className="fw-semibold">{infoDto?.edad || "-"}</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <UserInfoModal show={showModal} onClose={handleCloseModal} />

            <IframeModal
                show={showDocumentModal}
                onClose={handleCloseDocumentModal}
                title={'Documento'}
                src={documentoSeleccionado}
            />
        </>
    );
}
