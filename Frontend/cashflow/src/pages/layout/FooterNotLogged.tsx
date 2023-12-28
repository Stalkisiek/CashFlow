import {FC} from "react";
import '../styles/footerNotLoged.css'

interface FooterProps{

};

export const FooterNotLogged: FC<FooterProps> = ({}) => {
    return(
        <div id = 'footer'>
            <p className={'myFont'}>©2024 CashFlow S.A. </p>
            <a href="">Cookies</a>
            <p>Help number: 32 356 00 69</p>
        </div>
    );
};