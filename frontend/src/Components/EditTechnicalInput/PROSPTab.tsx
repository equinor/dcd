import {
    Typography,
    Button,
    Input,
    Progress,
    Switch,
    InputWrapper,
    Card,
} from "@equinor/eds-core-react"
import React, { ChangeEvent, useEffect, useState } from "react"
import Grid from "@mui/material/Grid"

import { GetProspService } from "@/Services/ProspService"
import { GetProjectService } from "@/Services/ProjectService"
import { DriveItem } from "@/Models/sharepoint/DriveItem"
import useEditDisabled from "@/Hooks/useEditDisabled"
import { useDataFetch } from "@/Hooks/useDataFetch"
import useEditProject from "@/Hooks/useEditProject"
import { useAppContext } from "@/Context/AppContext"
import PROSPCaseList from "./PROSPCaseList"

const PROSPTab = () => {
    const revisionAndProjectData = useDataFetch()
    const { addProjectEdit } = useEditProject()
    const { isEditDisabled } = useEditDisabled()
    const { editMode } = useAppContext()

    const [sharepointUrl, setSharepointUrl] = useState<string>()
    const [check, setCheck] = useState(false)
    const [driveItems, setDriveItems] = useState<DriveItem[]>()
    const [isRefreshing, setIsRefreshing] = useState<boolean>(false)
    const [errorMessage, setErrorMessage] = useState<string>("")

    const saveUrl: React.MouseEventHandler<HTMLButtonElement> = async (e) => {
        setIsRefreshing(true)
        e.preventDefault()
        try {
            const result = await (await GetProspService()).getSharePointFileNamesAndId({ url: sharepointUrl })
            setDriveItems(result)
            setErrorMessage("")

            if (revisionAndProjectData && sharepointUrl !== revisionAndProjectData.commonProjectAndRevisionData.sharepointSiteUrl) {
                const newProject: Components.Schemas.UpdateProjectDto = { ...revisionAndProjectData.commonProjectAndRevisionData }
                newProject.sharepointSiteUrl = sharepointUrl ?? null
                const projectResult = await (await GetProjectService()).updateProject(revisionAndProjectData.projectId, newProject)
                addProjectEdit(revisionAndProjectData.projectId, projectResult.commonProjectAndRevisionData)
                setSharepointUrl(projectResult.commonProjectAndRevisionData.sharepointSiteUrl ?? "")
            }
        } catch (error) {
            console.error("[PROSPTab] error while submitting SharePoint URL", error)
            setErrorMessage("Failed to submit SharePoint URL. Please check the URL and your permissions.")
        }
        setIsRefreshing(false)
    }

    const handleSharePointUrl: React.ChangeEventHandler<HTMLInputElement> = (e) => {
        setSharepointUrl(e.currentTarget.value)
    }

    useEffect(() => {
        if (revisionAndProjectData && revisionAndProjectData.commonProjectAndRevisionData.sharepointSiteUrl) {
            (async () => {
                setSharepointUrl(revisionAndProjectData.commonProjectAndRevisionData.sharepointSiteUrl ?? "")
                if (revisionAndProjectData.commonProjectAndRevisionData.sharepointSiteUrl && revisionAndProjectData.commonProjectAndRevisionData.sharepointSiteUrl !== "") {
                    try {
                        const result = await (await GetProspService())
                            .getSharePointFileNamesAndId({ url: revisionAndProjectData.commonProjectAndRevisionData.sharepointSiteUrl })
                        setDriveItems(result)
                        setErrorMessage("")
                    } catch (error) {
                        console.error("[PROSPTab] error while fetching SharePoint files", error)
                        setErrorMessage("Failed to fetch SharePoint files. Please check the URL and your permissions.")
                    }
                }
            })()
        }
    }, [revisionAndProjectData?.commonProjectAndRevisionData.sharepointSiteUrl])

    return (
        <Grid container rowSpacing={3} columnSpacing={2}>
            <Grid item xs={12} container spacing={2} alignItems="flex-end">
                <Grid item flex={1}>
                    <InputWrapper labelProps={{ label: "Sharepoint Site address" }}>
                        <Input
                            id="textfield-normal"
                            placeholder="Paste Uri here"
                            onChange={handleSharePointUrl}
                            value={sharepointUrl || ""}
                            disabled={isEditDisabled || !editMode}
                        />
                    </InputWrapper>
                </Grid>
                <Grid item>
                    {!isRefreshing
                        ? (
                            <Button
                                variant="outlined"
                                onClick={saveUrl}
                                disabled={isEditDisabled || !editMode}
                            >
                                Refresh
                            </Button>
                        ) : (
                            <Button variant="outlined">
                                <Progress.Dots color="primary" />
                            </Button>
                        )}
                </Grid>
                {errorMessage
                    && (
                        <Grid item xs={12}>
                            <Card variant="danger">
                                <Card.Header>
                                    <Typography>{errorMessage}</Typography>
                                </Card.Header>
                            </Card>
                        </Grid>
                    )}
            </Grid>
            <Grid item xs={12} container justifyContent="flex-end">
                <Grid item>
                    <Switch
                        onChange={(e: ChangeEvent<HTMLInputElement>) => {
                            setCheck(e.target.checked)
                        }}
                        checked={check}
                        label="Advance settings"
                        disabled={isEditDisabled || !editMode}
                    />
                </Grid>
                <Grid item xs={12}>
                    <PROSPCaseList
                        driveItems={driveItems}
                        check={check}
                    />
                </Grid>
            </Grid>
        </Grid>
    )
}

export default PROSPTab
