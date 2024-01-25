import {useCookies} from "react-cookie";
import {API_URL} from "../../config";
import {ServiceResponse} from "../../types/ServiceResponse";
import {BankAccount} from "../../types/BankAccount";
import '../alerts/styles.css'
import swal from "sweetalert";
import Swal from "sweetalert2";


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
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: serviceResponse.message,
                })
                throw new Error(serviceResponse.message);

            }
            Swal.fire({
                icon: 'success',
                title: 'Account created!',
                text: 'Account created successfully!',
            })
            return serviceResponse.success;
        }
        catch (e:any) {
            console.error(e);
        }
    }
    return{
        fetchData
    }
}