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

const submitProfileEdits = async (editQueue: EditInstance[], submitToApi: any) => {
    tableQueueLogger.log("Submitting campaign edit", { editQueue })

    // Group edits by their property key
    const rigUpgradingEdit = editQueue.find((edit) => edit.resourcePropertyKey === "rigUpgradingProfile")
    const rigMobDemobEdit = editQueue.find((edit) => edit.resourcePropertyKey === "rigMobDemobProfile")

    tableQueueLogger.log("Found campaign profile edits", {
        hasRigUpgradingEdit: !!rigUpgradingEdit,
        hasRigMobDemobEdit: !!rigMobDemobEdit,
        rigUpgradingEdit,
        rigMobDemobEdit,
    })
    // Handle profile updates
    const updateRigMobDemobDto = {
        startYear: (rigMobDemobEdit?.resourceObject as TimeSeriesResourceObject)?.startYear ?? 0,
        values: (rigMobDemobEdit?.resourceObject as TimeSeriesResourceObject)?.values ?? [],
    }

    const updateRigUpgradingDto = {
        startYear: (rigUpgradingEdit?.resourceObject as TimeSeriesResourceObject)?.startYear ?? 0,
        values: (rigUpgradingEdit?.resourceObject as TimeSeriesResourceObject)?.values ?? [],
    }

    tableQueueLogger.log("Sending campaign update", {
        updateDto: rigUpgradingEdit ? updateRigUpgradingDto : updateRigMobDemobDto,
    })

    const promises = editQueue.map((edit) => submitToApi({
        projectId: edit.projectId,
        caseId: edit.caseId,
        resourceName: rigUpgradingEdit ? "rigUpgrading" : "rigMobDemob",
        resourceId: edit.campaignId,
        resourceObject: rigUpgradingEdit ? updateRigUpgradingDto : updateRigMobDemobDto,
    }).catch((error: unknown) => {
        tableQueueLogger.error("Campaign update failed", { error })
        throw error
    }))

    await Promise.all(promises)
    tableQueueLogger.log("Campaign update successful")
}

const submitWellsEdits = async (editQueue: EditInstance[], submitToApi: any) => {
    tableQueueLogger.log("Submitting campaign wells edits", { editQueue })

    const promises = editQueue.map((edit) => {
        const wellEditObject = {
            wellId: edit.wellId,
            startYear: (edit.resourceObject as TimeSeriesResourceObject).startYear,
            values: (edit.resourceObject as TimeSeriesResourceObject).values,
        }
        return submitToApi({
            projectId: edit.projectId,
            caseId: edit.caseId,
            resourceName: edit.resourceName,
            resourceId: edit.resourceId,
            resourceObject: [wellEditObject],
        }).catch((error: unknown) => {
            tableQueueLogger.error("Campaign wells update failed", { error })
            throw error
        })
    })

    await Promise.all(promises)
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

const submitCaseEdit = async (editQueue: EditInstance[], submitToApi: any) => {
    tableQueueLogger.log("Submitting case edit", { editQueue })

    const firstEdit = editQueue[0]
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

export const useTableQueue = ({
    isSaving, setIsSaving, gridRef,
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
                await submitProfileEdits(editQueue, submitToApi)
            } else if (firstEdit.resourcePropertyKey === "campaignWells") {
                await submitWellsEdits(editQueue, submitToApi)
            } else {
                await submitCaseEdit(editQueue, submitToApi)
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
