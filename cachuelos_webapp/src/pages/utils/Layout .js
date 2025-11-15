import { Outlet } from 'react-router-dom';
import Header from './Header';

const Layout = ({ onLogout }) => {
    return (
        <div className="">
            {/* Pasa onLogout al Header */}
            <Header onLogout={onLogout} />
            <Outlet />
        </div>
    );
};

export default Layout;
