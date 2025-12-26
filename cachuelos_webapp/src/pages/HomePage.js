import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify"
import { logout } from "../sevices/apis/authServ";
import { useState } from "react";
import WizardTrabajo from "./modals/WizardTrabajo";
import { AnimatePresence } from "framer-motion";

export default function HomePage({onLogout}) {

    const navigate = useNavigate();

    const [mostrarWizard, setMostrarWizard] = useState(false);

    const handleLogout = async () => {
        try {
            const data = await logout();
            if (data.header.codigo === 200) {
                toast.success("Logout Exitoso!");
                onLogout();
                console.log("Logout ejecutado");
                navigate('/login');
            } else {
                toast.error(data.header.mensaje);
            }
        } catch (error) {
            toast.error("Error al cerrar sesión");
        }
    };
    const handleCrearTrabajo = () => {
        setMostrarWizard(true);
    };

    return(
        <>
            <button
                onClick={handleLogout}>
                LogOut
            </button>
            {/* Botón Crear Trabajo */}
            <button
                className="btn-crear-trabajo" 
                onClick={handleCrearTrabajo}>
                Publicar trabajo
            </button>
            <AnimatePresence mode="wait">
                {mostrarWizard && (
                    <WizardTrabajo
                        key="wizard-trabajo"
                        onClose={() => setMostrarWizard(false)}
                    />
                )}
            </AnimatePresence>
        </>
    )
}