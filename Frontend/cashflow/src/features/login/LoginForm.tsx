import {FC, useState} from "react";
import {useNavigate} from "react-router-dom";
import logo from '../../pictures/logoWithoutBackground.png'
import {useLoginApi} from "./api";
import './styles.css';
import '../../fonts/ZilapMonograma-L2J4.ttf'
import {isButtonElement} from "react-router-dom/dist/dom";


interface LoginFormProps{

};

export const LoginForm: FC<LoginFormProps> = ({}) => {
    const[email, changeEmail] = useState('');
    const[password, changePassword] = useState('');

    const navigate = useNavigate();

    const login = useLoginApi();

    async function handleSubmit(e : any){
        e.preventDefault();
        try{
            await login(email, password);

            await navigate('/home');
        }
        catch(e){
            console.log(e);
        }
    }

    async function handleRegisterClick(e : any){
        e.preventDefault();
        await navigate('/register');
    }

    return(
            <form id={'mainForm'} onSubmit={handleSubmit}>
                <img src={logo} alt=""/>
                <div id="email">
                    <label>Email</label>
                    <input type="email" value={email} onChange={(e) => changeEmail(e.target.value)} required={true} placeholder={'email'}/>
                </div>
                <div id="password">
                    <label>Password</label>
                    <input type="password" value={password} onChange={(e) => changePassword(e.target.value)} required placeholder={'password'}/>
                </div>
                <div id='LoginButtonContainer'>
                    <button>Login</button>
                </div>
                <button id='RegisterButton' onClick={handleRegisterClick}>Register</button>
            </form>
    );
};

export default LoginForm;