import { useCallback, useEffect } from "react"
import { useParams } from "react-router-dom"

import { useCampaignMutation } from "./Mutations/useCampaignMutation"
import { useTimeSeriesMutation } from "./Mutations/useTimeSeriesMutation"

import { CampaignProfileType } from "@/Components/CaseTabs/CaseDrillingSchedule/Components/CampaignProfileTypes"
import { ITimeSeries } from "@/Models/ITimeSeries"
import { EditInstance } from "@/Models/Interfaces"
import { useAppStore } from "@/Store/AppStore"
import { useEditQueue } from "@/Store/EditQueueContext"
import { useProjectContext } from "@/Store/ProjectContext"

// Queue processing utilities
const getLatestEdits = (queue: EditInstance[], keySelector: (edit: EditInstance) => string): EditInstance[] => {
    const latestEditsMap = new Map<string, EditInstance>()

    queue.forEach((edit) => latestEditsMap.set(keySelector(edit), edit))

    return Array.from(latestEditsMap.values())
}

/**
 * Structure for organizing edit queue by type
 */
interface QueueStats {
    profileEdits: EditInstance[] // Table edits for campaign profiles
    wellEdits: EditInstance[] // Campaign well edits
    caseEdits: EditInstance[] // All other edits for time series
}

/**
 * Categorizes edits into different types based on their resourcePropertyKey
 * - profileEdits: Campaign profile edits (rigUpgradingProfile, rigMobDemobProfile) from tables
 * - wellEdits: Campaign well edits (campaignWells) from tables
 * - caseEdits: All other edits that will be handled by the time series mutation
 */
const categorizeEdits = (queue: EditInstance[]): QueueStats => ({
    profileEdits: queue.filter((edit) => [CampaignProfileType.RigUpgrading, CampaignProfileType.RigMobDemob].includes(edit.resourcePropertyKey as CampaignProfileType)),
    wellEdits: queue.filter((edit) => edit.resourcePropertyKey === "campaignWells"),
    caseEdits: queue.filter((edit) => ![CampaignProfileType.RigUpgrading, CampaignProfileType.RigMobDemob, "campaignWells"].includes(edit.resourcePropertyKey)),
})

/**
 * Props for the edit queue handler hook
 */
interface EditQueueHandlerProps {
    gridRef?: React.RefObject<any>
    onSubmitSuccess?: () => void
    onSubmitError?: (error: Error) => void
    autoSubmitDelay?: number
}

/**
 * Hook for managing and processing edit queues
 * Handles batching, categorizing, and submitting edits to the appropriate services
 */
