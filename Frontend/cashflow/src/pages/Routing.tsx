import React, {FC} from "react";
import {Center} from "./layout/Center";
import {LoginPage} from "./LoginPage";
import {Navigate, useRoutes} from "react-router-dom";
import {MainPage} from "./MainPage";
import useAccountAuthorization from "../hooks/useAccountAuthorization";
import {RegisterPage} from './RegisterPage';
import {Main} from "./layout/Main";
import {MainAdmin} from "./layout/MainAdmin";
import {CreateAccountPage} from "./CreateAccountPage";
import {SavingsPage} from "./SavingsPage";
import {CreditPage} from "./CreditPage";
import {ChangeMain} from "./ChangingData/ChangeMain";
import {ChangeNames} from "./ChangingData/ChangeNames";
import {ChangeEmail} from "./ChangingData/ChangeEmail";
import {ChangePassword} from "./ChangingData/ChangePassword";
import useAccountAdministratorLevel from "../hooks/useAccountAdministratorLevel";
import {RequestsPanel} from "../features/requestsPanel/RequestsPanel";
import {UsersPanel} from "../features/usersPanel/UsersPanel";
import {BankAccountsPanel} from "../features/bankAccountsPanel/BankAccountsPanel";

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
                path: "/",
                element: <Navigate to="/login" replace/>
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

const adminRoutes = [
    {
        path: '/',
        element: <MainAdmin/>,
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
                path: 'admin/requests',
                element: <RequestsPanel/>
            },
            {
                path: 'admin/users',
                element: <UsersPanel/>
            },
            {
                path: 'admin/bankAccounts',
                element: <BankAccountsPanel/>
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

export const Routing: FC = function () {
    const isAuthorized = useAccountAuthorization();
    const isAdmin = useAccountAdministratorLevel();

    let routes;

    if (isAuthorized && isAdmin) {
        routes = adminRoutes;
    } else if (isAuthorized) {
        routes = privateRoutes;
    } else {
        routes = publicRoutes;
    }

    return useRoutes(routes);
};