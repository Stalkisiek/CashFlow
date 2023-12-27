import Cookies from 'universal-cookie';
import {useEffect, useState} from "react";

const useAccountAuthorization = () => {
    const cookies = new Cookies();
    return cookies.get('auth_token') !== undefined;
};

export default useAccountAuthorization;