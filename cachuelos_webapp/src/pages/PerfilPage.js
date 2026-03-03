import { useParams } from "react-router-dom";
import PerfilInfo from "./utils/PerfilInfo";
import TrabajosActivos from "./utils/TrabajosActivos";
import HistorialContratantes from "./utils/HistorialContratantes";
import HistorialTrabajadores from "./utils/HistorialTrabajadores";
import { useEffect, useState } from "react";
import { obtenerInfoToken } from "../sevices/apis/userServ";

import '../css/PerfilPage.css';

export default function PerfilPage() {
    const { id } = useParams();
    const [ownPerfil, setOwnPerfil] = useState(false);

    useEffect(() => {
    validarPerfil();
}, [id]);

    const validarPerfil = async () => {
        const token = await obtenerInfoToken();
        if (token && parseInt(token.Id) === parseInt(id)) {
            setOwnPerfil(true);
        } else {
            setOwnPerfil(false);
        }
    };

    return (
        <>
            <div className="perfil-scroll-container">
            <PerfilInfo id={id} />

            {ownPerfil && (
                <>
                    <TrabajosActivos rol="CLIENTE" userId={parseInt(id)} />
                    <TrabajosActivos rol="TRABAJADOR" userId={parseInt(id)} />
                </>
            )}

            <HistorialTrabajadores />
            <HistorialContratantes />
        </div>
        </>
    );
}
