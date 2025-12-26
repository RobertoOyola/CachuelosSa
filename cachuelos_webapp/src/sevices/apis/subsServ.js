import axios from "axios";


const BASE_URL = 'https://localhost:7256/api/Subasta/';

const api = axios.create({
    baseURL: BASE_URL,
    withCredentials: true
});

export const CrearTrabajo = async (form) => {
    try {
        const response = await api.post('CrearTrabajo', form);
        return response.data;
    } catch (error) {
        console.error("Error al crear trabajo:", error.response?.data.header.mensaje);
        return error.response?.data;
    }
};
