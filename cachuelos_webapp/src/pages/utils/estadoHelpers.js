export const getEstadoTrabajoTexto = (estado) => {
    const estados = {
        PE: "Pendiente",
        AS: "Asignado",
        EP: "En Proceso",
        FN: "Completado",
        CN: "Cancelado"
    };

    return estados[estado] || estado;
};

export const getEstadoTrabajoBadge = (estado) => {
    switch (estado) {
        case "FN":
            return "bg-success";
        case "EP":
            return "bg-primary";
        case "AS":
            return "bg-info";
        case "PE":
            return "bg-warning text-dark";
        case "CN":
            return "bg-danger";
        default:
            return "bg-secondary";
    }
};

export const getEstadoPagoTexto = (estado) => {
    const estados = {
        PE: "Pendiente",
        PA: "Pagado"
    };

    return estados[estado] || estado;
};

export const getEstadoPagoBadge = (estado) => {
    switch (estado) {
        case "PA":
            return "bg-success";
        case "PE":
            return "bg-warning text-dark";
        default:
            return "bg-secondary";
    }
};

export const formatearFecha = (fecha) => {
    if (!fecha) return "-";
    return new Date(fecha).toLocaleDateString("es-EC", {
        day: "2-digit",
        month: "short",
        year: "numeric"
    });
};