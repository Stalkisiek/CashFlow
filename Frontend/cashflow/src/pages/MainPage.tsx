import {FC} from "react";
import {Home} from "../features/home(accSelect)/Home";

interface MainPageProps{

};

export const MainPage: FC<MainPageProps> = ({}) => {
    return(
        <>
            <Home/>
        </>
    );
};