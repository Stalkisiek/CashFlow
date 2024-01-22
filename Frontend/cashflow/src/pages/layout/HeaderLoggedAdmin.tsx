import React, { FC } from "react";
import logo from '../../pictures/logo.png';
import '../styles/headerLoggedAdmin.css'
import { useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faSignOutAlt } from "@fortawesome/free-solid-svg-icons";
import { useCookies } from "react-cookie";

interface HeaderProps { }

export const HeaderLoggedAdmin: FC<HeaderProps> = ({ }) => {
    const [cookies, setCookie, removeCookie] = useCookies(["auth_token", "is_admin"]);
    const navigate = useNavigate();

    const handleLogout = () => {
        removeCookie("auth_token");
        removeCookie("is_admin")// Remove the auth_token cookie
        document.cookie = 'auth_token' + '=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
        // Redirect to the login page or any other desired destination
        navigate("/login"); // Update the destination route as needed
        window.location.reload();
    }

    const handleAdminPanel = () => {
        navigate('/admin/panel');
    }

    return (
        <div id='headerIsAdmin'>
            <img src={logo} alt="logo" onClick={() => { navigate('/home') }} />
            <div className={'elementsOnTheRight'}>
                <div id={'adminPanelButton'} onClick={handleAdminPanel}>
                    <p>Admin Panel</p>
                </div>
                <div id='logOut' onClick={handleLogout}>
                    <p>Logout <FontAwesomeIcon icon={faSignOutAlt} /></p>
                </div>
            </div>
        </div>
    );
};
