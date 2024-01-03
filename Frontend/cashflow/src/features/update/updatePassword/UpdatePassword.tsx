import {FC, useState} from "react";
import {useApi} from "./api";
import {useLocation, useNavigate} from "react-router-dom";
import './styles.css'

export const UpdatePassword = ({}) => {
    const[prevPassword,changePrevPassword] = useState('');
    const[newPassword, changeNewPassword] = useState('');
    const {fetchUserData} = useApi();
    const navigate = useNavigate();
    const location = useLocation();
    const handleClick = (e:any) => {
        e.preventDefault();
        fetchUserData(prevPassword,newPassword);
        navigate('/home');
    }

    return(
        <div className={'changePasswordContainer'}>
            <form action="" id={'mainForm'}>
                <div id={'name'}>
                    <label htmlFor="">Previous password</label>
                    <input type="password" onChange={(e) => changePrevPassword(e.target.value)} placeholder={'Previous password'} autoComplete={'password'}/>
                </div>
                <div id={'surname'}>
                    <label htmlFor="">New password</label>
                    <input type="password" onChange={(e) => changeNewPassword(e.target.value)} placeholder={'New password'}/>
                </div>
                <button onClick={(e) => handleClick(e)}>Update</button>
                {/*<button id={'backButton'} onClick={() => navigate(location.state)}>Back</button>*/}
            </form>

        </div>
    );
};