import { Typography } from "@equinor/eds-core-react"
import React, {
    useState, useMemo, useEffect, useCallback,
} from "react"
import styled from "styled-components"

import { useDataFetch } from "@/Hooks"
import useCanUserEdit from "@/Hooks/useCanUserEdit"
import { useAppStore } from "@/Store/AppStore"
import {
    FeedbackStatus,
    useFeedbackStatus,
    createSharePointFileOptions,
    getFileNameById,
    loadSharePointFiles,
    importFromSharePoint,
} from "@/Utils/ProspUtils"

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
    const sharePointFileOptions = useMemo(
        () => createSharePointFileOptions(sharePointFiles, "Select a file"),
        [sharePointFiles],
    )

    // Load SharePoint files
    const fetchSharePointFiles = useCallback(async () => {
        if (!projectId || !sharepointSiteUrl) { return }

        try {
            const result = await loadSharePointFiles(sharepointSiteUrl, projectId)

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

            const fileName = getFileNameById(fileId, sharePointFiles) || ""

            await importFromSharePoint(projectId, caseId, fileId, fileName, sharepointSiteUrl)

            setSnackBarMessage(`Successfully imported data from ${fileName}`)
        } catch (error: any) {
            console.error("[SharePointFileSelector] error while importing file data", error)
            const errorMessage = error.message || "Failed to import file data. The server might be experiencing issues."

            setSnackBarMessage(errorMessage)

            // If this fails, revert the selection to the previous value
            setSelectedSharePointFileId(currentSharePointFileId)
            if (onSharePointFileSelected) {
                onSharePointFileSelected(currentSharePointFileId)
            }
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

                const fileName = getFileNameById(selectedSharePointFileId, sharePointFiles) || ""

                await importFromSharePoint(
                    projectId,
                    caseId,
                    selectedSharePointFileId,
                    fileName,
                    sharepointSiteUrl,
                )

                setSnackBarMessage(`Successfully refreshed data from ${fileName}`)

                return true
            })()

            await withFeedback(importPromise)
        } catch (error: any) {
            console.error("[SharePointFileSelector] error while refreshing file data", error)
            const errorMessage = error.message || "Failed to refresh file data. The server might be experiencing issues."

            setSnackBarMessage(errorMessage)
        } finally {
            setIsSaving(false)
        }
    }

    // Load SharePoint files when component mounts
    useEffect(() => {
        if (projectId) {
            fetchSharePointFiles()
        }
    }, [projectId, fetchSharePointFiles])

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
    const fileName = selectedSharePointFileId ? getFileNameById(selectedSharePointFileId, sharePointFiles) : null

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
                            {fileName || (selectedSharePointFileId ? "Unknown file" : "No file selected")}
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
