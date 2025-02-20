import { useState, useCallback, useEffect } from "react"
import { EditInstance, ResourceObject } from "@/Models/Interfaces"
import { useSubmitToApi } from "./UseSubmitToApi"
import { createLogger } from "@/Utils/logger"

interface UseTableQueueProps {
    setIsSaving: (isSaving: boolean) => void
    isSaving: boolean
    addEdit: (edit: EditInstance) => Promise<any>
    gridRef: any
}

interface TimeSeriesResourceObject {
    startYear: number
    values: number[]
}

const tableQueueLogger = createLogger({
    name: "TABLE_QUEUE",
    enabled: true,
})

// Reduces the queue to the latest edit for each resource given the order of the queue and the resourceName
const reduceToLatestEdits = (queue: EditInstance[]): EditInstance[] => {
    tableQueueLogger.log("Reducing edit queue to latest edits", { originalQueue: queue })
    const latestEditsMap = new Map<string, EditInstance>()

    queue.forEach((edit) => {
        latestEditsMap.set(edit.resourceName, edit)
    })

    const result = Array.from(latestEditsMap.values())
    tableQueueLogger.log("Reduced queue result", { reducedQueue: result })
    return result
}

// todo: firstEdit is wrong, you may be able to get case and project id from context, but you need to get the campainId from the individual edit instances. also loop the edits and submit each one instead of firstEdit.blablabla

const submitCampaignEdit = async (editQueue: EditInstance[], submitToApi: any) => {
    tableQueueLogger.log("Submitting campaign edit", { editQueue })
    const firstEdit = editQueue[0]
    if (!firstEdit.projectId || !firstEdit.caseId) {
        tableQueueLogger.warn("Missing required project or case ID", { firstEdit })
        return
    }

    // For campaign edits, resourceId should be used as campaignId if campaignId is not present
    const campaignId = firstEdit.campaignId || firstEdit.resourceId
    if (!campaignId) {
        tableQueueLogger.warn("Missing both campaignId and resourceId", { firstEdit })
        return
    }

    // Group edits by their property key
    const rigUpgradingEdit = editQueue.find((edit) => edit.resourcePropertyKey === "rigUpgradingProfile")
    const rigMobDemobEdit = editQueue.find((edit) => edit.resourcePropertyKey === "rigMobDemobProfile")

    tableQueueLogger.log("Found campaign profile edits", {
        hasRigUpgradingEdit: !!rigUpgradingEdit,
        hasRigMobDemobEdit: !!rigMobDemobEdit,
        rigUpgradingEdit,
        rigMobDemobEdit,
    })

    if (rigUpgradingEdit || rigMobDemobEdit) {
        // Handle profile updates
        const updateDto = {
            rigUpgradingCostStartYear: (rigUpgradingEdit?.resourceObject as TimeSeriesResourceObject)?.startYear ?? 0,
            rigUpgradingCostValues: (rigUpgradingEdit?.resourceObject as TimeSeriesResourceObject)?.values ?? [],
            rigMobDemobCostStartYear: (rigMobDemobEdit?.resourceObject as TimeSeriesResourceObject)?.startYear ?? 0,
            rigMobDemobCostValues: (rigMobDemobEdit?.resourceObject as TimeSeriesResourceObject)?.values ?? [],
        }

        tableQueueLogger.log("Sending campaign update", {
            projectId: firstEdit.projectId,
            caseId: firstEdit.caseId,
            campaignId,
            updateDto,
        })

        try {
            await submitToApi({
                projectId: firstEdit.projectId,
                caseId: firstEdit.caseId,
                resourceName: rigUpgradingEdit ? "rigUpgrading" : "rigMobDemob",
                resourceId: campaignId,
                resourceObject: updateDto,
            })
            tableQueueLogger.log("Campaign update successful")
        } catch (error) {
            tableQueueLogger.error("Campaign update failed", { error })
            throw error
        }
    }
}

const submitCampaignWellsEdit = async (editQueue: EditInstance[], submitToApi: any) => {
    tableQueueLogger.log("Submitting campaign wells edit", { editQueue })
    const firstEdit = editQueue[0]
    if (!firstEdit.projectId || !firstEdit.caseId) {
        tableQueueLogger.warn("Missing required project or case ID", { firstEdit })
        return
    }

    // For campaign edits, resourceId should be used as campaignId if campaignId is not present
    const campaignId = firstEdit.resourceId
    if (!campaignId) {
        tableQueueLogger.warn("Missing resourceId for campaign wells", { firstEdit })
        return
    }

    console.log("editQueue _______________________", editQueue)

    // Map the well edits to the correct format
    const wellEdits = editQueue.map((edit) => ({
        wellId: edit.resourceId,
        startYear: (edit.resourceObject as TimeSeriesResourceObject).startYear,
        values: (edit.resourceObject as TimeSeriesResourceObject).values,
    }))

    tableQueueLogger.log("Sending campaign wells update", {
        projectId: firstEdit.projectId,
        caseId: firstEdit.caseId,
        campaignId,
        wellEdits,
    })

    try {
        await submitToApi({
            projectId: firstEdit.projectId,
            caseId: firstEdit.caseId,
            resourceName: firstEdit.resourceName,
            resourceId: campaignId,
            resourceObject: wellEdits,
        })
        tableQueueLogger.log("Campaign wells update successful")
    } catch (error) {
        tableQueueLogger.error("Campaign wells update failed", { error })
        throw error
    }
}

