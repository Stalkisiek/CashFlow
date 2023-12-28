import { FC, useEffect, useState } from "react";
import { useHomeApi, useUserAccountsApi} from "./api";
import {User} from "../../types/User";
import './styles.css'
import {BankAccount} from "../../types/BankAccount";
import savings from '../../pictures/savings.png';
import credit from '../../pictures/credit.png';
import addPhoto from '../../pictures/addAccount.png'

interface HomeProps {}

export const Home: FC<HomeProps> = ({}) => {
    const { fetchData } = useHomeApi();
    const { fetchBankData } = useUserAccountsApi();
    const [user, setUser] = useState<User | undefined>();
    const [bankAccounts, setBankAccounts] = useState<BankAccount[] | undefined>([]);

    useEffect(() => {
        fetchData()
            .then((newUser ) => {
                setUser(newUser);
            })
            .catch((error) => {
                console.error(error);
            });
        fetchBankData()
            .then((newBankAccounts) => {
                setBankAccounts(newBankAccounts)
            })
            .catch((error) => {
                console.error(error);
            });
    }, []);

    const temp: BankAccount = {
        id: 0,
        type: 0,
        name: 'add',
        balance: 0,
        creditBalance: 0,
        createdAt: '',
        updatedAt: ''
    };
    if (bankAccounts && bankAccounts.length < 2 && (bankAccounts[0] === undefined || bankAccounts[0].type !== 0)) {
        bankAccounts.push(temp);
    }

    console.log(bankAccounts);

    return (
        <div id={'mainContainer'}>
            <header id={'userData'}>
                <div id={'Names'}>
                    <p>Name: {user?.name}</p>
                    <p>Surname: {user?.surname}</p>
                </div>
                <div id={'AdditionalInfo'}>
                    <p>Email: {user?.email}</p>
                    <p>Account Level: {user?.authorizationLevel}</p>
                </div>
                <div id={'idNumber'}>
                    <p>Id: {user?.id}</p>
                </div>
            </header>
            <ul id={'bankAccountsList'}>
                {bankAccounts?.map((account) => (
                    <li key={account.id}>
                        {/* Render the details of each bank account here */}
                        {/*{account.type === 1 ? '../../pictures/savings.png' : '../../pictures/credit.png'}*/}
                        <p>{account.type === 1 ? 'Savings' : account.type === 0 ? 'Add account' : 'Credit'}</p>
                        <img src={account.type === 1 ? savings : account.type === 0 ? addPhoto : credit} alt=""/>
                        {/* Add more details as needed */}
                    </li>
                ))}
            </ul>
        </div>
    );
};
