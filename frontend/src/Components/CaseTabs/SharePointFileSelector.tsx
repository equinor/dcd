import { Typography } from "@equinor/eds-core-react"
import React, {
    useState, useCallback, useMemo, useEffect,
} from "react"
import styled from "styled-components"

import { useDataFetch } from "@/Hooks"
import useCanUserEdit from "@/Hooks/useCanUserEdit"
import { GetProspService } from "@/Services/ProspService"
import { useAppStore } from "@/Store/AppStore"

// Styled components for SharePoint section
const SharePointSection = styled.div`
    margin-bottom: 24px;
    border: 1px solid #E6E6E6;
    border-radius: 4px;
    padding: 16px;
`

const SharePointContainer = styled.div`
    display: flex;
    align-items: center;
    width: 100%;
    margin-top: 12px;
`

const SharePointLabel = styled.div`
    width: 25%;
    padding-right: 16px;
`

const SharePointInput = styled.div`
    flex: 1;
    padding-right: 16px;
`

const SharePointAction = styled.div`
    width: 100px;
    text-align: right;
`

const StyledSelect = styled.select`
    width: 100%;
    height: 36px;
    padding: 0 8px;
    border: 1px solid #6F6F6F;
    border-radius: 4px;
    background-color: white;
`

const ActionButton = styled.button`
    background-color: #007079;
    color: white;
    border: none;
    border-radius: 4px;
    padding: 8px 16px;
    cursor: pointer;
    font-weight: 500;
    display: flex;
    align-items: center;
    
    &:disabled {
        background-color: #BEBEBE;
        cursor: not-allowed;
    }
    
    &:hover:not(:disabled) {
        background-color: #006570;
    }
`

const TextDisplay = styled(Typography)`
    padding: 8px 0;
`

// Feedback status type
type FeedbackStatus = "none" | "success" | "error"

// Component to show feedback states (success, error, loading)
const FeedbackIndicator = ({ status, isLoading }: { status: FeedbackStatus, isLoading: boolean }) => {
    if (isLoading) {
        return <span style={{ color: "#6F6F6F", marginLeft: "8px" }}>Loading...</span>
    }

    if (status === "success") {
        return <span style={{ color: "#4BB748", marginLeft: "8px" }}>✓</span>
    }

    if (status === "error") {
        return <span style={{ color: "#EB0000", marginLeft: "8px" }}>✗</span>
    }

    return null
}

