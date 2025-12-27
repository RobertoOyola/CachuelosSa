import { useEffect, useState, useRef } from "react";
import Stepper, { Step } from "../components/Stepper";

import { MapContainer, Marker, TileLayer, useMapEvents } from "react-leaflet";
import L from "leaflet";
import icon from "../../assets/leaflet/marker-icon.png";
import icon2x from "../../assets/leaflet/marker-icon-2x.png";
import shadow from "../../assets/leaflet/marker-shadow.png";
import { motion } from "framer-motion";
import ClipLoader from "react-spinners/ClipLoader";

import CreatableSelect from "react-select/creatable";

import { toast } from "react-toastify";
import { ObtenerCatalogoT, ObtenerIdCatalogoT } from "../../sevices/apis/cataServ";
import { uploadPhoto } from "../../sevices/apis/docuServ";
import { CrearTrabajo } from "../../sevices/apis/subsServ";

import "leaflet/dist/leaflet.css";
import "./WizardTrabajo.css";

export default function WizardTrabajo({ onClose }) {
    const [usarUbicacionActual, setUsarUbicacionActual] = useState(false);
    const [cargandoUbicacion, setCargandoUbicacion] = useState(false);
    const [imagenesKey, setImagenesKey] = useState(0);
    const [imagenesPreview, setImagenesPreview] = useState([]);
    const [categorias, setCategorias] = useState([]);

    const [trabajo, setTrabajo] = useState({
        idCategoria: "",
        titulo: "",
        descripcion: "",
        direccion: "",
        latitud: null,
        longitud: null,
        precioReferencial: "",
        imagenesUrls: [],
        fechaFinSubasta: "",
        fechaTrabajo: "",
        tiempoEstimadoTrabajo: "",
    });

    const mapRef = useRef(null);

    const markerIcon = new L.Icon({
        iconUrl: icon,
        iconRetinaUrl: icon2x,
        shadowUrl: shadow,

        iconSize: [25, 41],
        iconAnchor: [12, 41],
        popupAnchor: [1, -34],
        shadowSize: [41, 41],
    });

    const opcionesCategorias = categorias.map(c => ({
        value: c.id,
        label: c.nombre
    }));

    const obtenerDireccionDesdeCoords = async (lat, lng) => {
        try {
            const response = await fetch(
                `https://nominatim.openstreetmap.org/reverse?format=json&lat=${lat}&lon=${lng}`
            );
            const data = await response.json();
            return data.display_name || "";
        } catch (error) {
            console.error("Error obteniendo dirección", error);
            return "";
        }
    };

    const centrarMapa = (lat, lng) => {
        if (!mapRef.current) return;

        try {
            mapRef.current.setView([lat, lng], 15, {
                animate: true
            });
        } catch (err) {
            console.warn("Mapa aún no listo, ignorando setView");
        }
    };

    useEffect(() => {
        const cargar = async () => {
            const data = await ObtenerCatalogoT();
            if (data?.body) setCategorias(data.body);
            else toast.error(data?.header?.mensaje);
        };
        cargar();
    }, []);

    useEffect(() => {
        setImagenesKey(prev => prev + 1);
    }, [trabajo.imagenesUrls]);

    const LocationPicker = () => {
        const map = useMapEvents({
            click(e) {
                setUsarUbicacionActual(false);
                const { lat, lng } = e.latlng;
                centrarMapa(lat, lng);
                setTrabajo(prev => ({
                    ...prev,
                    latitud: lat,
                    longitud: lng
                }));
            }
        });

        useEffect(() => {
            if (!usarUbicacionActual) return;

            if (!navigator.geolocation) {
                toast.warn("Tu navegador no soporta geolocalización");
                setUsarUbicacionActual(false);
                return;
            }

            setCargandoUbicacion(true);

            navigator.geolocation.getCurrentPosition(
                async (pos) => {
                    const { latitude, longitude } = pos.coords;

                    const direccion = await obtenerDireccionDesdeCoords(latitude, longitude);

                    centrarMapa(latitude, longitude);

                    setTrabajo(prev => ({
                        ...prev,
                        latitud: latitude,
                        longitud: longitude,
                        direccion: direccion
                    }));

                    setCargandoUbicacion(false);
                    setUsarUbicacionActual(false);
                },
                () => {
                    toast.warn("No se pudo obtener tu ubicación");
                    setCargandoUbicacion(false);
                    setUsarUbicacionActual(false);
                },
                { enableHighAccuracy: true }
            );
        }, [map]);


        return trabajo.latitud && trabajo.longitud ? (
            <Marker
                position={[Number(trabajo.latitud), Number(trabajo.longitud)]}
                icon={markerIcon}
            />
        ) : null;
    };


    const handleFinalSubmit = async () => {
        try {
            console.log("Como cadena JSON:", JSON.stringify(trabajo));

            const data = await CrearTrabajo(trabajo);

            if (data?.body) {
                toast.success("Trabajo creado correctamente");
                setTimeout(() => {
                    onClose?.();
                }, 300);
            } else {
                toast.error(data?.header?.mensaje);
            }

        } catch (error) {
            toast.error("Error al crear trabajo" || error?.mensaje);
            console.error(error);
            setTimeout(() => {
                onClose?.();
            }, 600);
        }
    };


    return (
        <motion.div 
            className="wizard-overlay"
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            exit={{ opacity: 0 }}
            transition={{ duration: 0.25 }}
            onClick={onClose} >
            <motion.div 
                className="wizard-container"
                initial={{ scale: 0.95, y: 20, opacity: 0 }}
                animate={{ scale: 1, y: 0, opacity: 1 }}
                exit={{ scale: 0.95, y: 20, opacity: 0 }}
                transition={{ duration: 0.25, ease: "easeOut" }}
                onClick={(e) => e.stopPropagation()} >
                <Stepper
                    initialStep={1}
                    onFinalStepCompleted={handleFinalSubmit}
                    backButtonText="Atrás"
                    nextButtonText="Siguiente"
                >
                    {/* PASO 1 */}
                    <Step>
                        <div className="wizard-header">
                            <h2>Creación de nuevo trabajo</h2>
                            <button className="wizard-close" onClick={onClose}>
                                Salir
                            </button>
                        </div>

                        <input
                            type="text"
                            placeholder="Título del trabajo"
                            value={trabajo.titulo}
                            onChange={(e) =>
                                setTrabajo({ ...trabajo, titulo: e.target.value })
                            }
                        />

                        <textarea
                            placeholder="Descripción del trabajo"
                            value={trabajo.descripcion}
                            onChange={(e) =>
                                setTrabajo({ ...trabajo, descripcion: e.target.value })
                            }
                        />
                    </Step>

                    {/* PASO 2 */}
                    <Step>
                        <h2>Categoría y fechas</h2>

                        <label className="wizard-label">Categoría del trabajo</label>

                        <CreatableSelect
                            classNamePrefix="wizard-select"
                            placeholder="Seleccione o cree una categoría"
                            options={opcionesCategorias}
                            value={
                                trabajo.idCategoria
                                    ? opcionesCategorias.find(o => o.value === trabajo.idCategoria)
                                    : null
                            }
                            isClearable
                            onChange={async (selected) => {
                                if (!selected) {
                                    setTrabajo({ ...trabajo, idCategoria: "" });
                                    return;
                                }
                                if (typeof selected.value === "number") {
                                    setTrabajo({ ...trabajo, idCategoria: selected.value });
                                    return;
                                }
                                try {
                                    const data = await ObtenerIdCatalogoT(selected.label);
                                    if (data?.body) {
                                        setTrabajo({
                                            ...trabajo,
                                            idCategoria: data.body
                                        });
                                        toast.success("Categoría creada");
                                    } else {
                                        toast.error(data?.header?.mensaje || "No se pudo crear la categoría");
                                    }
                                } catch (error) {
                                    toast.error("Error al crear la categoría");
                                }
                            }}
                        />

                        <label className="wizard-label">
                            ¿Hasta cuándo se aceptan ofertas?
                        </label>
                        <input
                            type="date"
                            value={trabajo.fechaFinSubasta}
                            onChange={(e) =>
                                setTrabajo({ ...trabajo, fechaFinSubasta: e.target.value })
                            }
                        />

                        <label className="wizard-label">
                            ¿Qué día se realizará el trabajo?
                        </label>
                        <input
                            type="date"
                            value={trabajo.fechaTrabajo}
                            onChange={(e) =>
                                setTrabajo({ ...trabajo, fechaTrabajo: e.target.value })
                            }
                        />
                    </Step>

                    {/* PASO 3 */}
                    <Step>
                        <h2>Tiempo y precio</h2>

                        <label className="wizard-label">Tiempo estimado del trabajo</label>

                        <div className="tiempo-container">
                            <input
                                type="number"
                                min="0"
                                placeholder="Horas"
                                onChange={(e) => {
                                    const horas = Number(e.target.value) || 0;
                                    const minutos = trabajo._minutosTemp || 0;

                                    setTrabajo({
                                        ...trabajo,
                                        tiempoEstimadoTrabajo: horas * 60 + minutos,
                                        _horasTemp: horas,
                                    });
                                }}
                            />

                            <span>:</span>

                            <input
                                type="number"
                                min="0"
                                max="59"
                                placeholder="Min"
                                onChange={(e) => {
                                    const minutos = Number(e.target.value) || 0;
                                    const horas = trabajo._horasTemp || 0;

                                    setTrabajo({
                                        ...trabajo,
                                        tiempoEstimadoTrabajo: horas * 60 + minutos,
                                        _minutosTemp: minutos,
                                    });
                                }}
                            />
                        </div>

                        <label className="wizard-label">Precio referencial</label>

                        <input
                            type="number"
                            placeholder="Precio referencial"
                            value={trabajo.precioReferencial}
                            onChange={(e) =>
                                setTrabajo({ ...trabajo, precioReferencial: e.target.value })
                            }
                        />
                    </Step>

                    {/* PASO 4 */}
                    <Step>
                        <h2>Imágenes del trabajo</h2>
                        <input
                            type="file"
                            accept="image/png,image/jpeg"
                            onChange={async (e) => {
                                if (!e.target.files[0]) return;
                                if (trabajo.imagenesUrls.length >= 6) {
                                    toast.warning("Máximo 6 imágenes");
                                    return;
                                }
                                const file = e.target.files[0];
                                const localPreview = URL.createObjectURL(file);
                                setImagenesPreview(prev => [...prev, localPreview]);
                                const id = await uploadPhoto(file);
                                setTrabajo(prev => ({
                                    ...prev,
                                    imagenesUrls: [...prev.imagenesUrls, id],
                                }));
                                e.target.value = null;
                            }}
                        />

                        <div className="imagenes-preview pb-3">
                            {imagenesPreview.map((src, index) => (
                                <img
                                    key={index}
                                    src={src}
                                    width={50}
                                    height={50}
                                    style={{ borderRadius: 6, objectFit: "cover" }}
                                    alt="preview"
                                />
                            ))}
                        </div>

                    </Step>

                    {/* PASO 5 */}
                    <Step>
                        <h2>Ubicación</h2>

                        <input
                            type="text"
                            placeholder="Dirección"
                            value={trabajo.direccion}
                            onChange={(e) =>
                                setTrabajo({ ...trabajo, direccion: e.target.value })
                            }
                        />
                        {cargandoUbicacion && (
                            <div className="ubicacion-loading">
                                <ClipLoader size={18} color="#5227ff" />
                                <span>Obteniendo tu ubicación…</span>
                            </div>
                        )}
                        <MapContainer
                            center={[-2.058762, -79.908688]}
                            zoom={13}
                            style={{ height: 300, width: "100%" }}
                            whenReady={(e) => {
                                mapRef.current = e.target;

                                setTimeout(() => {
                                    e.target.invalidateSize();
                                }, 200);
                            }}
                        >
                        <TileLayer url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png" />
                        <LocationPicker />
                        </MapContainer>
                        <div
                            className="text-center pt-3">
                            <button
                                type="button"
                                className="btn-location"
                                onClick={() => setUsarUbicacionActual(true)}>
                                Usar mi ubicación actual
                            </button>
                        </div>
                    </Step>
                </Stepper>
            </motion.div>
        </motion.div>
    );
}
