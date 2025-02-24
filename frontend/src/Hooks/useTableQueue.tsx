import { useState, useCallback, useEffect } from "react"
import { useParams } from "react-router-dom"
import { EditInstance, ResourceObject } from "@/Models/Interfaces"
import { useSubmitToApi } from "./UseSubmitToApi"
import { useProjectContext } from "@/Store/ProjectContext"

// Types and Interfaces
interface TableQueueProps {
    setIsSaving: (isSaving: boolean) => void
    isSaving: boolean
    gridRef: React.RefObject<any>
}

interface TimeSeriesData {
    startYear: number
    values: number[]
}

interface QueueStats {
    profileEdits: EditInstance[]
    wellEdits: EditInstance[]
    caseEdits: EditInstance[]
}

interface TimeSeriesEntry {
    profileType: string
    startYear: number
    values: number[]
    override?: boolean
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
const submitProfileUpdates = async (editQueue: EditInstance[], submitToApi: any) => {
    const rigUpgradingEdits = getLatestEdits(
        editQueue.filter((edit) => edit.resourcePropertyKey === "rigUpgradingProfile"),
        (edit) => edit.resourceName,
    )
    const rigMobDemobEdits = getLatestEdits(
        editQueue.filter((edit) => edit.resourcePropertyKey === "rigMobDemobProfile"),
        (edit) => edit.resourceName,
    )

    const allEdits = [...rigUpgradingEdits, ...rigMobDemobEdits]
    await Promise.all(allEdits.map(async (edit) => {
        const timeSeriesData = edit.resourceObject as TimeSeriesData
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

    await Promise.all(uniqueWellEdits.map(async (edit) => {
        const timeSeriesData = edit.resourceObject as TimeSeriesData
        try {
            await submitToApi({
                projectId: edit.projectId,
                caseId: edit.caseId,
                resourceName: edit.resourceName,
                resourceId: edit.resourceId,
                resourceObject: [{
                    wellId: edit.wellId,
                    startYear: timeSeriesData.startYear,
                    values: timeSeriesData.values,
                }],
            })
        } catch (error) {
            console.error("Campaign wells update failed", { error, wellId: edit.wellId })
            throw error
        }
    }))
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
    isSaving, setIsSaving, gridRef,
}: TableQueueProps) => {
    const [editQueue, setEditQueue] = useState<EditInstance[]>([])
    const [lastEditTime, setLastEditTime] = useState<number>(Date.now())
    const { submitToApi } = useSubmitToApi()
    const { projectId } = useProjectContext()
    const { caseId } = useParams()

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
                profileEdits.length > 0 && submitProfileUpdates(profileEdits, submitToApi),
                wellEdits.length > 0 && submitWellUpdates(wellEdits, submitToApi),
                caseEdits.length > 0 && submitCaseUpdates(caseEdits, submitToApi, projectId, caseId),
            ].filter(Boolean))
        } catch (error) {
            console.error("Edit submission failed:", error)
            throw error
        } finally {
            setIsSaving(false)
            setEditQueue([])
        }
    }, [editQueue, isSaving, submitToApi, setIsSaving, projectId, caseId])

    const addToQueue = useCallback((edit: EditInstance) => {
        setLastEditTime(Date.now())
        setEditQueue((prev) => {
            const newQueue = [...prev, edit]
            return newQueue
        })
    }, [])

    useEffect(() => {
        if (editQueue.length === 0) { return }

        const timer = setTimeout(() => {
            const timeSinceLastEdit = Date.now() - lastEditTime
            const hasEditingCells = gridRef.current?.api?.getEditingCells().length > 0

            if (timeSinceLastEdit >= 3000) {
                if (hasEditingCells) {
                    gridRef.current.api.stopEditing()
                } else {
                    submitEditQueue()
                }
            }
        }, 3000)

        const cleanup = () => {
            clearTimeout(timer)
        }
        // eslint-disable-next-line consistent-return
        return cleanup
    }, [editQueue, lastEditTime, submitEditQueue, gridRef])

    return { editQueue, addToQueue, submitEditQueue }
}
