import { useEffect, useState } from "react";
import "./TrabajosActivos.css";
import TrabajoActivoModal from "../modals/TrabajoActivoModal";
import { ObtenerTrabajosActivos } from "../../sevices/apis/subsServ";

export default function TrabajosActivos({ rol, userId }) {

    const [trabajos, setTrabajos] = useState([]);
    const [trabajoSeleccionado, setTrabajoSeleccionado] = useState(null);
    const [cargando, setCargando] = useState(true);

    const cargarTrabajos = async () => {
        try {
            setCargando(true);

            const res = await ObtenerTrabajosActivos();

            if (res?.header?.codigo === 200 && Array.isArray(res.body)) {

                const filtrados = res.body.filter(t => {
                    if (rol === "CLIENTE") {
                        return t.creador?.id === userId;
                    } else {
                        return t.trabajadorAsignado?.id === userId;
                    }
                });

                setTrabajos(filtrados);
            } else {
                setTrabajos([]);
            }

        } catch {
            setTrabajos([]);
        } finally {
            setCargando(false);
        }
    };

    useEffect(() => {
        cargarTrabajos();
    }, []);

    if (cargando) {
        return (
            <div className="container mt-5">
                <h5>Cargando trabajos...</h5>
            </div>
        );
    }

    const mapEstado = {
        PE: "Pendiente",
        AS: "Asignado",
        EP: "En proceso",
        FN: "Finalizado"
    };

    return (
        <div className="container mt-5">
            <h4 className="fw-bold mb-4 text-purple">
                {rol === "CLIENTE"
                    ? "Solicitudes activas"
                    : "Trabajos asignados"}
            </h4>

            {trabajos.length === 0 && (
                <div>
                    {rol === "CLIENTE"
                        ? "No tienes solicitudes activas."
                        : "No tienes trabajos asignados."}
                </div>
            )}

            <div className="row g-4">
                {trabajos.map((t) => (
                    <div className="col-md-6 col-lg-4" key={t.id}>
                        <div
                            className="card trabajo-card h-100"
                            onClick={() => setTrabajoSeleccionado(t)}
                        >
                            <div className="card-body">

                                <h5 className="fw-bold mt-2">
                                    {t.titulo}
                                </h5>

                                <p className="text-muted small">
                                    {t.descripcion}
                                </p>

                                <ul className="list-unstyled small">
                                    <li>Pago: ${t.precioReferencial}</li>
                                    <li>Estado: {mapEstado[t.estado] || t.estado}</li>
                                </ul>

                                <button className="btn btn-subasta w-100 mt-2">
                                    Ver detalle
                                </button>

                            </div>
                        </div>
                    </div>
                ))}
            </div>

            {trabajoSeleccionado && (
                <TrabajoActivoModal
                    data={trabajoSeleccionado}
                    rol={rol}
                    onClose={() => setTrabajoSeleccionado(null)}
                    refresh={cargarTrabajos}
                />
            )}
        </div>
    );
}