// Separates the queue into timeseries and override entries
const separateOverrideEntries = (queue: EditInstance[]): {
    timeseriesEntries: EditInstance[],
    overrideEntries: EditInstance[]
} => {
    tableQueueLogger.log("Separating override entries", { queue })
    const timeseriesEntries: EditInstance[] = []
    const overrideEntries: EditInstance[] = []

    queue.forEach((edit) => {
        if (edit.resourceName.endsWith("Override")) {
            overrideEntries.push(edit)
        } else {
            timeseriesEntries.push(edit)
        }
    })

    tableQueueLogger.log("Separated entries result", { timeseriesEntries, overrideEntries })
    return { timeseriesEntries, overrideEntries }
}

export const useTableQueue = ({
    isSaving, setIsSaving, addEdit, gridRef,
}: UseTableQueueProps) => {
    const [editQueue, setEditQueue] = useState<EditInstance[]>([])
    const [lastEditTime, setLastEditTime] = useState<number>(Date.now())
    const { submitToApi } = useSubmitToApi()

    const submitEditQueue = useCallback(async () => {
        tableQueueLogger.log("Attempting to submit edit queue", {
            isSaving,
            queueLength: editQueue.length,
            editQueue,
        })

        if (isSaving || editQueue.length === 0) {
            tableQueueLogger.log("Skipping submit - saving in progress or empty queue", {
                isSaving,
                queueLength: editQueue.length,
            })
            return
        }

        const firstEdit = editQueue[0]
        if (!firstEdit.projectId || !firstEdit.caseId) {
            tableQueueLogger.warn("Missing project or case ID", { firstEdit })
            return
        }

        tableQueueLogger.log("Processing edit based on resource key", {
            resourcePropertyKey: firstEdit.resourcePropertyKey,
        })

        try {
            setIsSaving(true)
            if (firstEdit.resourcePropertyKey === "rigUpgradingProfile" || firstEdit.resourcePropertyKey === "rigMobDemobProfile") {
                await submitCampaignEdit(editQueue, submitToApi)
            } else if (firstEdit.resourcePropertyKey === "campaignWells") {
                await submitCampaignWellsEdit(editQueue, submitToApi)
            } else {
                const filteredEditQueue = reduceToLatestEdits(editQueue)
                const separatedEntries = separateOverrideEntries(filteredEditQueue)

                const timeSeriesEntries = separatedEntries.timeseriesEntries.map((edit) => {
                    const resourceObject = edit.resourceObject as Components.Schemas.SaveTimeSeriesDto | Components.Schemas.SaveTimeSeriesOverrideDto
                    return {
                        profileType: edit.resourceName,
                        startYear: resourceObject.startYear,
                        values: resourceObject.values,
                    }
                })

                const overrideEntries = separatedEntries.overrideEntries.map((edit) => {
                    const resourceObject = edit.resourceObject as Components.Schemas.SaveTimeSeriesOverrideDto
                    return {
                        profileType: edit.resourceName,
                        startYear: resourceObject.startYear,
                        values: resourceObject.values,
                        override: true,
                    }
                })

                tableQueueLogger.log("Sending case profiles update", {
                    projectId: firstEdit.projectId,
                    caseId: firstEdit.caseId,
                    timeSeriesEntries,
                    overrideEntries,
                })

                await submitToApi({
                    projectId: firstEdit.projectId,
                    caseId: firstEdit.caseId,
                    resourceName: "case",
                    resourceObject: {
                        timeSeries: timeSeriesEntries,
                        overrideTimeSeries: overrideEntries,
                    } as unknown as ResourceObject,
                })
                tableQueueLogger.log("Case profiles update successful")
            }
        } catch (error) {
            tableQueueLogger.error("Edit submission failed", { error })
            throw error
        } finally {
            setIsSaving(false)
            setEditQueue([])
        }
    }, [editQueue, isSaving, submitToApi, setIsSaving])

    const addToQueue = useCallback((edit: EditInstance) => {
        tableQueueLogger.log("Adding edit to queue", { edit })
        setLastEditTime(Date.now())
        setEditQueue((prev) => {
            const newQueue = [...prev, edit]
            tableQueueLogger.log("Updated queue", { previousQueue: prev, newQueue })
            return newQueue
        })
    }, [])

    useEffect(() => {
        if (editQueue.length > 0) {
            tableQueueLogger.log("Setting up submit timer", {
                queueLength: editQueue.length,
                lastEditTime,
            })

            const timer = setTimeout(() => {
                const timeSinceLastEdit = Date.now() - lastEditTime
                tableQueueLogger.log("Checking submit conditions", {
                    timeSinceLastEdit,
                    hasEditingCells: gridRef.current?.api?.getEditingCells().length > 0,
                })

                if (timeSinceLastEdit >= 3000) {
                    if (gridRef.current?.api?.getEditingCells().length > 0) {
                        tableQueueLogger.log("Stopping grid editing")
                        gridRef.current.api.stopEditing()
                    } else {
                        tableQueueLogger.log("Triggering queue submission")
                        submitEditQueue()
                    }
                }
            }, 3000)
            return () => clearTimeout(timer)
        }
        return undefined
    }, [editQueue, lastEditTime, submitEditQueue, gridRef])

    return {
        editQueue,
        addToQueue,
        submitEditQueue,
    }
}
