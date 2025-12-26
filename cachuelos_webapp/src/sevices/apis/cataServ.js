import axios from "axios";


const BASE_URL = 'https://localhost:7256/api/Catalogo/';

const api = axios.create({
    baseURL: BASE_URL,
    withCredentials: true
});

export const ObtenerCatalogoT = async () =>{
    try{
        const response = await api.post('ObtenerCatalogoTrabajo');
        return response.data
    } catch (error) {
        console.error("Error al Obtener Catalogo Trabajo:", error.response?.data.header.mensaje);
        return error.response?.data;
    }
};

export const ObtenerIdCatalogoT = async (nombre) =>{
    try{
        const response = await api.post('ObtenerIdCatalogoTrabajo', nombre);
        return response.data
    } catch (error) {
        console.error("Error al Obtener Id Catalogo:", error.response?.data.header.mensaje);
        return error.response?.data;
    }
};