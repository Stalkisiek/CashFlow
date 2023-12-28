import {FC} from "react";
import {Header} from "./layout/Header";
import {Footer} from "./layout/Footer";
import RegisterForm from "../features/register/RegisterForm";

interface RegisterPageProps{

};

export const RegisterPage: FC<RegisterPageProps> = ({}) => {
    return(
        <div id = 'main'>
            <Header/>
            <RegisterForm/>
            <Footer/>
        </div>
    );
};