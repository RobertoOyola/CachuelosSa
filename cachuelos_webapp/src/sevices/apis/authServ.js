import axios from "axios";
const BASE_URL = 'https://localhost:7256/api/Auth/';

const api = axios.create({
    baseURL: BASE_URL,
    withCredentials: true
});

export const login = async (credentials) => {
    try {
        const response = await api.post("login", credentials);
        return response.data;
    } catch (error) {
        if (error.response?.data.header.codigo !== 206) {
            console.error("Error en login:", error.response?.data || error.message);
        }
        return error.response?.data;
    }
};

export const Register = async (credentials) => {
    try {
        const response = await api.post('Register', credentials);
        return response.data
    } catch (error) {
        if (error.response?.data.header.codigo !== 409) {
            console.error("Error en Register:", error.response?.data || error.message);
        }
        return error.response?.data;
    }
};

export const logout = async () => {
    try {
        const response = await api.post("Logout");
        return response.data;
    } catch (error) {
        console.error("Error en login:", error.response?.data || error.message);
        return error.response?.data;
    }
};

export const checkAuth = async () => {
    try {
        const response = await api.get("check");
        return response.data.isAuthenticated;
    } catch (error) {
        return false;
    }
};

//#region Envio Otp
export const EmailOtpVerificarUsu = async (Mail) => {
    try {
        const response = await api.post("EmailOtpVerificarUsu", Mail);
        return response.data;
    } catch (error) {
        console.error("Error en envio otp:", error.response?.data.header.mensaje);
        return error.response?.data
    }
};

export const EmailOtpCambioContra = async (Mail) => {
    try {
        const response = await api.post("EmailOtpCambioContra", Mail);
        return response.data;
    } catch (error) {
        console.error("Error en envio otp:", error.response?.data.header.mensaje);
        return error.response?.data
    }
};

export const EmailOtpEliminarUsu = async (Mail) => {
    try {
        const response = await api.post("EmailOtpEliminarUsu", Mail);
        return response.data;
    } catch (error) {
        console.error("Error en envio otp:", error.response?.data.header.mensaje);
        return error.response?.data
    }
};

export const EmailOtpIniciarTbj = async (Mail) => {
    try {
        const response = await api.post("EmailOtpIniciarTbj", Mail);
        return response.data;
    } catch (error) {
        console.error("Error en envio otp:", error.response?.data.header.mensaje);
        return error.response?.data
    }
};

export const EmailOtpFinalizarTbj = async (Mail) => {
    try {
        const response = await api.post("EmailOtpFinalizarTbj", Mail);
        return response.data;
    } catch (error) {
        console.error("Error en envio otp:", error.response?.data.header.mensaje);
        return error.response?.data
    }
};
//#endregion

export const VerificarOtp = async (otp) => {
    try {
        const response = await api.post('VerificarOtp', otp);
        return response.data
    } catch (error) {
        console.error("Error al verificar la otp:", error.response?.data || error.message);
        return error.response?.data;
    }
};

export const RecuperarContrasena = async (info) => {
    try {
        const response = await api.post('RecuperarContrasena', info);
        return response.data
    } catch (error) {
        console.error("Error al recuperar la contraseña:", error.response?.data || error.message);
        return error.response?.data;
    }
};

export const estaTokenExpirado = (token) => {
    if (!token) return true;

    const exp = token.exp;
    if (!exp) return true;

    const now = Math.floor(Date.now() / 1000);
    return exp < now;
};
