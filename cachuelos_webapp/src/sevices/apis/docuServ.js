import { fill } from "@cloudinary/url-gen/actions/resize";
import { Cloudinary } from "@cloudinary/url-gen/index";
import axios from "axios";

const CLOUDNAME = 'dkbtvl4oz';
const PRESETPERFIL = 'profile';
const PRESETDOCU = 'documents';
const BASE_URL = 'https://localhost:7256/api/Documento/';

const api = axios.create({
    baseURL: BASE_URL,
    withCredentials: true
});

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

    const allowedTypes = ['image/jpeg', 'image/png', 'image/webp'];
    if (!image || !allowedTypes.includes(image.type)) {
        throw new Error('Solo se permiten imágenes JPG, PNG o WEBP.');
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

export const uploadFile = async (file) => {
    const formData = new FormData();
    formData.append("file", file);
    formData.append("upload_preset", PRESETDOCU);

    if (!file || file.type !== 'application/pdf') {
        throw new Error('Solo se permiten PDF');
    }

    try {
        const response = await fetch(
            `https://api.cloudinary.com/v1_1/${CLOUDNAME}/raw/upload`,
            {
                method: "POST",
                body: formData,
            }
        );
        const data = await response.json();
        if (data.secure_url) {
            return data.secure_url;
        } else {
            throw new Error("No se pudo subir el archivo.");
        }
    } catch (error) {
        console.error("Error al subir el archivo:", error);
        throw new Error('Error al cargar la imagen: ' + (error.response?.data?.message || error.message));
    }
};

export const updateDocu = async (docu) => {
    try {
        const response = await api.post('CrearDocumento', docu);
        return response.data
    } catch (error) {
        if (error.response?.data.header.codigo !== 409) {
            console.error("Error al Actualizar Documento:", error.response?.data || error.message);
        }
        return error.response?.data;
    }
};

export const ObtenerDocumentosUsuario = async () => {
    try {
        const response = await api.post('ObtenerDocumentosUsuario');
        return response.data
    } catch (error) {
        if (error.response?.data.header.codigo !== 409) {
            console.error("Error al Obtener Documento del Usuario:", error.response?.data || error.message);
        }
        return error.response?.data;
    }
};

export const ObtenerDocumentosUsuarioxId = async (id) => {
    try {
        const response = await api.post('ObtenerDocumentosUsuarioid', id);
        return response.data
    } catch (error) {
        if (error.response?.data.header.codigo !== 409) {
            console.error("Error al Obtener Documento del Usuario:", error.response?.data || error.message);
        }
        return error.response?.data;
    }
};