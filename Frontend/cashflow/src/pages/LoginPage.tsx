import {FC} from "react";
import {HeaderNotLogged} from "./layout/HeaderNotLogged";
import {FooterNotLogged} from "./layout/FooterNotLogged";
import './styles/center.css'
import {LoginForm} from "../features/login/LoginForm";

interface LoginPageProps{

};

export const LoginPage: FC<LoginPageProps> = ({}) => {
    return(
        <div id = 'main'>
            <HeaderNotLogged/>
            <LoginForm/>
            <FooterNotLogged/>
        </div>
    );
};