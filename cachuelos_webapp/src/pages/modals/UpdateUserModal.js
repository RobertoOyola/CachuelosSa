import React, { useEffect, useState } from 'react';
import Uploader from '../components/Uploader.js';
import { updateDocu } from '../../sevices/apis/docuServ';
import { toast } from 'react-toastify';
import { obtenerUser, updateUser } from '../../sevices/apis/userServ.js';
import { ObtenerInfoParaRegister } from '../../sevices/apis/cataServ.js';

const UserInfoModal = ({ show, onClose }) => {

    /* ================= FORM ================= */
    const [formData, setFormData] = useState({
        nombre: '',
        apellido: '',
        fechaNacimiento: '',
        tipoIdentificacion: '',
        identificacion: '',
        estadoCivil: '',
        direccion: '',
        telefono: '',
        nacionalidad: '',
        provincia: '',
        ciudad: '',
        discapacidad: false,
        tipoDiscapacidad: '',
        urlImg: '',
        descripcion: ''
    });

    /* ================= DOCUMENTOS ================= */
    const [HistorialPolicial, setHistorialPolicial] = useState(null);
    const [Cedula, setCedula] = useState(null);
    const [Curriculum, setCurriculum] = useState(null);
    const [Titulo, setTitulo] = useState(null);

    /* ================= CATÁLOGOS ================= */
    const [catalogos, setCatalogos] = useState({
        tipoIdentificacion: [],
        estadoCivil: [],
        nacionalidades: [],
        provincias: [],
        ciudades: []
    });

    const [provinciasFiltradas, setProvinciasFiltradas] = useState([]);
    const [ciudadesFiltradas, setCiudadesFiltradas] = useState([]);

    /* ================= CARGA INICIAL ================= */
    useEffect(() => {
        const cargar = async () => {
            try {
                const datos = await obtenerUser();
                const usuario = datos.body.usuarioInfoDto;

                setFormData({
                    ...usuario,
                    fechaNacimiento: usuario.fechaNacimiento
                        ? new Date(usuario.fechaNacimiento).toISOString().split('T')[0]
                        : ''
                });

                const info = await ObtenerInfoParaRegister();
                setCatalogos(info.body);

                // precarga dependencias si ya existen
                if (usuario?.nacionalidad) {
                    const prov = info.body.provincias.filter(
                        p => p.adicional === usuario.nacionalidad
                    );
                    setProvinciasFiltradas(prov);
                }

                if (usuario?.provincia) {
                    const ciu = info.body.ciudades.filter(
                        c => c.adicional === usuario.provincia
                    );
                    setCiudadesFiltradas(ciu);
                }

            } catch {
                toast.error("Error al cargar información del usuario");
            }
        };

        if (show) cargar();
    }, [show]);

    /* ================= HANDLERS ================= */
    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: type === 'checkbox' ? checked : value
        }));
    };

    const handleNacionalidadChange = (e) => {
        const value = e.target.value;

        setFormData(prev => ({
            ...prev,
            nacionalidad: value,
            provincia: '',
            ciudad: ''
        }));

        setProvinciasFiltradas(
            catalogos.provincias.filter(p => p.adicional === value)
        );

        setCiudadesFiltradas([]);
    };

    const handleProvinciaChange = (e) => {
        const value = e.target.value;

        setFormData(prev => ({
            ...prev,
            provincia: value,
            ciudad: ''
        }));

        setCiudadesFiltradas(
            catalogos.ciudades.filter(c => c.adicional === value)
        );
    };

    /* ================= SUBIR DOCUMENTOS ================= */
    const subirDocus = async () => {
        try {
            if (HistorialPolicial)
                await updateDocu({ urlString: HistorialPolicial, idTipoDocumento: 3 });

            if (Cedula)
                await updateDocu({ urlString: Cedula, idTipoDocumento: 4 });

            if (Curriculum)
                await updateDocu({ urlString: Curriculum, idTipoDocumento: 1 });

            if (Titulo)
                await updateDocu({ urlString: Titulo, idTipoDocumento: 2 });

            toast.success("Documentos guardados correctamente");
            onClose();

        } catch {
            toast.error("Error al subir documentos");
        }
    };

    /* ================= GUARDAR FORM ================= */
    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            await updateUser({
                ...formData,
                fechaNacimiento: new Date(formData.fechaNacimiento).toISOString()
            });

            toast.success("Información actualizada correctamente");
            onClose();

        } catch {
            toast.error("Error al guardar información");
        }
    };

    if (!show) return null;

    return (
        <div className="modal modal-xl show" style={{ display: 'block' }}>
            <div className="modal-dialog">
                <div className="modal-content">

                    <div className="modal-header row">
                        <h5 className="modal-title col">Información del Usuario</h5>
                        <button type="button" className="btn btn-secondary btn-sm col-1" onClick={onClose}>
                            &times;
                        </button>
                    </div>

                    <div className="modal-body row">

                        {/* ===== FORM ===== */}
                        <form onSubmit={handleSubmit} className="col-7">

                            <label>Nombre</label>
                            <input className="form-control" name="nombre" value={formData.nombre} onChange={handleChange} />

                            <label>Apellido</label>
                            <input className="form-control" name="apellido" value={formData.apellido} onChange={handleChange} />

                            <label>Fecha de nacimiento</label>
                            <input type="date" className="form-control" name="fechaNacimiento" value={formData.fechaNacimiento} onChange={handleChange} />

                            <label>Tipo de identificación</label>
                            <select className="form-control" name="tipoIdentificacion" value={formData.tipoIdentificacion} onChange={handleChange}>
                                <option value="">Seleccione</option>
                                {catalogos.tipoIdentificacion.map(t => (
                                    <option key={t.codigo} value={t.codigo}>{t.nombre}</option>
                                ))}
                            </select>

                            <label>Identificación</label>
                            <input className="form-control" name="identificacion" value={formData.identificacion} onChange={handleChange} />

                            <label>Estado civil</label>
                            <select className="form-control" name="estadoCivil" value={formData.estadoCivil} onChange={handleChange}>
                                <option value="">Seleccione</option>
                                {catalogos.estadoCivil.map(e => (
                                    <option key={e.codigo} value={e.codigo}>{e.nombre}</option>
                                ))}
                            </select>

                            <label>Dirección</label>
                            <input className="form-control" name="direccion" value={formData.direccion} onChange={handleChange} />

                            <label>Teléfono</label>
                            <input className="form-control" name="telefono" value={formData.telefono} onChange={handleChange} />

                            <label>Nacionalidad</label>
                            <select className="form-control" name="nacionalidad" value={formData.nacionalidad} onChange={handleNacionalidadChange}>
                                <option value="">Seleccione</option>
                                {catalogos.nacionalidades.map(n => (
                                    <option key={n.codigo} value={n.codigo}>{n.nombre}</option>
                                ))}
                            </select>

                            <label>Provincia</label>
                            <select className="form-control" name="provincia" value={formData.provincia} onChange={handleProvinciaChange} disabled={!formData.nacionalidad}>
                                <option value="">Seleccione</option>
                                {provinciasFiltradas.map(p => (
                                    <option key={p.codigo} value={p.codigo}>{p.nombre}</option>
                                ))}
                            </select>

                            <label>Ciudad</label>
                            <select className="form-control" name="ciudad" value={formData.ciudad} onChange={handleChange} disabled={!formData.provincia}>
                                <option value="">Seleccione</option>
                                {ciudadesFiltradas.map(c => (
                                    <option key={c.codigo} value={c.codigo}>{c.nombre}</option>
                                ))}
                            </select>

                            <div className="d-flex align-items-center mt-2">
                                <input type="checkbox" id="chkDisc" name="discapacidad" checked={formData.discapacidad} onChange={handleChange} />
                                <label htmlFor="chkDisc" className="ms-2">
                                    ¿Posees alguna discapacidad?
                                </label>
                            </div>

                            {formData.discapacidad && (
                                <>
                                    <label>Tipo de discapacidad</label>
                                    <input className="form-control" name="tipoDiscapacidad" value={formData.tipoDiscapacidad} onChange={handleChange} />
                                </>
                            )}

                            <label>Descripción</label>
                            <textarea className="form-control" name="descripcion" value={formData.descripcion} onChange={handleChange} />

                            <button type="submit" className="btn btn-primary mt-3">
                                Guardar Información
                            </button>
                        </form>

                        {/* ===== DOCUMENTOS ===== */}
                        <div className="col">
                            <p><strong>Historial Policial</strong></p>
                            <Uploader label="Subir historial policial" onUploadSuccess={setHistorialPolicial} />

                            <p><strong>Cédula</strong></p>
                            <Uploader label="Subir cédula" onUploadSuccess={setCedula} />

                            <p><strong>Curriculum</strong></p>
                            <Uploader label="Subir curriculum" onUploadSuccess={setCurriculum} />

                            <p><strong>Título</strong></p>
                            <Uploader label="Subir título" onUploadSuccess={setTitulo} />

                            <button className="btn btn-secondary mt-3" onClick={subirDocus}>
                                Guardar Documentos
                            </button>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    );
};

export default UserInfoModal;
