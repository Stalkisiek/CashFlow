import {FC} from "react";
import '../styles/footerLoged.css'

interface FooterProps{

};

export const FooterLogged: FC<FooterProps> = ({}) => {
    return(
        <div id = 'footer'>
            <p className={'myFont'}>©2024 CashFlow S.A. </p>
            <a href="">Cookies</a>
            <p>Help number: 32 356 00 69</p>
        </div>
    );
};