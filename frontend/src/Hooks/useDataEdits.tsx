import { v4 as uuidv4 } from "uuid"
import { useEffect, useState } from "react"
import { useCaseContext } from "../Context/CaseContext"
import { EditInstance, EditEntry } from "../Models/Interfaces"
import { getCurrentEditId } from "../Utils/common"

const useDataEdits = (): {
    addEdit: (
        newValue: string | number | undefined,
        previousValue: string | number | undefined,
        objectKey: string | number,
        inputLabel: string,
        level: "project" | "case",
        objectId: string,
        newDisplayValue?: string | number | undefined,
        previousDisplayValue?: string | number | undefined,
    ) => void;
    undoEdit: () => void;
    redoEdit: () => void;
} => {
    const {
        caseEdits,
        setCaseEdits,
        projectCase,
        editIndexes,
        setEditIndexes,
    } = useCaseContext()

    const [caseEditsBelongingToCurrentCase, setCaseEditsBelongingToCurrentCase] = useState<EditInstance[]>([])

    useEffect(() => {
        if (projectCase) {
            setCaseEditsBelongingToCurrentCase(caseEdits.filter((edit) => edit.objectId === projectCase.id))
        }
    }, [projectCase, caseEdits])

    // TODO: move this out, it runs every time the hook is called
    useEffect(() => {
        const storedCaseEdits = localStorage.getItem("caseEdits")
        const caseEditsArray = storedCaseEdits ? JSON.parse(storedCaseEdits) : []

        if (caseEditsArray.length === 0) {
            // reset editIndexes if there are no recent edits
            localStorage.setItem("editIndexes", JSON.stringify([]))
            setEditIndexes([])
        } else {
            // otherwise, load the editIndexes from localStorage
            const storedEditIndexes = localStorage.getItem("editIndexes")
            const editIndexesArray = storedEditIndexes ? JSON.parse(storedEditIndexes) : []
            setEditIndexes(editIndexesArray)
        }
    }, [])

    const updateEditIndex = (newEditId: string) => {
        if (!projectCase) {
            console.log("you are not in a project case")
            return
        }

        const editEntry: EditEntry = { caseId: projectCase.id, currentEditId: newEditId }
        const storedEditIndexes = localStorage.getItem("editIndexes")
        const editIndexesArray = storedEditIndexes ? JSON.parse(storedEditIndexes) : []
        const currentCasesEditIndex = editIndexesArray.findIndex((entry: { caseId: string }) => entry.caseId === projectCase.id)

        if (currentCasesEditIndex !== -1) {
            editIndexesArray[currentCasesEditIndex].currentEditId = newEditId
        } else {
            editIndexesArray.push(editEntry)
        }

        localStorage.setItem("editIndexes", JSON.stringify(editIndexesArray))
        setEditIndexes(editIndexesArray)
    }

    const addEdit = (
        newValue: string | number | undefined,
        previousValue: string | number | undefined,
        objectKey: string | number,
        inputLabel: string,
        level: "project" | "case",
        objectId: string,
        newDisplayValue?: string | number | undefined,
        previousDisplayValue?: string | number | undefined,
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
            newDisplayValue,
            previousDisplayValue,
        }

        const currentEditIndex = caseEditsBelongingToCurrentCase.findIndex((edit) => edit.uuid === getCurrentEditId(editIndexes, projectCase))
        let edits = caseEditsBelongingToCurrentCase

        console.log("Removing edits from index", currentEditIndex)

        if (currentEditIndex > 0) {
            edits = caseEditsBelongingToCurrentCase.slice(currentEditIndex)
        }

        if (currentEditIndex === -1) {
            edits = []
        }

        edits = [editInstanceObject, ...edits]
        setCaseEdits(edits)
        updateEditIndex(editInstanceObject.uuid)
    }

    const undoEdit = () => {
        const currentEditIndex = caseEditsBelongingToCurrentCase.findIndex((edit) => edit.uuid === getCurrentEditId(editIndexes, projectCase))

        const editThatWillBeUndone = caseEditsBelongingToCurrentCase[currentEditIndex]
        const updatedEditIndex = currentEditIndex + 1
        const updatedEdit = caseEditsBelongingToCurrentCase[updatedEditIndex]

        console.log("Undoing edit", editThatWillBeUndone) // todo: submit to api

        if (currentEditIndex === -1) {
            return
        }
        if (!updatedEdit) {
            updateEditIndex("")
        } else {
            updateEditIndex(updatedEdit.uuid)
        }
    }

    const redoEdit = () => {
        const currentEditIndex = caseEditsBelongingToCurrentCase.findIndex((edit) => edit.uuid === getCurrentEditId(editIndexes, projectCase))

        if (currentEditIndex <= 0) {
            // If the current edit is the first one or not found, redo the last edit.
            const lastEdit = caseEditsBelongingToCurrentCase[caseEditsBelongingToCurrentCase.length - 1]
            if (lastEdit) {
                updateEditIndex(lastEdit.uuid)
                console.log("Redoing edit", lastEdit) // todo: submit to api
            }
        } else {
            // Otherwise, redo the previous edit.
            const updatedEdit = caseEditsBelongingToCurrentCase[currentEditIndex - 1]
            updateEditIndex(updatedEdit.uuid)
            console.log("Redoing edit", updatedEdit) // todo: submit to api
        }
    }

    return {
        addEdit, undoEdit, redoEdit,
    }
}

export default useDataEdits
