import {
    Typography,
    Card,
} from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid2"
import React, {
    useEffect, useState, useMemo,
} from "react"
import styled from "styled-components"

import ActionButton from "./ActionButton"
import SharePointFileDropdown from "./SharePointFileDropdown"
import {
    Container,
    HeaderContainer,
    LeftColumn,
    MiddleColumn,
    RightColumn,
    StyledInput,
    TextDisplay,
    UrlLink,
} from "./SharedStyledComponents"

import { useDataFetch } from "@/Hooks"
import useCanUserEdit from "@/Hooks/useCanUserEdit"
import useEditProject from "@/Hooks/useEditProject"
import { GetProjectService } from "@/Services/ProjectService"
import { useAppStore } from "@/Store/AppStore"
import {
    useFeedbackStatus,
    createSharePointFileOptions,
    getFileNameById,
    loadSharePointFiles,
    importFromSharePoint,
} from "@/Utils/ProspUtils"

const StyledCard = styled(Card)`
    padding: 0 16px;
`

const Header = styled(Grid)`
    margin-bottom: 44px;
    gap: 16px;
    display: flex;
    flex-direction: column;
    align-items: flex-start;
`
const ConfigCard = styled(StyledCard)`
    margin-bottom: 24px;
`

interface CaseSharePointMapping {
    caseId: string;
    sharePointFileId: string | null;
    sharePointFileName: string | null;
}

export enum SharePointFileStatus {
    IMPORTING = "IMPORTING",
    NO_FILE_SELECTED = "NO_FILE_SELECTED",
    IMPORTABLE = "IMPORTABLE",
    UNCHANGED_IN_SHAREPOINT = "UNCHANGED_IN_SHAREPOINT",
}

