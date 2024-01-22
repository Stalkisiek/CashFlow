import {FC} from "react";
import {HeaderLoggedAdmin} from "./HeaderLoggedAdmin";
import {Outlet} from "react-router-dom";
import {FooterLogged} from "./FooterLogged";
import '../styles/center.css'

interface MainProps{

};

export const MainAdmin: FC<MainProps> = ({}) => {
    return(
        <div id={'main'}>
            <HeaderLoggedAdmin/>
            <Outlet/>
            <FooterLogged/>
        </div>
    );
};