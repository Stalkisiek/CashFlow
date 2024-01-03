import React, {FC} from "react";
import {Center} from "./layout/Center";
import {LoginPage} from "./LoginPage";
import {Navigate, useRoutes} from "react-router-dom";
import {MainPage} from "./MainPage";
import useAccountAuthorization from "../hooks/useAccountAuthorization";
import {RegisterPage} from './RegisterPage';
import {Main} from "./layout/Main";
import {CreateAccountPage} from "./CreateAccountPage";
import {SavingsPage} from "./SavingsPage";
import {CreditPage} from "./CreditPage";
import {ChangeMain} from "./ChangingData/ChangeMain";
import {ChangeNames} from "./ChangingData/ChangeNames";
import {ChangeEmail} from "./ChangingData/ChangeEmail";
import {ChangePassword} from "./ChangingData/ChangePassword";

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
              path: '/register',
                element: <RegisterPage/>
            },
            {
                path: "*",
                element: <Navigate to="/login" replace/>
            },
            {
                path: "",
                element: <MainPage/> /// CHANGE THIS!!!!!
            }
        ]
    }
]

const privateRoutes = [
    {
        path: '/',
        element: <Main/>,
        children: [
            {
                path: '/home',
                element: <MainPage/>
            },
            {
              path: '/createAccount',
              element: <CreateAccountPage/>
            },
            {
              path: '/savings',
              element: <SavingsPage/>
            },
            {
              path: '/credit',
                element: <CreditPage/>
            },
            {
              path: '/update/home',
                element: <ChangeMain/>
            },
            {
                path: 'update/names',
                element: <ChangeNames/>
            },
            {
                path: 'update/email',
                element: <ChangeEmail/>
            },
            {
                path: 'update/password',
                element: <ChangePassword/>
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