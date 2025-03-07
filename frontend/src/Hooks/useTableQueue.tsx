import { useCallback, useEffect } from "react"
import { useParams } from "react-router-dom"
import { EditInstance, ResourceObject } from "@/Models/Interfaces"
import { useSubmitToApi } from "./UseSubmitToApi"
import { useProjectContext } from "@/Store/ProjectContext"
import { ITimeSeries, TimeSeriesEntry } from "@/Models/ITimeSeries"
import { useEditQueue } from "@/Store/EditQueueContext"

interface TableQueueProps {
    setIsSaving: (isSaving: boolean) => void
    isSaving: boolean
    gridRef: React.RefObject<any>
    alignedGridsRef?: React.RefObject<any>[]
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

// API submission handlers
const submitCampaignUpdates = async (editQueue: EditInstance[], submitToApi: any) => {
    const rigUpgradingEdits = getLatestEdits(
        editQueue.filter((edit) => edit.resourcePropertyKey === "rigUpgradingProfile"),
        (edit) => edit.resourceId + edit.resourceName,
    )
    const rigMobDemobEdits = getLatestEdits(
        editQueue.filter((edit) => edit.resourcePropertyKey === "rigMobDemobProfile"),
        (edit) => edit.resourceId + edit.resourceName,
    )

    const allEdits = [...rigUpgradingEdits, ...rigMobDemobEdits]
    await Promise.all(allEdits.map(async (edit) => {
        const timeSeriesData = edit.resourceObject as ITimeSeries
        const resourceName = edit.resourcePropertyKey === "rigUpgradingProfile" ? "rigUpgrading" : "rigMobDemob"

        try {
            await submitToApi({
                projectId: edit.projectId,
                caseId: edit.caseId,
                resourceName,
                resourceObject: {
                    startYear: timeSeriesData?.startYear ?? 0,
                    values: timeSeriesData?.values ?? [],
                },
                resourceId: edit.resourceId,
            })
        } catch (error) {
            console.error("Campaign profile update failed", { error, edit })
            throw error
        }
    }))
}

const submitWellUpdates = async (editQueue: EditInstance[], submitToApi: any) => {
    const uniqueWellEdits = getLatestEdits(editQueue, (edit) => edit.wellId!)

    if (uniqueWellEdits.length === 0) {
        return
    }

    const firstEdit = uniqueWellEdits[0]
    const wellUpdates = uniqueWellEdits.map((edit) => {
        const timeSeriesData = edit.resourceObject as ITimeSeries
        return {
            wellId: edit.wellId,
            startYear: timeSeriesData.startYear,
            values: timeSeriesData.values,
        }
    })

    try {
        await submitToApi({
            projectId: firstEdit.projectId,
            caseId: firstEdit.caseId,
            resourceName: firstEdit.resourceName,
            resourceId: firstEdit.resourceId,
            resourceObject: wellUpdates,
        })
    } catch (error) {
        console.error("Campaign wells update failed", { error, wells: wellUpdates.map((w) => w.wellId) })
        throw error
    }
}

const submitCaseUpdates = async (editQueue: EditInstance[], submitToApi: any, projectId: string, caseId: string) => {
    const uniqueEdits = getLatestEdits(editQueue, (edit) => edit.resourceName)
    const { timeseriesEntries, overrideEntries } = categorizeTimeSeriesEntries(uniqueEdits)

    const createTimeSeriesEntry = (edit: EditInstance, isOverride = false): TimeSeriesEntry => {
        const resourceObject = edit.resourceObject as Components.Schemas.SaveTimeSeriesDto | Components.Schemas.SaveTimeSeriesOverrideDto
        return {
            profileType: edit.resourceName,
            startYear: resourceObject.startYear,
            values: resourceObject.values,
            ...(isOverride && { override: true }),
        }
    }

    const timeSeriesData = timeseriesEntries.map((edit) => createTimeSeriesEntry(edit))
    const overrideData = overrideEntries.map((edit) => createTimeSeriesEntry(edit, true))

    await submitToApi({
        projectId,
        caseId,
        resourceName: "caseProfiles",
        resourceObject: {
            timeSeries: timeSeriesData,
            overrideTimeSeries: overrideData,
        } as unknown as ResourceObject,
    })
}

// Main hook
export const useTableQueue = ({
    isSaving, setIsSaving, gridRef, alignedGridsRef,
}: TableQueueProps) => {
    const {
        editQueue,
        addToQueue,
        clearQueue,
        setTimer,
        clearTimer,
    } = useEditQueue()
    const { submitToApi } = useSubmitToApi()
    const { projectId } = useProjectContext()
    const { caseId, tab } = useParams()

    const submitEditQueue = useCallback(async () => {
        if (isSaving || editQueue.length === 0 || !projectId || !caseId) {
            if (!projectId || !caseId) {
                console.warn("Cannot submit edits - missing project or case ID", { projectId, caseId })
            }
            return
        }

        try {
            setIsSaving(true)
            const { profileEdits, wellEdits, caseEdits } = categorizeEdits(editQueue)

            await Promise.all([
                profileEdits.length > 0 && submitCampaignUpdates(profileEdits, submitToApi),
                wellEdits.length > 0 && submitWellUpdates(wellEdits, submitToApi),
                caseEdits.length > 0 && submitCaseUpdates(caseEdits, submitToApi, projectId, caseId),
            ].filter(Boolean))
        } catch (error) {
            console.error("Edit submission failed:", error)
            throw error
        } finally {
            setIsSaving(false)
            clearQueue()
        }
    }, [editQueue, isSaving, submitToApi, setIsSaving, projectId, caseId])

    useEffect(() => {
        if (editQueue.length === 0) { return }

        clearTimer()

        const newTimer = setTimeout(() => {
            const refsWithEditingCells = alignedGridsRef?.filter((ref) => ref.current?.api?.getEditingCells().length > 0)
            if (refsWithEditingCells && refsWithEditingCells.length > 0) {
                refsWithEditingCells.forEach((ref) => ref.current.api.stopEditing())
            } else {
                submitEditQueue()
            }
        }, 3000)

        setTimer(newTimer)
    }, [editQueue, gridRef, alignedGridsRef, clearTimer, setTimer])

    return { editQueue, addToQueue, submitEditQueue }
}
