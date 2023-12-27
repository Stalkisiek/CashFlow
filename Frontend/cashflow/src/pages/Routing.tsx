import {FC} from "react";
import {Center} from "./layout/Center";
import {LoginPage} from "./LoginPage";
import {Navigate, useRoutes} from "react-router-dom";
import {MainPage} from "./MainPage";
import useAccountAuthorization from "../hooks/useAccountAuthorization";
import {Main} from "./layout/Main";

const publicRoutes = [
    {
        path: '/',
        element: <Center/>,
        children: [
            {
                path: '/login',
                element: <LoginPage/>
            },
            {
                path: "*",
                element: <Navigate to="/login" replace/>
            },
            {
                path: "",
                element: <MainPage/>
            }
        ]
    }
]

const privateRoutes = [
    {
        path: '/',
        element: <Center/>,
        children: [
            {
                path: '/home',
                element: <MainPage/> /// CHANGE THIS!!!!!
            },
            {
                path: "*",
                element: <Navigate to="/home" replace/>
            }
        ]
    }
]

interface MainPageProps {
}

export const Routing: FC = function() {
    const routes = useAccountAuthorization() ? privateRoutes : publicRoutes;
    return useRoutes(routes);
}