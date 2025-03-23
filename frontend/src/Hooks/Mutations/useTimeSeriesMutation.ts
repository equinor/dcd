import { useParams } from "react-router"

import { useBaseMutation, MutationParams } from "./useBaseMutation"

import { EditInstance, ResourceName } from "@/Models/Interfaces"
import { TimeSeriesService, TimeSeriesData } from "@/Services/TimeSeriesService"
import { useProjectContext } from "@/Store/ProjectContext"

const getLatestEdits = (queue: EditInstance[], keySelector: (edit: EditInstance) => string): EditInstance[] => {
    const latestEditsMap = new Map<string, EditInstance>()

    queue.forEach((edit) => latestEditsMap.set(keySelector(edit), edit))

    return Array.from(latestEditsMap.values())
}

/**
 * Categorizes time series entries into regular entries and override entries
 * based on whether the resource name ends with "Override"
 */
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

    /**
     * Processes time series edits to be sent to the backend
     * Handles both regular edits and override edits
     */
    const timeSeriesMutationFn = async (
        service: typeof TimeSeriesService,
        projectIdParam: string,
        caseIdParam: string,
        params: MutationParams<TimeSeriesMutationParams>,
    ) => {
        const { timeSeriesEdits } = params.updatedValue

        // Get the latest edit for each resource name
        const uniqueEdits = getLatestEdits(timeSeriesEdits, (edit) => edit.resourceName)

        // Pre-process edits to ensure they're directed to the correct profile type
        const modifiedEdits = uniqueEdits.map((edit) => {
            const resourceObj = edit.resourceObject as any

            // Check two things:
            // 1. If this edit is for a row with an active override profile
            // 2. If the row is in override mode (override toggle is enabled)
            const hasOverrideProfile = resourceObj?.overrideProfile !== undefined
            const isOverrideActive = hasOverrideProfile && resourceObj.overrideProfile?.override === true

            // If the row is in override mode and we're editing a regular profile,
            // redirect the edit to the override profile
            if (isOverrideActive && !edit.resourceName.endsWith("Override")) {
                return {
                    ...edit,
                    resourceName: `${edit.resourceName}Override` as ResourceName,
                }
            }

            // Otherwise, keep the edit as is
            return edit
        })

        // Categorize into regular and override entries based on resource name
        const { timeseriesEntries, overrideEntries } = categorizeTimeSeriesEntries(modifiedEdits)

        // Convert EditInstance objects to TimeSeriesData format
        const timeSeriesData: TimeSeriesData[] = [
            // Regular profiles with override=false
            ...timeseriesEntries.map((edit) => {
                const resourceObj = edit.resourceObject as any

                return {
                    profileType: edit.resourceName,
                    startYear: resourceObj.startYear ?? 0,
                    values: resourceObj.values ?? [],
                    override: false,
                }
            }),

            // Override profiles with override=true (or the value from overrideProfile)
            ...overrideEntries.map((edit) => {
                const resourceObj = edit.resourceObject as any
                const override = resourceObj.overrideProfile?.override !== undefined
                    ? resourceObj.overrideProfile.override
                    : true

                return {
                    // Keep the Override suffix
                    profileType: edit.resourceName,
                    startYear: resourceObj.startYear ?? 0,
                    values: resourceObj.values ?? [],
                    override,
                }
            }),
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
        invalidateQueries: caseId && projectId ? [["caseApiData", projectId, caseId]] : [],
    })

    const submitTimeSeriesEdits = (timeSeriesEdits: EditInstance[]) => mutation.mutateAsync({
        updatedValue: { timeSeriesEdits },
        propertyKey: "timeSeriesEdits",
    })

    /**
     * Specifically handles toggling the override state of a profile
     * This is called directly from the CalculationSourceToggle component
     */
    const updateProfileOverride = (profile: any) => {
        // Always use a profile name with the "Override" suffix for toggle operations
        const overrideProfileType = profile.resourceName.endsWith("Override")
            ? profile.resourceName
            : `${profile.resourceName}Override`

        // and override flag
        const timeSeriesData: TimeSeriesData[] = [{
            profileType: overrideProfileType,
            startYear: profile.startYear ?? 0,
            values: profile.values ?? [],
            override: !!profile.override, // Ensure it's a boolean
        }]

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
