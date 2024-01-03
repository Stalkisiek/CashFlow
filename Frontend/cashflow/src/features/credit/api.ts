import {useCookies} from "react-cookie";
import {API_URL, maxCCredit, maxSCredit} from "../../config";
import {ServiceResponse} from "../../types/ServiceResponse";
import {BankAccount} from "../../types/BankAccount";
import {useNavigate} from "react-router-dom";
import '../alerts/styles.css'
import Swal from "sweetalert2";

export const useSavingsApi = () => {
    const tokenName = 'auth_token';
    const[cookies] = useCookies([tokenName]);
    const jwt = cookies[tokenName];
    const navigate = useNavigate();

    const headersCreator = () =>{
        return {
            'Authorization': `Bearer ${jwt}`,
            'Content-Type': 'application/json',
        };
    }

    const fetchData = async() => {
        try{
            const response = await fetch(`${API_URL}/BankAccounts/self`, {
                method: 'GET',
                headers: headersCreator(),
            })

            const serviceResponse: ServiceResponse<BankAccount[]> = await response.json();
            if(!serviceResponse.success){
                throw new Error(serviceResponse.message);
            }

            const hasCorrectType = serviceResponse.data.some((bankAccount) => bankAccount.type === 2);
            if(!hasCorrectType){
                navigate('/home');
                throw new Error('You dont have this account');
            }

            const bankAccount:BankAccount|undefined = serviceResponse.data.find((bankAccount) => bankAccount.type === 2);

            return bankAccount;
        }
        catch(e : any){
            console.error(`Account fetch data error: ${e}`);
        }
    }

    return{
        fetchData
    }
}

export const useTransferApi = () => {
    const tokenName = 'auth_token';
    const[cookies] = useCookies([tokenName]);
    const jwt = cookies[tokenName];
    const navigate = useNavigate();

    const headersCreator = () =>{
        return {
            'Authorization': `Bearer ${jwt}`,
            'Content-Type': 'application/json',
            'amount' : '5000'
        };
    }

    const fetchId = async() => {
        try{
            const response = await fetch(`${API_URL}/BankAccounts/self`, {
                method: 'GET',
                headers: headersCreator(),
            })

            const serviceResponse: ServiceResponse<BankAccount[]> = await response.json();
            if(!serviceResponse.success){
                throw new Error(serviceResponse.message);
            }

            const hasCorrectType = serviceResponse.data.some((bankAccount) => bankAccount.type === 2);
            if(!hasCorrectType){
                navigate('/home');
                throw new Error('You dont have this account');
            }

            const bankAccount:BankAccount|undefined = serviceResponse.data.find((bankAccount) => bankAccount.type === 2);

            return bankAccount?.id;
        }
        catch(e:any){
            throw new Error(e);
        }
    }

    const fetchDataTransfer = async(requestType: number, amount:number) => { // 1 - Pay, 2 - PayCredit, 3 - AddMoney; 4 - AddCredit
        try{
            const accountId = await fetchId();
            let requestUrl:string;

            switch (requestType){
                case 1:
                    requestUrl = 'balance/subtract';
                    break;
                case 2:
                    requestUrl = 'credit/subtract';
                    break;
                case 3:
                    requestUrl = 'balance/add';
                    break;
                case 4:
                    requestUrl = 'credit/add';
                    break;
                default:
                    requestUrl = '';
                    break;
            }

            const response = await fetch(`${API_URL}/BankAccounts/${accountId}/${requestUrl}?amount=${amount}`, {
                method: 'PUT',
                headers: headersCreator()
            });

            const serviceResponse:ServiceResponse<BankAccount> = await response.json();

            if(!serviceResponse.success){
                Swal.fire({
                    title: 'Error!',
                    text: serviceResponse.message,
                    icon: 'error',
                    confirmButtonText: 'Confirm'
                })
                throw new Error(serviceResponse.message);
            }
            else{
                Swal.fire({
                    title: 'Success!',
                    text: serviceResponse.message,
                    icon: 'success',
                    confirmButtonText: 'Confirm'
                })
            }

            return serviceResponse.data;
        }
        catch(e:any){
            console.error(e);
        }
    }

    const fetchTransfer = async(targetId: number, amount: number) => {
        try{
            const userId = await fetchId();

            // console.log(`${API_URL}/BankAccounts/${userId}/transfer?targetId=${targetId}&amount=${amount}`);

            const response = await fetch(`${API_URL}/BankAccounts/${userId}/transfer?targetId=${targetId}&amount=${amount}`,{
                method: 'PUT',
                headers: headersCreator(),
            });
            
            const serviceResponse:ServiceResponse<BankAccount> = await response.json();

            if(!serviceResponse.success){
                Swal.fire({
                    title: 'Error!',
                    text: serviceResponse.message,
                    icon: 'error',
                    confirmButtonText: 'Confirm'
                })
                throw new Error(serviceResponse.message);
            }
            else{
                Swal.fire({
                    title: 'Success!',
                    text: serviceResponse.message,
                    icon: 'success',
                    confirmButtonText: 'Confirm'
                })
            }

            return serviceResponse.data;
        }
        catch (e) {
            console.error(e);
        }
    }

    return {
        fetchDataTransfer, fetchTransfer
    }
}