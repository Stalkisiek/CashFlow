import {FC, useState} from "react";
import {useApi} from "./api";
import {useNavigate} from "react-router-dom";
import './styles.css'

export const UpdateEmail = ({}) => {
    const[email, changeEmail] = useState('');
    const {fetchUserData} = useApi();
    const navigate = useNavigate();
    const handleClick = (e:any) => {
        e.preventDefault();
        fetchUserData(email);
        navigate('/home');
    }

    return(
        <div className={'changeEmailContainer'}>
            <div id={'updateEmailTitle'}>
                <p>Update email</p>
            </div>
            <form action="" id={'mainForm'}>
                <div id={'email'}>
                    <label htmlFor="">Email</label>
                    <input type="email" onChange={(e) => changeEmail(e.target.value)} placeholder={'email'} autoComplete={'email'} required={true}/>
                </div>
                <button onClick={(e) => handleClick(e)}>Update</button>
            </form>
        </div>
    );
};