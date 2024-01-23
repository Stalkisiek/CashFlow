import {FC, useState} from "react";
import './styles.css'
import {useApi} from "./api";
import {useNavigate} from "react-router-dom";

export const UpdateNames = () => {
    const[name,changeName] = useState('');
    const[surname, changeSurname] = useState('');
    const {fetchUserData} = useApi();
    const navigate = useNavigate();
    const handleClick = (e:any) => {
        e.preventDefault();
        fetchUserData(name,surname);
        navigate('/home');
    }

    return(
        <div className={'changeNamesContainer'}>
            <div id={'updateNamesTitle'}>
                <p>Update user names</p>
            </div>
                <form action="" id={'mainForm'}>
                    <div id={'name'}>
                        <label htmlFor="">Name</label>
                        <input type="text" onChange={(e) => changeName(e.target.value)} placeholder={'name'}/>
                    </div>
                    <div id={'surname'}>
                        <label htmlFor="">Surname</label>
                        <input type="text" onChange={(e) => changeSurname(e.target.value)} placeholder={'surname'}/>
                    </div>
                    <button onClick={(e) => handleClick(e)}>Update</button>
                </form>
        </div>
    );
};