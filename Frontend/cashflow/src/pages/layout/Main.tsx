import {FC} from "react";
import {Header} from "./Header";
import {Outlet} from "react-router-dom";
import {Footer} from "./Footer";

interface MainProps{

};

export const Main: FC<MainProps> = ({}) => {
    return(
        <>
            <Header/>
            <Outlet/>
            <Footer/>
        </>
    );
};