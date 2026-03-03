import React, { useState } from "react";
import { uploadFile } from "../../sevices/apis/docuServ";

const Uploader = ({ onUploadSuccess, label }) => {
    const [file, setFile] = useState(null);
    const [fileName, setFileName] = useState("");
    const [uploadStatus, setUploadStatus] = useState({
        isUploading: false,
        success: false,
    });
    const [pdfUrl, setPdfUrl] = useState(null);

    const inputId = `file-input-${label.replace(/\s+/g, '-')}`;

    const handleFileChange = (e) => {
        const selectedFile = e.target.files?.[0];
        if (!selectedFile) return;

        if (selectedFile.type !== "application/pdf") {
            alert("Solo se permiten archivos PDF.");
            return;
        }

        setFile(selectedFile);
        setFileName(selectedFile.name); 
    };

    const handleUpload = async () => {
        if (!file) return;

        setUploadStatus({ isUploading: true, success: false });

        try {
            const url = await uploadFile(file);
            if (!url) throw new Error("No se pudo subir el archivo");

            setUploadStatus({ isUploading: false, success: true });
            setPdfUrl(url);
            onUploadSuccess(url);
        } catch (err) {
            console.error("Error al subir el archivo:", err);
            alert("Ocurrió un error al subir el archivo.");
            setUploadStatus({ isUploading: false, success: false });
        } finally {
            setFile(null);
        }
    };

    return (
        <div className="p-1">
            <input
                type="file"
                accept="application/pdf"
                id={inputId}
                style={{ display: "none" }}
                onChange={handleFileChange}
            />

            {!file && !uploadStatus.success && (
                <button
                    className="px-3 rounded bg-blue-500 text-white"
                    onClick={() => document.getElementById(inputId)?.click()}
                >
                    Seleccionar PDF
                </button>
            )}

            {(file || uploadStatus.success) && (
                <div className="flex items-center gap-4 mt-2">
                    {file && (
                        <button
                            className="px-4 rounded bg-green-600 text-white"
                            onClick={handleUpload}
                            disabled={uploadStatus.isUploading}
                        >
                            {uploadStatus.isUploading ? "Subiendo..." : "Subir"}
                        </button>
                    )}

                    {fileName && !uploadStatus.success && (
                        <span className="text-sm text-gray-700">{fileName}</span>
                    )}

                    {uploadStatus.success && pdfUrl && (
                        <span className="text-green-600 whitespace-nowrap">
                            {fileName} subido correctamente
                        </span>
                    )}
                </div>
            )}
        </div>
    );
};

export default Uploader;
