import {FC} from "react";
import {AddAcountForm} from "../features/addAccount/AddAcountForm";

interface CreateAccountPageProps{

};

export const CreateAccountPage: FC<CreateAccountPageProps> = ({}) => {
    return(
        <>
            <AddAcountForm/>
        </>
    );
};