import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify"
import { logout } from "../sevices/apis/authServ";
import { cld } from "../sevices/apis/docuServ";
import { fill } from "@cloudinary/url-gen/actions/resize";
import { AdvancedImage } from "@cloudinary/react";

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
            toast.error("Error al cerrar sesión");
        }
    };

    const myImage = cld.image('samples/ecommerce/shoes'); 

  // Resize to 250 x 250 pixels using the 'fill' crop mode.
    myImage.resize(fill().width(500).height(500));

    return(
        <>
            <button
                onClick={handleLogout}>
                LogOut
            </button>
            <AdvancedImage cldImg={myImage} />
        </>
    )
}