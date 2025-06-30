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
        console.error("Error en login:", error.response?.data || error.message);
        throw error;
    }
};

export const logout = async () => {
    try {
        const response = await api.post("Logout");
        return response.data;
    } catch (error) {
        console.error("Error en login:", error.response?.data || error.message);
        throw error;
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

export const Register = async (credentials) =>{
    try{
        const response = await api.post('Register', credentials);
        return response.data
    } catch (error) {
        console.error("Error en Register:", error.response?.data || error.message);
        throw error;
    }
}


