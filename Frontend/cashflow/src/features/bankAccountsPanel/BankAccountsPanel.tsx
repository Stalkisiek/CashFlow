import React, { FC, useEffect, useState } from "react";
import './styles.css'
import { API_URL, maxCCredit, maxSCredit } from "../../config";
import { ServiceResponse } from "../../types/ServiceResponse";
import { Request } from "../../types/Request";
import { useAdminPanelApi } from "./api";
import plusPhoto from '../../pictures/plusSign.png'
import minusPhoto from '../../pictures/minusSign.png'
import savingsPhoto from '../../pictures/savings.png'
import creditPhoto from '../../pictures/credit.png'
import arrowPhoto from '../../pictures/arrow.png'
import Swal, {SweetAlertResult} from "sweetalert2";
import {User} from "../../types/User";
import {BankAccount} from "../../types/BankAccount";

interface BankAccountsPanelProps { }

export const BankAccountsPanel: FC<BankAccountsPanelProps> = ({ }) => {
    const [showFilters, setShowFilters] = useState(false);
    const [bankAccounts, setBankAccounts] = useState<BankAccount[] | undefined>([]);

    const[filterAccountId, setFilterAccountId] = useState<number>();
    const[filterType, setFilterType] = useState<number>();

    const { fetchBankAccounts, deleteBankAccount, updateBankAccount } = useAdminPanelApi();

    useEffect(() => {
        fetchBankAccounts(filterAccountId, filterType).then((response) => {
            if (response) {
                setBankAccounts(response);
            }
        });

        const fetchInterval = setInterval(() => {
            fetchBankAccounts(filterAccountId, filterType).then((response) => {
                if (response) {
                    setBankAccounts(response);
                }
            });
        }, 500);

        // Cleanup function
        return () => clearInterval(fetchInterval);
    }, [filterAccountId, filterType]); // Dependency list

    const handleDeleteBankAccount = (id: number) => {
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
                await deleteBankAccount(id);
            }
        })
    }

    const handleBankAccountClick = (id: number, prevType:number, prevBalance:number, prevCredit:number) => {
        Swal.fire({
            title: 'Update Bank Account',
            text: 'Do you want to continue to update this bank account details?',
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: 'Continue'
        }).then((result) => {
            if (result.isConfirmed) {
                Swal.fire({
                    title: 'Enter Account Details',
                    html:
                        '<div class="input-container">' + // Container for the select input
                        '<label for="account-type">Account Type</label><br>' +
                        '<select id="account-type" class="custom-select swalSelect">' +
                        '<option value="0" disabled selected>Account Type</option>' +
                        '<option value="1">Savings</option>' +
                        '<option value="2">Credit</option>' +
                        '</select>' +
                        '</div>' +
                        '<div class="input-container">' + // Container for the balance input
                        '<label for="balance">Balance</label><br>' +
                        '<input id="balance" class="swal2-input swalInput" type="number" placeholder="Balance" value="' + prevBalance + '">' +
                        '</div>' +
                        '<div class="input-container">' + // Container for the credit input
                        '<label for="credit">Credit</label><br>' +
                        '<input id="credit" class="swal2-input swalInput" type="number" placeholder="Credit" value="' + prevCredit + '">' +
                        '</div>',
                    focusConfirm: false,
                    showCancelButton: true,
                    preConfirm: () => {
                        const accountTypeElement = document.getElementById('account-type') as HTMLSelectElement;
                        const balanceElement = document.getElementById('balance') as HTMLInputElement;
                        const creditElement = document.getElementById('credit') as HTMLInputElement;

                        return [
                            accountTypeElement ? accountTypeElement.value : null,
                            balanceElement ? balanceElement.value : null,
                            creditElement ? creditElement.value : null
                        ]
                    }
                }).then(async (result: SweetAlertResult<any[]>) => {
                    if (result.value)
                    {
                        // console.log(`prevType: ${prevType}, prevBalance: ${prevBalance}, prevCredit: ${prevCredit}, newType: ${result.value[0]}, newBalance: ${result.value[1]}, newCredit: ${result.value[2]}`)
                        if(result.value[0] == '0'){
                            result.value[0] = prevType;
                        }
                        if(result.value[1] === ''){
                            result.value[1] = prevBalance;
                        }
                        if(result.value[2] === ''){
                            result.value[2] = prevCredit;
                        }
                        await updateBankAccount(id, prevType, prevBalance, prevCredit, result.value[0], result.value[1], result.value[2]);
                    }
                });
            }
        });
    }

    const handleShowFilters = () => {
        setShowFilters(!showFilters);
    }


    return (
        <div className={'adminPanelContainer'}>
            <img src={arrowPhoto} alt="" id={'filtersButton'} onClick={handleShowFilters}/>
            <div id={'accountsPanelTitle'}>
                <p>Accounts Panel</p>
            </div>
            <div id={`accountsFilters`} className={showFilters ? 'show' : 'hide'}>
                <div id={'filtersHeader'}>
                    <p>Filters</p>
                </div>
                <div id={'filterByUserId'}>
                    <p>Filter by account id:</p>
                    <div className="filter-input-container">
                        <input type="number" id={'filterAccountId'} value={filterAccountId || ''} onChange={(e) => setFilterAccountId(Number(e.target.value))} />
                        <button onClick={() =>
                        { setFilterAccountId(undefined);
                            const requestIdInput = document.getElementById('filterUserId') as HTMLInputElement | null;
                            if (requestIdInput) {
                                requestIdInput.value = '';
                            }}}>Clear</button>
                    </div>
                </div>
                <div id={'filterByType'}>
                    <p>Filter by type:</p>
                    <div className="filter-input-container">
                        <select name="filterType" id="filterType" onChange={(e) => {setFilterType(parseInt(e.target.value))}}>
                            <option value="0">All</option>
                            <option value="1">Savings</option>
                            <option value="2">Credit</option>
                        </select>
                        <button onClick={() =>
                        { setFilterType(undefined);
                            const requestIdInput = document.getElementById('filterType') as HTMLInputElement | null;
                            if (requestIdInput) {
                                requestIdInput.value = '0';
                            }}}>Clear</button>
                    </div>
                </div>
            </div>
            <ul id={'bankAccountsListAdmin'}>
                {bankAccounts?.map((bankAccount) => (
                    <div key={bankAccount.id}>
                        <li className={'singleBankAccount'} onClick={() => handleBankAccountClick(bankAccount.id, bankAccount.type, bankAccount.balance, bankAccount.creditBalance)}>
                            <div className={'initialInformation'}>
                                <div id={'userId'}>
                                    <p>Id:</p>
                                    <p> {bankAccount.id}</p>
                                </div>
                                <div id={'bankType'}>
                                    <p>{bankAccount.type === 1 ? 'Savings' : 'Credit'}</p>
                                    <img src={bankAccount.type === 1 ? savingsPhoto : creditPhoto } alt=""/>
                                </div>
                            </div>
                            <div className={'moreUserInformation'}>
                                <div id={'userUser'}>
                                    <p>Balance:</p>
                                </div>
                                <div id={'userName'}>
                                    <p>{bankAccount.balance}$</p>
                                </div>
                            </div>
                            <div className={'moreMoreUserInformation'}>
                                <div id={'userUserUser'}>
                                    <p>Credit:</p>
                                </div>
                                <div id={'userName'}>
                                    <p>{bankAccount.creditBalance}$</p>
                                </div>
                            </div>
                        </li>
                        <div className={'decisionBankAccountImages'}>
                            <img src={minusPhoto} alt="" onClick={() => handleDeleteBankAccount(bankAccount.id)}/>
                        </div>
                    </div>
                ))}
            </ul>
        </div>
    );
};