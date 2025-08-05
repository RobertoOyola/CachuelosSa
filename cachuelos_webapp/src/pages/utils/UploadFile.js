import { useState } from 'react';
import Uploader from '../components/Uploader.tsx'

export default function UploadFile() {
    const [file, setFile] = useState(null);


    return (
        <div className="p-6">
            <Uploader onUploadSuccess={(url) => setFile(url)} />
            
            {file && (
                <div className="mt-4">
                    <p className="text-sm">URL guardada: {file}</p>
                    <iframe
                        src={file}
                        className="w-full h-96 border"
                        title="PDF Viewer"
                    />
                </div>
            )}
        </div>
    );
}
