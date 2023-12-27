import {FC} from "react";
import '../styles/footerLoged.css'

interface FooterProps{

};

export const Footer: FC<FooterProps> = ({}) => {
    return(
        <div id = 'footer'>
            <p>©2024 CashFlow S.A. </p>
            <a href="">Cookies</a>
            <p>Help number: <a href="">32 356 00 69</a></p>
        </div>
    );
};