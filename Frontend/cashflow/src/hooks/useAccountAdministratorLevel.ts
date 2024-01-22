import Cookies from 'universal-cookie';
import {useEffect, useState} from "react";

const useAccountAuthorization = () => {
    const cookies = new Cookies();
    return cookies.get('is_admin') !== undefined;
};

export default useAccountAuthorization;