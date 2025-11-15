import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify"
import { logout } from "../sevices/apis/authServ";

export default function HomePage({onLogout}) {

    const navigate = useNavigate();

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
            const mensaje = error?.header?.mensaje || "Error al cerrar sesión";
            toast.error(mensaje);
        }
    };

    return(
        <>
            <button
                onClick={handleLogout}>
                LogOut
            </button>
        </>
    )
}