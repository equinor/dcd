import { useQueryClient } from "@tanstack/react-query"
import { useParams } from "react-router"
import { useProjectContext } from "@/Store/ProjectContext"
import { EditInstance } from "@/Models/Interfaces"
import { TimeSeriesService, TimeSeriesData } from "@/Services/TimeSeriesService"
import { useBaseMutation, MutationParams } from "./useBaseMutation"

const getLatestEdits = (queue: EditInstance[], keySelector: (edit: EditInstance) => string): EditInstance[] => {
    const latestEditsMap = new Map<string, EditInstance>()
    queue.forEach((edit) => latestEditsMap.set(keySelector(edit), edit))
    return Array.from(latestEditsMap.values())
}

const categorizeTimeSeriesEntries = (queue: EditInstance[]): { timeseriesEntries: EditInstance[], overrideEntries: EditInstance[] } => queue.reduce((acc, edit) => {
    if (edit.resourceName.endsWith("Override")) {
        acc.overrideEntries.push(edit)
    } else {
        acc.timeseriesEntries.push(edit)
    }
    return acc
}, { timeseriesEntries: [] as EditInstance[], overrideEntries: [] as EditInstance[] })

export interface TimeSeriesMutationParams {
    timeSeriesEdits: EditInstance[];
}

/**
 * Hook for handling time series data mutations
 * Processes and submits time series edits to the API
 */
export const useTimeSeriesMutation = () => {
    const { caseId } = useParams()
    const { projectId } = useProjectContext()

    const timeSeriesMutationFn = async (
        service: typeof TimeSeriesService,
        projectIdParam: string,
        caseIdParam: string,
        params: MutationParams<TimeSeriesMutationParams>,
    ) => {
        const { timeSeriesEdits } = params.updatedValue

        const uniqueEdits = getLatestEdits(timeSeriesEdits, (edit) => edit.resourceName)
        const { timeseriesEntries, overrideEntries } = categorizeTimeSeriesEntries(uniqueEdits)

        // Convert EditInstance objects to TimeSeriesData format
        const timeSeriesData: TimeSeriesData[] = [
            ...timeseriesEntries.map((edit) => ({
                profileType: edit.resourceName,
                startYear: (edit.resourceObject as any).startYear,
                values: (edit.resourceObject as any).values,
                override: false,
            })),
            ...overrideEntries.map((edit) => ({
                profileType: edit.resourceName.replace("Override", ""),
                startYear: (edit.resourceObject as any).startYear,
                values: (edit.resourceObject as any).values,
                override: true,
            })),
        ]

        // Use the TimeSeriesService to save the profiles
        return service.saveProfiles(projectIdParam, caseIdParam, timeSeriesData)
    }

    const mutation = useBaseMutation({
        resourceName: "timeSeries",
        getService: () => TimeSeriesService,
        updateMethod: "saveProfiles", // Not directly used with custom mutation function
        customMutationFn: timeSeriesMutationFn,
        getResourceFromApiData: () => null, // Not used with custom mutation function
        loggerName: "TIME_SERIES_MUTATION",
        invalidateQueries: caseId && projectId ? [["caseApiData", projectId, caseId]] : [],
    })

    const submitTimeSeriesEdits = (timeSeriesEdits: EditInstance[]) => mutation.mutateAsync({
        updatedValue: { timeSeriesEdits },
        propertyKey: "timeSeriesEdits",
    })

    const updateProfileOverride = (profile: any) => {
        console.log("Processing override toggle:", {
            resourceId: profile.resourceId,
            resourceName: profile.resourceName,
            newOverrideState: profile.override,
            values: profile.values,
        })

        // When toggling an override we need to create either:
        // 1. A new entry in the override array (when turning override ON)
        // 2. A new entry in the regular array (when turning override OFF)

        // The TimeSeriesService will correctly separate entries based on the override flag
        const timeSeriesData: TimeSeriesData[] = [{
            profileType: profile.resourceName,
            startYear: profile.startYear,
            values: profile.values,
            override: profile.override,
        }]

        // When turning override OFF, we need to ensure we're sending an
        // entry that will clear any existing override
        return TimeSeriesService.saveProfiles(
            projectId || "",
            caseId || "",
            timeSeriesData,
        )
    }

    return {
        submitTimeSeriesEdits,
        updateProfileOverride,
        isLoading: mutation.isPending,
    }
}
