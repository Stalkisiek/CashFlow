import { useCookies } from "react-cookie";
import { API_URL } from "../../config";
import {ServiceResponse} from "../../types/ServiceResponse";

export const useLoginApi = () => {
    const authCookieName = 'auth_token';
    const [, setCookie] = useCookies([authCookieName]);

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

            if (response.status !== 200) {
                throw new Error('Login failed');
            }

            const JWT = await response.json();
            let service : ServiceResponse<string> = JWT;
            if(service.success === false){
                throw new Error(`Login failed ${service.message}`);
            }

            setCookie(authCookieName, service.data, {
                expires: new Date(Date.now() + 1000 * 60 * 15), // 15 minutes
                sameSite: true,
            });

            console.log(`Logged in successfully ${service.data}`);
        } catch (error) {
            console.error('Login error:', error);
            // Handle the login error, display a message, or redirect the user.
        }
    };
};
