import {FC} from "react";
import {HeaderLogged} from "./HeaderLogged";
import {Outlet} from "react-router-dom";
import {FooterLogged} from "./FooterLogged";
import '../styles/center.css'

interface MainProps{

};

export const Main: FC<MainProps> = ({}) => {
    return(
        <div id={'main'}>
            <HeaderLogged/>
            <Outlet/>
            <FooterLogged/>
        </div>
    );
};