import { useCallback, useState } from "react"

import { GetProspService } from "@/Services/ProspService"

/**
 * Type for feedback status in UI components
 */
export type FeedbackStatus = "none" | "success" | "error"

/**
 * Hook to manage feedback status with loading, success, and error states
 * @param resetDelay Time in ms after which feedback status resets to "none"
 * @returns Object with loading state, feedback status, and utility methods
 */
export const useFeedbackStatus = (resetDelay = 3000) => {
    const [isLoading, setIsLoading] = useState(false)
    const [feedbackStatus, setFeedbackStatus] = useState<FeedbackStatus>("none")

    const resetStatus = useCallback(() => {
        setFeedbackStatus("none")
    }, [])

    const setSuccess = useCallback(() => {
        setFeedbackStatus("success")
        setTimeout(resetStatus, resetDelay)
    }, [resetDelay, resetStatus])

    const setError = useCallback(() => {
        setFeedbackStatus("error")
        setTimeout(resetStatus, resetDelay)
    }, [resetDelay, resetStatus])

    const withFeedback = useCallback(async <T,>(promise: Promise<T>): Promise<T> => {
        setIsLoading(true)

        try {
            const result = await promise

            setSuccess()

            return result
        } catch (error) {
            setError()
            throw error
        } finally {
            setIsLoading(false)
        }
    }, [setSuccess, setError])

    return {
        isLoading,
        feedbackStatus,
        setLoading: setIsLoading,
        setSuccess,
        setError,
        resetStatus,
        withFeedback,
    }
}

/**
 * Creates a map of SharePoint file ID to file name from an array of SharePoint files
 * @param sharePointFiles Array of SharePoint files from the API
 * @param emptyLabel Label to use for the empty option (default: "Select a file")
 * @returns An object mapping file IDs to their names
 */
export const createSharePointFileOptions = (
    sharePointFiles: Components.Schemas.SharePointFileDto[],
    emptyLabel = "Select a file",
): Record<string, string> => {
    const options: Record<string, string> = { "": emptyLabel }

    sharePointFiles.forEach((file) => {
        if (file.id && file.name) {
            options[file.id] = file.name
        }
    })

    return options
}

/**
 * Gets the file name for a given SharePoint file ID
 * @param fileId SharePoint file ID to lookup
 * @param sharePointFiles Array of SharePoint files from the API
 * @returns The file name or null if not found
 */
export const getFileNameById = (
    fileId: string | null,
    sharePointFiles: Components.Schemas.SharePointFileDto[],
): string | null => {
    if (!fileId) { return null }

    return sharePointFiles.find((f) => f.id === fileId)?.name || null
}

/**
 * Loads SharePoint files from a given URL
 * @param url SharePoint site URL
 * @param projectId Project ID
 * @returns Promise resolving to array of SharePoint files
 */
export const loadSharePointFiles = async (
    url: string,
    projectId: string,
): Promise<Components.Schemas.SharePointFileDto[]> => GetProspService().getSharePointFileNamesAndId({ url }, projectId)

/**
 * Imports data from a SharePoint file into a case
 * @param projectId Project ID
 * @param caseId Case ID
 * @param sharePointFileId SharePoint file ID
 * @param sharePointFileName SharePoint file name
 * @param sharePointSiteUrl SharePoint site URL
 * @returns Promise resolving to project data
 */
export const importFromSharePoint = async (
    projectId: string,
    caseId: string,
    sharePointFileId: string,
    sharePointFileName: string,
    sharePointSiteUrl: string,
): Promise<Components.Schemas.ProjectDataDto> => {
    try {
        const dto = {
            caseId,
            sharePointFileId,
            sharePointFileName,
            sharePointSiteUrl,
        }

        return GetProspService().importFromSharePoint(projectId, [dto])
    } catch (error: any) {
        console.error(`SharePoint import error: ${error.message || "Unknown error"}`, error)

        // If we have a response with detailed error message, extract it
        const responseData = error.response?.data
        const errorMessage = responseData?.message || responseData?.error || error.message || "Unknown server error"

        throw new Error(`Failed to import from SharePoint: ${errorMessage}`)
    }
}
