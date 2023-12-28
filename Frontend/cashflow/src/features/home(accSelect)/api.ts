import { API_URL } from "../../config";
import { ServiceResponse } from "../../types/ServiceResponse";
import {useCookies} from "react-cookie";
import {User} from "../../types/User";
import {BankAccount} from "../../types/BankAccount";

export const useHomeApi = () => {
    const authCookieName = 'auth_token';
    const [cookies] = useCookies([authCookieName]);
    const token = cookies[authCookieName];
    const getAuthHeaders = () => {
        const token = cookies[authCookieName];
        return {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
        };
    };
    const fetchData = async() => {
        try{
            const response = await fetch(`${API_URL}/Users/self`,{
                method: 'GET',
                headers: getAuthHeaders(),
            });

            const serviceResponse: ServiceResponse<User> = await response.json();

            if(!serviceResponse.success){
                console.error(`Error: ${serviceResponse.message}`);
            }

            return serviceResponse.data;
        }
        catch(e){
            console.error(`Request error: ${e}`)
        }
    }

    return{
        fetchData,
    }
}

export const useUserAccountsApi = () => {
    const cookieName = 'auth_token';
    const[cookies] = useCookies([cookieName]);
    const token = cookies[cookieName];

    const getAuthorizationHeaders = () => {
        return {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
        };
    }

    const fetchBankData = async() => {
        try{
            const response = await fetch(`${API_URL}/BankAccounts/Self`, {
                method: 'GET',
                headers: getAuthorizationHeaders(),
            });

            const serviceResponse: ServiceResponse<BankAccount[]> = await response.json();

            if(!serviceResponse.success){
                console.error(`Error: ${serviceResponse.message}`);
            }

            return serviceResponse.data;
        }
        catch(error){
            console.error(error);
        }
    }

    return{
        fetchBankData
    }
}