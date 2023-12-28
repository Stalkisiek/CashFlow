import {FC, useState} from "react";
import {useNavigate} from "react-router-dom";
import logo from '../../pictures/logoWithoutBackground.png'
import {useRegisterApi} from "./api";
import './styles.css';
import '../../fonts/ZilapMonograma-L2J4.ttf'
import {isButtonElement} from "react-router-dom/dist/dom";


interface LoginFormProps{

};

export const LoginForm: FC<LoginFormProps> = ({}) => {
    const[name, changeName] = useState('');
    const[surname, changeSurname] = useState('');
    const[email, changeEmail] = useState('');
    const[password, changePassword] = useState('');

    const navigate = useNavigate();
    const register = useRegisterApi();

    async function handleSubmit(e : any){
        e.preventDefault();
        try{
            await register(name, surname, email, password);
            await navigate('/home');
        }
        catch(e){
            console.log(e);
        }
    }

    async function handleLoginClick(e : any){
        e.preventDefault();
        await navigate('/login');
    }

    return(
        <form onSubmit={handleSubmit}>
            <div id="name">
                <label>Name</label>
                <input type="text" value={name} onChange={(e) => changeName(e.target.value)} required={true} placeholder={'name'}/>
            </div>
            <div id="surname">
                <label>Surname</label>
                <input type="text" value={surname} onChange={(e) => changeSurname(e.target.value)} required={true} placeholder={'surname'}/>
            </div>
            <div id="email">
                <label>Email</label>
                <input type="email" value={email} onChange={(e) => changeEmail(e.target.value)} required={true} placeholder={'email'}/>
            </div>
            <div id="password">
                <label>Password</label>
                <input type="password" value={password} onChange={(e) => changePassword(e.target.value)} required placeholder={'password'}/>
            </div>
            <div id="RegisterButtonContainer">
                <button>Register</button>
            </div>
            <button id='LoginButton' onClick={handleLoginClick}>Login</button>
        </form>
    );
};

export default LoginForm;