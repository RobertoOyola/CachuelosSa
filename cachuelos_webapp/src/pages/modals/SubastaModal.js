import { useState, useEffect } from 'react';
import { obtenerFoto } from '../../sevices/apis/docuServ';
import { CrearOferta } from '../../sevices/apis/subsServ';
import { toast } from "react-toastify";

import './SubastaModal.css';

export default function SubastaModal({ onClose, subasta }) {

    const [oferta, setOferta] = useState("");
    const [imagenActiva, setImagenActiva] = useState(null);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        if (subasta?.listImg?.length > 0) {
            setImagenActiva(subasta.listImg[0]);
        }
    }, [subasta]);

    if (!subasta || !subasta.trabajo) return null;

    const { trabajo, listImg } = subasta;
    const latAprox = Number(trabajo.latitud).toFixed(2);
    const lngAprox = Number(trabajo.longitud).toFixed(2);

    const obtenerUbicacionGeneral = (direccion) => {
        if (!direccion) return "";
        const partes = direccion.split(",");
        const ciudad = partes[partes.length - 3]?.trim();
        const zona = partes[partes.length - 4]?.trim();
        return `${zona}, ${ciudad}`;
    };

    const enviarOferta = async () => {
        try {
            if (!subasta?.subasta?.id) {
                toast.error("No se encontró la subasta");
                return;
            }

            setLoading(true);
            const form = {
                idSubasta: subasta.subasta.id,
                Monto: Number(oferta)
            };

            const response = await CrearOferta(form);
            if (response?.header?.codigo === 200) {
                toast.success("Oferta enviada con éxito");
                setOferta("");
                subasta.subastaOfertas = [
                    ...(subasta.subastaOfertas || []),
                    response.body
                ];
                onClose()
            } else {
                toast.error(response?.header?.mensaje || "Error al enviar oferta");
            }

        } catch (error) {
            console.error("Error enviando oferta:", error);
            toast.error("Error al enviar la oferta");
        } finally {
            setLoading(false);
        }
    };

    const ofertaValida =
        oferta &&
        !isNaN(oferta) &&
        Number(oferta) > 0;

    return (
        <div className="subasta-overlay">
            <div className="subasta-modal modern">

                <div className="modal-header-modern">
                    <div>
                        <h3 className="titulo-trabajo">{trabajo.titulo}</h3>
                        <span className="badge-subasta">
                            Termina: {new Date(trabajo.fechaFinSubasta).toLocaleDateString()}
                        </span>
                    </div>
                    <button className="btn-close" onClick={onClose}></button>
                </div>

                {imagenActiva && (
                    <div className="imagen-principal-modern">
                        <img
                            src={obtenerFoto(imagenActiva, 'card').toURL()}
                            alt="Trabajo"
                        />
                    </div>
                )}

                {listImg?.length > 1 && (
                    <div className="miniaturas-modern">
                        {listImg.map((imgId, index) => (
                            <img
                                key={index}
                                src={obtenerFoto(imgId, 'icon').toURL()}
                                alt="mini"
                                className={imagenActiva === imgId ? "active" : ""}
                                onClick={() => setImagenActiva(imgId)}
                            />
                        ))}
                    </div>
                )}

                <div className="card-section">
                    <h5>Descripción</h5>
                    <p>{trabajo.descripcion}</p>
                </div>

                <div className="info-grid">
                    <div className="info-card highlight">
                        <span>Presupuesto</span>
                        <h4>${trabajo.precioReferencial}</h4>
                    </div>
                    <div className="info-card">
                        <span>Fecha trabajo </span>
                        <strong>
                            {new Date(trabajo.fechaTrabajo).toLocaleDateString()}
                        </strong>
                    </div>
                    <div className="info-card">
                        <span>Duración </span>
                        <strong>{trabajo.tiempoEstimadoTrabajo} min</strong>
                    </div>
                    <div className="info-card">
                        <span>Ofertas </span>
                        <strong>{subasta.subastaOfertas?.length || 0}</strong>
                    </div>
                </div>

                <div className="card-section">
                    <h5>Ubicación aproximada</h5>
                    <p>{obtenerUbicacionGeneral(trabajo.direccion)}</p>

                    <iframe
                        title={`Mapa de ubicación aproximada del trabajo ${trabajo.titulo}`}
                        width="100%"
                        height="220"
                        className="mapa-modern"
                        loading="lazy"
                        allowFullScreen
                        src={`https://www.google.com/maps?q=${latAprox},${lngAprox}&z=14&output=embed`}
                    ></iframe>
                </div>

                <div className="oferta-modern">
                    <label>Tu oferta (USD)</label>
                    <div className="input-wrapper">
                        <span>$</span>
                        <input
                            type="number"
                            value={oferta}
                            onChange={(e) => setOferta(e.target.value)}
                            placeholder="0.00"
                        />
                    </div>
                    <button
                        className="btn-enviar-modern"
                        disabled={!ofertaValida || loading}
                        onClick={enviarOferta}
                    >
                        {loading ? "Enviando..." : "Enviar oferta"}
                    </button>
                </div>

            </div>
        </div>
    );
}