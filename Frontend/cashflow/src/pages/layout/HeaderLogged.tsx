import React, {FC} from "react";
import logo from '../../pictures/logo.png'
import '../styles/headerLogged.css'
import {useNavigate} from "react-router-dom";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faSignOutAlt} from "@fortawesome/free-solid-svg-icons";
import {useCookies} from "react-cookie";

interface HeaderProps{

};

const handleLogout = () => {
    document.cookie = 'auth_token' + '=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
    window.location.reload();
}

export const HeaderLogged: FC<HeaderProps> = ({}) => {
    const navigate = useNavigate();
    return(
        <div id = 'header'>
            <img src={logo} alt="logo" onClick={() => {navigate('/home')}}/>
            <div id={'logOut'} onClick={handleLogout}>
                <p>Logout <FontAwesomeIcon icon={faSignOutAlt} /></p>
            </div>
        </div>
    );
};