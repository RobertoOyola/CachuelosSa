import { Outlet } from 'react-router-dom'; // `Outlet` es el lugar donde se renderizarán las rutas hijas
import Header from './Header';

const Layout = ({ onLogout }) => {
    return (
        <div className="">
            {/* Pasa onLogout al Header */}
            <Header onLogout={onLogout} />
            <Outlet />  {/* Aquí se renderizarán las rutas hijas */}
        </div>
    );
};

export default Layout;
