import {FC} from "react";
import {useCookies} from "react-cookie";
import {useNavigate} from "react-router-dom";
import '../alerts/styles.css'
import {ServiceResponse} from "../../types/ServiceResponse";
import {Request} from "../../types/Request";
import Swal from "sweetalert2";
import {API_URL} from "../../config";

export const useAdminPanelApi = () => {
    const tokenName = 'auth_token';
    const[cookies] = useCookies([tokenName]);
    const jwt = cookies[tokenName];
    const navigate = useNavigate();

    const headersCreator = () =>{
        return {
            'Authorization': `Bearer ${jwt}`,
            'Content-Type': 'application/json',
        };
    }

    const fetchData = async (userId?: number, requestId?: number, requestType?: number) => {
        try {
            let url = `${API_URL}/Requests`;
            console.log(`userId: ${userId}, requestId: ${requestId}, requestType: ${requestType}`);

            const queryParams = new URLSearchParams();
            if (userId !== undefined && userId !== 0) {
                queryParams.append('userId', userId.toString());
            }
            if (requestId !== undefined && requestId !== 0) {
                queryParams.append('requestId', requestId.toString());
            }
            if (requestType !== undefined && requestType !== 0) {
                queryParams.append('requestType', requestType.toString());
            }
            // console.log(queryParams);
            url += '?' + queryParams.toString();

            const response = await fetch(url, {
                method: 'GET',
                headers: headersCreator(),
            });

            const serviceResponse: ServiceResponse<Request[]> = await response.json();
            console.log(url);
            console.log(serviceResponse);

            if (!serviceResponse.success) {
                Swal.fire({
                    title: 'Error!',
                    text: serviceResponse.message,
                    icon: 'error',
                    confirmButtonText: 'Confirm'
                });
                throw new Error(serviceResponse.message);
            }

            return serviceResponse.data;
        } catch (e: any) {
            console.error(e);
        }
    }


    const fulfillRequest = async(id:number, ifAccept:boolean, message:string) => {
        try{
            const repsponse = await fetch(`${API_URL}/Requests/fulfill`, {
                method: 'PUT',
                headers: headersCreator(),
                body: JSON.stringify({
                    id: id,
                    accepted: ifAccept,
                    message: message,
                })
            })
            const serviceResponse : ServiceResponse<number> = await repsponse.json();
            if(!serviceResponse.success){
                Swal.fire({
                    title: 'Error!',
                    text: serviceResponse.message,
                    icon: 'error',
                    confirmButtonText: 'Confirm'
                });
                throw new Error(serviceResponse.message);
            }
            Swal.fire({
                title: 'Success!',
                text: serviceResponse.message,
                icon: 'success',
                confirmButtonText: 'Confirm'
            });
        }
        catch (e:any) {
            console.error(e);
        }
    }
    return{
        fetchData, fulfillRequest
    }
}