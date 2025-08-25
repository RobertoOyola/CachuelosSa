import React, { useEffect, useState } from 'react';
import Uploader from '../components/Uploader.tsx';
import { updateDocu } from '../../sevices/apis/docuServ';
import { toast } from 'react-toastify';
import { obtenerUser, updateUser } from '../../sevices/apis/userServ.js';

const UserInfoModal = ({ show, onClose }) => {
    const [formData, setFormData] = useState({
        nombre: '',
        apellido: '',
        fechaNacimiento: '',
        tipoIdentificacion: '',
        identificacion: '',
        estadoCivil: '',
        direccion: '',
        telefono: '',
        ciudad: '',
        provincia: '',
        nacionalidad: '',
        discapacidad: false,
        tipoDiscapacidad: '',
        urlImg: '',
        descripcion: ''
    });
    const [HistorialPolicial, setHistorialPolicial] = useState(null);
    const [Cedula, setCedula] = useState(null);
    const [Curriculum, setCurriculum] = useState(null);
    const [Titulo, setTitulo] = useState(null);

    useEffect(() => {
        const IngresarInfo = async () => {
            const datos = await obtenerUser();
            const usuario = datos.body.usuarioInfoDto;
            const fechaISO = new Date(usuario.fechaNacimiento).toISOString().split('T')[0];
            setFormData({
                ...usuario,
                fechaNacimiento: fechaISO
            });
        };

        IngresarInfo();
    }, []);

    const handleInputChange = (e) => {
        const { name, value, type, checked } = e.target;
        setFormData({
            ...formData,
            [name]: type === 'checkbox' ? checked : value
        });
    };

    const subirDocus = async () => {

        if (HistorialPolicial) {
            try {
                const data = await updateDocu({ urlString: HistorialPolicial, idTipoDocumento: 3 });
                if (data.header) {
                    toast.success(`Historial Policial: ${data.header.mensaje}`)
                }
                else {
                    toast.error(data.header?.mensaje || "Error al guardar el documento Historial Policial");
                }
            } catch {
                toast.error("Error al guardar el documento Historial Policial");
            }
        }
        if (Cedula) {
            try {
                const data = await updateDocu({ urlString: Cedula, idTipoDocumento: 4 });
                if (data.header) {
                    toast.success(`Cedula: ${data.header.mensaje}`)
                }
                else {
                    toast.error(data.header?.mensaje || "Error al guardar el documento Cedula");
                }
            } catch {
                toast.error("Error al guardar el documento Cedula");
            }
        }
        if (Curriculum) {
            try {
                const data = await updateDocu({ urlString: Curriculum, idTipoDocumento: 1 });
                if (data.header) {
                    toast.success(`Curriculum: ${data.header.mensaje}`)
                }
                else {
                    toast.error(data.header?.mensaje || "Error al guardar el documento Curriculum");
                }
            } catch {
                toast.error("Error al guardar el documento Curriculum");
            }
        }
        if (Titulo) {
            try {
                const data = await updateDocu({ urlString: Titulo, idTipoDocumento: 2 });
                if (data.header) {
                    toast.success(`Titulo: ${data.header.mensaje}`)
                }
                else {
                    toast.error(data.header?.mensaje || "Error al guardar el documento Titulo");
                }
            } catch {
                toast.error("Error al guardar el documento Titulo");
            }
        }

        onClose();
    }

    const handleSubmit = async (e) => {
        e.preventDefault();

        const fechaFormateada = new Date(formData.fechaNacimiento).toISOString();

        const payload = {
            ...formData,
            fechaNacimiento: fechaFormateada
        };

        try {
            console.log(payload)
            const data = await updateUser(payload);
            if (data) {
                toast.success(data.header?.mensaje || "Error al guardar el documento Historial Policial")
            }
            else {
                toast.error(data.header?.mensaje || "Error al guardar el documento Historial Policial");
            }
        } catch (error) {
            toast.error(error.message);
        }

        onClose();
    };

    return (
        show && (
            <div className="modal modal-xl show" style={{ display: 'block' }}>
                <div className="modal-dialog">
                    <div className="modal-content">
                        <div className="modal-header row">
                            <h5 className="modal-title col">Ingresar Información del Usuario</h5>
                            <button type="button" className="btn btn-secondary btn-sm col-1" onClick={onClose}>
                                &times;
                            </button>
                        </div>
                        <div className="modal-body row">
                            <form onSubmit={handleSubmit} className='col-7'>
                                <div className="form-group">
                                    <label>Nombre</label>
                                    <input
                                        type="text"
                                        name="nombre"
                                        className="form-control"
                                        value={formData.nombre}
                                        onChange={handleInputChange}
                                        required
                                    />
                                </div>

                                <div className="form-group">
                                    <label>Apellido</label>
                                    <input
                                        type="text"
                                        name="apellido"
                                        className="form-control"
                                        value={formData.apellido}
                                        onChange={handleInputChange}
                                        required
                                    />
                                </div>

                                <div className="form-group">
                                    <label>Fecha de Nacimiento</label>
                                    <input
                                        type="date"
                                        name="fechaNacimiento"
                                        className="form-control"
                                        value={formData.fechaNacimiento}
                                        onChange={handleInputChange}
                                        required
                                    />
                                </div>

                                <div className="form-group">
                                    <label>Tipo de Identificación</label>
                                    <input
                                        type="text"
                                        name="tipoIdentificacion"
                                        className="form-control"
                                        value={formData.tipoIdentificacion}
                                        onChange={handleInputChange}
                                        required
                                    />
                                </div>

                                <div className="form-group">
                                    <label>Identificación</label>
                                    <input
                                        type="text"
                                        name="identificacion"
                                        className="form-control"
                                        value={formData.identificacion}
                                        onChange={handleInputChange}
                                        required
                                    />
                                </div>

                                <div className="form-group">
                                    <label>Estado Civil</label>
                                    <input
                                        type="text"
                                        name="estadoCivil"
                                        className="form-control"
                                        value={formData.estadoCivil}
                                        onChange={handleInputChange}
                                        required
                                    />
                                </div>

                                <div className="form-group">
                                    <label>Dirección</label>
                                    <input
                                        type="text"
                                        name="direccion"
                                        className="form-control"
                                        value={formData.direccion}
                                        onChange={handleInputChange}
                                        required
                                    />
                                </div>

                                <div className="form-group">
                                    <label>Teléfono</label>
                                    <input
                                        type="text"
                                        name="telefono"
                                        className="form-control"
                                        value={formData.telefono}
                                        onChange={handleInputChange}
                                        required
                                    />
                                </div>

                                <div className="form-group">
                                    <label>Ciudad</label>
                                    <input
                                        type="text"
                                        name="ciudad"
                                        className="form-control"
                                        value={formData.ciudad}
                                        onChange={handleInputChange}
                                        required
                                    />
                                </div>

                                <div className="form-group">
                                    <label>Provincia</label>
                                    <input
                                        type="text"
                                        name="provincia"
                                        className="form-control"
                                        value={formData.provincia}
                                        onChange={handleInputChange}
                                        required
                                    />
                                </div>

                                <div className="form-group">
                                    <label>Nacionalidad</label>
                                    <input
                                        type="text"
                                        name="nacionalidad"
                                        className="form-control"
                                        value={formData.nacionalidad}
                                        onChange={handleInputChange}
                                        required
                                    />
                                </div>

                                <div className="form-group">
                                    <label>Discapacidad</label>
                                    <input
                                        type="checkbox"
                                        name="discapacidad"
                                        checked={formData.discapacidad}
                                        onChange={handleInputChange}
                                    />
                                </div>

                                <div className="form-group">
                                    <label>Tipo de Discapacidad</label>
                                    <input
                                        type="text"
                                        name="tipoDiscapacidad"
                                        className="form-control"
                                        value={formData.tipoDiscapacidad}
                                        onChange={handleInputChange}
                                    />
                                </div>

                                <div className="form-group">
                                    <label>URL Imagen</label>
                                    <input
                                        type="text"
                                        name="urlImg"
                                        className="form-control"
                                        value={formData.urlImg}
                                        onChange={handleInputChange}
                                    />
                                </div>

                                <div className="form-group">
                                    <label>Descripción</label>
                                    <textarea
                                        name="descripcion"
                                        className="form-control"
                                        value={formData.descripcion}
                                        onChange={handleInputChange}
                                        required
                                    />
                                </div>
                                <button type="submit" className="btn btn-primary mt-3">
                                    Guardar
                                </button>
                            </form>
                            <div className='col'>
                                <div>
                                    <label>Ingresar Historial Policial</label>
                                    <Uploader label="Historial Policial" onUploadSuccess={setHistorialPolicial} />
                                </div>
                                <div>
                                    <label>Ingresar Cédula</label>
                                    <Uploader label="Cédula" onUploadSuccess={setCedula} />
                                </div>
                                <div>
                                    <label>Ingresar Curriculum</label>
                                    <Uploader label="Curriculum Vitae" onUploadSuccess={setCurriculum} />
                                </div>
                                <div>
                                    <label>Ingresar Título</label>
                                    <Uploader label="Título Profesional" onUploadSuccess={setTitulo} />
                                </div>
                                <button
                                    className='btn btn-secondary mt-3'
                                    onClick={subirDocus}>
                                    Guardar Documentos
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    );
};

export default UserInfoModal;
