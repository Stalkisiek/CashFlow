import {FC} from "react";
import {Header} from "./layout/Header";
import {Footer} from "./layout/Footer";
import './styles/center.css'

interface LoginPageProps{

};

export const LoginPage: FC<LoginPageProps> = ({}) => {
    return(
        <div id = 'main'>
            <Header/>

            <Footer/>
        </div>
    );
};