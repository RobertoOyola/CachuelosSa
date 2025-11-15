import { useParams } from "react-router-dom";
import PerfilInfo from "./utils/PerfilInfo";

export default function PerfilPage() {
    const { id } = useParams();
    return(
        <>
            <PerfilInfo id={id} />
        </>
    )
}