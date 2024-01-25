import {FC} from "react";
import {useCookies} from "react-cookie";
import {useNavigate} from "react-router-dom";
import '../alerts/styles.css'
import {ServiceResponse} from "../../types/ServiceResponse";
import Swal from "sweetalert2";
import {API_URL} from "../../config";
import {User} from "../../types/User";
import {BankAccount} from "../../types/BankAccount";

export const useAdminPanelApi = () => {
    const navigate = useNavigate();
    const [cookies] = useCookies(['auth_token']);
    const jwt = cookies['auth_token'];

    const headersCreator = () => {
        return {
            'Authorization': `Bearer ${jwt}`,
            'Content-Type': 'application/json',
        };
    }

    const fetchUsersData = async (id?:number, type?:number, name?:string, surname?:string, email?:string): Promise<User[]>  => {
        let url = `${API_URL}/Users`;

        const queryParams = new URLSearchParams();
        if (id !== undefined && id !== 0) {
            queryParams.append('id', id.toString());
        }
        if (type !== undefined && type !== 0) {
            queryParams.append('authLvl', type.toString());
        }
        if (name !== undefined && name !== '') {
            queryParams.append('name', name.toString());
        }
        if (surname !== undefined && surname !== '') {
            queryParams.append('surname', surname.toString());
        }
        if (email !== undefined && email !== '') {
            queryParams.append('email', email.toString());
        }

        // console.log(queryParams);
        url += '?' + queryParams.toString();
        const response = await fetch(url, {
            method: 'GET',
            headers: headersCreator(),
        });

        const serviceResponse:ServiceResponse<User[]> = await response.json();
        if(!serviceResponse.success){
            throw new Error(serviceResponse.message);
        }
        // console.log(serviceResponse.data);
        return serviceResponse.data;
    }

    const fetchUser = async (id: number): Promise<User>  => {
        const response = await fetch(`${API_URL}/Users/${id}`, {
            method: 'GET',
            headers: headersCreator(),
        });

        const serviceResponse:ServiceResponse<User> = await response.json();
        if(!serviceResponse.success){
            Swal.fire({
                title: 'Error!',
                text: serviceResponse.message,
                icon: 'error',
                confirmButtonText: 'Ok'
            });
        }
        console.log(serviceResponse.data);
        return serviceResponse.data;
    }

    const deleteUser = async (id: number) => {
        const response = await fetch(`${API_URL}/Users/${id}`, {
            method: 'DELETE',
            headers: headersCreator(),
        });

        const serviceResponse:ServiceResponse<User> = await response.json();
        if(!serviceResponse.success){
            Swal.fire({
                title: 'Error!',
                text: serviceResponse.message,
                icon: 'error',
                confirmButtonText: 'Ok'
            });
        }
        else{
            Swal.fire({
                title: 'Success!',
                text: 'User deleted successfully',
                icon: 'success',
                confirmButtonText: 'Ok'
            });
        }
        navigate('/admin/users');
    }

    const updateAuthLvl = async (id: number, authLvl: number) => {
        const response = await fetch(`${API_URL}/Users/auth`, {
            method: 'PUT',
            headers: headersCreator(),
            body: JSON.stringify({
                id: id,
                authorizationLevel: authLvl
            })
        });
        const serviceResponse:ServiceResponse<User> = await response.json();
        if(!serviceResponse.success){
            Swal.fire({
                title: 'Error!',
                text: serviceResponse.message,
                icon: 'error',
                confirmButtonText: 'Ok'
            });
        }
        else {
            Swal.fire({
                title: 'Success!',
                text: 'User authorization level updated successfully',
                icon: 'success',
                confirmButtonText: 'Ok'
            });
        }
    }



    return{
        fetchUserData: fetchUsersData, fetchUser: fetchUser, deleteUser: deleteUser, updateAuthLvl: updateAuthLvl
    }
}