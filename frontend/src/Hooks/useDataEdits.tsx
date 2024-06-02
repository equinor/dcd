import { v4 as uuidv4 } from "uuid"
import { useEffect, useState } from "react"
import { useCaseContext } from "../Context/CaseContext"
import { EditInstance, EditEntry, ServiceName } from "../Models/Interfaces"
import { getCurrentEditId } from "../Utils/common"
import useQuery from "../Hooks/useQuery"
import { GetCaseService } from "../Services/CaseService"
import { GetTopsideService } from "../Services/TopsideService"
import { GetSurfService } from "../Services/SurfService"
import { GetSubstructureService } from "../Services/SubstructureService"
import { GetTransportService } from "../Services/TransportService"

const useDataEdits = (
    projectId: string,
    caseId: string,
    topside?: Components.Schemas.TopsideDto,
    surf?: Components.Schemas.SurfDto,
    substructure?: Components.Schemas.SubstructureDto,
    transport?: Components.Schemas.TransportDto,
): {
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
    updateCase(key: any, value: any): void;
    updateTopside(key: any, value: any): void;
    updateSurf(key: any, value: any): void;
    updateSubstructure(key: any, value: any): void;
    updateTransport(key: any, value: any): void;
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

    const { updateData: updateCase } = useQuery({
        queryKey: ["caseData", projectId, caseId],
        mutationFn: async (updatedData: Components.Schemas.CaseDto) => {
            const caseService = await GetCaseService()
            return caseService.updateCase(projectId, caseId, updatedData)
        },
    })

    const { updateData: updateTopside } = useQuery({
        queryKey: ["topsideData", projectId, caseId],
        mutationFn: async (updatedData: Components.Schemas.APIUpdateTopsideDto) => {
            const topsideService = await GetTopsideService()
            if (!topside) {
                console.log("you are not in a topside")
                return null
            }
            return topsideService.updateTopside(projectId, caseId, topside.id, updatedData)
        },
    })

    const { updateData: updateSurf } = useQuery({
        queryKey: ["surfData", projectId, caseId],
        mutationFn: async (updatedData: Components.Schemas.APIUpdateSurfDto) => {
            const surfService = await GetSurfService()
            if (!surf) {
                console.log("you are not in a surf")
                return null
            }
            return surfService.updateSurf(projectId, caseId, surf.id, updatedData)
        },
    })

    const { updateData: updateTransport } = useQuery({
        queryKey: ["transportData", projectId, caseId],
        mutationFn: async (updatedData: Components.Schemas.APIUpdateTransportDto) => {
            const transportService = await GetTransportService()
            if (!transport) {
                console.log("you are not in a transport")
                return null
            }
            return transportService.updateTransport(projectId, caseId, transport.id, updatedData)
        },
    })

    const { updateData: updateSubstructure } = useQuery({
        queryKey: ["substructureData", projectId, caseId],
        mutationFn: async (updatedData: Components.Schemas.APIUpdateSubstructureDto) => {
            const substructureService = await GetSubstructureService()
            if (!substructure) {
                console.log("you are not in a substructure")
                return null
            }
            return substructureService.updateSubstructure(projectId, caseId, substructure.id, updatedData)
        },
    })

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
        const caseEditsNotBelongingToCurrentCase = caseEdits.filter((edit) => edit.objectId !== objectId)

        let edits = caseEditsBelongingToCurrentCase

        if (currentEditIndex > 0) {
            edits = caseEditsBelongingToCurrentCase.slice(currentEditIndex)
        }

        if (currentEditIndex === -1) {
            edits = []
        }

        edits = [editInstanceObject, ...edits, ...caseEditsNotBelongingToCurrentCase]
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
        addEdit,
        undoEdit,
        redoEdit,
        updateCase,
        updateTopside,
        updateSurf,
        updateSubstructure,
        updateTransport,
    }
}

export default useDataEdits
