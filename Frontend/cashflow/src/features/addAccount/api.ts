import {useCookies} from "react-cookie";
import {API_URL} from "../../config";
import {ServiceResponse} from "../../types/ServiceResponse";
import {BankAccount} from "../../types/BankAccount";
import {Simulate} from "react-dom/test-utils";
import error = Simulate.error;

export const useAddAccountApi = () => {
    const cookieName = 'auth_token';
    const[cookies] = useCookies([cookieName]);
    const jwt = cookies[cookieName];

    const getHeaders = () => {
        return {
            'Authorization': `Bearer ${jwt}`,
            'Content-Type': 'application/json',
        };   
    }
    
    const fetchData = async(accountType: number) => {
        try{
            const response = await fetch(`${API_URL}/BankAccounts`,{
                method: 'POST',
                headers: getHeaders(),
                    body: JSON.stringify({
                        type: accountType
                    }),
            }
            )

            const serviceResponse: ServiceResponse<BankAccount> = await response.json();

            if(!serviceResponse.success){
                throw new Error(serviceResponse.message);
            }
        }
        catch (e) {
            alert(e);
        }
    }
    return{
        fetchData
    }
}