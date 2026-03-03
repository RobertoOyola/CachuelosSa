import { useState } from "react";
import { useNavigate } from "react-router-dom";
import {
    SeleccionarGanador,
    SubirComprobante,
    ConfirmarFinalizacion,
    GenerarOtpInicio,
    ConfirmarInicioTrabajo,
    GenerarOtpFinal
} from "../../sevices/apis/subsServ";
import { uploadPhoto } from "../../sevices/apis/docuServ";
import { toast } from "react-toastify";

import "./TrabajoActivoModal.css";

export default function TrabajoActivoModal({ data, rol, onClose, refresh }) {

    const navigate = useNavigate();
    const trabajo = data;
    const ofertas = data.ofertas || [];

    const [ofertaSeleccionada, setOfertaSeleccionada] = useState(null);
    const [image, setImage] = useState(null);
    const [uploading, setUploading] = useState(false);
    const [otpInicio, setOtpInicio] = useState("");
    const [otpFinal, setOtpFinal] = useState("");

    const tienePago = trabajo.pagos?.length > 0;
    const pago = tienePago ? trabajo.pagos[0] : null;
    const pagoConfirmado = pago?.estadoPago === "CO";
    const pagoTrabajadorHecho = pago?.pagoTrabajador != null;

    const trabajoAsignado = trabajo.estado === "AS";
    const trabajoEnProceso = trabajo.estado === "EP";

    const montoBase = ofertaSeleccionada?.monto || 0;
    const comision = montoBase * 0.10;
    const totalTransferir = montoBase + comision;

    const handleConfirmarAsignacion = async () => {

        if (!ofertaSeleccionada)
            return toast.error("Selecciona una oferta");

        if (!image)
            return toast.error("Debes subir el comprobante");

        setUploading(true);

        try {

            const idImg = await uploadPhoto(image);

            const resGanador = await SeleccionarGanador({
                IdSubasta: trabajo.subasta.id,
                IdOferta: ofertaSeleccionada.id
            });

            if (resGanador?.header?.codigo !== 200)
                return toast.error(resGanador?.header?.mensaje);

            const resComprobante = await SubirComprobante({
                idTrabajo: trabajo.id,
                url: idImg
            });

            if (resComprobante?.header?.codigo === 200) {
                toast.success("Trabajo asignado correctamente");
                refresh();
                onClose();
            }

        } catch {
            toast.error("Error en la asignación");
        } finally {
            setUploading(false);
        }
    };

    const handleGenerarOtpInicio = async () => {

        const res = await GenerarOtpInicio({ idTrabajo: trabajo.id });

        if (res?.header?.codigo === 200)
            toast.success("OTP enviado al trabajador");
        else
            toast.error(res?.header?.mensaje);
    };

    const handleGenerarOtpFinal = async () => {
        const res = await GenerarOtpFinal({
            idTrabajo: trabajo.id
        });

        if (res?.header?.codigo === 200)
            toast.success("OTP final enviado al cliente");
        else
            toast.error(res?.header?.mensaje);
    };

    const handleConfirmarInicio = async () => {

        if (!otpInicio)
            return toast.error("Ingresa el OTP de inicio");

        const res = await ConfirmarInicioTrabajo({
            idTrabajo: trabajo.id,
            otp: otpInicio
        });

        if (res?.header?.codigo === 200) {
            toast.success("Trabajo iniciado correctamente");
            refresh();
            onClose();
        } else {
            toast.error(res?.header?.mensaje);
        }
    };

    const handleConfirmarFinalizacion = async () => {

        if (!otpFinal)
            return toast.error("Ingresa el OTP final");

        const res = await ConfirmarFinalizacion({
            idTrabajo: trabajo.id,
            otp: otpFinal
        });

        if (res?.header?.codigo === 200) {
            toast.success("Trabajo finalizado correctamente");
            refresh();
            onClose();
        } else {
            toast.error(res?.header?.mensaje);
        }
    };

    return (
        <div className="otp-modal">
            <div className="modal-Otp-content">

                <h3 className="fw-bold text-purple mb-2">
                    {trabajo.titulo}
                </h3>

                <p className="text-muted mb-3">
                    {trabajo.descripcion}
                </p>

                {/* =========================
                   FASE 1 – OFERTAS
                ========================== */}
                {rol === "CLIENTE" && !tienePago && (
                    <div className="section-card">
                        <h5 className="section-title">Trabajadores postulados</h5>

                        {ofertas.map((oferta) => (
                            <div
                                key={oferta.id}
                                className={`offer-card ${ofertaSeleccionada?.id === oferta.id ? "selected" : ""}`}
                            >
                                <div>
                                    <strong>{oferta.trabajador?.nombreUsuario}</strong>
                                    <div className="small text-muted">
                                        Oferta: ${oferta.monto}
                                    </div>
                                </div>

                                <div className="d-flex gap-2">
                                    <button
                                        className="btn btn-outline-secondary btn-sm"
                                        onClick={() => navigate(`/perfil/${oferta.idUsuarioTrabajador}`)}>
                                        Ver perfil
                                    </button>

                                    <button
                                        className="btn btn-primary btn-sm"
                                        onClick={() => setOfertaSeleccionada(oferta)}>
                                        Seleccionar
                                    </button>
                                </div>
                            </div>
                        ))}

                        {ofertaSeleccionada && (
                            <div className="transfer-card mt-3">

                                <h6>Monto a transferir</h6>

                                <div>Oferta: ${montoBase}</div>
                                <div>Comisión (10%): ${comision.toFixed(2)}</div>
                                <h4 className="monto-transferir">
                                    Total: ${totalTransferir.toFixed(2)}
                                </h4>

                                <input
                                    type="file"
                                    accept="image/png,image/jpeg"
                                    onChange={(e) => setImage(e.target.files[0])}
                                />

                                <button
                                    className="btn btn-success w-100 mt-3"
                                    disabled={uploading}
                                    onClick={handleConfirmarAsignacion}>
                                    Confirmar asignación
                                </button>
                            </div>
                        )}
                    </div>
                )}

                {/* =========================
                   DETALLES DEL TRABAJO
                ========================== */}
                {tienePago && (
                    <div className="section-card">
                        <h5 className="section-title">Detalles del trabajo</h5>

                        <div className="detail-item">
                            <strong>Dirección:</strong> {trabajo.direccion}
                        </div>

                        <div className="detail-item">
                            <strong>Fecha:</strong>{" "}
                            {new Date(trabajo.fechaTrabajo).toLocaleDateString()}
                        </div>

                        <div className="detail-item">
                            <strong>Categoría:</strong> {trabajo.categoria?.nombre}
                        </div>

                        <button
                            className="btn btn-outline-primary btn-sm mt-2"
                            onClick={() =>
                                window.open(
                                    `https://www.google.com/maps/search/?api=1&query=${trabajo.latitud},${trabajo.longitud}`,
                                    "_blank"
                                )
                            }>
                            Ver en Google Maps
                        </button>

                        {rol === "TRABAJADOR" && (
                            <button
                                className="btn btn-outline-dark btn-sm mt-2"
                                onClick={() => navigate(`/perfil/${trabajo.creador?.id}`)}>
                                Ver perfil de {trabajo.creador?.nombreUsuario}
                            </button>
                        )}
                    </div>
                )}

                {/* INICIAR TRABAJO – CLIENTE */}
                {rol === "CLIENTE" && pagoConfirmado && trabajoAsignado && (
                    <div className="section-card warning-card">
                        <h5 className="section-title">Iniciar trabajo</h5>

                        <button
                            className="btn btn-warning w-100"
                            onClick={handleGenerarOtpInicio}>
                            Generar OTP de inicio
                        </button>
                    </div>
                )}

                {rol === "TRABAJADOR" && pagoConfirmado && trabajo.estado === "AS" && (
                    <div className="section-card info-card">
                        <h5 className="section-title">Confirmar inicio</h5>

                        <input
                            type="text"
                            placeholder="Ingrese OTP de inicio"
                            className="form-control mt-2"
                            value={otpInicio}
                            onChange={(e) => setOtpInicio(e.target.value)}
                        />

                        <button
                            className="btn btn-primary w-100 mt-3"
                            onClick={handleConfirmarInicio}>
                            Confirmar inicio
                        </button>
                    </div>
                )}

                {/* GENERAR OTP FINAL – TRABAJADOR */}
                {rol === "TRABAJADOR" && trabajoEnProceso && !pagoTrabajadorHecho && (
                    <div className="section-card danger-card">
                        <h5 className="section-title">Finalizar trabajo</h5>

                        <p className="small text-muted">
                            Genera el OTP que el cliente deberá ingresar.
                        </p>

                        <button
                            className="btn btn-danger w-100"
                            onClick={handleGenerarOtpFinal}>
                            Generar OTP final
                        </button>
                    </div>
                )}

                {/* FINALIZAR – CLIENTE */}
                {rol === "CLIENTE" && trabajoEnProceso && !pagoTrabajadorHecho && (
                    <div className="section-card success-card">
                        <h5 className="section-title">Finalizar trabajo</h5>

                        <input
                            type="text"
                            placeholder="Ingrese OTP final"
                            className="form-control mt-2"
                            value={otpFinal}
                            onChange={(e) => setOtpFinal(e.target.value)}
                        />

                        <button
                            className="btn btn-success w-100 mt-3"
                            onClick={handleConfirmarFinalizacion}>
                            Confirmar finalización
                        </button>
                    </div>
                )}

                {pagoTrabajadorHecho && (
                    <div className="alert alert-success mt-3">
                        Trabajo finalizado correctamente ✅
                    </div>
                )}

                <div className="d-grid mt-4">
                    <button className="btn btn-secondary" onClick={onClose}>
                        Cerrar
                    </button>
                </div>

            </div>
        </div>
    );
}