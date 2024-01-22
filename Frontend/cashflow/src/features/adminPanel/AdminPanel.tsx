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

interface AdminPanelProps { }

export const AdminPanel: FC<AdminPanelProps> = ({ }) => {
    const [requests, changeRequests] = useState<Request[] | undefined>([]);
    const [showFilters, setShowFilters] = useState<boolean>(false);
    const [filterRequestId, setFilterRequestId] = useState<number | undefined>(undefined);
    const [filterUserId, setFilterUserId] = useState<number | undefined>(undefined);
    const [filterType, setFilterType] = useState<number | undefined>(0);
    const [easyFinalize, setEasyFinalize] = useState<boolean>(false);
    const { fetchData, fulfillRequest } = useAdminPanelApi();

    useEffect(() => {
        const fetchInterval = setInterval(() => {
            fetchData(filterUserId,filterRequestId, filterType)
                .then((newRequests) => {
                    changeRequests(newRequests);
                })
                .catch((error) => {
                    console.error(error);
                });
        }, 500);

        // Funkcja czyszcząca przy odmontowywaniu komponentu
        return () => clearInterval(fetchInterval);
    }, [filterRequestId, filterUserId, filterType]); // Lista zależności


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
        if(isConfirmed && easyFinalize){
            fulfillRequest(requestId, ifAccept, 'Further details are not needed');
        }
        if (isConfirmed && !easyFinalize) {
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

    const handleShowFilters = () => {
        setShowFilters(!showFilters);
    }

    const handleFilter = () => {

    }

    return (
        <div className={'adminPanelContainer'}>
            <img src={arrowPhoto} alt="" id={'filtersButton'} onClick={handleShowFilters}/>
            <div id={`requestFilters`} className={showFilters ? 'show' : 'hide'}>
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
                <div id={'filterByRequestId'}>
                    <p>Filter by request id:</p>
                    <div className="filter-input-container">
                        <input type="number" id={'filterRequestId'} value={filterRequestId || ''} onChange={(e) => setFilterRequestId(Number(e.target.value))} />
                        <button onClick={() =>
                        { setFilterRequestId(undefined);
                            const requestIdInput = document.getElementById('filterRequestId') as HTMLInputElement | null;
                            if (requestIdInput) {
                                requestIdInput.value = '';
                            }}}>Clear</button>
                    </div>
                </div>
                <div id={'filterByType'}>
                    <p>Filter by type:</p>
                    <div className="filter-input-container">
                        <select name="filterType" id="filterType" onChange={(e) => {setFilterType(parseInt(e.target.value)); console.log(filterType)}}>
                            <option value="0">All</option>
                            <option value="1">Delete User</option>
                            <option value="2">Delete Account</option>
                            <option value="3">Add Money</option>
                            <option value="4">Add Credit</option>
                        </select>
                        <button onClick={() =>
                        { setFilterType(undefined);
                            const requestIdInput = document.getElementById('filterType') as HTMLInputElement | null;
                            if (requestIdInput) {
                                requestIdInput.value = '0';
                            }}}>Clear</button>
                    </div>
                </div>
                <div id={'easyFinalize'}>
                    <p>Easy finalize:</p>
                    <input type="checkbox" id={'easyFinalizeCheckbox'} onClick={() => setEasyFinalize(!easyFinalize)}/>
                </div>
            </div>
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
