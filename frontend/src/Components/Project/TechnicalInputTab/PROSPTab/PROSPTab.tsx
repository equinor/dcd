import {
    Typography,
    Card,
} from "@equinor/eds-core-react"
import React, {
    useEffect, useState, useCallback, useMemo,
} from "react"
import Grid from "@mui/material/Grid2"
import styled from "styled-components"
import { GetProspService } from "@/Services/ProspService"
import { GetProjectService } from "@/Services/ProjectService"
import useCanUserEdit from "@/Hooks/useCanUserEdit"
import { useDataFetch } from "@/Hooks"
import useEditProject from "@/Hooks/useEditProject"
import { useAppStore } from "@/Store/AppStore"
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
import { useFeedbackStatus } from "./useFeedbackStatus"

// Card styled components
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

const PROSPTab = () => {
    const revisionAndProjectData = useDataFetch()
    const { addProjectEdit } = useEditProject()
    const { canEdit } = useCanUserEdit()

    const [sharepointUrl, setSharepointUrl] = useState<string>("")
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

    const sharePointFileOptions = useMemo(() => {
        const options: { [key: string]: string } = { "": "" }

        sharePointFiles.forEach((file) => {
            if (file.id && file.name) {
                options[file.id] = file.name
            }
        })

        return options
    }, [sharePointFiles])

    const getFileNameById = useCallback((fileId: string | null): string | null => {
        if (!fileId) { return null }
        return sharePointFiles.find((f) => f.id === fileId)?.name || null
    }, [sharePointFiles])

    const loadSharePointFiles = async (url: string) => {
        try {
            const result = await GetProspService().getSharePointFileNamesAndId({ url }, projectId!)
            setSharePointFiles(result)
        } catch (error) {
            console.error("[PROSPTab] error while fetching SharePoint files", error)
        }
    }

    useEffect(() => {
        if (cases.length > 0) {
            const mappings = cases.map((c) => ({
                caseId: c.caseId!,
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
                loadSharePointFiles(currentSharePointSiteUrl)
            }
        }
    }, [projectId, currentSharePointSiteUrl])

    const refreshSharePointUrl = async (e: React.MouseEvent<HTMLButtonElement>) => {
        if (!projectId) { return }

        e.preventDefault()

        if (!sharepointUrl) {
            return
        }

        setIsSaving(true)

        try {
            await withFeedback(
                (async () => {
                    await loadSharePointFiles(sharepointUrl)

                    if (sharepointUrl !== currentSharePointSiteUrl) {
                        const newProject: Components.Schemas.UpdateProjectDto = {
                            ...revisionAndProjectData!.commonProjectAndRevisionData,
                            sharepointSiteUrl: sharepointUrl,
                        }

                        const projectResult = await GetProjectService().updateProject(projectId, newProject)
                        addProjectEdit(projectId, projectResult.commonProjectAndRevisionData)
                    }

                    setSnackBarMessage("Successfully imported SharePoint files")
                })(),
            )
        } catch (error) {
            console.error("[PROSPTab] error while submitting SharePoint URL", error)
            setSnackBarMessage("Failed to import SharePoint files. Please check the URL and your permissions.")
        } finally {
            setIsSaving(false)
        }
    }

    const handleFileChange = async (caseId: string, fileId: string) => {
        if (!projectId || !currentSharePointSiteUrl) { return }

        setIsSaving(true)

        // Immediately update local state for better UI responsiveness
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
            const dto = {
                caseId,
                sharePointFileId: fileId || "",
                sharePointFileName: getFileNameById(fileId) || "",
                sharePointSiteUrl: currentSharePointSiteUrl,
            }

            const newProject = await GetProspService().importFromSharePoint(projectId, [dto])
            addProjectEdit(projectId, newProject.commonProjectAndRevisionData)

            setCaseMappings((prev) => prev.map((item) => {
                if (item.caseId === caseId) {
                    return {
                        ...item,
                        sharePointFileId: fileId || null,
                        sharePointFileName: getFileNameById(fileId),
                    }
                }
                return item
            }))

            setSnackBarMessage(`Successfully updated file selection to ${getFileNameById(fileId)}`)
        } catch (error) {
            console.error("[PROSPTab] error while submitting file change", error)
            setSnackBarMessage("Failed to update file selection. Please try again.")
        }
    }

    const handleRefreshFile = async (caseId: string, fileId: string | null): Promise<boolean> => {
        if (!projectId || !currentSharePointSiteUrl || !fileId) { return false }

        setIsSaving(true)

        try {
            const dto = {
                caseId,
                sharePointFileId: fileId,
                sharePointFileName: getFileNameById(fileId) || "",
                sharePointSiteUrl: currentSharePointSiteUrl,
            }

            const newProject = await GetProspService().importFromSharePoint(projectId, [dto])
            addProjectEdit(projectId, newProject.commonProjectAndRevisionData)

            setSnackBarMessage(`Successfully imported data from ${getFileNameById(fileId)}`)
            return true
        } catch (error) {
            console.error("[PROSPTab] error while refreshing file data", error)
            setSnackBarMessage("Failed to import file data. Please try again.")
            return false
        } finally {
            setIsSaving(false)
        }
    }

    const renderCaseDropdown = (caseMapping: CaseSharePointMapping) => {
        const caseItem = cases.find((c) => c.caseId === caseMapping.caseId)
        if (!caseItem) { return null }

        const isInputDisabled = !canEdit() || isSaving

        return (
            <Grid size={12} key={caseMapping.caseId}>
                <SharePointFileDropdown
                    caseItem={caseItem}
                    value={caseMapping.sharePointFileId}
                    options={sharePointFileOptions}
                    onChange={handleFileChange}
                    onRefresh={handleRefreshFile}
                    disabled={isInputDisabled}
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
                        This import tool collects data from the “Main” worksheet in each PROSP file and displays it in the Development Parameters -, Cost - and CO2 emissions pages.
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
