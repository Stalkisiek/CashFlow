import { useCookies } from "react-cookie";
import { API_URL } from "../../config";

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

            const JWT = await response.text();

            setCookie(authCookieName, JWT, {
                expires: new Date(Date.now() + 1000 * 60 * 15), // 15 minutes
                sameSite: true,
            });

            console.log('Logged in successfully');
        } catch (error) {
            console.error('Login error:', error);
            // Handle the login error, display a message, or redirect the user.
        }
    };
};
