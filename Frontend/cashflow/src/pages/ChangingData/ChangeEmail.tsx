import {FC} from "react";
import {UpdateEmail} from "../../features/update/updateEmail/UpdateEmail";

interface ChangeEmailProps{

};

export const ChangeEmail: FC<ChangeEmailProps> = ({}) => {
    return(
        <>
            <UpdateEmail/>
        </>
    );
};