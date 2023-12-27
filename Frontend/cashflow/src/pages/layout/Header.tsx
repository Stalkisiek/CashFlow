import {FC} from "react";
import logo from '../../pictures/logo.png'
import '../styles/HeaderLogged.css'

interface HeaderProps{

};

export const Header: FC<HeaderProps> = ({}) => {
    return(
        <div id = 'header'>
            <img src={logo} alt="logo"/>
        </div>
    );
};