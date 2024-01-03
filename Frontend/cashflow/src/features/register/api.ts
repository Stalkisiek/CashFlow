import { useCookies } from "react-cookie";
import { API_URL } from "../../config";
import {ServiceResponse} from "../../types/ServiceResponse";
import Swal from "sweetalert2";

export const useRegisterApi = () => {
    const authCookieName = 'auth_token';
    const [, setCookie] = useCookies([authCookieName]);

    return async(name: string, surname:string, email: string, password: string) => {
        try{
            const response = await fetch(`${API_URL}/Auth/Register`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Access-Control-Allow-Origin': '*',
                },
                body: JSON.stringify({
                    name: name,
                    surname: surname,
                    email: email,
                    password: password,
                }),
            });

            const serviceResponse: ServiceResponse<string> = await response.json();

            if (!serviceResponse.success) {
                Swal.fire({
                    title: 'Error!',
                    text: serviceResponse.message,
                    icon: 'error',
                    confirmButtonText: 'Confirm'
                });
                throw new Error(serviceResponse.message);
            }
            setCookie(authCookieName, serviceResponse.data, {
                expires: new Date(Date.now() + 1000 * 60 * 15), // 15 minutes
                sameSite: true,
            });

        }
        catch(error){
            console.log(`Error: ${error}`);
        }
    }
}