import {FC} from "react";
import {HeaderNotLogged} from "./layout/HeaderNotLogged";
import {FooterNotLogged} from "./layout/FooterNotLogged";
import RegisterForm from "../features/register/RegisterForm";

interface RegisterPageProps{

};

export const RegisterPage: FC<RegisterPageProps> = ({}) => {
    return(
        <div id = 'main'>
            <HeaderNotLogged/>
            <RegisterForm/>
            <FooterNotLogged/>
        </div>
    );
};