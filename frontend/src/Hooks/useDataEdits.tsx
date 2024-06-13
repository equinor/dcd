/* eslint-disable indent */
import { v4 as uuidv4 } from "uuid"
import { useMutation, useQueryClient } from "react-query"
import { useCaseContext } from "../Context/CaseContext"
import {
    EditInstance,
    EditEntry,
    ResourceName,
    ResourcePropertyKey,
    ResourceObject,
} from "../Models/Interfaces"
import { getCurrentEditId } from "../Utils/common"
import { GetCaseService } from "../Services/CaseService"
import { GetTopsideService } from "../Services/TopsideService"
import { GetSurfService } from "../Services/SurfService"
import { GetSubstructureService } from "../Services/SubstructureService"
import { GetTransportService } from "../Services/TransportService"
import { GetDrainageStrategyService } from "../Services/DrainageStrategyService"
import { useAppContext } from "../Context/AppContext"

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
    newResourceObject?: ResourceObject;
}

const useDataEdits = (): {
    addEdit: (params: AddEditParams) => void;
    undoEdit: () => void;
    redoEdit: () => void;
} => {
    const { setSnackBarMessage } = useAppContext()
    const {
        caseEdits,
        setCaseEdits,
        projectCase,
        editIndexes,
        setEditIndexes,
        caseEditsBelongingToCurrentCase,
    } = useCaseContext()

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
            onSuccess: (
                results: any,
                variables,
            ) => {
                const { projectId, caseId } = variables
                /* this should work but doesnt :(
                const assetId = caseId === results.id ? "" : results.id
                queryClient.setQueryData([{ caseId, projectId , assetId }], results)
                */

                // this makes the app refetch all data. We should only refetch the data that was updated in the future.
                queryClient.invalidateQueries(["apiData", { projectId, caseId }])
            },
            onError: (error: any) => {
                console.error("Failed to update data:", error)
                setSnackBarMessage(error.message)
            },
        },
    )

    const updateTopside = async (
        projectId: string,
        caseId: string,
        topsideId: string,
        resourcePropertyKey: ResourcePropertyKey,
        value: string | number | undefined,
        resourceObject?: ResourceObject,

    ) => {
        const service = await GetTopsideService()
        const existingDataInClient: object | undefined = queryClient.getQueryData([{ projectId, caseId, resourceId: topsideId }])
        const updatedData = resourceObject || { ...existingDataInClient, [resourcePropertyKey]: value }
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
        resourceObject?: ResourceObject,
    ) => {
        const service = await GetSurfService()
        const existingDataInClient: object | undefined = queryClient.getQueryData([{ projectId, caseId, resourceId: surfId }])
        const updatedData = resourceObject || { ...existingDataInClient, [resourcePropertyKey]: value }
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
        resourceObject?: object,
    ) => {
        const service = await GetSubstructureService()
        const existingDataInClient: object | undefined = queryClient.getQueryData([{ projectId, caseId, resourceId: substructureId }])
        const updatedData = resourceObject || { ...existingDataInClient, [resourcePropertyKey]: value }
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
        resourceObject?: object,
    ) => {
        const service = await GetTransportService()
        const existingDataInClient: object | undefined = queryClient.getQueryData([{ projectId, caseId, resourceId: transportId }])
        const updatedData = resourceObject || { ...existingDataInClient, [resourcePropertyKey]: value }
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
        resourceObject?: object,

    ) => {
        const service = await GetDrainageStrategyService()
        const existingDataInClient: object | undefined = queryClient.getQueryData([{ projectId, caseId, resourceId: drainageStrategyId }])
        const updatedData = resourceObject || { ...existingDataInClient, [resourcePropertyKey]: value }
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
        resourceObject?: ResourceObject,

    ) => {
        const caseService = await GetCaseService()
        const existingDataInClient: object | undefined = queryClient.getQueryData([{ projectId, caseId, resourceId: "" }])
        const updatedData = resourceObject || { ...existingDataInClient, [resourcePropertyKey]: value }
        const serviceMethod = caseService.updateCase(projectId, caseId, updatedData as Components.Schemas.APIUpdateCaseDto)

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

    type SubmitToApiParams = {
        projectId: string,
        caseId: string,
        resourceName: string,
        resourcePropertyKey: ResourcePropertyKey,
        value: string,
        resourceId?: string,
        resourceObject?: ResourceObject,
    }

    const submitToApi = async ({
        projectId,
        caseId,
        resourceName,
        resourcePropertyKey,
        value,
        resourceId,
        resourceObject,
    }: SubmitToApiParams): Promise<boolean> => {
        if (resourceName !== "case" && !resourceId) {
            console.log("asset id is required for this service")
            return false
        }
        let sucess = false
        switch (resourceName) {
            case "case":
                sucess = await updateCase(projectId, caseId, resourcePropertyKey, value, resourceObject)
                break
            case "topside":
                sucess = await updateTopside(projectId, caseId, resourceId!, resourcePropertyKey, value, resourceObject)
                break
            case "surf":
                sucess = await updateSurf(projectId, caseId, resourceId!, resourcePropertyKey, value, resourceObject)
                break
            case "substructure":
                sucess = await updateSubstructure(projectId, caseId, resourceId!, resourcePropertyKey, value, resourceObject)
                break
            case "transport":
                sucess = await updateTransport(projectId, caseId, resourceId!, resourcePropertyKey, value, resourceObject)
                break
            case "drainageStrategy":
                sucess = await updateDrainageStrategy(projectId, caseId, resourceId!, resourcePropertyKey, value, resourceObject)
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
        newResourceObject,
    }: AddEditParams) => {
        if (resourceName !== "case" && !resourceId) {
            console.log("asset id is required for this service")
            return
        }

        if (newValue === previousValue && !newResourceObject) {
            console.log("No changes detected")
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
            newResourceObject,
        }

        const success = await submitToApi(
            {
                projectId,
                caseId: caseId!,
                resourceName,
                resourcePropertyKey,
                value: newValue as string,
                resourceId,
                resourceObject: newResourceObject as ResourceObject | undefined,
            },
        )

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
                {
                    projectId: editThatWillBeUndone.projectId,
                    caseId: editThatWillBeUndone.caseId!,
                    resourceName: editThatWillBeUndone.resourceName,
                    resourcePropertyKey: editThatWillBeUndone.resourcePropertyKey,
                    value: editThatWillBeUndone.previousValue as string,
                    resourceId: editThatWillBeUndone.resourceId,
                    resourceObject: editThatWillBeUndone.newResourceObject as ResourceObject,
                },
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
                    {
                        projectId: lastEdit.projectId,
                        caseId: lastEdit.caseId!,
                        resourceName: lastEdit.resourceName,
                        resourcePropertyKey: lastEdit.resourcePropertyKey,
                        value: lastEdit.newValue as string,
                        resourceId: lastEdit.resourceId,
                        resourceObject: lastEdit.newResourceObject as ResourceObject,
                    },
                )
            }
        } else {
            // Otherwise, redo the previous edit.
            const updatedEdit = caseEditsBelongingToCurrentCase[currentEditIndex - 1]
            updateEditIndex(updatedEdit.uuid)

            if (updatedEdit) {
                submitToApi(
                    {
                        projectId: updatedEdit.projectId,
                        caseId: updatedEdit.caseId!,
                        resourceName: updatedEdit.resourceName,
                        resourcePropertyKey: updatedEdit.resourcePropertyKey,
                        value: updatedEdit.newValue as string,
                        resourceId: updatedEdit.resourceId,
                        resourceObject: updatedEdit.newResourceObject as ResourceObject,
                    },
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
