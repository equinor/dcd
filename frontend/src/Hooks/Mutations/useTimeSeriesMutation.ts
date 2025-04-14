import { useMutation, useQueryClient } from "@tanstack/react-query"
import { useParams } from "react-router"

import { EditInstance, ResourceName } from "@/Models/Interfaces"
import { TimeSeriesService, TimeSeriesData } from "@/Services/TimeSeriesService"
import { useAppStore } from "@/Store/AppStore"
import { useProjectContext } from "@/Store/ProjectContext"

export interface Params {
    updatedValue: string | number | boolean;
    propertyKey: keyof Components.Schemas.SaveTimeSeriesDto | keyof Components.Schemas.SaveTimeSeriesOverrideDto;
}

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
    const queryClient = useQueryClient()
    const { setIsSaving, setSnackBarMessage } = useAppStore()

    const mutation = useMutation({
        mutationFn: async (params: any) => {
            if (!projectId || !caseId) {
                throw new Error("Project ID and Case ID are required")
            }

            setIsSaving(true)

            try {
                const { timeSeriesEdits } = params.updatedValue

                const uniqueEdits = getLatestEdits(timeSeriesEdits, (edit) => edit.resourceName)

                const modifiedEdits = uniqueEdits.map((edit) => {
                    const resourceObj = edit.resourceObject as any

                    const hasOverrideProfile = resourceObj?.overrideProfile !== undefined
                    const isOverrideActive = hasOverrideProfile && resourceObj.overrideProfile?.override === true

                    if (isOverrideActive && !edit.resourceName.endsWith("Override")) {
                        return {
                            ...edit,
                            resourceName: `${edit.resourceName}Override` as ResourceName,
                        }
                    }

                    return edit
                })

                const { timeseriesEntries, overrideEntries } = categorizeTimeSeriesEntries(modifiedEdits)

                const timeSeriesData: TimeSeriesData[] = [
                    ...timeseriesEntries.map((edit) => {
                        const resourceObj = edit.resourceObject as any

                        return {
                            profileType: edit.resourceName,
                            startYear: resourceObj.startYear ?? 0,
                            values: resourceObj.values ?? [],
                            override: false,
                        }
                    }),

                    ...overrideEntries.map((edit) => {
                        const resourceObj = edit.resourceObject as any
                        const override = resourceObj.overrideProfile?.override !== undefined
                            ? resourceObj.overrideProfile.override
                            : true

                        return {
                            profileType: edit.resourceName,
                            startYear: resourceObj.startYear ?? 0,
                            values: resourceObj.values ?? [],
                            override,
                        }
                    }),
                ]

                return TimeSeriesService.saveProfiles(projectId, caseId, timeSeriesData)
            } finally {
                setIsSaving(false)
            }
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["caseApiData", projectId, caseId] })
        },
        onError: (error: unknown) => {
            if (error instanceof Error) {
                setSnackBarMessage(error.message || "Failed to update timeseriesprofile")
            }
        },
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
