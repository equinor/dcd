import { useMutation, useQueryClient } from "@tanstack/react-query"
import { useParams } from "react-router"
import { useProjectContext } from "@/Store/ProjectContext"
import { useAppStore } from "@/Store/AppStore"
import { createLogger } from "@/Utils/logger"
import { GetCaseService } from "@/Services/CaseService"
import { EditInstance } from "@/Models/Interfaces"
import { ProfileTypes } from "@/Models/enums"

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

interface TimeSeriesEntry {
    profileType: string;
    startYear: number;
    values: number[];
}

interface SaveTimeSeriesOverrideDto extends TimeSeriesEntry {
    override: boolean;
}

interface SaveTimeSeriesListDto {
    timeSeries: TimeSeriesEntry[];
    overrideTimeSeries: SaveTimeSeriesOverrideDto[];
}

export const useTimeSeriesQueueMutation = () => {
    const queryClient = useQueryClient()
    const { caseId } = useParams()
    const { projectId } = useProjectContext()
    const { setIsSaving, setSnackBarMessage } = useAppStore()

    const logger = createLogger({
        name: "TIME_SERIES_QUEUE_MUTATION",
        enabled: false,
    })

    const mutation = useMutation({
        mutationFn: async ({
            timeSeriesEdits,
        }: {
            timeSeriesEdits: EditInstance[],
        }) => {
            if (!projectId || !caseId) {
                throw new Error("Project ID and Case ID are required")
            }

            setIsSaving(true)
            logger.info("Updating time series queue:", { timeSeriesEdits })

            try {
                const service = GetCaseService()
                const uniqueEdits = getLatestEdits(timeSeriesEdits, (edit) => edit.resourceName)
                const { timeseriesEntries, overrideEntries } = categorizeTimeSeriesEntries(uniqueEdits)

                const createTimeSeriesEntry = (edit: EditInstance): TimeSeriesEntry => {
                    const resourceObject = edit.resourceObject as any

                    let profileType = edit.resourceName

                    // Handle specific profile types with correct casing
                    if (profileType.toLowerCase() === "onshorerelatedopexcostprofile") {
                        profileType = "OnshoreRelatedOpexCostProfile" as any
                    } else if (profileType.toLowerCase() === "additionalopexcostprofile") {
                        profileType = "AdditionalOpexCostProfile" as any
                    } else {
                        const enumKey = Object.keys(ProfileTypes).find(
                            (key) => key.toLowerCase() === profileType.toLowerCase(),
                        )
                        if (enumKey) {
                            profileType = ProfileTypes[enumKey as keyof typeof ProfileTypes]
                        }
                    }

                    return {
                        profileType,
                        startYear: resourceObject.startYear,
                        values: resourceObject.values,
                    }
                }

                const createOverrideEntry = (edit: EditInstance): SaveTimeSeriesOverrideDto => {
                    const baseEntry = createTimeSeriesEntry(edit)
                    return {
                        ...baseEntry,
                        override: true,
                    }
                }

                const timeSeriesData = timeseriesEntries.map(createTimeSeriesEntry)
                const overrideData = overrideEntries.map(createOverrideEntry)

                const saveTimeSeriesDto: SaveTimeSeriesListDto = {
                    timeSeries: timeSeriesData,
                    overrideTimeSeries: overrideData,
                }

                const result = await service.saveProfiles(projectId, caseId, saveTimeSeriesDto)
                return result
            } finally {
                setIsSaving(false)
            }
        },
        onSuccess: () => {
            if (projectId && caseId) {
                queryClient.invalidateQueries({ queryKey: ["caseApiData", projectId, caseId] })
            }
        },
        onError: (error: any) => {
            setSnackBarMessage(error.message || "Failed to update time series profiles")
            logger.error("Error updating time series profiles:", error)
        },
    })

    const submitTimeSeriesQueue = (timeSeriesEdits: EditInstance[]) => mutation.mutateAsync({
        timeSeriesEdits,
    })

    return {
        submitTimeSeriesQueue,
        isLoading: mutation.isPending,
    }
}
