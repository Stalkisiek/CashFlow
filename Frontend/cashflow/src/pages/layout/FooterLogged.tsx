import {FC} from "react";
import '../styles/footerLoged.css'
import {useLocation, useNavigate} from "react-router-dom";

interface FooterProps{

};

export const FooterLogged: FC<FooterProps> = ({}) => {
    const navigate = useNavigate();
    const location = useLocation();
    const currUrl = location.pathname;
    return(
        <div id = 'footer'>
            <p className={'myFont'}>©2024 CashFlow S.A. </p>
            <a href="">Cookies</a>
            <p>Help number: 32 356 00 69</p>
            <a href="" onClick={(e) => {navigate('/update/home', {state: currUrl})}}>Change user data</a>
        </div>
    );
};