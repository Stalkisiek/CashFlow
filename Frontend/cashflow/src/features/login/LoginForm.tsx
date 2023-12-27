import {FC, useState} from "react";
import {useNavigate} from "react-router-dom";
import {LoginFormDto} from "./login-form.types";
import {useLoginApi} from "./api";

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

    return(
        <div id="formContainer">
            <form onSubmit={handleSubmit}>
                <label>Email</label>
                <input type="email" value={email} onChange={(e) => changeEmail(e.target.value)} required={true}/>
                <label>Password</label>
                <input type="password" value={password} onChange={(e) => changePassword(e.target.value)} required/>
                <button>Login</button>
            </form>
        </div>
    );
};

export default LoginForm;