/* eslint-disable indent */
import { v4 as uuidv4 } from "uuid"
import { useEffect, useState } from "react"
import { useMutation, useQueryClient } from "react-query"
import { useCaseContext } from "../Context/CaseContext"
import {
    EditInstance,
    EditEntry,
    ResourceName,
    ResourcePropertyKey,
} from "../Models/Interfaces"
import { getCurrentEditId } from "../Utils/common"
import { GetCaseService } from "../Services/CaseService"
import { GetTopsideService } from "../Services/TopsideService"
import { GetSurfService } from "../Services/SurfService"
import { GetSubstructureService } from "../Services/SubstructureService"
import { GetTransportService } from "../Services/TransportService"
import { GetDrainageStrategyService } from "../Services/DrainageStrategyService"

interface AddEditParams {
    newValue: string | number | undefined;
    previousValue: string | number | undefined;
    inputLabel: string;
    projectId: string;
    resourceName: ResourceName;
    resourcePropertyKey: ResourcePropertyKey;
    resourceId?: string;
    caseId?: string;
    newDisplayValue?: string | number | undefined;
    previousDisplayValue?: string | number | undefined;
}

const useDataEdits = (): {
    addEdit: (params: AddEditParams) => void;
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
            setCaseEditsBelongingToCurrentCase(caseEdits.filter((edit) => edit.caseId === projectCase.id))
        }
    }, [projectCase, caseEdits])

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

    const queryClient = useQueryClient()

    const mutation = useMutation(
        async ({ serviceMethod }: {
            projectId: string,
            caseId: string,
            resourceId?: string,
            serviceMethod: object,
        }) => serviceMethod,
        {
            onSuccess: (results, variables) => {
                const { projectId, caseId, resourceId } = variables
                queryClient.setQueryData([{ resourceId, projectId, caseId }], results)
            },
            onError: (error) => {
                console.error("Failed to update data:", error)
            },
        },
    )

    const updateTopside = async (
        projectId: string,
        caseId: string,
        topsideId: string,
        resourcePropertyKey: ResourcePropertyKey,
        value: string | number | undefined,
    ) => {
        const updatedData = { [resourcePropertyKey]: value }
        const service = await GetTopsideService()
        const serviceMethod = service.updateTopside(projectId, caseId, topsideId, updatedData)

        try {
            await mutation.mutateAsync({
                projectId,
                caseId,
                resourceId: topsideId,
                serviceMethod,
            })
            return true
        } catch (error) {
            return false
        }
    }

    const updateSurf = async (
        projectId: string,
        caseId: string,
        surfId: string,
        resourcePropertyKey: ResourcePropertyKey,
        value: string | number | undefined,
    ) => {
        const updatedData = { [resourcePropertyKey]: value }
        const service = await GetSurfService()

        const serviceMethod = service.updateSurf(projectId, caseId, surfId, updatedData)
        try {
            await mutation.mutateAsync({
                projectId,
                caseId,
                resourceId: surfId,
                serviceMethod,
            })
            return true
        } catch (error) {
            return false
        }
    }

    const updateSubstructure = async (
        projectId: string,
        caseId: string,
        substructureId: string,
        resourcePropertyKey: ResourcePropertyKey,
        value: string | number | undefined,
    ) => {
        const updatedData = { [resourcePropertyKey]: value }
        const service = await GetSubstructureService()
        const serviceMethod = service.updateSubstructure(projectId, caseId, substructureId, updatedData)

        try {
            await mutation.mutateAsync({
                projectId,
                caseId,
                resourceId: substructureId,
                serviceMethod,
            })
            return true
        } catch (error) {
            return false
        }
    }

    const updateTransport = async (
        projectId: string,
        caseId: string,
        transportId: string,
        resourcePropertyKey: ResourcePropertyKey,
        value: string | number | undefined,
    ) => {
        const updatedData = { [resourcePropertyKey]: value }
        const service = await GetTransportService()
        const serviceMethod = service.updateTransport(projectId, caseId, transportId, updatedData)

        try {
            await mutation.mutateAsync({
                projectId,
                caseId,
                resourceId: transportId,
                serviceMethod,
            })
            return true
        } catch (error) {
            return false
        }
    }

    const updateDrainageStrategy = async (
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        resourcePropertyKey: ResourcePropertyKey,
        value: any,
    ) => {
        const updatedData = { [resourcePropertyKey]: value }
        const service = await GetDrainageStrategyService()
        const serviceMethod = service.updateDrainageStrategy(projectId, caseId, drainageStrategyId, updatedData)

        try {
            await mutation.mutateAsync({
                projectId,
                caseId,
                resourceId: drainageStrategyId,
                serviceMethod,
            })
            return true
        } catch (error) {
            return false
        }
    }

    const updateCase = async (
        projectId: string,
        caseId: string,
        resourcePropertyKey: ResourcePropertyKey,
        value: any,
    ) => {
        const updatedData = { [resourcePropertyKey]: value }
        const caseService = await GetCaseService()
        const serviceMethod = caseService.updateCase(projectId, caseId, updatedData)

        try {
            await mutation.mutateAsync({
                projectId,
                caseId,
                serviceMethod,
            })
            return true
        } catch (error) {
            return false
        }
    }

    const submitToApi = async (
        projectId: string,
        caseId: string,
        resourceName: ResourceName,
        resourcePropertyKey: ResourcePropertyKey,
        value: any,
        resourceId?: string,
    ): Promise<boolean> => {
        if (resourceName !== "case" && !resourceId) {
            console.log("asset id is required for this service")
            return false
        }

        let sucess = false
        switch (resourceName) {
            case "case":
                sucess = await updateCase(projectId, caseId, resourcePropertyKey, value)
                break
            case "topside":
                sucess = await updateTopside(projectId, caseId, resourceId!, resourcePropertyKey, value)
                break
            case "surf":
                sucess = await updateSurf(projectId, caseId, resourceId!, resourcePropertyKey, value)
                break
            case "substructure":
                sucess = await updateSubstructure(projectId, caseId, resourceId!, resourcePropertyKey, value)
                break
            case "transport":
                sucess = await updateTransport(projectId, caseId, resourceId!, resourcePropertyKey, value)
                break
            case "drainageStrategy":
                sucess = await updateDrainageStrategy(projectId, caseId, resourceId!, resourcePropertyKey, value)
                break
            default:
                console.log("Service not found")
        }

        return sucess
    }

    const addToHistoryTracker = async (editInstanceObject: EditInstance, caseId: string) => {
        const currentEditIndex = caseEditsBelongingToCurrentCase.findIndex((edit) => edit.uuid === getCurrentEditId(editIndexes, projectCase))
        const caseEditsNotBelongingToCurrentCase = caseEdits.filter((edit) => edit.caseId !== caseId)

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

    const addEdit = async ({
        newValue,
        previousValue,
        inputLabel,
        projectId,
        resourceName,
        resourcePropertyKey,
        resourceId,
        caseId,
        newDisplayValue,
        previousDisplayValue,
    }: AddEditParams) => {
        if (resourceName !== "case" && !resourceId) {
            console.log("asset id is required for this service")
            return
        }

        const editInstanceObject: EditInstance = {
            uuid: uuidv4(),
            timeStamp: new Date().getTime(),
            newValue,
            previousValue,
            inputLabel,
            projectId,
            resourceName,
            resourcePropertyKey,
            resourceId,
            caseId,
            newDisplayValue,
            previousDisplayValue,
        }

        const success = await submitToApi(projectId, caseId!, resourceName, resourcePropertyKey, newValue, resourceId)

        if (success && caseId) {
            addToHistoryTracker(editInstanceObject, caseId)
        }
    }

    const undoEdit = () => {
        const currentEditIndex = caseEditsBelongingToCurrentCase.findIndex((edit) => edit.uuid === getCurrentEditId(editIndexes, projectCase))
        const editThatWillBeUndone = caseEditsBelongingToCurrentCase[currentEditIndex]
        const updatedEditIndex = currentEditIndex + 1
        const updatedEdit = caseEditsBelongingToCurrentCase[updatedEditIndex]

        if (currentEditIndex === -1) {
            return
        }
        if (!updatedEdit) {
            updateEditIndex("")
        } else {
            updateEditIndex(updatedEdit.uuid)
        }

        if (editThatWillBeUndone) {
            submitToApi(
                editThatWillBeUndone.projectId,
                editThatWillBeUndone.caseId!,
                editThatWillBeUndone.resourceName,
                editThatWillBeUndone.resourcePropertyKey,
                editThatWillBeUndone.previousValue,
                editThatWillBeUndone.resourceId,
            )
        }
    }

    const redoEdit = () => {
        const currentEditIndex = caseEditsBelongingToCurrentCase.findIndex((edit) => edit.uuid === getCurrentEditId(editIndexes, projectCase))

        if (currentEditIndex <= 0) {
            // If the current edit is the first one or not found, redo the last edit.
            const lastEdit = caseEditsBelongingToCurrentCase[caseEditsBelongingToCurrentCase.length - 1]
            if (lastEdit) {
                updateEditIndex(lastEdit.uuid)
                submitToApi(
                    lastEdit.projectId,
                    lastEdit.caseId!,
                    lastEdit.resourceName,
                    lastEdit.resourcePropertyKey,
                    lastEdit.newValue,
                    lastEdit.resourceId,
                )
            }
        } else {
            // Otherwise, redo the previous edit.
            const updatedEdit = caseEditsBelongingToCurrentCase[currentEditIndex - 1]
            updateEditIndex(updatedEdit.uuid)

            if (updatedEdit) {
                submitToApi(
                    updatedEdit.projectId,
                    updatedEdit.caseId!,
                    updatedEdit.resourceName,
                    updatedEdit.resourcePropertyKey,
                    updatedEdit.newValue,
                    updatedEdit.resourceId,
                )
            }
        }
    }

    return {
        addEdit,
        undoEdit,
        redoEdit,
    }
}

export default useDataEdits
