import {FC, useEffect, useState} from "react";
import './styles.css'
import {useSavingsApi, useTransferApi} from "./api";
import {BankAccount} from "../../types/BankAccount";
import savings from '../../pictures/credit.png'
import {API_URL, maxCCredit} from "../../config";

interface SavingsProps{

};

export const Credit: FC<SavingsProps> = ({}) => {
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
                <div className={'mainSegment'}>
                    <p className={'pulsingValue'}>Balance: {bankAccount?.balance}$</p>
                    <p className={'presentation'}>Deposit</p>
                    <div className={'segment'}>
                        <form action="">
                            <p>{moneyAddValue}$</p>
                            <input type={'range'} value={moneyAddValue} min={1} max={10_001} step={1_000} onChange={(e)=>
                            {if(Number(e.target.value) > 1){
                                changeMoneyAddValue(Number(e.target.value) - 1);
                            }
                            else{
                                changeMoneyAddValue(Number(e.target.value));
                            }}}/>
                            <button id={'AddingMoneyButton'} onClick={(e) => handleClick(e,3,moneyAddValue)}>Add money</button>
                        </form>
                    </div>
                    <p className={'presentation'}>ATM</p>
                    <div className={'segment'}>
                        <form action="">
                            <p>{moneyPayValue}$</p>
                            <input type="range" value={moneyPayValue} min={1} max={bankAccount?.balance} onChange={(e) => {changeMoneyPayValue(Number(e.target.value))}} />
                            <button id={'PayButton'} onClick={(e) => handleClick(e, 1,moneyPayValue)}>Pay</button>
                        </form>
                    </div>
                </div>
                <div className={'mainSegment'}>
                    <p className={'pulsingValue'}>Credit: {bankAccount?.creditBalance}$</p>
                    <p className={'presentation'}>Take credit</p>
                    <div className={'segment'}>
                        <form action="">
                            <p>{creditAddValue}$</p>
                            <input type="range" value={creditAddValue} min={1} max={Number(maxCCredit) - (bankAccount?.creditBalance ?? 0)} onChange={(e) => changeCreditAddValue(Number(e.target.value))}/>
                            <button id={'AddingCreditButton'} onClick={(e) => handleClick(e,4,creditAddValue)}>Add credit</button>
                        </form>
                    </div>
                    <p className={'presentation'}>Pay credit</p>
                    <div className={'segment'}>
                        <form action="">
                            <p>{creditPayValue}$</p>
                            <input type="range" value={creditPayValue} min={1} max={bankAccount?.creditBalance} onChange={(e) => changeCreditPayValue(Number(e.target.value))}/>
                            <button id={'PayingUpCreditButton'} onClick={(e) => handleClick(e,2,creditPayValue)}>Pay up</button>
                        </form>
                    </div>
                </div>
            </div>
            <div id={'transferBox'}>
                <form action="">
                    <p>{transferAmount}$</p>
                    <input
                        type="range"
                        min={1}
                        max={bankAccount?.balance}
                        value={transferAmount} // Set the starting value to 1
                        onChange={(e) => changeTransferAmount(Number(e.target.value))}
                    />
                    <input type="number" onChange={(e) => changeTargetId(Number(e.target.value))} required={true} placeholder={'Id'}/>
                    <button id={'TransferButton'} onClick={(e) => handleTransfer(e,targetId,transferAmount)}>Transfer</button>
                </form>
            </div>
        </div>
    );
};