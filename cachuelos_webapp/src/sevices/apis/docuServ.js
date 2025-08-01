import { fill } from "@cloudinary/url-gen/actions/resize";
import { Cloudinary } from "@cloudinary/url-gen/index";
import axios from "axios";

const CLOUDNAME = 'dkbtvl4oz';
const PRESETPERFIL = 'profile';
//const PRESETDOCU = 'documents';

export const cld = new Cloudinary({
    cloud: {
        cloudName: CLOUDNAME
    }
});

export const obtenerFoto = (imgId, size) => {
    try {
        const myImage = cld.image(imgId);
        
        switch (size) {
            case 'icon':
                myImage.resize(fill().width(50).height(50));
                break;

            case 'card':
                myImage.resize(fill().width(500).height(500));
                break;

            case 'header':
                myImage.resize(fill().width(200).height(200));
                break;

            case 'orig':
                
                break;
            default:
                myImage.resize(fill().width(size).height(size));
                break;
        }
        return myImage;
    } catch (error) {
        console.error("Error al obtener la imagen:", error.message);
        throw new Error("No se pudo obtener la imagen.");
    }
};

export const uploadPhoto = async (image) => {
    const formData = new FormData();
    formData.append('file', image);
    formData.append('upload_preset', PRESETPERFIL);

    if (!image || (image.type !== 'image/jpeg' && image.type !== 'image/png')) {
        throw new Error('Solo se permiten imágenes JPG y PNG.');
    }

    try {
        const response = await axios.post(
            `https://api.cloudinary.com/v1_1/${CLOUDNAME}/image/upload`,
            formData
        );
        if (response.data.secure_url) {
            console.log(response.data.public_id);
            const result = response.data.public_id;
            return result;
        }
    } catch (error) {
        throw new Error('Error al cargar la imagen: ' + (error.response?.data?.message || error.message));
    }
};