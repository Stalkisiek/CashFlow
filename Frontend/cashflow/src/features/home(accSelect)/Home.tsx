import { FC, useEffect, useState } from "react";
import { useHomeApi, useUserAccountsApi} from "./api";
import {User} from "../../types/User";
import './styles.css'
import {BankAccount} from "../../types/BankAccount";
import savings from '../../pictures/savings.png';
import credit from '../../pictures/credit.png';
import addPhoto from '../../pictures/addAccount.png'
import {useNavigate} from "react-router-dom";

interface HomeProps {}

export const Home: FC<HomeProps> = ({}) => {
    const { fetchData } = useHomeApi();
    const { fetchBankData } = useUserAccountsApi();
    const [user, setUser] = useState<User | undefined>();
    const [bankAccounts, setBankAccounts] = useState<BankAccount[] | undefined>([]);
    const navigate = useNavigate();

    const [counter, setCounter] = useState(0);
    useEffect(() => {
        fetchData()
            .then((newUser ) => {
                setUser(newUser);
            })
            .catch((error) => {
                console.error(error);
            });
        const intervalNr = setInterval(() => {
            fetchData()
                .then((newUser ) => {
                    setUser(newUser);
                })
                .catch((error) => {
                    console.error(error);
                });
            setCounter((prevCounter) => prevCounter + 1);
        }, 1000)
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

    return (
        <div className={'homePageContainer'}>
            <div id={'mainContainer'}>
                <header id={'userData'}>
                    <div id={'Names'}>
                        <p>Name: {user?.name}</p>
                        <p>Surname: {user?.surname}</p>
                    </div>
                    <div id={'AdditionalInfo'}>
                        <p>Email: {user?.email}</p>
                        <p>Account Level: {user?.authorizationLevel === 1 ? 'User' : user?.authorizationLevel === 2 ? 'Admin' : 'Server'}</p>
                    </div>
                    <div id={'idNumber'}>
                        <p>Id: {user?.id}</p>
                    </div>
                </header>
                <div id={'homePageTitle'}>
                    <p>Account Select</p>
                </div>
                <ul id={'bankAccountsList'}>
                    {bankAccounts?.map((account) => (
                        <li key={account.id}>
                            {/* Render the details of each bank account here */}
                            {/*{account.type === 1 ? '../../pictures/savings.png' : '../../pictures/credit.png'}*/}
                            <p>{account.type === 1 ? 'Savings' : account.type === 0 ? 'Add account' : 'Credit'}</p>
                            <img
                                src={account.type === 1 ? savings : account.type === 0 ? addPhoto : credit}
                                alt=""
                                onClick={() => {
                                    switch (account.type) {
                                        case 1:
                                            navigate('/savings');
                                            break;
                                        case 0:
                                            navigate('/createAccount');
                                            break;
                                        default:
                                            navigate('/credit');
                                            break;
                                    }
                                }}
                            />
                        </li>
                    ))}
                </ul>
            </div>
        </div>
    );
};
