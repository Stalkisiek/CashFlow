import {FC} from "react";
import {UpdateNames} from "../../features/update/updateNames/UpdateNames";

interface ChangeNamesProps{

};

export const ChangeNames: FC<ChangeNamesProps> = ({}) => {
    return(
        <>
            <UpdateNames/>
        </>
    );
};