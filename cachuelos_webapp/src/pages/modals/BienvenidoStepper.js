import { useEffect, useState } from "react";
import Stepper, { Step } from "../components/Stepper";
import Uploader from "../components/Uploader.js";
import { toast } from "react-toastify";

import { obtenerUser, updateUser } from "../../sevices/apis/userServ";
import { updateDocu } from "../../sevices/apis/docuServ";

import "./BienvenidoStepper.css";
import { ObtenerInfoParaRegister } from "../../sevices/apis/cataServ.js";

export default function BienvenidoStepper() {

    const [pasoActual, setPasoActual] = useState(1);

    /* ================= FORM ================= */
    const [formData, setFormData] = useState({
        nombre: "",
        apellido: "",
        fechaNacimiento: "",
        tipoIdentificacion: "",
        identificacion: "",
        estadoCivil: "",
        direccion: "",
        telefono: "",
        nacionalidad: "",
        provincia: "",
        ciudad: "",
        discapacidad: false,
        tipoDiscapacidad: "",
        urlImg: "",
        descripcion: ""
    });

    /* ================= DOCUMENTOS ================= */
    const [docs, setDocs] = useState({
        historial: null,
        cedula: null,
        curriculum: null,
        titulo: null
    });

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
                const dataUser = await obtenerUser();
                const usuario = dataUser.body.usuarioInfoDto;

                setFormData({
                    ...usuario,
                    fechaNacimiento: usuario.fechaNacimiento
                        ? new Date(usuario.fechaNacimiento).toISOString().split("T")[0]
                        : ""
                });

                const info = await ObtenerInfoParaRegister();
                setCatalogos(info.body);

            } catch {
                toast.error("No se pudo cargar la información inicial");
            }
        };

        cargar();
    }, []);

    /* ================= HANDLERS ================= */
    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: type === "checkbox" ? checked : value
        }));
    };

    const handleNacionalidadChange = (e) => {
        const nacionalidadCodigo = e.target.value;

        setFormData(prev => ({
            ...prev,
            nacionalidad: nacionalidadCodigo,
            provincia: "",
            ciudad: ""
        }));

        const provincias = catalogos.provincias.filter(
            p => p.adicional === nacionalidadCodigo
        );

        setProvinciasFiltradas(provincias);
        setCiudadesFiltradas([]);
    };


    const handleProvinciaChange = (e) => {
        const provinciaCodigo = e.target.value;

        setFormData(prev => ({
            ...prev,
            provincia: provinciaCodigo,
            ciudad: ""
        }));

        const ciudades = catalogos.ciudades.filter(
            c => c.adicional === provinciaCodigo
        );

        setCiudadesFiltradas(ciudades);
    };


    /* ================= VALIDACIONES ================= */
    const validarPaso = (step) => {
        switch (step) {

            case 1:
                return true;

            case 2:
                if (
                    !formData.nombre ||
                    !formData.apellido ||
                    !formData.fechaNacimiento ||
                    !formData.tipoIdentificacion ||
                    !formData.identificacion ||
                    !formData.estadoCivil
                ) {
                    toast.error("Completa tu información personal");
                    return false;
                }
                return true;

            case 3:
                if (
                    !formData.direccion ||
                    !formData.telefono ||
                    !formData.nacionalidad ||
                    !formData.provincia ||
                    !formData.ciudad
                ) {
                    toast.error("Completa tu información de contacto");
                    return false;
                }
                return true;

            case 4:
                if (!formData.descripcion) {
                    toast.error("Agrega una descripción sobre ti");
                    return false;
                }
                return true;

            case 5:
                if (!docs.historial || !docs.cedula || !docs.curriculum || !docs.titulo) {
                    toast.error("Debes subir todos los documentos");
                    return false;
                }
                return true;

            default:
                return true;
        }
    };

    /* ================= FINALIZAR ================= */
    const finalizarRegistro = async () => {
        try {
            await updateUser({
                ...formData,
                fechaNacimiento: new Date(formData.fechaNacimiento).toISOString()
            });

            await updateDocu({ urlString: docs.curriculum, idTipoDocumento: 1 });
            await updateDocu({ urlString: docs.titulo, idTipoDocumento: 2 });
            await updateDocu({ urlString: docs.historial, idTipoDocumento: 3 });
            await updateDocu({ urlString: docs.cedula, idTipoDocumento: 4 });

            toast.success("🎉 Registro completado correctamente");
            window.location.reload();

        } catch {
            toast.error("Error al completar el registro");
        }
    };

    return (
        <div className="wizard-overlay locked">
            <div className="wizard-container">

                <Stepper
                    initialStep={1}
                    onStepChange={(s) => setPasoActual(s)}
                    onFinalStepCompleted={finalizarRegistro}
                    disableStepIndicators={true}
                    backButtonText="Atrás"
                    nextButtonText={pasoActual === 1 ? "Comenzar" : "Siguiente"}
                    nextButtonProps={{
                        onClickCapture: (e) => {
                            if (!validarPaso(pasoActual)) {
                                e.preventDefault();
                                e.stopPropagation();
                            }
                        }
                    }}
                >

                    {/* ===== PASO 1 – BIENVENIDA ===== */}
                    <Step>
                        <h1>👋 Bienvenido a Cachuelos SA</h1>
                        <p>Completa tu perfil para continuar.</p>
                    </Step>

                    {/* ===== PASO 2 – INFO PERSONAL ===== */}
                    <Step>
                        <h2>Información personal</h2>

                        <label>Nombre</label>
                        <input name="nombre" value={formData.nombre} onChange={handleChange} />

                        <label>Apellido</label>
                        <input name="apellido" value={formData.apellido} onChange={handleChange} />

                        <label>Fecha de nacimiento</label>
                        <input type="date" name="fechaNacimiento" value={formData.fechaNacimiento} onChange={handleChange} />

                        <label>Tipo de identificación</label>
                        <select name="tipoIdentificacion" value={formData.tipoIdentificacion} onChange={handleChange}>
                            <option value="">Seleccione</option>
                            {catalogos.tipoIdentificacion.map(t => (
                                <option key={t.codigo} value={t.codigo}>{t.nombre}</option>
                            ))}
                        </select>

                        <label>Número de identificación</label>
                        <input name="identificacion" value={formData.identificacion} onChange={handleChange} />

                        <label>Estado civil</label>
                        <select name="estadoCivil" value={formData.estadoCivil} onChange={handleChange}>
                            <option value="">Seleccione</option>
                            {catalogos.estadoCivil.map(e => (
                                <option key={e.codigo} value={e.codigo}>{e.nombre}</option>
                            ))}
                        </select>
                    </Step>

                    {/* ===== PASO 3 – CONTACTO ===== */}
                    <Step>
                        <h2>Información de contacto</h2>

                        <label>Dirección</label>
                        <input name="direccion" value={formData.direccion} onChange={handleChange} />

                        <label>Teléfono</label>
                        <input name="telefono" value={formData.telefono} onChange={handleChange} />

                        <label>Nacionalidad</label>
                        <select name="nacionalidad" value={formData.nacionalidad} onChange={handleNacionalidadChange}>
                            <option value="">Seleccione</option>
                            {catalogos.nacionalidades.map(n => (
                                <option key={n.codigo} value={n.codigo}>{n.nombre}</option>
                            ))}
                        </select>

                        <select
                            name="provincia"
                            value={formData.provincia}
                            onChange={handleProvinciaChange}
                            disabled={!formData.nacionalidad}
                        >
                            <option value="">Seleccione</option>
                            {provinciasFiltradas.map(p => (
                                <option key={p.codigo} value={p.codigo}>
                                    {p.nombre}
                                </option>
                            ))}
                        </select>


                        <select
                            key={formData.provincia}   // 👈 ESTA ES LA CLAVE
                            name="ciudad"
                            value={formData.ciudad}
                            onChange={handleChange}
                            disabled={!formData.provincia}
                        >
                            <option value="">Seleccione</option>
                            {ciudadesFiltradas.map(c => (
                                <option key={c.codigo} value={c.codigo}>
                                    {c.nombre}
                                </option>
                            ))}
                        </select>


                    </Step>

                    {/* ===== PASO 4 – ADICIONAL ===== */}
                    <Step>
                        <h2>Información adicional</h2>

                        <label>Descripción personal</label>
                        <textarea name="descripcion" value={formData.descripcion} onChange={handleChange} />

                        <div className="checkbox-row">
                            <label htmlFor="chkDiscapacidad">
                                ¿Posees alguna discapacidad?
                            </label>
                            <input
                                type="checkbox"
                                id="chkDiscapacidad"
                                name="discapacidad"
                                checked={formData.discapacidad}
                                onChange={handleChange}
                            />
                        </div>

                        {formData.discapacidad && (
                            <>
                                <label>Tipo de discapacidad</label>
                                <input name="tipoDiscapacidad" value={formData.tipoDiscapacidad} onChange={handleChange} />
                            </>
                        )}
                    </Step>

                    {/* ===== PASO 5 – DOCUMENTOS ===== */}
                    <Step>
                        <h2>Documentos obligatorios</h2>

                        <p><strong>Historial Policial:</strong> Certificado vigente que valida antecedentes.</p>
                        <Uploader label="Subir historial policial" onUploadSuccess={(v) => setDocs(d => ({ ...d, historial: v }))} />

                        <p><strong>Cédula:</strong> Documento de identidad legible (PDF o imagen).</p>
                        <Uploader label="Subir cédula" onUploadSuccess={(v) => setDocs(d => ({ ...d, cedula: v }))} />

                        <p><strong>Curriculum:</strong> Hoja de vida actualizada.</p>
                        <Uploader label="Subir curriculum" onUploadSuccess={(v) => setDocs(d => ({ ...d, curriculum: v }))} />

                        <p><strong>Título:</strong> Certificado académico o título profesional.</p>
                        <Uploader label="Subir título" onUploadSuccess={(v) => setDocs(d => ({ ...d, titulo: v }))} />
                    </Step>

                </Stepper>
            </div>
        </div>
    );
}
