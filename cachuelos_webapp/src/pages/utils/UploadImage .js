import React, { useState } from 'react';
import { toast } from 'react-toastify';
import { uploadPhoto } from '../../sevices/apis/docuServ';
import { AdvancedImage } from '@cloudinary/react';

export const UploadImage = () => {
    const [image, setImage] = useState(null);
    const [cldImage, setCldImage] = useState(null);
    const [errorMessage, setErrorMessage] = useState('');
    const [uploading, setUploading] = useState(false);

    const handleImageChange = (e) => {
        const selectedImage = e.target.files[0];
        setImage(selectedImage);
        setErrorMessage('');
    };

    const handleUpload = async () => {
        if (!image) {
            toast.error('Por favor selecciona una imagen.');
            return;
        }
        setUploading(true);

        try {
            const myImg = await uploadPhoto(image);  // Subir imagen
            if (myImg) {
                setCldImage(myImg.img);  // Actualizar la imagen cargada
            }
        } catch (error) {
            if (error.message !== errorMessage) {
                setErrorMessage(error.message);
                toast.error(error.message);
            }
        } finally {
            setUploading(false);
        }
    };

    return (
        <div>
            <input type="file" onChange={handleImageChange} />
            <button onClick={handleUpload}>
                {uploading ? 'Subiendo...' : 'Subir Imagen'}
            </button>
            {cldImage && <AdvancedImage cldImg={cldImage} />}
        </div>
    );
};
