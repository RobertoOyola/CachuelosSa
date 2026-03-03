import { useEffect, useState } from "react";
import { HistorialTrabajador } from "../../sevices/apis/subsServ";
import { 
    formatearFecha, 
    getEstadoPagoBadge, 
    getEstadoPagoTexto 
} from "./estadoHelpers";

export default function HistorialTrabajadores() {

    const [historial, setHistorial] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchHistorial = async () => {
            const response = await HistorialTrabajador();

            if (response?.header?.codigo === 200) {
                setHistorial(response.body ?? []);
            } else {
                setHistorial([]);
            }

            setLoading(false);
        };

        fetchHistorial();
    }, []);

    if (loading) return <p className="text-center mt-4">Cargando historial...</p>;

    return (
        <div className="container mt-5">
            <h4 className="fw-bold mb-4 text-purple">
                Historial como Trabajador
            </h4>

            <div className="card border-0 shadow-sm rounded-4">
                <div className="card-body p-0">
                    <table className="table mb-0 align-middle">
                        <thead className="table-light">
                            <tr>
                                <th>Trabajo</th>
                                <th>Categoría</th>
                                <th>Fecha</th>
                                <th>Pago</th>
                                <th>Estado Pago</th>
                            </tr>
                        </thead>
                        <tbody>
                            {historial.length === 0 ? (
                                <tr>
                                    <td colSpan="5" className="text-center p-4">
                                        No hay trabajos realizados
                                    </td>
                                </tr>
                            ) : (
                                historial.map((item, index) => (
                                    <tr key={index}>
                                        <td>{item.titulo}</td>
                                        <td>{item.categoria}</td>
                                        <td>{formatearFecha(item.fechaTrabajo)}</td>
                                        <td>${item.pagoRecibido ?? 0}</td>
                                        <td>
                                            <span className={`badge ${getEstadoPagoBadge(item.estadoPago)}`}>
                                                {getEstadoPagoTexto(item.estadoPago)}
                                            </span>
                                        </td>
                                    </tr>
                                ))
                            )}
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    );
}