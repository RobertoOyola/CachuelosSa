import React, { useState } from "react";
import { uploadFile } from "../../sevices/apis/docuServ";

interface UploaderProps {
    onUploadSuccess: (url: string) => void;
}

const Uploader: React.FC<UploaderProps> = ({ onUploadSuccess }) => {
    const [file, setFile] = useState<File | null>(null);
    const [uploadStatus, setUploadStatus] = useState({
        isUploading: false,
        success: false,
    });
    const [pdfUrl, setPdfUrl] = useState<string | null>(null);

    const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const selectedFile = e.target.files?.[0];
        if (!selectedFile) return;

        if (selectedFile.type !== "application/pdf") {
            alert("Solo se permiten archivos PDF.");
            return;
        }

        setFile(selectedFile);
    };

    const handleUpload = async () => {
        if (!file) return;

        setUploadStatus({ isUploading: true, success: false });

        try {
            const url = await uploadFile(file);
            if (!url) throw new Error("No se pudo subir el archivo");

            setUploadStatus({ isUploading: false, success: true });
            setPdfUrl(url);
            onUploadSuccess(url); // 🔥 devolver la URL al padre
        } catch (err) {
            console.error("Error al subir el archivo:", err);
            alert("Ocurrió un error al subir el archivo.");
            setUploadStatus({ isUploading: false, success: false });
        } finally {
            setFile(null);
        }
    };

    return (
        <div className="space-y-4 p-4 border border-gray-200 rounded-lg">
            <input
                type="file"
                accept="application/pdf"
                id="hiddenFileInput"
                style={{ display: "none" }}
                onChange={handleFileChange}
            />

            {!file && (
                <button
                    className="bg-blue-500 hover:bg-blue-600 text-white px-4 py-2 rounded"
                    onClick={() => document.getElementById("hiddenFileInput")?.click()}
                >
                    Seleccionar PDF
                </button>
            )}

            {file && (
                <div className="flex items-center gap-4">
                    <span className="text-gray-700">{file.name}</span>
                    <button
                        className="bg-green-500 hover:bg-green-600 text-white px-4 py-2 rounded"
                        onClick={handleUpload}
                        disabled={uploadStatus.isUploading}
                    >
                        {uploadStatus.isUploading ? "Subiendo..." : "Subir"}
                    </button>
                </div>
            )}

            {uploadStatus.success && pdfUrl && (
                <div className="text-green-600">
                    ✅ Archivo subido correctamente.{" "}
                    <a
                        href={pdfUrl}
                        target="_blank"
                        rel="noopener noreferrer"
                        className="underline text-blue-600"
                    >
                        Ver PDF
                    </a>
                </div>
            )}
        </div>
    );
};

export default Uploader;
