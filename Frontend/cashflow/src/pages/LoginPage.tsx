import {FC} from "react";
import {Header} from "./layout/Header";
import {Footer} from "./layout/Footer";
import './styles/center.css'
import {LoginForm} from "../features/login/LoginForm";

interface LoginPageProps{

};

export const LoginPage: FC<LoginPageProps> = ({}) => {
    return(
        <div id = 'main'>
            <Header/>
            <LoginForm/>
            <Footer/>
        </div>
    );
};