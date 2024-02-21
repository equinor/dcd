import { Typography } from "@mui/material"
import React, { ChangeEvent, useEffect, useState } from "react"
import styled from "styled-components"
import {
    Button, Input, Label, Progress, Switch,
} from "@equinor/eds-core-react"
import { GetProspService } from "../../Services/ProspService"
import { GetProjectService } from "../../Services/ProjectService"
import { DriveItem } from "../../Models/sharepoint/DriveItem"
import PROSPCaseList from "./PROSPCaseList"
import { useAppContext } from "../../Context/AppContext"

const ProspFieldWrapper = styled.div`
    margin-bottom: 2.5rem;
    width: 100%;
    display: flex;
    flex-direction: row;
    justify-content: space-between;
`

const ProspURLInputField = styled(Input)`
    margin-right: 20px;
`

const TopWrapper = styled.div`
    display: flex;
    flex-direction: row;
    justify-content: space-between;
`

const SwitchWrapper = styled.div`
    margin-left: auto;
    display: flex;
    flex-direction: column;
    align-items: flex-end;
`

const ErrorMessage = styled.div`
    color: red;
    margin-top: 10px;
`

const PROSPTab = () => {
    const { project, setProject } = useAppContext()

    const [sharepointUrl, setSharepointUrl] = useState<string>()
    const [check, setCheck] = useState(false)
    const [driveItems, setDriveItems] = useState<DriveItem[]>()
    const [isRefreshing, setIsRefreshing] = useState<boolean>(false)
    const [errorMessage, setErrorMessage] = useState<string>("")

    useEffect(() => {
        if (project && project.sharepointSiteUrl) {
            (async () => {
                setSharepointUrl(project.sharepointSiteUrl ?? "")
                if (project.sharepointSiteUrl && project.sharepointSiteUrl !== "") {
                    try {
                        const result = await (await GetProspService())
                            .getSharePointFileNamesAndId({ url: project.sharepointSiteUrl })
                        setDriveItems(result)
                        setErrorMessage("")
                    } catch (error) {
                        console.error("[PROSPTab] error while fetching SharePoint files", error)
                        setErrorMessage("Failed to fetch SharePoint files. Please check the URL and your permissions.")
                    }
                }
            })()
        }
    }, [project?.sharepointSiteUrl])

    const saveUrl: React.MouseEventHandler<HTMLButtonElement> = async (e) => {
        setIsRefreshing(true)
        e.preventDefault()
        try {
            const result = await (await GetProspService()).getSharePointFileNamesAndId({ url: sharepointUrl })
            setDriveItems(result)
            setErrorMessage("")

            if (project && sharepointUrl !== project.sharepointSiteUrl) {
                const newProject: Components.Schemas.ProjectDto = { ...project }
                newProject.sharepointSiteUrl = sharepointUrl
                const projectResult = await (await GetProjectService()).updateProject(project.id, newProject)
                setProject(projectResult)
                setSharepointUrl(projectResult.sharepointSiteUrl ?? "")
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

    return (
        <div>
            <TopWrapper>
                <Typography variant="h4">PROSP</Typography>
            </TopWrapper>
            <Label htmlFor="textfield-normal" label="Sharepoint Site address" />
            <ProspFieldWrapper>
                <ProspURLInputField
                    id="textfield-normal"
                    placeholder="Paste Uri here"
                    onChange={handleSharePointUrl}
                    value={sharepointUrl}
                />
                {!isRefreshing ? <Button variant="outlined" onClick={saveUrl}>Refresh</Button>
                    : (
                        <Button variant="outlined">
                            <Progress.Dots color="primary" />
                        </Button>
                    )}
            </ProspFieldWrapper>
            {errorMessage && <ErrorMessage>{errorMessage}</ErrorMessage>}
            <SwitchWrapper>
                <Switch
                    onChange={(e: ChangeEvent<HTMLInputElement>) => {
                        setCheck(e.target.checked)
                    }}
                    checked={check}
                    label="Advance settings"
                />
            </SwitchWrapper>
            <PROSPCaseList
                driveItems={driveItems}
                check={check}
            />

        </div>
    )
}

export default PROSPTab
