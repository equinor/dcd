import { useCallback } from "react"
import { v4 as uuidv4 } from "uuid"
import { debounce } from "lodash"
import { useCaseContext } from "../Context/CaseContext"
import { getCurrentTime } from "../Utils/common"
import { EditInstance } from "../Models/Interfaces"

const useDataEdits = (): {
    addEdit: (
        newValue: string | number | undefined,
        previousValue: string | number | undefined,
        objectKey: keyof Components.Schemas.CaseDto,
        inputLabel: string,
        level: "project" | "case",
        objectId: string,
    ) => void;
} => {
    const TIMER = 800
    const { setCaseEdits } = useCaseContext()

    const debouncedChangeHandler = useCallback(debounce((nextValue: EditInstance) => {
        console.log("just saved edits")
        setCaseEdits((prevEdits) => [nextValue, ...prevEdits])
    }, TIMER), [])

    const addEdit = (
        newValue: string | number | undefined,
        previousValue: string | number | undefined,
        objectKey: keyof Components.Schemas.CaseDto,
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

        debouncedChangeHandler(editInstanceObject)
    }

    return { addEdit }
}

export default useDataEdits
