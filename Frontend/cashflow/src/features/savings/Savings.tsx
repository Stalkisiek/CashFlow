import {FC, useEffect, useState} from "react";
import './styles.css'
import {useSavingsApi, useTransferApi} from "./api";
import {BankAccount} from "../../types/BankAccount";
import savings from '../../pictures/savings.png'
import {API_URL, maxCCredit, maxSCredit} from "../../config";
import {isNumber} from "util";

interface SavingsProps{

};

export const Savings: FC<SavingsProps> = ({}) => {
    const {fetchData} = useSavingsApi();
    const {fetchDataTransfer, fetchTransfer} = useTransferApi();
    const[bankAccount, changeBankAccount] = useState<BankAccount | undefined>();
    const[moneyAddValue, changeMoneyAddValue] = useState<number>(1);
    const[moneyPayValue, changeMoneyPayValue] = useState<number>(1);
    const[creditAddValue, changeCreditAddValue] = useState<number>(1);
    const[creditPayValue, changeCreditPayValue] = useState<number>(1);


    const [counter, setCounter] = useState(0);

    useEffect(() => {
        // Start an interval when the component mounts
        fetchData()
            .then((newBankAccount) => {
                changeBankAccount(newBankAccount);
            })
            .catch((error) => {
                console.error(error);
            })
        const intervalId = setInterval(() => {
            fetchData()
                .then((newBankAccount) => {
                    changeBankAccount(newBankAccount);
                })
                .catch((error) => {
                    console.error(error);
                })
            setCounter((prevCounter) => prevCounter + 1);
        }, 1000);

        // Clean up the interval when the component unmounts
        return () => {
            clearInterval(intervalId);
        };
    }, []);

    const handleClick = (e:any, typeOf: number, amount: number) => {
        e.preventDefault();
        try{
            fetchDataTransfer(typeOf, amount);
        }
        catch(e:any){
            console.error(e);
        }
    }
    
    
    const[targetId, changeTargetId] = useState<number>(0);
    const[transferAmount, changeTransferAmount] = useState<number>(0);
    const handleTransfer = (e:any, targetedId: number, amount: number) => {
        e.preventDefault();
        try{
            fetchTransfer(targetedId, amount);
        }
        catch (e){
            console.error(e);
        }
    }
    
    return(
        <div id={'mainAccountContainer'}>
            <header id={'accountData'}>
                <img src={savings} alt=""/>
                <div id={'randomDataAccount'}>
                    <p>{bankAccount?.id}</p>
                    {/*<p>Name: {bankAccount?.name}</p>*/}
                </div>
            </header>
            <div id={'accountMoney'}>
                <div className={'segment'}>
                    <p>Balance: {bankAccount?.balance}</p>
                    <form action="">
                        <button id={'AddingMoneyButton'} onClick={(e) => handleClick(e,3,moneyAddValue)}>Add money</button>
                        <input type={'range'} min={1} max={10_001} step={1_000} onChange={(e)=>
                        {if(Number(e.target.value) > 1){
                            changeMoneyAddValue(Number(e.target.value) - 1);
                        }
                        else{
                            changeMoneyAddValue(Number(e.target.value));
                        }}}/>
                        <p>{moneyAddValue}</p>
                    </form>
                    <form action="">
                        <button id={'PayButton'} onClick={(e) => handleClick(e, 1,moneyPayValue)}>Pay</button>
                        <input type="range" min={1} max={bankAccount?.balance} onChange={(e) => {changeMoneyPayValue(Number(e.target.value))}}/>
                        <p>{moneyPayValue}</p>
                    </form>
                </div>
                <div className={'segment'}>
                    <p>Credit: {bankAccount?.creditBalance}</p>
                    <form action="">
                        <button id={'AddingCreditButton'} onClick={(e) => handleClick(e,4,creditAddValue)}>Add credit</button>
                        <input type="range" min={1} max={Number(maxSCredit) - (bankAccount?.creditBalance ?? 0)} onChange={(e) => changeCreditAddValue(Number(e.target.value))}/>
                        <p>{creditAddValue}</p>
                    </form>
                    <form action="">
                        <button id={'PayingUpCreditButton'} onClick={(e) => handleClick(e,2,creditPayValue)}>Pay up</button>
                        <input type="range" min={1} max={bankAccount?.creditBalance} onChange={(e) => changeCreditPayValue(Number(e.target.value))}/>
                        <p>{creditPayValue}</p>
                    </form>
                </div>
            </div>
            <div id={'transferBox'}>
                <form action="">
                    <button id={'TransferButton'} onClick={(e) => handleTransfer(e,targetId,transferAmount)}>Transfer</button>
                    <input type="range" min={1} max={bankAccount?.balance} onChange={(e) => changeTransferAmount(Number(e.target.value))}/>
                    <p>{transferAmount}</p>
                    <input type="number" onChange={(e) => changeTargetId(Number(e.target.value))}/>
                </form>
            </div>
        </div>
    );
};