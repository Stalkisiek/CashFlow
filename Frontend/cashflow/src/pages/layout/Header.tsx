import {FC} from "react";
import logo from '../../pictures/logo.png'
import '../styles/headerLogged.css'
import {useNavigate} from "react-router-dom";

interface HeaderProps{

};

export const Header: FC<HeaderProps> = ({}) => {
    const navigate = useNavigate();
    return(
        <div id = 'header'>
            <img src={logo} alt="logo" onClick={() => {navigate('/')}}/>
        </div>
    );
};