import { useState } from 'react';
import Uploader from '../components/Uploader.js'

export default function UploadFile() {
    const [HistorialPolicial, setHistorialPolicial] = useState(null);
    const [Cedula, setCedula] = useState(null);
    const [Curriculum, setCurriculum] = useState(null);
    const [Titulo, setTitulo] = useState(null);

    return (    
        <div className="p-6">
            <Uploader label="Historial Policial" onUploadSuccess={setHistorialPolicial} />
            <Uploader label="Cédula" onUploadSuccess={setCedula} />
            <Uploader label="Curriculum Vitae" onUploadSuccess={setCurriculum} />
            <Uploader label="Título Profesional" onUploadSuccess={setTitulo} />

            {HistorialPolicial && Cedula && Curriculum && Titulo && (
                <div className="mt-4 p-4 bg-gray-100 rounded">
                    <p className="text-sm">📎 Historial Policial: {HistorialPolicial}</p>
                    <p className="text-sm">📎 Cédula: {Cedula}</p>
                    <p className="text-sm">📎 Curriculum: {Curriculum}</p>
                    <p className="text-sm">📎 Título: {Titulo}</p>
                </div>
            )}
        </div>
    );
}
