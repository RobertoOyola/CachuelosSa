import { useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import {
    EmailOtpCambioContra,
    EmailOtpIniciarTbj,
    VerificarOtp,
    EmailOtpVerificarUsu
} from '../../sevices/apis/authServ';

import './OtpModal.css'

export default function OtpModal({ onClose, actionType, email, onSuccess }) {
    const [otp, setOtp] = useState('');
    const [newEmail, setNewEmail] = useState(email || '');
    const [step, setStep] = useState(1); // 1: Ingresar correo, 2: Ingresar OTP
    const [loading, setLoading] = useState(false);
    const [title, settitle] = useState('');

    useEffect(() => {
        switch (actionType) {
            case 'CAMBIO_CONTRA':
                settitle('Recuperar Contraseña');
                break;
            case 'INICIO_TBJ':
                settitle('Iniciar Trabajo');
                break;
            case 'VERIFICAR_USU':
                settitle('Verificacion de Usuario');
                break;
            case 'FINALIZAR_TBJ':
                settitle('Finalizar Trabajo');
                break;  
            default:
                settitle('Autentificacion Otp');
        }
    }, [actionType, settitle])

    const bloquearCorreo =
        actionType === 'INICIO_TBJ' ||
        actionType === 'FINALIZAR_TBJ' ||
        actionType === 'VERIFICAR_USU';


    const isEmailValid = /^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$/.test(newEmail);

    const handleEmailSubmit = async (e) => {
        e.preventDefault();
        if (!newEmail.trim()) return toast.error("Ingrese su correo electrónico");

        try {
            setLoading(true);
            let response;
            switch (actionType) {
                case 'CAMBIO_CONTRA':
                    response = await EmailOtpCambioContra({ Mail: newEmail });
                    break;
                case 'INICIO_TBJ':
                    response = await EmailOtpIniciarTbj({ Mail: newEmail });
                    break;
                case 'VERIFICAR_USU':
                    response = await EmailOtpVerificarUsu({ Mail: newEmail });
                    break;
                default:
                    response = await EmailOtpCambioContra({ Mail: newEmail });
            }

            if (response?.header?.codigo === 200) {
                toast.success(response.body.message);
                setStep(2);
            } else {
                toast.error(response.header.mensaje);
            }
        } catch (error) {
            toast.error("Error al enviar el OTP", error);
        } finally {
            setLoading(false);
        }
    };


    const handleOtpSubmit = async (e) => {
        e.preventDefault();
        if (!otp.trim()) return toast.error("Ingrese el OTP");

        try {
            setLoading(true);
            const response = await VerificarOtp({ otp });
            if (response?.header?.codigo === 200) {
                toast.success(response?.header?.mensaje);
                onSuccess(response);
                onClose();
            } else {
                toast.error(response.header.mensaje);
            }
        } catch (error) {
            toast.error("Error al verificar el OTP ");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="otp-modal">
            <div className="modal-Otp-content">
                {step === 1 ? (
                    <form onSubmit={handleEmailSubmit}>
                        <h3>{title}</h3>
                        <div className='row'>
                            <label
                                className=' 
                                    col-5 
                                    d-flex 
                                    align-items-center 
                                    text-center'>
                                Correo Electrónico
                            </label>
                            <input
                                className='form-control col'
                                type="email"
                                value={newEmail}
                                onChange={(e) => setNewEmail(e.target.value)}
                                placeholder="correo@ejemplo.com"
                                disabled={bloquearCorreo}
                                required
                            />
                        </div>
                        <div
                            className='
                                d-grid 
                                col-8 
                                mx-auto
                                p-2'>
                            <button
                                type="submit"
                                disabled={!isEmailValid || loading}
                                className={`btn ${isEmailValid ? 'btn-primary' : 'btn-secondary'} w-100`}>
                                {loading ? 'Enviando...' : 'Enviar OTP'}
                            </button>
                        </div>
                    </form>
                ) : (
                    <form onSubmit={handleOtpSubmit}>
                        <h3>Ingresa el OTP</h3>
                        <div className='row'>
                            <label
                                className='
                                    col-4
                                    align-items-center
                                    justify-content-center 
                                    d-flex '
                            >
                                OTP
                            </label>
                            <input
                                className="form-control col"
                                type="text"
                                value={otp}
                                onChange={(e) => {
                                    const value = e.target.value;
                                    if (/^\d{0,6}$/.test(value)) {
                                        setOtp(value);
                                    }
                                }}
                                placeholder="Ingresa el OTP"
                                required
                            />
                        </div>
                        <div
                            className='
                                d-grid 
                                col-8 
                                mx-auto
                                p-2'>
                            <button id='btn-confirm-otp'
                                type="submit"
                                disabled={loading || otp.length !== 6}
                                className={`btn ${otp.length === 6 ? 'btn-primary' : 'btn-secondary'} w-100`}>
                                {loading ? 'Verificando...' : 'Verificar OTP'}
                            </button>
                        </div>
                    </form>
                )}
                <button onClick={onClose}>Cerrar</button>
            </div>
        </div>
    );
}