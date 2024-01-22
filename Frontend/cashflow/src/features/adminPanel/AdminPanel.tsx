import React, { FC, useEffect, useState } from "react";
import './styles.css'
import { API_URL, maxCCredit, maxSCredit } from "../../config";
import { ServiceResponse } from "../../types/ServiceResponse";
import { Request } from "../../types/Request";
import { useAdminPanelApi } from "./api";
import plusPhoto from '../../pictures/plusSign.png'
import minusPhoto from '../../pictures/minusSign.png'
import Swal from "sweetalert2";

interface AdminPanelProps { }

export const AdminPanel: FC<AdminPanelProps> = ({ }) => {
    const [requests, changeRequests] = useState<Request[] | undefined>([]);

    const { fetchData, fulfillRequest } = useAdminPanelApi();

    useEffect(() => {
        fetchData()
            .then((newRequests) => {
                changeRequests(newRequests);
            })
            .catch((error) => {
                console.error(error);
            });
        const setIntervalNr = setInterval(() => {
            fetchData()
                .then((newRequests) => {
                    changeRequests(newRequests);
                })
                .catch((error) => {
                    console.error(error);
                });
        }, 1000);
    }, []);

    const handleAcceptClick = async (requestId: number, ifAccept: boolean) => {
        const { isConfirmed } = await Swal.fire({
            title: `Are you sure you want to ${ifAccept?'accept':'decline'} this request?`,
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Proceed",
            cancelButtonText: "Go back"
        });

        if (isConfirmed) {
            const { value: userInput } = await Swal.fire({
                title: 'Attached request message',
                input: 'text',
                icon: 'question',
                showCancelButton: true,
                inputValidator: (value) => {
                    if (!value) {
                        return 'Please enter something!';
                    }
                }
            });
            fulfillRequest(requestId, ifAccept, userInput);
        }
    };


    return (
        <div className={'adminPanelContainer'}>
                <ul id={'requestsList'}>
                    {requests && requests.map((request) => (
                        <div key={request.id}>
                            <li className={'singleRequest'}>
                                <div className={'initialInformation'}>
                                    <div id={'requestId'}>
                                        <p>Id:</p>
                                        <p> {request.id}</p>
                                    </div>
                                    <div id={'requestUserId'}>
                                        <p>UserId:</p>
                                        <p> {request.userId}</p>
                                    </div>
                                    <div id={'requestType'}>
                                        <p>Type:</p>
                                        <p> {request.type === 1 ? 'Delete User' : request.type === 2 ? 'Delete Account' : request.type === 3? 'Add money' : 'Add credit'}</p>
                                    </div>
                                </div>
                                <div className={'balanceRequestInformation'}>
                                    <div id={'requestAccountBalance'}>
                                        <p>Account Balance:</p>
                                        <p> {request.accountBalance}</p>
                                    </div>
                                    <div id={'requestAmountBalance'}>
                                        <p>Amount Balance:</p>
                                        <p> {request.amountBalance}</p>
                                    </div>
                                    <div id={'requestFinalBalance'}>
                                        <p>Final Balance:</p>
                                        <p> {request.finallBalance}</p>
                                    </div>
                                </div>
                                <div className={'creditRequestInformation'}>
                                    <div id={'requestAccountCredit'}>
                                        <p>Account Credit:</p>
                                        <p> {request.accountCredit}</p>
                                    </div>
                                    <div id={'requestAmountCredit'}>
                                        <p>Amount Credit:</p>
                                        <p> {request.amountCredit}</p>
                                    </div>
                                    <div id={'requestFinalCredit'}>
                                        <p>Final Credit:</p>
                                        <p> {request.finallCredit}</p>
                                    </div>
                                </div>

                                {/* Add more properties here as needed */}
                            </li>
                            <div className={'decisionRequestsImages'}>
                                <img src={plusPhoto} alt="" onClick={() => handleAcceptClick(request.id, true)}/>
                                <img src={minusPhoto} alt="" onClick={() => handleAcceptClick(request.id, false)}/>
                            </div>
                        </div>
                    ))}
                </ul>
            </div>
    );
};
