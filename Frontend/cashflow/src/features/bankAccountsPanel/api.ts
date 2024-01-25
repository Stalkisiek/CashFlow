import {FC} from "react";
import {useCookies} from "react-cookie";
import {useNavigate} from "react-router-dom";
import '../alerts/styles.css'
import {ServiceResponse} from "../../types/ServiceResponse";
import Swal from "sweetalert2";
import {API_URL} from "../../config";
import {User} from "../../types/User";
import {BankAccount} from "../../types/BankAccount";

export const useAdminPanelApi = () => {
    const navigate = useNavigate();
    const [cookies] = useCookies(['auth_token']);
    const jwt = cookies['auth_token'];

    const headersCreator = () => {
        return {
            'Authorization': `Bearer ${jwt}`,
            'Content-Type': 'application/json',
        };
    }

    const fetchBankAccounts = async (id?:number, type?:number) => {
        try{
            let url = `${API_URL}/BankAccounts`;

            const queryParams = new URLSearchParams();
            if (id !== undefined && id !== 0) {
                queryParams.append('id', id.toString());
            }
            if (type !== undefined && type !== 0) {
                queryParams.append('type', type.toString());
            }

            // console.log(queryParams);
            url += '?' + queryParams.toString();
            const response = await fetch(url, {
                method: 'GET',
                headers: headersCreator(),
            });

            const serviceResponse: ServiceResponse<BankAccount[]> = await response.json();

            if(!serviceResponse.success){
                throw new Error(serviceResponse.message);
            }
            return serviceResponse.data;
        }
        catch (e){
            console.error(e);
        }
    }

    const deleteBankAccount = async (id: number) => {
        try{
            const response = await fetch(`${API_URL}/BankAccounts/${id}`, {
                method: 'DELETE',
                headers: headersCreator(),
            });

            const serviceResponse: ServiceResponse<BankAccount> = await response.json();

            if(!serviceResponse.success){
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: serviceResponse.message,
                });
                throw new Error(serviceResponse.message);
            }
            Swal.fire({
                icon: 'success',
                title: 'Deleted!',
                text: 'Bank account has been deleted.',
            });
            return serviceResponse.success;
        }
        catch (e){
            console.error(e);
        }
    }

    const updateBankAccount = async (id:number, prevType:number, prevBalance:number, prevCredit:number, type?:number, balance?:number, credit?:number) => {
        try{
            let url = `${API_URL}/BankAccounts`;

            const queryParams = new URLSearchParams();
            queryParams.append('id', id.toString());
            if (type !== undefined && type !== 0) {
                queryParams.append('type', encodeURIComponent(type.toString()));
            } else {
                queryParams.append('type', encodeURIComponent(prevType.toString()));
            }
            if (balance !== undefined && balance !== 0) {
                queryParams.append('balance', encodeURIComponent(balance.toString()));
            } else {
                queryParams.append('balance', encodeURIComponent(prevBalance.toString()));
            }
            if (credit !== undefined && credit !== 0) {
                queryParams.append('credit', encodeURIComponent(credit.toString()));
            } else {
                queryParams.append('credit', encodeURIComponent(prevCredit.toString()));
            }


            // console.log(queryParams);
            url += '?' + queryParams.toString();
            const response = await fetch(url, {
                method: 'PUT',
                headers: headersCreator(),
            });

            const serviceResponse: ServiceResponse<BankAccount> = await response.json();
            // console.log(serviceResponse);
            if(!serviceResponse.success){
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: serviceResponse.message,
                });
                throw new Error(serviceResponse.message);
            }
            Swal.fire({
                icon: 'success',
                title: 'Updated!',
                text: 'New data:',
                html: `
                       <p>Account id: ${serviceResponse.data.id}</p>
                       <p>Type: ${serviceResponse.data.type}</p>
                       <p>Balance: ${serviceResponse.data.balance}</p>
                       <p>Credit balance: ${serviceResponse.data.creditBalance}</p>`
            });
            return serviceResponse.success;
        }
        catch (e){
            console.error(e);
        }
    }

    return{
        fetchBankAccounts, deleteBankAccount, updateBankAccount
    }
}