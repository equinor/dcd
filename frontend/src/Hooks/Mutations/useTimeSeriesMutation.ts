import { useMutation, useQueryClient } from "@tanstack/react-query"
import { useParams } from "react-router"
import { useProjectContext } from "@/Store/ProjectContext"
import { useAppStore } from "@/Store/AppStore"
import { createLogger } from "@/Utils/logger"
import { GetCaseService } from "@/Services/CaseService"

export const useTimeSeriesMutation = () => {
    const queryClient = useQueryClient()
    const { caseId } = useParams()
    const { projectId } = useProjectContext()
    const { setIsSaving, setSnackBarMessage } = useAppStore()

    const logger = createLogger({
        name: "TIME_SERIES_MUTATION",
        enabled: false,
    })

    const mutation = useMutation({
        mutationFn: async ({
            profile,
            toggleOverride = false,
        }: {
            profile: any,
            toggleOverride?: boolean,
        }) => {
            if (!projectId || !caseId) {
                throw new Error("Project ID and Case ID are required")
            }

            setIsSaving(true)
            logger.info("Updating time series profile:", { profile, toggleOverride })

            try {
                const service = GetCaseService()

                // If we're toggling the override flag, update it
                const updatedProfile = toggleOverride
                    ? { ...profile, override: !profile.override }
                    : profile

                // Create the SaveTimeSeriesListDto structure
                const saveTimeSeriesDto = {
                    timeSeries: [],
                    overrideTimeSeries: [{
                        profileType: updatedProfile.resourceName,
                        startYear: updatedProfile.startYear,
                        values: updatedProfile.values,
                        override: updatedProfile.override,
                    }],
                }

                // Call the saveProfiles method
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
            setSnackBarMessage(error.message || "Failed to update time series profile")
            logger.error("Error updating time series profile:", error)
        },
    })

    const updateProfileOverride = (profile: any) => mutation.mutateAsync({
        profile,
        toggleOverride: true,
    })

    const updateProfileValues = (profile: any) => mutation.mutateAsync({
        profile,
    })

    return {
        updateProfileOverride,
        updateProfileValues,
        isLoading: mutation.isPending,
    }
}
