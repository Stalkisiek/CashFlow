import React, { FC, useEffect, useState } from "react";
import './styles.css'
import { API_URL, maxCCredit, maxSCredit } from "../../config";
import { ServiceResponse } from "../../types/ServiceResponse";
import { Request } from "../../types/Request";
import { useAdminPanelApi } from "./api";
import plusPhoto from '../../pictures/plusSign.png'
import minusPhoto from '../../pictures/minusSign.png'
import arrowPhoto from '../../pictures/arrow.png'
import Swal from "sweetalert2";
import {User} from "../../types/User";

interface RequestsPanelProps { }

export const UsersPanel: FC<RequestsPanelProps> = ({ }) => {
    const[showFilters, setShowFilters] = useState(false);
    const[showNewUser, setShowNewUser] = useState(false);
    const[users, setUsers] = useState<User[]>([]);
    const[filterUserId, setFilterUserId] = useState<number>();
    const[filterName, setFilterName] = useState<string>();
    const[filterSurname, setFilterSurname] = useState<string>();
    const[filterEmail, setFilterEmail] = useState<string>();
    const[filterAuthLvl, setFilterAuthLvl] = useState<number>();

    const { fetchUserData, fetchUser, deleteUser, updateAuthLvl} = useAdminPanelApi();

    useEffect(() => {
        fetchUserData(filterUserId, filterAuthLvl, filterName, filterSurname, filterEmail).then((newUsers : User[]) => {
            setUsers(newUsers);
        }).catch((error: any) => {
            console.error(error);
            setUsers([]);
        });
        const fetchInterval = setInterval(() => {
            fetchUserData(filterUserId, filterAuthLvl, filterName, filterSurname, filterEmail).then((newUsers : User[]) => {
                setUsers(newUsers);
            }).catch((error: any) => {
                console.error(error);
                setUsers([]);
            });
        }, 500);

        // Cleanup function
        return () => clearInterval(fetchInterval);
    }, [filterUserId, filterAuthLvl, filterName, filterSurname, filterEmail]); // Dependency list

    const handleUserActionClick = async (userId: number, shouldDeleteUser: boolean) =>{
        if(shouldDeleteUser){
            Swal.fire({
                title: 'Are you sure?',
                text: "You won't be able to revert this!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: "Proceed",
                cancelButtonText: "Go back"
            }).then(async (result) => {
                if (result.isConfirmed) {
                    await deleteUser(userId);
                }
            })
        }
        else{
            Swal.fire({
                title: 'Are you sure?',
                text: "You want to change user authorization level?",
                icon: 'warning',
                input: 'select',
                inputOptions: {
                    '1': 'User',
                    '2': 'Admin',
                    '3': 'Server'
                },
                inputPlaceholder: 'Select user authorization level',
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: "Proceed",
                cancelButtonText: "Go back"
            }).then((result) => {
                if (result.isConfirmed) {
                    updateAuthLvl(userId, Number(result.value));
                }
            })
        }
    }

    const handleUserClick = async (userId:number) => {
        const userData = await fetchUser(userId);
        if(userData){
            Swal.fire({
                title: `User data`,
                html: `<p>Id: ${userData.id}</p>
                   <p>Username: ${userData.name}</p>
                   <p>First name: ${userData.surname}</p>
                   <p>Last name: ${userData.email}</p>
                   <p>Account authorization: ${userData.authorizationLevel === 1 ? 'User' : userData.authorizationLevel === 2 ? 'Admin': 'Server'}</p>`,
                icon: 'info',
                denyButtonColor: "#dd33bb",
                confirmButtonColor: "#3085d6",
                confirmButtonText: "Ok",
            })
        }
    }

    const handleShowFilters = () => {
        setShowFilters(!showFilters);
    }



    return (
        <div className={'adminPanelContainer'}>
            <img src={arrowPhoto} alt="" id={'filtersButton'} onClick={handleShowFilters}/>
            <div id={'usersPanelTitle'}>
                <p>Users Panel</p>
            </div>
            <div id={`usersFilters`} className={showFilters ? 'show' : 'hide'}>
                <div id={'filtersHeader'}>
                    <p>Filters</p>
                </div>
                <div id={'filterByUserId'}>
                    <p>Filter by user id:</p>
                    <div className="filter-input-container">
                        <input type="number" id={'filterUserId'} value={filterUserId || ''} onChange={(e) => setFilterUserId(Number(e.target.value))} />
                        <button onClick={() =>
                        { setFilterUserId(undefined);
                            const requestIdInput = document.getElementById('filterUserId') as HTMLInputElement | null;
                            if (requestIdInput) {
                                requestIdInput.value = '';
                            }}}>Clear</button>
                    </div>
                </div>
                <div id={'filterByType'}>
                    <p>Filter by type:</p>
                    <div className="filter-input-container">
                        <select name="filterType" id="filterType" onChange={(e) => {setFilterAuthLvl(parseInt(e.target.value))}}>
                            <option value="0">All</option>
                            <option value="1">User</option>
                            <option value="2">Admin</option>
                            <option value="3">Server</option>
                        </select>
                        <button onClick={() =>
                        { setFilterAuthLvl(undefined);
                            const requestIdInput = document.getElementById('filterType') as HTMLInputElement | null;
                            if (requestIdInput) {
                                requestIdInput.value = '0';
                            }}}>Clear</button>
                    </div>
                </div>
                <div id={'filterByName'}>
                    <p>Filter by name:</p>
                    <div className="filter-input-container">
                        <input type="text" id={'filterName'} value={filterName || ''} onChange={(e) => setFilterName(e.target.value)} />
                        <button onClick={() =>
                        { setFilterName(undefined);
                            const requestIdInput = document.getElementById('filterName') as HTMLInputElement | null;
                            if (requestIdInput) {
                                requestIdInput.value = '';
                            }}}>Clear</button>
                    </div>
                </div>
                <div id={'filterBySurname'}>
                    <p>Filter by surname:</p>
                    <div className="filter-input-container">
                        <input type="text" id={'filterSurname'} value={filterSurname || ''} onChange={(e) => setFilterSurname(e.target.value)} />
                        <button onClick={() =>
                        { setFilterSurname(undefined);
                            const requestIdInput = document.getElementById('filterSurname') as HTMLInputElement | null;
                            if (requestIdInput) {
                                requestIdInput.value = '';
                            }}}>Clear</button>
                    </div>
                </div>
                <div id={'filterByEmail'}>
                    <p>Filter by email:</p>
                    <div className="filter-input-container">
                        <input type="text" id={'filterEmail'} value={filterEmail || ''} onChange={(e) => setFilterEmail(e.target.value)} />
                        <button onClick={() =>
                        { setFilterEmail(undefined);
                            const requestIdInput = document.getElementById('filterEmail') as HTMLInputElement | null;
                            if (requestIdInput) {
                                requestIdInput.value = '';
                            }}}>Clear</button>
                    </div>
                </div>
            </div>
            <ul id={'usersList'}>
                {users?.map((user : User) => (
                    <div key={user.id}>
                        <li key={user.id} className={'singleUser'} onClick={() => handleUserClick(user.id)}>
                            <div className={'initialInformation'}>
                                <div id={'userId'}>
                                    <p>Id:</p>
                                    <p> {user.id}</p>
                                </div>
                            </div>
                            <div className={'moreUserInformation'}>
                                <div id={'userName'}>
                                    <p>Name:</p>
                                    <p> {user.name}</p>
                                </div>
                                <div id={'userSurname'}>
                                    <p>Surname:</p>
                                    <p> {user.surname}</p>
                                </div>
                            </div>
                            <div className={'moreMoreUserInformation'}>
                                <div id={'userEmail'}>
                                    <p>Email:</p>
                                    <p> {user.email}</p>
                                </div>
                                <div id={'userAuthLvl'}>
                                    <p>Auth lvl:</p>
                                    <p> {user.authorizationLevel === 1 ? 'User' : user.authorizationLevel === 2 ? 'Admin' : 'Server'}</p>
                                </div>
                            </div>
                        </li>
                        <div className={'decisionUsersImages'}>
                            <img src={plusPhoto} alt="" onClick={() => handleUserActionClick(user.id, false)}/>
                            <img src={minusPhoto} alt="" onClick={() => handleUserActionClick(user.id,true)}/>
                        </div>
                    </div>
                ))}
            </ul>
        </div>
    );
};