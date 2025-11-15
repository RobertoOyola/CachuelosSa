

import { AdvancedImage } from '@cloudinary/react'
import './PerfilInfo.css'
import { obtenerFoto } from '../../sevices/apis/docuServ'
export default function PerfilInfo() {
    
    const img = obtenerFoto('eqqgrxwezptgzq1skfni', 'orig')

    return(
        <div className="container text-center">
            <div className="row p-3">
                <div className="col">
                    <h1 className="title">
                        Roberto Oyola Vera
                    </h1>
                </div>
            </div>
            <div className='row mt-1'>
                <div className='col'>
                    Descripción
                </div>
                <div className='col'>
                    <AdvancedImage cldImg={img} className='Fperfil' />
                </div>
                <div className='col'>

                </div>
            </div>
        </div>
    )
}