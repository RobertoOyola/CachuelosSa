
function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
}

function parseJwt(token) {
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