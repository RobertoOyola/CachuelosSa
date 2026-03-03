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

export const TraerTrabajos = async () => {
    try {
        const response = await api.post('TraerTrabajos');
        return response.data;
    } catch (error) {
        console.error("Error al traer trabajo:", error.response?.data.header.mensaje);
        return error.response?.data;
    }
};

export const CrearOferta = async (form) => {
    try {
        const response = await api.post('CrearOferta', form);
        return response.data;
    } catch (error) {
        console.error("Error al crear oferta:", error.response?.data.header.mensaje);
        return error.response?.data;
    }
};

export const HistorialContratante = async () => {
    try {
        const response = await api.post('HistorialContratante');
        return response.data;
    } catch (error) {
        console.error("Error el historial trabajos:", error.response?.data.header.mensaje);
        return error.response?.data;
    }
};

export const HistorialTrabajador = async () => {
    try {
        const response = await api.post('HistorialTrabajador');
        return response.data;
    } catch (error) {
        console.error("Error el historial trabajos:", error.response?.data.header.mensaje);
        return error.response?.data;
    }
};

export const SeleccionarGanador = async (form) => {
    try {
        const response = await api.post("SeleccionarGanador", form);
        return response.data;
    } catch (error) {
        return error.response?.data;
    }
};

export const SubirComprobante = async (form) => {
    try {
        const response = await api.post("SubirComprobante", form);
        return response.data;
    } catch (error) {
        return error.response?.data;
    }
};

export const GenerarOtpInicio = async (idTrabajo) => {
    try {
        const response = await api.post("GenerarOtpInicio", idTrabajo);
        return response.data;
    } catch (error) {
        return error.response?.data;
    }
};

export const ConfirmarFinalizacion = async (form) => {
    try {
        const response = await api.post("ConfirmarFinalizacion", form);
        return response.data;
    } catch (error) {
        return error.response?.data;
    }
};

export const ObtenerTrabajosActivos  = async () => {
    try {
        const response = await api.post("TrabajosActivos");
        return response.data;
    } catch (error) {
        return error.response?.data;
    }
};

export const ConfirmarInicioTrabajo = async (form) => {
    try {
        const response = await api.post("ConfirmarInicioTrabajo", form);
        return response.data;
    } catch (error) {
        return error.response?.data;
    }
};

export const GenerarOtpFinal = async (form) => {
    try {
        const response = await api.post("GenerarOtpFinal", form);
        return response.data;
    } catch (error) {
        return error.response?.data;
    }
};