import { v4 as uuidv4 } from "uuid"
import { useEffect, useState } from "react"
import { useMutation, useQueryClient } from "react-query"
import { useCaseContext } from "../Context/CaseContext"
import {
    EditInstance,
    EditEntry,
    ServiceName,
    ServiceKey,
} from "../Models/Interfaces"
import { getCurrentEditId } from "../Utils/common"
import { GetCaseService } from "../Services/CaseService"
import { GetTopsideService } from "../Services/TopsideService"
import { GetSurfService } from "../Services/SurfService"
import { GetSubstructureService } from "../Services/SubstructureService"
import { GetTransportService } from "../Services/TransportService"
import { GetDrainageStrategyService } from "../Services/DrainageStrategyService"

const useDataEdits = (): {
    addEdit: (
        newValue: string | number | undefined,
        previousValue: string | number | undefined,
        inputLabel: string,
        projectId: string,
        serviceName: ServiceName,
        serviceKey: ServiceKey,
        serviceId?: string,
        caseId?: string,
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
            assetId?: string,
            serviceMethod: object,
        }) => serviceMethod,
        {
            onSuccess: (results, variables) => {
                const { projectId, caseId, assetId } = variables
                queryClient.setQueryData([{ assetId, projectId, caseId }], results)
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
        serviceKey: ServiceKey,
        value: string | number | undefined,
    ) => {
        const updatedData = { [serviceKey]: value }
        const service = await GetTopsideService()
        const serviceMethod = service.updateTopside(projectId, caseId, topsideId, updatedData)

        mutation.mutate({
            projectId,
            caseId,
            assetId: topsideId,
            serviceMethod,
        })
    }

    const updateSurf = async (
        projectId: string,
        caseId: string,
        surfId: string,
        serviceKey: ServiceKey,
        value: string | number | undefined,
    ) => {
        const updatedData = { [serviceKey]: value }
        const service = await GetSurfService()

        const serviceMethod = service.updateSurf(projectId, caseId, surfId, updatedData)
        mutation.mutate({
            projectId,
            caseId,
            assetId: surfId,
            serviceMethod,
        })
    }

    const updateSubstructure = async (
        projectId: string,
        caseId: string,
        substructureId: string,
        serviceKey: ServiceKey,
        value: string | number | undefined,
    ) => {
        const updatedData = { [serviceKey]: value }
        const service = await GetSubstructureService()
        const serviceMethod = service.updateSubstructure(projectId, caseId, substructureId, updatedData)

        mutation.mutate({
            projectId,
            caseId,
            assetId: substructureId,
            serviceMethod,
        })
    }

    const updateTransport = async (
        projectId: string,
        caseId: string,
        transportId: string,
        serviceKey: ServiceKey,
        value: string | number | undefined,
    ) => {
        const updatedData = { [serviceKey]: value }
        const service = await GetTransportService()
        const serviceMethod = service.updateTransport(projectId, caseId, transportId, updatedData)

        mutation.mutate({
            projectId,
            caseId,
            assetId: transportId,
            serviceMethod,
        })
    }

    const updateDrainageStrategy = async (
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        serviceKey: ServiceKey,
        value: any,
    ) => {
        const service = await GetDrainageStrategyService()
        const serviceMethod = service.updateDrainageStrategy(projectId, caseId, drainageStrategyId, value)

        mutation.mutate({
            projectId,
            caseId,
            assetId: drainageStrategyId,
            serviceMethod,
        })
    }

    const updateCase = async (
        projectId: string,
        caseId: string,
        serviceKey: ServiceKey,
        value: any,
    ) => {
        const caseService = await GetCaseService()
        const serviceMethod = caseService.updateCase(projectId, caseId, value)

        mutation.mutate({
            projectId,
            caseId,
            serviceMethod,
        })
    }

    const submitToApi = (
        projectId: string,
        caseId: string,
        serviceName: ServiceName,
        serviceKey: ServiceKey,
        value: any,
        serviceId?: string,
    ) => {
        switch (serviceName) {
        case "case":
            updateCase(projectId, caseId, serviceKey, value)
            break
        case "topside":
            updateTopside(projectId, caseId, serviceId!, serviceKey, value)
            break
        case "surf":
            updateSurf(projectId, caseId, serviceId!, serviceKey, value)
            break
        case "substructure":
            updateSubstructure(projectId, caseId, serviceId!, serviceKey, value)
            break
        case "transport":
            updateTransport(projectId, caseId, serviceId!, serviceKey, value)
            break
        case "drainageStrategy":
            updateDrainageStrategy(projectId, caseId, serviceId!, serviceKey, value)
            break
        default:
            console.log("Service not found")
        }
    }

    const addEdit = (
        newValue: string | number | undefined,
        previousValue: string | number | undefined,
        inputLabel: string,
        projectId: string,
        serviceName: ServiceName,
        serviceKey: ServiceKey,
        serviceId?: string,
        caseId?: string,
        newDisplayValue?: string | number | undefined,
        previousDisplayValue?: string | number | undefined,
    ) => {
        if (newValue === previousValue) { return }

        const editInstanceObject: EditInstance = {
            uuid: uuidv4(),
            timeStamp: new Date().getTime(),
            newValue,
            previousValue,
            inputLabel,
            projectId,
            serviceName,
            serviceKey,
            serviceId,
            caseId,
            newDisplayValue,
            previousDisplayValue,
        }

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
        submitToApi(projectId, caseId!, serviceName, serviceKey, newValue, serviceId)
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
                editThatWillBeUndone.serviceName,
                editThatWillBeUndone.serviceKey,
                editThatWillBeUndone.previousValue,
                editThatWillBeUndone.serviceId,
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
                    lastEdit.serviceName,
                    lastEdit.serviceKey,
                    lastEdit.newValue,
                    lastEdit.serviceId,
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
                    updatedEdit.serviceName,
                    updatedEdit.serviceKey,
                    updatedEdit.newValue,
                    updatedEdit.serviceId,
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
