import axios from "axios";


const BASE_URL = 'https://localhost:7256/api/User/';

const api = axios.create({
    baseURL: BASE_URL,
    withCredentials: true
});

function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
}

export const parseJwt = (token) => {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
}

export const obtenerInfoToken = async () => {
    try {
        const token = getCookie('auth_token');
        if (token) {
            const decodedToken = parseJwt(token);
            return decodedToken;
        } else {
            return null;
        }
    } catch (error) {
        console.error("Error al validar el token");
        return null;
    }
};

export const updateUser = async (user) =>{
    try{
        const response = await api.post('ActualizarUsuario', user);
        return response.data
    } catch (error) {
        if (error.response?.data.header.codigo !== 409){
            console.log(error)
        console.error("Error al Actualizar Usuario:", error.response?.data || error.message);
        }
        return error.response?.data;
    }
};

export const obtenerUser = async () =>{
    try{
        const response = await api.post('ObtenerUser');
        return response.data
    } catch (error) {
        if (error.response?.data.header.codigo !== 409){
        console.error("Error al Obtener Usuario:", error.response?.data || error.message);
        }
        return error.response?.data;
    }
};

export const obtenerOtrosUser = async (id) =>{
    try{
        const response = await api.post('ObtenerUsuarioOtros', id);
        return response.data
    } catch (error) {
        if (error.response?.data.header.codigo !== 409){
        console.error("Error al Obtener Usuario:", error.response?.data || error.message);
        }
        return error.response?.data;
    }
};
