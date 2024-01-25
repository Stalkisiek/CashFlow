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
    const [idFilter, setIdFilter] = useState<number>();
    const [typeFilter, setTypeFilter] = useState<number>();

    const { fetchBankAccounts, deleteBankAccount, updateBankAccount } = useAdminPanelApi();

    useEffect(() => {
        fetchBankAccounts(idFilter, typeFilter).then((response) => {
            if (response) {
                setBankAccounts(response);
            }
        });

        const fetchInterval = setInterval(() => {
            fetchBankAccounts(idFilter, typeFilter).then((response) => {
                if (response) {
                    setBankAccounts(response);
                }
            });
        }, 500);

        // Cleanup function
        return () => clearInterval(fetchInterval);
    }, [idFilter, typeFilter]); // Dependency list

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

    const handleBankAccountClick = (id: number) => {
        Swal.fire({
            title: 'Update Bank Account',
            text: 'Do you want to continue to update your bank account details?',
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: 'Continue'
        }).then((result) => {
            if (result.isConfirmed) {
                Swal.fire({
                    title: 'Enter Account Details',
                    html:
                        '<div class="input-container">' + // Container for the select input
                        '<select id="account-type" class="custom-select swalSelect">' +
                        '<option value="savings">Savings</option>' +
                        '<option value="credit">Credit</option>' +
                        '</select>' +
                        '</div>' +
                        '<div class="input-container">' + // Container for the balance input
                        '<input id="balance" class="swal2-input swalInput" type="number" placeholder="Balance">' +
                        '</div>' +
                        '<div class="input-container">' + // Container for the credit input
                        '<input style=\'color:red\'id="credit" class="swal2-input swalInput" type="number" placeholder="Credit">' +
                        '</div>',
                    focusConfirm: false,
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
                }).then((result: SweetAlertResult<any[]>) => {
                    if (result.value) {
                        updateBankAccount(id, result.value[0], result.value[1], result.value[2]);
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
            <ul id={'bankAccountsListAdmin'}>
                {bankAccounts?.map((bankAccount) => (
                    <div key={bankAccount.id}>
                        <li className={'singleBankAccount'} onClick={() => handleBankAccountClick(bankAccount.id)}>
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