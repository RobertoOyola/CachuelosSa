import { useEffect, useState } from "react";
import { TieneInfoUser } from "../sevices/apis/userServ";
import BienvenidoStepper from "./modals/BienvenidoStepper";

import '../css/HomePage.css';
import SubastaModal from "./modals/SubastaModal";
import { TraerTrabajos } from "../sevices/apis/subsServ";

export default function HomePage({ onLogout }) {
    const [mostrarBienvenidoStepper, setMostrarsetBienvenidoStepper] = useState(false);
    const [subastaSeleccionada, setSubastaSeleccionada] = useState(null);
    const [trabajos, setTrabajos] = useState([]);
    const [loadingTrabajos, setLoadingTrabajos] = useState(true);

    useEffect(() => {
        const verificar = async () => {
            const data = await TieneInfoUser();
            if (!data.body === false) {
                setMostrarsetBienvenidoStepper(true);
            }
        };
        verificar();
    }, []);

    useEffect(() => {
        const cargarTrabajos = async () => {
            setLoadingTrabajos(true);
            const data = await TraerTrabajos();

            if (data?.header?.codigo === 200) {
                setTrabajos(data.body);
            }

            setLoadingTrabajos(false);
        };

        cargarTrabajos();
    }, []);

    return (
        <>
            {mostrarBienvenidoStepper && <BienvenidoStepper />}
            {subastaSeleccionada && (
                <SubastaModal
                    subasta={{
                        ...subastaSeleccionada,
                        publicadoPor: "Empresa Creativa EC"
                    }}
                    onClose={() => setSubastaSeleccionada(null)}
                />
            )}

            <div className="container mt-5">
                <h3 className="mb-4 fw-bold text-purple">
                    Trabajos abiertos a subasta
                </h3>

                <div className="row g-4">
                    {loadingTrabajos ? (
                        <p>Cargando trabajos...</p>
                    ) : trabajos.length === 0 ? (
                        <p>No hay trabajos disponibles</p>
                    ) : (
                        trabajos.map((item, index) => {
                            const trabajo = item.trabajo;

                            return (
                                <div className="col-md-6 col-lg-4" key={trabajo.id}>
                                    <div className="card h-100 shadow-sm border-0 rounded-4">
                                        <div className="card-body">
                                            <h5 className="card-title fw-bold mt-2">
                                                {trabajo.titulo}
                                            </h5>
                                            <p className="text-muted small">
                                                {trabajo.descripcion}
                                            </p>

                                            <ul className="list-unstyled small mb-3">
                                                <li>Presupuesto: <b>${trabajo.precioReferencial}</b></li>
                                                <li>Fecha trabajo: {new Date(trabajo.fechaTrabajo).toLocaleDateString()}</li>
                                                <li>Ofertas: <b>{item.subastaOfertas?.length || 0}</b></li>
                                                <li>Termina subasta:{" "}<b>{new Date(trabajo.fechaFinSubasta).toLocaleDateString()}</b></li>
                                            </ul>

                                            <button
                                                className="btn btn-subasta w-100"
                                                onClick={() => setSubastaSeleccionada(item)}
                                            >
                                                Ver subasta
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            );
                        })
                    )}
                </div>
            </div>
        </>
    );
}
