import React, {FC} from "react";
import {Savings} from "../features/savings/Savings";

interface SavingsPageProps{

};

export const SavingsPage: FC<SavingsPageProps> = ({}) => {
    return(
        <>
            <Savings/>
        </>
    );
};