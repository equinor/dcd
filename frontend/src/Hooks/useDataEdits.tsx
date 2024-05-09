import { v4 as uuidv4 } from "uuid"
import { useCaseContext } from "../Context/CaseContext"
import { getCurrentTime } from "../Utils/common"
import { EditInstance } from "../Models/Interfaces"

const useDataEdits = (): {
    addEdit: (
        newValue: string | number | undefined,
        previousValue: string | number | undefined,
        objectKey: string | number,
        inputLabel: string,
        level: "project" | "case",
        objectId: string,
    ) => void;
} => {
    const { setCaseEdits } = useCaseContext()

    const addEdit = (
        newValue: string | number | undefined,
        previousValue: string | number | undefined,
        objectKey: string | number,
        inputLabel: string,
        level: "project" | "case",
        objectId: string,
    ) => {
        if (newValue === previousValue) { return }

        const editInstanceObject: EditInstance = {
            newValue,
            previousValue,
            objectKey,
            inputLabel,
            uuid: uuidv4(),
            timeStamp: getCurrentTime(),
            level,
            objectId,
        }

        setCaseEdits((prevEdits) => [editInstanceObject, ...prevEdits])
    }

    return { addEdit }
}

export default useDataEdits
