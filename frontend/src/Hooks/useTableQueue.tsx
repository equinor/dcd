import { useCallback, useEffect } from "react"
import { useParams } from "react-router-dom"
import { EditInstance } from "@/Models/Interfaces"
import { useTimeSeriesQueueMutation } from "./Mutations/useTimeSeriesQueueMutation"
import { useProjectContext } from "@/Store/ProjectContext"
import { ITimeSeries } from "@/Models/ITimeSeries"
import { useEditQueue } from "@/Store/EditQueueContext"
import { useAppStore } from "@/Store/AppStore"
import { useCampaignMutation } from "./Mutations/useCampaignMutation"
import { useWellMutation } from "./Mutations/useWellMutation"

interface TableQueueProps {
    gridRef: React.RefObject<any>
    onSubmitSuccess?: () => void
    onSubmitError?: (error: any) => void
}

interface QueueStats {
    profileEdits: EditInstance[]
    wellEdits: EditInstance[]
    caseEdits: EditInstance[]
}

// Queue processing utilities
const getLatestEdits = (queue: EditInstance[], keySelector: (edit: EditInstance) => string): EditInstance[] => {
    const latestEditsMap = new Map<string, EditInstance>()
    queue.forEach((edit) => latestEditsMap.set(keySelector(edit), edit))
    return Array.from(latestEditsMap.values())
}

const categorizeEdits = (queue: EditInstance[]): QueueStats => ({
    profileEdits: queue.filter((edit) => ["rigUpgradingProfile", "rigMobDemobProfile"].includes(edit.resourcePropertyKey)),
    wellEdits: queue.filter((edit) => edit.resourcePropertyKey === "campaignWells"),
    caseEdits: queue.filter((edit) => !["rigUpgradingProfile", "rigMobDemobProfile", "campaignWells"].includes(edit.resourcePropertyKey)),
})

const categorizeTimeSeriesEntries = (queue: EditInstance[]): { timeseriesEntries: EditInstance[], overrideEntries: EditInstance[] } => queue.reduce((acc, edit) => {
    if (edit.resourceName.endsWith("Override")) {
        acc.overrideEntries.push(edit)
    } else {
        acc.timeseriesEntries.push(edit)
    }
    return acc
}, { timeseriesEntries: [] as EditInstance[], overrideEntries: [] as EditInstance[] })

export const useTableQueue = ({
    gridRef,
    onSubmitSuccess,
    onSubmitError,
}: TableQueueProps) => {
    const {
        editQueue, clearQueue, addToQueue, lastEditTime,
    } = useEditQueue()
    const { submitTimeSeriesQueue } = useTimeSeriesQueueMutation()
    const { updateRigUpgradingCost, updateRigMobDemobCost } = useCampaignMutation()
    const { updateCampaignWells } = useWellMutation()
    const { projectId } = useProjectContext()
    const { caseId } = useParams()
    const { setIsSaving: appSetIsSaving } = useAppStore()

    const submitCampaignUpdates = async (campaignEdits: EditInstance[]) => {
        const rigUpgradingEdits = getLatestEdits(
            campaignEdits.filter((edit) => edit.resourcePropertyKey === "rigUpgradingProfile"),
            (edit) => edit.resourceId + edit.resourceName,
        )
        const rigMobDemobEdits = getLatestEdits(
            campaignEdits.filter((edit) => edit.resourcePropertyKey === "rigMobDemobProfile"),
            (edit) => edit.resourceId + edit.resourceName,
        )

        const allEdits = [...rigUpgradingEdits, ...rigMobDemobEdits]
        await Promise.all(allEdits.map(async (edit) => {
            const timeSeriesData = edit.resourceObject as ITimeSeries

            if (!edit.resourceId) {
                console.error("Campaign edit missing resourceId", edit)
                return
            }

            try {
                if (edit.resourcePropertyKey === "rigUpgradingProfile") {
                    await updateRigUpgradingCost(
                        edit.resourceId,
                        {
                            startYear: timeSeriesData?.startYear ?? 0,
                            values: timeSeriesData?.values ?? [],
                        },
                    )
                } else {
                    await updateRigMobDemobCost(
                        edit.resourceId,
                        {
                            startYear: timeSeriesData?.startYear ?? 0,
                            values: timeSeriesData?.values ?? [],
                        },
                    )
                }
            } catch (error) {
                console.error("Campaign profile update failed", { error, edit })
                throw error
            }
        }))
    }

    const submitWellUpdates = async (wellEdits: EditInstance[]) => {
        const uniqueWellEdits = getLatestEdits(wellEdits, (edit) => edit.wellId!)

        if (uniqueWellEdits.length === 0) {
            return
        }

        const firstEdit = uniqueWellEdits[0]

        // Skip if resourceId is undefined
        if (!firstEdit.resourceId) {
            console.error("Well edit missing resourceId", firstEdit)
            return
        }

        const wellUpdates = uniqueWellEdits.map((edit) => {
            const timeSeriesData = edit.resourceObject as ITimeSeries
            return {
                wellId: edit.wellId,
                startYear: timeSeriesData.startYear,
                values: timeSeriesData.values,
            }
        })

        try {
            await updateCampaignWells({
                campaignId: firstEdit.resourceId,
                wells: wellUpdates,
            })
        } catch (error) {
            console.error("Campaign wells update failed", { error, wells: wellUpdates.map((w) => w.wellId) })
            throw error
        }
    }

    const submitEditQueue = useCallback(async () => {
        if (editQueue.length === 0) {
            return
        }

        if (!projectId || !caseId) {
            return
        }

        try {
            appSetIsSaving(true)
            const categorizedEdits = categorizeEdits(editQueue)

            if (categorizedEdits.profileEdits.length > 0) {
                await submitCampaignUpdates(categorizedEdits.profileEdits)
            }

            if (categorizedEdits.caseEdits.length > 0) {
                await submitTimeSeriesQueue(categorizedEdits.caseEdits)
            }

            if (categorizedEdits.wellEdits.length > 0) {
                await submitWellUpdates(categorizedEdits.wellEdits)
            }

            clearQueue()
            onSubmitSuccess?.()
        } catch (error: any) {
            onSubmitError?.(error)
        } finally {
            appSetIsSaving(false)
        }
    }, [
        editQueue,
        projectId,
        caseId,
        submitTimeSeriesQueue,
        clearQueue,
        onSubmitSuccess,
        onSubmitError,
        appSetIsSaving,
        updateRigUpgradingCost,
        updateRigMobDemobCost,
        updateCampaignWells,
    ])

    useEffect(() => {
        if (editQueue.length === 0) { return }

        const timer = setTimeout(() => {
            submitEditQueue()
        }, 2000)

        const cleanup = () => {
            clearTimeout(timer)
        }

        // eslint-disable-next-line consistent-return
        return cleanup
    }, [editQueue, lastEditTime, submitEditQueue, gridRef])

    return { editQueue, addToQueue, submitEditQueue }
}
