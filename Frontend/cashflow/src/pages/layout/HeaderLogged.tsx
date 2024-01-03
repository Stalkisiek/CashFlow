import React, { FC } from "react";
import logo from '../../pictures/logo.png';
import '../styles/headerLogged.css';
import { useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faSignOutAlt } from "@fortawesome/free-solid-svg-icons";
import { useCookies } from "react-cookie";

interface HeaderProps { }

export const HeaderLogged: FC<HeaderProps> = ({ }) => {
    const [cookies, setCookie, removeCookie] = useCookies(["auth_token"]);
    const navigate = useNavigate();

    const handleLogout = () => {
        removeCookie("auth_token"); // Remove the auth_token cookie

        // Redirect to the login page or any other desired destination
        navigate("/login"); // Update the destination route as needed
    }

    return (
        <div id='header'>
            <img src={logo} alt="logo" onClick={() => { navigate('/home') }} />
            <div id='logOut' onClick={handleLogout}>
                <p>Logout <FontAwesomeIcon icon={faSignOutAlt} /></p>
            </div>
        </div>
    );
};
