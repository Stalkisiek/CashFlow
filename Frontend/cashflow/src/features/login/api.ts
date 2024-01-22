import { useCookies } from "react-cookie";
import { API_URL } from "../../config";
import {ServiceResponse} from "../../types/ServiceResponse";
import Swal from "sweetalert2";
import {User} from "../../types/User";

const headersCreator = (jwt:string) => {
    return {
        'Authorization' : `Bearer ${jwt}`,
        'Content-Type': 'application/json',
    };
}

export const useLoginApi = () => {
    const authCookieName = 'auth_token';
    const isAdminCookieName = 'is_admin';
    const [, setCookie] = useCookies([authCookieName, isAdminCookieName]);
    const tokenName = 'auth_token';
    const[cookies] = useCookies([tokenName]);
    return async (email: string, password: string) => {
        try {
            const response = await fetch(`${API_URL}/Auth/Login`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Access-Control-Allow-Origin': '*',
                },
                body: JSON.stringify({
                    email: email,
                    password: password,
                }),
            });

            const JWT = await response.json();
            let serviceResponse : ServiceResponse<string> = JWT;
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


            const jwt:string = serviceResponse.data;
            // console.log(jwt);
            const userData = await fetch(`${API_URL}/Users/Self`, {
                method: 'GET',
                headers: headersCreator(jwt),
            })
            const userServiceResponse: ServiceResponse<User> = await userData.json();

            if (!userServiceResponse.success) {
                Swal.fire({
                    title: 'Error!',
                    text: userServiceResponse.message,
                    icon: 'error',
                    confirmButtonText: 'Confirm'
                });
                throw new Error(userServiceResponse.message);
            }

            if(userServiceResponse.data.authorizationLevel > 1){
                setCookie(isAdminCookieName, true, {
                    expires: new Date(Date.now() + 1000 * 60 * 15), // 15 minutes
                    sameSite: true,
                });
            }
            // console.log(`Logged in successfully ${serviceResponse.data}`);
        } catch (error) {
            console.error('Login error:', error);
            // Handle the login error, display a message, or redirect the user.
        }
    };
};
