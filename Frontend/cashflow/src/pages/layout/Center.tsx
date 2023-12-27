import {FC} from "react";
import {Outlet} from "react-router-dom";

interface CenterProps{

};

export const Center: FC<CenterProps> = ({}) => {
    return(
        <>
            <Outlet/>
        </>
    );
};