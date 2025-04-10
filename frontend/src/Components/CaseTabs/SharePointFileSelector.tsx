import { DotProgress, Typography } from "@equinor/eds-core-react"
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

const SharePointSection = styled.div`
    border-radius: 4px;
    padding: 10px 16px;
`

const SharePointContainer = styled.div`
    display: flex;
    align-items: center;
    width: 100%;
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
    width: 208px;
    text-align: right;
`

const StyledSelect = styled.select`
    width: 100%;
    height: 36px;
    padding: 0 8px;
    border: 2px solid #6F6F6F;
    border-radius: 1px;
    background-color: white;
`

const ActionButton = styled.button`
    background-color: transparent;
    color: black;
    border: 1px solid black;
    border-radius: 4px;
    padding: 10px 18px;
    width: 100%;
    height: 36px;
    cursor: pointer;
    font-weight: 500;
    display: flex;
    align-items: center;
    justify-content: center;

    &:disabled {
        background-color: #BEBEBE;
        cursor: not-allowed;
    }

    &:hover:not(:disabled) {
        background-color: black;
        color: white;
    }
`

const TextDisplay = styled(Typography)`
    padding: 8px 0;
`

const FeedbackIndicator = ({ status }: { status: FeedbackStatus }) => {

    if (status === "success") {
        return (<>
         <span style={{ marginLeft: "8px" }}>Case has been updated </span><span style={{ color: "#4BB748", marginLeft: "8px"}}>✓</span>
        </>)
    }

    if (status === "error") {
        return <span style={{ color: "#EB0000", marginLeft: "8px" }}>Error</span>
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

    const sharepointSiteUrl = useMemo(
        () => projectData?.commonProjectAndRevisionData?.sharepointSiteUrl || "",
        [projectData],
    )

    const sharePointFileOptions = useMemo(
        () => createSharePointFileOptions(sharePointFiles, "Select a file"),
        [sharePointFiles],
    )

    const fetchSharePointFiles = useCallback(async () => {
        if (!projectId || !sharepointSiteUrl) { return }

        try {
            const result = await loadSharePointFiles(sharepointSiteUrl, projectId)

            setSharePointFiles(result)

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

    const handleFileChange = async (e: React.ChangeEvent<HTMLSelectElement>) => {
        const fileId = e.target.value || null

        if (fileId === selectedSharePointFileId) { return }

        setSelectedSharePointFileId(fileId)

        if (onSharePointFileSelected) {
            onSharePointFileSelected(fileId)
        }

        if (!fileId || !sharepointSiteUrl) { return }

    }

    const handleRefreshFile = async () => {
        if (!projectId || !sharepointSiteUrl) { return }

        try {
            const importPromise = (async () => {
                setIsSaving(true)

                const fileName = getFileNameById(selectedSharePointFileId, sharePointFiles) || ""

                await importFromSharePoint(
                    projectId,
                    caseId,
                    selectedSharePointFileId || "",
                    fileName,
                    sharepointSiteUrl,
                )

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

    useEffect(() => {
        if (projectId) {
            fetchSharePointFiles()
        }
    }, [projectId, fetchSharePointFiles])

    useEffect(() => {
        if (currentSharePointFileId !== selectedSharePointFileId) {
            setSelectedSharePointFileId(currentSharePointFileId)
        }
    }, [currentSharePointFileId])

    if (!sharepointSiteUrl) {
        return null
    }

    const isDisabled = !canEdit() || isSaving || isLoading
    const fileName = selectedSharePointFileId ? getFileNameById(selectedSharePointFileId, sharePointFiles) : null

    return (
        <SharePointSection>
            <Typography variant="caption" style={{ marginLeft: "8px", width: "fit-content" }}>
                Import PROSP file from SharePoint
            </Typography>

            <SharePointContainer>
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
                       disabled={isDisabled}
                   >
                       {isLoading ? <DotProgress /> : !selectedSharePointFileId ? "Remove file" : "Import"}
                   </ActionButton>
                    )}
                </SharePointAction>
                <FeedbackIndicator status={feedbackStatus} />
            </SharePointContainer>
        </SharePointSection>
    )
}

export default SharePointFileSelector