const PROSPTab = () => {
    const revisionAndProjectData = useDataFetch()
    const { addProjectEdit } = useEditProject()
    const { canEdit } = useCanUserEdit()

    const [sharepointUrl, setSharepointUrl] = useState<string>("")
    const [caseIdBeingSaved, setCaseIdBeingSaved] = useState<string | null>(null)
    const [sharePointFiles, setSharePointFiles] = useState<Components.Schemas.SharePointFileDto[]>([])
    const {
        isLoading: isRefreshing,
        feedbackStatus: urlFeedbackStatus,
        withFeedback,
    } = useFeedbackStatus()

    const [caseMappings, setCaseMappings] = useState<CaseSharePointMapping[]>([])
    const { setIsSaving, isSaving, setSnackBarMessage } = useAppStore()

    const projectId = revisionAndProjectData?.projectId
    const cases = revisionAndProjectData?.commonProjectAndRevisionData.cases || []
    const currentSharePointSiteUrl = revisionAndProjectData?.commonProjectAndRevisionData.sharepointSiteUrl

    const sharePointFileOptions = useMemo(
        () => createSharePointFileOptions(sharePointFiles, ""),
        [sharePointFiles],
    )

    const fetchSharePointFiles = async (url: string) => {
        try {
            const result = await loadSharePointFiles(url, projectId!)

            setSharePointFiles(result)
        } catch (error) {
            console.error("[PROSPTab] error while fetching SharePoint files", error)
        }
    }

    useEffect(() => {
        if (cases.length > 0) {
            const mappings = cases.map((c) => ({
                caseId: c.caseId,
                sharePointFileId: c.sharepointFileId,
                sharePointFileName: c.sharepointFileName,
            }))

            setCaseMappings(mappings)
        }
    }, [cases])

    useEffect(() => {
        if (projectId && currentSharePointSiteUrl) {
            setSharepointUrl(currentSharePointSiteUrl)

            if (currentSharePointSiteUrl !== "") {
                fetchSharePointFiles(currentSharePointSiteUrl)
            }
        }
    }, [projectId, currentSharePointSiteUrl])

    const refreshSharePointUrl = async (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault()

        if (!projectId || !sharepointUrl) { return }
        setIsSaving(true)

        try {
            await withFeedback(
                (async () => {
                    await fetchSharePointFiles(sharepointUrl)

                    if (sharepointUrl !== currentSharePointSiteUrl) {
                        const newProject: Components.Schemas.UpdateProjectDto = {
                            ...revisionAndProjectData.commonProjectAndRevisionData,
                            sharepointSiteUrl: sharepointUrl,
                        }

                        const projectResult = await GetProjectService().updateProject(projectId, newProject)

                        addProjectEdit(projectId, projectResult.commonProjectAndRevisionData)
                    }

                    setSnackBarMessage("Successfully imported SharePoint files")
                })(),
            )
        } catch (error) {
            setSnackBarMessage("Failed to import SharePoint files. Please check the URL and your permissions.")
        } finally {
            setIsSaving(false)
        }
    }

    const handleFileChange = async (caseId: string, fileId: string) => {
        if (!projectId || !currentSharePointSiteUrl) { return }

        setIsSaving(true)
        setCaseIdBeingSaved(caseId)

        setCaseMappings((prev) => prev.map((item) => {
            if (item.caseId === caseId) {
                return {
                    ...item,
                    sharePointFileId: fileId || null,
                }
            }

            return item
        }))

        try {
            const fileName = getFileNameById(fileId, sharePointFiles) || ""
            const newProject = await importFromSharePoint(
                projectId,
                caseId,
                fileId,
                fileName,
                currentSharePointSiteUrl,
            )

            addProjectEdit(projectId, newProject.commonProjectAndRevisionData)

            setCaseMappings((prev) => prev.map((item) => {
                if (item.caseId === caseId) {
                    return {
                        ...item,
                        sharePointFileId: fileId || null,
                        sharePointFileName: getFileNameById(fileId, sharePointFiles),
                    }
                }

                return item
            }))

            setCaseIdBeingSaved(null)
            setSnackBarMessage(`Successfully updated file selection to ${fileName}`)
        } catch (error: unknown) {
            setCaseIdBeingSaved(null)
            setCaseMappings((prev) => prev.map((item) => {
                if (item.caseId === caseId) {
                    // Revert to the original values
                    const originalMapping = cases.find((c) => c.caseId === caseId)

                    return {
                        caseId,
                        sharePointFileId: originalMapping?.sharepointFileId || null,
                        sharePointFileName: originalMapping?.sharepointFileName || null,
                    }
                }

                return item
            }))

            if (error instanceof Error) {
                const errorMessage = error.message || "Failed to update file selection. Please try again."

                setSnackBarMessage(errorMessage)
            }
        } finally {
            setIsSaving(false)
        }
    }

    const handleRefreshFile = async (caseId: string, fileId: string | null): Promise<boolean> => {
        if (!projectId || !currentSharePointSiteUrl || !fileId) { return false }

        setIsSaving(true)
        setCaseIdBeingSaved(caseId)

        try {
            const fileName = getFileNameById(fileId, sharePointFiles) || ""
            const newProject = await importFromSharePoint(
                projectId,
                caseId,
                fileId,
                fileName,
                currentSharePointSiteUrl,
            )

            addProjectEdit(projectId, newProject.commonProjectAndRevisionData)
            setSnackBarMessage(`Successfully imported data from ${fileName}`)

            return true
        } catch (error: unknown) {
            if (error instanceof Error) {
                const errorMessage = error.message || "Failed to import file data. Please try again."

                setSnackBarMessage(errorMessage)
            }

            return false
        } finally {
            setCaseIdBeingSaved(null)
            setIsSaving(false)
        }
    }

    const getSharePointFileStatus = (caseItem: Components.Schemas.CaseOverviewDto, selectedFileId: string | null): SharePointFileStatus => {
        if (caseItem.caseId === caseIdBeingSaved) {
            return SharePointFileStatus.IMPORTING
        }

        if (selectedFileId && selectedFileId !== caseItem.sharepointFileId) {
            return SharePointFileStatus.IMPORTABLE
        }

        const changeUtcOnCaseItemProspFile = caseItem.sharepointUpdatedTimestampUtc

        if (!changeUtcOnCaseItemProspFile) {
            return SharePointFileStatus.NO_FILE_SELECTED
        }

        const sharepointFile = sharePointFiles.find((f) => f.id === caseItem.sharepointFileId)

        if (!sharepointFile) {
            return SharePointFileStatus.NO_FILE_SELECTED
        }

        if (caseItem.sharepointUpdatedTimestampUtc === sharepointFile.lastModifiedUtc) {
            return SharePointFileStatus.UNCHANGED_IN_SHAREPOINT
        }

        return SharePointFileStatus.IMPORTABLE
    }

    const renderCaseDropdown = (caseMapping: CaseSharePointMapping) => {
        const caseItem = cases.find((c) => c.caseId === caseMapping.caseId)

        if (!caseItem) { return null }

        const isInputDisabled = !canEdit() || isSaving

        const sharePointFileStatus = getSharePointFileStatus(caseItem, caseMapping.sharePointFileId)

        return (
            <Grid size={12} key={caseMapping.caseId}>
                <SharePointFileDropdown
                    caseItem={caseItem}
                    value={caseMapping.sharePointFileId}
                    options={sharePointFileOptions}
                    onChange={handleFileChange}
                    onRefresh={handleRefreshFile}
                    disabled={isInputDisabled}
                    sharePointFileStatus={sharePointFileStatus}
                />
            </Grid>
        )
    }

    const renderUrlActionButton = () => (
        <ActionButton
            isLoading={isRefreshing}
            feedbackStatus={urlFeedbackStatus}
            onClick={(e) => e && refreshSharePointUrl(e)}
            disabled={!canEdit() || isSaving}
            buttonText="Link files"
        />
    )

    const renderViewModeUrl = () => (
        sharepointUrl ? (
            <UrlLink
                href={sharepointUrl.startsWith("http") ? sharepointUrl : `https://${sharepointUrl}`}
                target="_blank"
                rel="noopener noreferrer"
            >
                {sharepointUrl}
            </UrlLink>
        ) : (
            <TextDisplay>
                No URL specified
            </TextDisplay>
        )
    )

    return (
        <Grid container rowSpacing={3} columnSpacing={2}>
            <Grid size={12}>
                <Header size={12}>
                    <Typography variant="h3">PROSP Import</Typography>
                    <Typography>
                        This import tool collects data from the &quot;Main&quot; worksheet in each PROSP file and displays it in the Development Parameters -, Cost - and CO2 emissions pages.
                    </Typography>
                </Header>
                <ConfigCard>
                    <HeaderContainer>
                        <LeftColumn>
                            <Typography variant="h6">SharePoint Configuration</Typography>
                        </LeftColumn>
                        <MiddleColumn>
                            <Typography variant="h6">Site URL</Typography>
                        </MiddleColumn>
                        <RightColumn>
                            <Typography variant="h6" />
                        </RightColumn>
                    </HeaderContainer>

                    <Container>
                        <LeftColumn>
                            <Typography>SharePoint Site</Typography>
                        </LeftColumn>

                        <MiddleColumn>
                            {!canEdit() || isSaving ? (
                                renderViewModeUrl()
                            ) : (
                                <StyledInput
                                    id="sharepoint-url-input"
                                    placeholder="Paste Uri here"
                                    onChange={(e: React.ChangeEvent<HTMLInputElement>) => setSharepointUrl(e.target.value)}
                                    value={sharepointUrl}
                                    disabled={!canEdit() || isSaving}
                                />
                            )}
                        </MiddleColumn>

                        <RightColumn>
                            {renderUrlActionButton()}
                        </RightColumn>
                    </Container>
                </ConfigCard>
            </Grid>

            {cases.length > 0 && (
                <Grid size={12} container spacing={2}>
                    <Grid size={12}>
                        <Typography variant="h4">PROSP Case Files</Typography>
                    </Grid>
                    <Grid size={12}>
                        <StyledCard>
                            <HeaderContainer>
                                <LeftColumn>
                                    <Typography variant="h6">Case</Typography>
                                </LeftColumn>
                                <MiddleColumn>
                                    <Typography variant="h6">SharePoint File</Typography>
                                </MiddleColumn>
                                <RightColumn>
                                    <Typography variant="h6" />
                                </RightColumn>
                            </HeaderContainer>

                            {caseMappings.map(renderCaseDropdown)}
                        </StyledCard>
                    </Grid>
                </Grid>
            )}
        </Grid>
    )
}

export default PROSPTab