// Hook to manage feedback status
const useFeedbackStatus = (resetDelay = 3000) => {
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

    const withFeedback = useCallback(async <T, >(promise: Promise<T>): Promise<T> => {
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

interface SharePointFileSelectorProps {
    projectId: string;
    caseId: string;
    currentSharePointFileId: string | null;
    onSharePointFileSelected?: (fileId: string | null) => void;
}

const SharePointFileSelector: React.FC<SharePointFileSelectorProps> = ({
    projectId,
    caseId,
    currentSharePointFileId,
    onSharePointFileSelected,
}) => {
    const { canEdit } = useCanUserEdit()
    const { setIsSaving, isSaving, setSnackBarMessage } = useAppStore()
    const projectData = useDataFetch()

    const [sharePointFiles, setSharePointFiles] = useState<Components.Schemas.SharePointFileDto[]>([])
    const [selectedSharePointFileId, setSelectedSharePointFileId] = useState<string | null>(currentSharePointFileId)
    const { isLoading, feedbackStatus, withFeedback } = useFeedbackStatus()

    // Get the SharePoint URL from project data
    const sharepointSiteUrl = useMemo(
        () => projectData?.commonProjectAndRevisionData?.sharepointSiteUrl || "",
        [projectData],
    )

    // SharePoint file options converted to a map
    const sharePointFileOptions = useMemo(() => {
        const options: { [key: string]: string } = { "": "Select a file" }

        sharePointFiles.forEach((file) => {
            if (file.id && file.name) {
                options[file.id] = file.name
            }
        })

        return options
    }, [sharePointFiles])

    // Get file name by ID
    const getFileNameById = useCallback((fileId: string | null): string | null => {
        if (!fileId) { return null }

        return sharePointFiles.find((f) => f.id === fileId)?.name || null
    }, [sharePointFiles])

    // Load SharePoint files
    const loadSharePointFiles = useCallback(async () => {
        if (!projectId || !sharepointSiteUrl) { return }

        try {
            const result = await GetProspService().getSharePointFileNamesAndId(
                { url: sharepointSiteUrl },
                projectId,
            )

            setSharePointFiles(result)

            // Update selectedSharePointFileId if the current one is not in the options
            if (selectedSharePointFileId && !result.some((f) => f.id === selectedSharePointFileId)) {
                setSelectedSharePointFileId(null)
                if (onSharePointFileSelected) {
                    onSharePointFileSelected(null)
                }
            }
        } catch (error) {
            console.error("[SharePointFileSelector] error while fetching SharePoint files", error)
            setSnackBarMessage("Failed to load SharePoint files")
        }
    }, [projectId, sharepointSiteUrl, selectedSharePointFileId, onSharePointFileSelected, setSnackBarMessage])

    // Handle file selection change
    const handleFileChange = async (e: React.ChangeEvent<HTMLSelectElement>) => {
        const fileId = e.target.value || null

        if (fileId === selectedSharePointFileId) { return }

        setSelectedSharePointFileId(fileId)

        if (onSharePointFileSelected) {
            onSharePointFileSelected(fileId)
        }

        if (!fileId || !sharepointSiteUrl) { return }

        try {
            setIsSaving(true)

            const dto = {
                caseId,
                sharePointFileId: fileId,
                sharePointFileName: getFileNameById(fileId) || "",
                sharePointSiteUrl: sharepointSiteUrl,
            }

            await GetProspService().importFromSharePoint(projectId, [dto])
            setSnackBarMessage(`Successfully imported data from ${getFileNameById(fileId)}`)
        } catch (error) {
            console.error("[SharePointFileSelector] error while importing file data", error)
            setSnackBarMessage("Failed to import file data")
        } finally {
            setIsSaving(false)
        }
    }

    // Refresh data from the selected file
    const handleRefreshFile = async () => {
        if (!projectId || !selectedSharePointFileId || !sharepointSiteUrl) { return }

        try {
            const importPromise = (async () => {
                setIsSaving(true)

                const dto = {
                    caseId,
                    sharePointFileId: selectedSharePointFileId,
                    sharePointFileName: getFileNameById(selectedSharePointFileId) || "",
                    sharePointSiteUrl: sharepointSiteUrl,
                }

                await GetProspService().importFromSharePoint(projectId, [dto])
                setSnackBarMessage(`Successfully refreshed data from ${getFileNameById(selectedSharePointFileId)}`)

                return true
            })()

            await withFeedback(importPromise)
        } catch (error) {
            console.error("[SharePointFileSelector] error while refreshing file data", error)
            setSnackBarMessage("Failed to refresh file data")
        } finally {
            setIsSaving(false)
        }
    }

    // Load SharePoint files when component mounts
    useEffect(() => {
        if (projectId) {
            loadSharePointFiles()
        }
    }, [projectId, loadSharePointFiles])

    // Update selected file ID when prop changes
    useEffect(() => {
        if (currentSharePointFileId !== selectedSharePointFileId) {
            setSelectedSharePointFileId(currentSharePointFileId)
        }
    }, [currentSharePointFileId])

    // If there's no SharePoint URL configured, don't render anything
    if (!sharepointSiteUrl) {
        return null
    }

    const isDisabled = !canEdit() || isSaving || isLoading
    const selectedOptionText = selectedSharePointFileId ? sharePointFileOptions[selectedSharePointFileId] || "" : ""

    return (
        <SharePointSection>
            <Typography variant="h5">PROSP Integration</Typography>
            <Typography variant="body_short" style={{ marginBottom: "16px" }}>
                Link this case to a PROSP file to import production, cost, and CO2 emissions data.
            </Typography>

            <SharePointContainer>
                <SharePointLabel>
                    <Typography>SharePoint File</Typography>
                </SharePointLabel>

                <SharePointInput>
                    {isDisabled ? (
                        <TextDisplay>
                            {selectedSharePointFileId
                                ? getFileNameById(selectedSharePointFileId) || "Unknown file"
                                : "No file selected"}
                        </TextDisplay>
                    ) : (
                        <StyledSelect
                            id={`case-sharepointFileId-${caseId}`}
                            value={selectedSharePointFileId || ""}
                            onChange={handleFileChange}
                            disabled={isDisabled}
                        >
                            {Object.entries(sharePointFileOptions).map(([key, value]) => (
                                <option key={key} value={key}>{value}</option>
                            ))}
                        </StyledSelect>
                    )}
                </SharePointInput>

                <SharePointAction>
                    {canEdit() && (
                        <ActionButton
                            onClick={handleRefreshFile}
                            disabled={isDisabled || !selectedSharePointFileId}
                        >
                            Import
                            <FeedbackIndicator status={feedbackStatus} isLoading={isLoading} />
                        </ActionButton>
                    )}
                </SharePointAction>
            </SharePointContainer>
        </SharePointSection>
    )
}

export default SharePointFileSelector
