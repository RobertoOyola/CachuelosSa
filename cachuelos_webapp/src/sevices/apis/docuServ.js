import { fill } from "@cloudinary/url-gen/actions/resize";
import { Cloudinary } from "@cloudinary/url-gen/index";
import axios from "axios";

const CLOUDNAME = 'dgqyibpj1';
const PRESETPERFIL = 'profile';

export const cld = new Cloudinary({
    cloud: {
        cloudName: CLOUDNAME
    }
});

export const uploadPhoto = async (image) => {
    const formData = new FormData();
    formData.append('file', image);
    formData.append('upload_preset', PRESETPERFIL);

    // Verifica si la imagen no es válida (por tipo de archivo)
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
            const myImage = cld.image(response.data.public_id);
            myImage.resize(fill().width(500).height(500));  // Redimensionamos la imagen
            const result = {
                id: response.data.public_id,
                img: myImage,
            };
            return result;  // Devolvemos el resultado con la imagen redimensionada
        }
    } catch (error) {
        // Lanza un error si algo va mal en la carga
        throw new Error('Error al cargar la imagen: ' + (error.response?.data?.message || error.message));
    }
};