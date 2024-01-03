import {useCookies} from "react-cookie";
import {API_URL} from "../../../config";
import {ServiceResponse} from "../../../types/ServiceResponse";
import {User} from "../../../types/User";
import Swal from "sweetalert2";

export const useApi = () => {
    const tokenName = 'auth_token';
    const[cookies] = useCookies([tokenName]);
    const jwt = cookies[tokenName];

    const headersCreator = () =>{
        return {
            'Authorization': `Bearer ${jwt}`,
            'Content-Type': 'application/json',
        };
    }

    const fetchId = async() => {
        const response = await fetch(`${API_URL}/Users/self`, {
            method: 'GET',
            headers: headersCreator(),
        })
        const serviceResponse: ServiceResponse<User> = await response.json();
        return serviceResponse.data.id;
    }

    const fetchUserData = async (prevPassword: string, newPassword: string) => {
        try {
            const id: number = await fetchId();

            const requestBody = JSON.stringify({
                currentPassword: prevPassword,
                newPassword: newPassword,
            });

            const response = await fetch(`${API_URL}/Users/password`, {
                method: 'PUT',
                headers: {
                    'Authorization': `Bearer ${jwt}`,
                    'Content-Type': 'application/json',
                },
                body: requestBody,
            });

            const serviceResponse: ServiceResponse<User> = await response.json();
            if (!serviceResponse.success) {
                Swal.fire({
                    title: 'Error!',
                    text: serviceResponse.message,
                    icon: 'error',
                    confirmButtonText: 'Confirm'
                });
                throw new Error(serviceResponse.message);
            } else {
                Swal.fire({
                    title: 'Success!',
                    text: serviceResponse.message,
                    icon: 'success',
                    confirmButtonText: 'Confirm'
                });
            }

            return serviceResponse.data;
        } catch (e) {
            console.error(e);
        }
    };


    return{
        fetchUserData
    }
}