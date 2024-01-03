import {FC} from "react";
import '../styles/changeMain.css'
import {useLocation, useNavigate} from "react-router-dom";

interface ChangeMainProps{

};



export const ChangeMain: FC<ChangeMainProps> = ({}) => {
    const location = useLocation();
    const navigate = useNavigate();

    const receivedData = location.state;

    const handleBack = () => {
        navigate(receivedData);
    }

    const handleNames = () => {
        navigate('/update/names');
    }

    const handleEmail = () => {
        navigate('/update/email');
    }

    const handlePassword = () => {
        navigate('/update/password');
    }

    return(
        <div className={'changeHomeContainer'}>
            <div id={'mainContainer'}>
                <button onClick={() => handleNames()}>Change names</button>
                <button onClick={() => handleEmail()}>Change email</button>
                <button onClick={() => handlePassword()}>Change password</button>
                <button id={'backButton'} onClick={() => handleBack()}>Back</button>
            </div>
        </div>
    );
};