import { v4 as uuidv4 } from "uuid"
import { useEffect, useState } from "react"
import { useCaseContext } from "../Context/CaseContext"
import { EditInstance, EditEntry } from "../Models/Interfaces"

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
    redoEdit: () => void;
    getCurrentEditId: () => string | undefined;
} => {
    const {
        caseEdits,
        setCaseEdits,
        projectCase,
        editIndexes,
        setEditIndexes,
    } = useCaseContext()

    const updateEditIndex = (newEditId: string) => {
        if (!projectCase) {
            console.log("you are not in a project case")
            return
        }

        const editEntry: EditEntry = { caseId: projectCase.id, currentEditId: newEditId }

        // Retrieve existing editIndexes from localStorage, or initialize an empty array if it doesn't exist
        const storedEditIndexes = localStorage.getItem("editIndexes")
        const updatedEditIndexes = storedEditIndexes ? JSON.parse(storedEditIndexes) : []

        // Check if an entry with the same caseId exists
        const existingEntryIndex = updatedEditIndexes.findIndex((entry: { caseId: string }) => entry.caseId === projectCase.id)

        if (existingEntryIndex !== -1) {
            // If it exists, update the currentEditId for that entry
            updatedEditIndexes[existingEntryIndex].currentEditId = newEditId
        } else {
            // If it doesn't exist, add the new entry
            updatedEditIndexes.push(editEntry)
        }

        localStorage.setItem("editIndexes", JSON.stringify(updatedEditIndexes))
        setEditIndexes(updatedEditIndexes)
    }

    useEffect(() => {
        const storedCaseEdits = localStorage.getItem("caseEdits")
        const parsedCaseEdits = storedCaseEdits ? JSON.parse(storedCaseEdits) : []
        if (parsedCaseEdits.length === 0) {
            // reset editIndexes if there are no recent edits
            localStorage.setItem("editIndexes", JSON.stringify([]))
            setEditIndexes([])
        } else {
            const storedEditIndexes = localStorage.getItem("editIndexes")
            const parsedEditIndexes = storedEditIndexes ? JSON.parse(storedEditIndexes) : []
            setEditIndexes(parsedEditIndexes)
        }
    }, [])

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
        updateEditIndex(editInstanceObject.uuid)
    }

    const getCurrentEditId = () => {
        const currentCaseEditId = editIndexes.find((entry: EditEntry) => entry.caseId === projectCase?.id && entry.currentEditId)
        return (currentCaseEditId as unknown as EditEntry)?.currentEditId
    }

    const [caseEditsBelongingToCurrentCase, setCaseEditsBelongingToCurrentCase] = useState<EditInstance[]>([])

    useEffect(() => {
        if (projectCase) {
            const edits = caseEdits.filter((edit) => edit.objectId === projectCase.id)
            setCaseEditsBelongingToCurrentCase(edits)
        }
    }, [projectCase])

    const undoEdit = () => {
        const currentEditIndex = caseEditsBelongingToCurrentCase.findIndex((edit) => edit.uuid === getCurrentEditId())

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
        const currentEditIndex = caseEditsBelongingToCurrentCase.findIndex((edit) => edit.uuid === getCurrentEditId())

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
        addEdit, undoEdit, redoEdit, getCurrentEditId,
    }
}

export default useDataEdits
