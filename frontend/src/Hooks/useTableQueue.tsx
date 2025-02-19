import { useState, useCallback, useEffect } from "react"
import { EditInstance } from "@/Models/Interfaces"
import { GetCaseService } from "@/Services/CaseService"
import { GetDrillingCampaignsService } from "@/Services/DrillingCampaignsService"
// Reduces the queue to the latest edit for each resource given the order of the queue and the resourceName
const reduceToLatestEdits = (queue: EditInstance[]): EditInstance[] => {
    const latestEditsMap = new Map<string, EditInstance>()

    queue.forEach((edit) => {
        latestEditsMap.set(edit.resourceName, edit)
    })

    return Array.from(latestEditsMap.values())
}

interface UseTableQueueProps {
    isSaving: boolean
    addEdit: (edit: EditInstance) => Promise<any>
    gridRef: any
}

// Separates the queue into timeseries and override entries
const separateOverrideEntries = (queue: EditInstance[]):
{
    timeseriesEntries: EditInstance[],
    overrideEntries: EditInstance[]
} => {
    const timeseriesEntries: EditInstance[] = []
    const overrideEntries: EditInstance[] = []

    queue.forEach((edit) => {
        if (edit.resourceName.endsWith("Override")) {
            overrideEntries.push(edit)
        } else {
            timeseriesEntries.push(edit)
        }
    })

    return { timeseriesEntries, overrideEntries }
}

export const useTableQueue = ({ isSaving, addEdit, gridRef }: UseTableQueueProps) => {
    const [editQueue, setEditQueue] = useState<EditInstance[]>([])
    const [lastEditTime, setLastEditTime] = useState<number>(Date.now())

    const submitEditQueue = useCallback(async () => {
        if (isSaving || editQueue.length === 0) { return }

        const firstEdit = editQueue[0]
        if (!firstEdit.projectId || !firstEdit.caseId) { return }

        const filteredEditQueue = reduceToLatestEdits(editQueue)
        const separatedEntries = separateOverrideEntries(filteredEditQueue)
        console.log("editQueue", editQueue)
        console.log("filteredEditQueue", filteredEditQueue)
        console.log("separatedEntries", separatedEntries)

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

        await GetCaseService().saveProfiles(
            firstEdit.projectId,
            firstEdit.caseId,
            {
                timeSeries: timeSeriesEntries,
                overrideTimeSeries: overrideEntries,
            },
        )

        // await GetDrillingCampaignsService().updateCampaign(
        //     firstEdit.projectId,
        //     firstEdit.caseId,
        //     firstEdit.campaignId!,
        //     {
        //         rigUpgradingCost: 0,
        //         rigUpgradingCostStartYear: 0,
        //         rigUpgradingCostValues: "",
        //         rigMobDemobCost: 0,
        //         rigMobDemobCostStartYear: "",
        //         rigMobDemobCostValues: "",
        //         campaignWells: [],
        //     },
        // )
        // invalidate case api data
        setEditQueue([])
    }, [editQueue, isSaving, addEdit])

    const addToQueue = useCallback((edit: EditInstance) => {
        setLastEditTime(Date.now())
        setEditQueue((prev) => [...prev, edit])
    }, [])

    useEffect(() => {
        if (editQueue.length > 0) {
            const timer = setTimeout(() => {
                const timeSinceLastEdit = Date.now() - lastEditTime
                if (timeSinceLastEdit >= 3000) {
                    if (gridRef.current?.api?.getEditingCells().length > 0) {
                        gridRef.current.api.stopEditing()
                    } else {
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