export const useEditQueueHandler = ({
    gridRef,
    onSubmitSuccess,
    onSubmitError,
    autoSubmitDelay = 2000,
}: EditQueueHandlerProps = {}) => {
    const {
        editQueue,
        clearQueue,
        addToQueue,
        lastEditTime,
    } = useEditQueue()
    const { submitTimeSeriesEdits } = useTimeSeriesMutation()
    const {
        updateRigUpgradingProfile,
        updateRigMobDemobProfile,
        updateCampaignWells,
    } = useCampaignMutation()

    const { projectId } = useProjectContext()
    const { caseId } = useParams()
    const { setIsSaving } = useAppStore()

    const processCampaignProfileEdits = useCallback(async (profileEdits: EditInstance[]) => {
        if (profileEdits.length === 0) { return }

        // Group by profile type and get latest edits
        const editsByType = {
            [CampaignProfileType.RigUpgrading]: getLatestEdits(
                profileEdits.filter((edit) => edit.resourcePropertyKey === CampaignProfileType.RigUpgrading),
                (edit) => edit.resourceId + edit.resourceName,
            ),
            [CampaignProfileType.RigMobDemob]: getLatestEdits(
                profileEdits.filter((edit) => edit.resourcePropertyKey === CampaignProfileType.RigMobDemob),
                (edit) => edit.resourceId + edit.resourceName,
            ),
        }

        const updatePromises = [
            // Process rig upgrading profiles
            ...editsByType[CampaignProfileType.RigUpgrading]
                .filter((edit) => edit.resourceId)
                .map((edit) => {
                    const profile = edit.resourceObject as ITimeSeries

                    try {
                        return updateRigUpgradingProfile(edit.resourceId!, profile)
                    } catch (error) {
                        console.error(`Failed to process ${CampaignProfileType.RigUpgrading} edit`, error)

                        return Promise.resolve()
                    }
                }),

            // Process rig mob/demob profiles
            ...editsByType[CampaignProfileType.RigMobDemob]
                .filter((edit) => edit.resourceId)
                .map((edit) => {
                    const profile = edit.resourceObject as ITimeSeries

                    try {
                        return updateRigMobDemobProfile(edit.resourceId!, profile)
                    } catch (error) {
                        console.error(`Failed to process ${CampaignProfileType.RigMobDemob} edit`, error)

                        return Promise.resolve()
                    }
                }),
        ]

        if (updatePromises.length > 0) {
            try {
                await Promise.all(updatePromises)
            } catch (error) {
                console.error("Error processing campaign profile edits:", error)
                throw error
            }
        }
    }, [updateRigUpgradingProfile, updateRigMobDemobProfile])

    const processWellEdits = useCallback(async (wellEdits: EditInstance[]) => {
        if (wellEdits.length === 0) {
            return
        }

        const editsByCampaign = wellEdits.reduce((acc, edit) => {
            if (!edit.resourceId) {
                console.error("Well edit missing resourceId", edit)

                return acc
            }

            if (!acc[edit.resourceId]) {
                acc[edit.resourceId] = []
            }
            acc[edit.resourceId].push(edit)

            return acc
        }, {} as Record<string, EditInstance[]>)

        await Promise.all(Object.entries(editsByCampaign).map(async ([campaignId, campaignWellEdits]) => {
            // Get the latest edits for each well to avoid duplicate updates
            const uniqueWellEdits = getLatestEdits(campaignWellEdits, (edit) => edit.wellId!)

            // Transform to the format expected by the API
            const wellUpdates = uniqueWellEdits
                .filter((edit) => edit.wellId !== undefined)
                .map((edit) => {
                    const timeSeriesData = edit.resourceObject as ITimeSeries

                    return {
                        wellId: edit.wellId!,
                        startYear: timeSeriesData.startYear,
                        values: timeSeriesData.values ?? [],
                    }
                })

            if (wellUpdates.length === 0) {
                return Promise.resolve()
            }

            try {
                await updateCampaignWells({
                    campaignId,
                    wells: wellUpdates,
                })

                return Promise.resolve()
            } catch (error) {
                console.error("Campaign wells update failed", { error, campaignId, wells: wellUpdates.map((w) => w.wellId) })
                throw error
            }
        }))
    }, [updateCampaignWells])

    /**
     * Processes all edits in the queue
     * Categorizes them and dispatches them to the appropriate processors
     */
    const processAllEdits = useCallback(async (edits: EditInstance[]) => {
        if (!projectId || !caseId || edits.length === 0) {
            return
        }

        const categorizedEdits = categorizeEdits(edits)
        const processingPromises: Promise<void>[] = []

        if (categorizedEdits.profileEdits.length > 0) {
            processingPromises.push(processCampaignProfileEdits(categorizedEdits.profileEdits))
        }

        if (categorizedEdits.caseEdits.length > 0) {
            processingPromises.push(submitTimeSeriesEdits(categorizedEdits.caseEdits) as unknown as Promise<void>)
        }

        if (categorizedEdits.wellEdits.length > 0) {
            processingPromises.push(processWellEdits(categorizedEdits.wellEdits))
        }

        await Promise.all(processingPromises)
    }, [projectId, caseId, processCampaignProfileEdits, processWellEdits, submitTimeSeriesEdits])

    const submitQueue = useCallback(async () => {
        if (editQueue.length === 0 || !projectId || !caseId) {
            return
        }

        try {
            setIsSaving(true)
            await processAllEdits(editQueue)
            clearQueue()
            onSubmitSuccess?.()
        } catch (error: unknown) {
            if (error instanceof Error) {
                onSubmitError?.(error)
            }
        } finally {
            setIsSaving(false)
        }
    }, [
        editQueue,
        projectId,
        caseId,
        processAllEdits,
        clearQueue,
        onSubmitSuccess,
        onSubmitError,
        setIsSaving,
    ])

    // Auto-submit queue after delay
    useEffect(() => {
        if (editQueue.length === 0) { return undefined }

        const timer = setTimeout(() => {
            submitQueue()
        }, autoSubmitDelay)

        return () => {
            clearTimeout(timer)
        }
    }, [editQueue, lastEditTime, submitQueue, autoSubmitDelay])

    return {
        editQueue,
        addToQueue,
        submitQueue,
        processAllEdits,
    }
}
