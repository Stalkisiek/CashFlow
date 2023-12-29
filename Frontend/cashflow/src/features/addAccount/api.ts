import {useCookies} from "react-cookie";
import {API_URL} from "../../config";
import {ServiceResponse} from "../../types/ServiceResponse";
import {BankAccount} from "../../types/BankAccount";
import '../alerts/styles.css'
import swal from "sweetalert";


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
                swal("Oops!", serviceResponse.message, "error");
                throw new Error(serviceResponse.message);

            }
        }
        catch (e:any) {
            console.error(e);
        }
    }
    return{
        fetchData
    }
}