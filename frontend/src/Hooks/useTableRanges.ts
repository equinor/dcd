import { useMutation, useQueryClient } from "@tanstack/react-query"
import { useParams } from "react-router-dom"

import { GetTableRangesService } from "../Services/TableRangesService"

import { useCaseApiData } from "./useCaseApiData"

import { useAppStore } from "@/Store/AppStore"
import { useProjectContext } from "@/Store/ProjectContext"

/**
 * Hook for managing table ranges
 * Provides functions to get and update table ranges
 */
export const useTableRanges = () => {
    const { caseId } = useParams()
    const { projectId } = useProjectContext()
    const queryClient = useQueryClient()
    const { setIsSaving, setSnackBarMessage } = useAppStore()
    const { apiData, isLoading: isLoadingCaseData } = useCaseApiData()

    // Extract table ranges from the case data
    const tableRanges = apiData?.case?.tableRanges

    // Mutation to update table ranges
    const mutation = useMutation({
        mutationFn: async (newRanges: Components.Schemas.UpdateTableRangesDto) => {
            if (!projectId || !caseId) {
                console.error("useTableRanges - Missing projectId or caseId", { projectId, caseId })
                throw new Error("Project ID and Case ID are required")
            }

            // Validate array lengths - ensure all arrays have at least 2 elements
            const validationErrors = []

            if (!newRanges.co2EmissionsYears || newRanges.co2EmissionsYears.length < 2) {
                validationErrors.push("CO2 emissions years must have at least 2 elements")
            }

            if (!newRanges.drillingScheduleYears || newRanges.drillingScheduleYears.length < 2) {
                validationErrors.push("Drilling schedule years must have at least 2 elements")
            }

            if (!newRanges.caseCostYears || newRanges.caseCostYears.length < 2) {
                validationErrors.push("Case cost years must have at least 2 elements")
            }

            if (!newRanges.productionProfilesYears || newRanges.productionProfilesYears.length < 2) {
                validationErrors.push("Production profiles years must have at least 2 elements")
            }

            if (validationErrors.length > 0) {
                console.error("useTableRanges - Validation errors", validationErrors)
                throw new Error(`Validation errors: ${validationErrors.join(", ")}`)
            }

            setIsSaving(true)

            try {
                return await GetTableRangesService().updateTableRanges(
                    projectId,
                    caseId,
                    newRanges,
                )
            } catch (error) {
                console.error("useTableRanges - API call failed", error)

                // Enhanced error handling
                if (error instanceof Error) {
                    // Check if we have a more specific error message in the response
                    const anyError = error as any

                    if (anyError.response?.data?.message) {
                        throw new Error(`API Error: ${anyError.response.data.message}`)
                    } else if (anyError.response?.status === 500) {
                        throw new Error("Server error occurred. The year ranges may be invalid or out of range.")
                    }
                }

                throw error
            } finally {
                setIsSaving(false)
            }
        },
        onSuccess: () => {
            if (projectId && caseId) {
                queryClient.invalidateQueries({ queryKey: ["caseApiData", projectId, caseId] })
            }
        },
        onError: (error: unknown) => {
            console.error("useTableRanges - Mutation error", error)
            if (error instanceof Error) {
                setSnackBarMessage(error.message || "Failed to update table ranges")
            }
        },
    })

    // Helper function to ensure we have a valid range
    const ensureValidRange = (startYear: number, endYear: number): [number, number] => {
        if (startYear > endYear) {
            console.warn("useTableRanges - Start year is greater than end year, swapping values", { startYear, endYear })

            return [endYear, startYear]
        }

        // Ensure years are integers
        const start = Math.floor(startYear)
        const end = Math.floor(endYear)

        // If they're different from the input, log it
        if (start !== startYear || end !== endYear) {
            console.warn("useTableRanges - Years were not integers, converting", {
                originalStart: startYear,
                originalEnd: endYear,
                newStart: start,
                newEnd: end,
            })
        }

        return [start, end]
    }

    // Helper function to update CO2 emissions years
    const updateCo2EmissionsYears = (startYear: number, endYear: number): Promise<void> => {
        if (!tableRanges) {
            console.error("useTableRanges - updateCo2EmissionsYears: Table ranges not available")

            return Promise.reject(new Error("Table ranges not available"))
        }

        const [start, end] = ensureValidRange(startYear, endYear)

        return mutation.mutateAsync({
            ...tableRanges,
            co2EmissionsYears: [start, end],
        })
    }

    // Helper function to update drilling schedule years
    const updateDrillingScheduleYears = (startYear: number, endYear: number): Promise<void> => {
        if (!tableRanges) {
            console.error("useTableRanges - updateDrillingScheduleYears: Table ranges not available")

            return Promise.reject(new Error("Table ranges not available"))
        }

        const [start, end] = ensureValidRange(startYear, endYear)

        return mutation.mutateAsync({
            ...tableRanges,
            drillingScheduleYears: [start, end],
        })
    }

    // Helper function to update case cost years
    const updateCaseCostYears = (startYear: number, endYear: number): Promise<void> => {
        if (!tableRanges) {
            console.error("useTableRanges - updateCaseCostYears: Table ranges not available")

            return Promise.reject(new Error("Table ranges not available"))
        }

        const [start, end] = ensureValidRange(startYear, endYear)

        return mutation.mutateAsync({
            ...tableRanges,
            caseCostYears: [start, end],
        })
    }

    // Helper function to update production profiles years
    const updateProductionProfilesYears = (startYear: number, endYear: number): Promise<void> => {
        if (!tableRanges) {
            console.error("useTableRanges - updateProductionProfilesYears: Table ranges not available")

            return Promise.reject(new Error("Table ranges not available"))
        }

        const [start, end] = ensureValidRange(startYear, endYear)

        return mutation.mutateAsync({
            ...tableRanges,
            productionProfilesYears: [start, end],
        })
    }

    return {
        tableRanges,
        isLoadingTableRanges: isLoadingCaseData,
        isUpdatingTableRanges: mutation.isPending,
        updateCo2EmissionsYears,
        updateDrillingScheduleYears,
        updateCaseCostYears,
        updateProductionProfilesYears,
    }
}

export default useTableRanges
