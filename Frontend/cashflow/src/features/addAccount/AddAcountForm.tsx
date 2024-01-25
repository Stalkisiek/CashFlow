import {FC} from "react";
import './styles.css'
import {useNavigate} from "react-router-dom";
import savings from "../../pictures/savings.png";
import credit from "../../pictures/credit.png";
import {useAddAccountApi} from "./api";

interface AddAcountFormProps{

};


export const AddAcountForm: FC<AddAcountFormProps> = ({}) => {
    const navigate = useNavigate();
    const createAccount = useAddAccountApi();
    const {fetchData} = createAccount;

    const handleRequest = async(nr: number) => {
        try{
            const ifTrue = await fetchData(nr);
            if(ifTrue){
                await navigate(nr === 1 ? '/savings':'/credit');
            }
            //
        }
        catch (e){
            console.log(e);
            await navigate('/home');
        }
    }


    return (
        <div id={'mainContainer'}>
            <header id={'textContainer'}>
                <p>Which one?</p>
            </header>
            <ul id={'bankAccountsList'}>
                <li>
                    <p>Savings</p>
                    <img src={savings} alt="" onClick={() => handleRequest(1)}/>
                </li>
                <li>
                    <p>Credit</p>
                    <img src={credit} alt="" onClick={() => handleRequest(2)}/>
                </li>
            </ul>
        </div>
    );
};