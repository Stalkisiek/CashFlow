import {FC} from "react";
import {UpdatePassword} from "../../features/update/updatePassword/UpdatePassword";

interface ChangePasswordProps{

};

export const ChangePassword: FC<ChangePasswordProps> = ({}) => {
    return(
        <>
            <UpdatePassword/>
        </>
    );
};