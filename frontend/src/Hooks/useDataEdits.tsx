import { v4 as uuidv4 } from "uuid"
import { useCaseContext } from "../Context/CaseContext"
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
    undoEdit: () => void;
    redoEdits: () => void;
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
            timeStamp: new Date().getTime(),
            level,
            objectId,
        }

        setCaseEdits((prevEdits) => [editInstanceObject, ...prevEdits])
    }

    const undoEdit = () => {
        console.log("Undoing edit")
        /*
        console.log(
            `Unding edit:
                ${latestEdit.inputLabel}
                from ${latestEdit.newValue}
                to ${latestEdit.previousValue}
            `,
        ) */
    }

    const redoEdits = () => {
        console.log("Redoing edit")
        /*
            console.log(
                `Redoing edit:
                ${latestUndo.inputLabel}
                from ${latestUndo.previousValue}
                to ${latestUndo.newValue}
            `,
            )
        */
    }

    return { addEdit, undoEdit, redoEdits }
}

export default useDataEdits
