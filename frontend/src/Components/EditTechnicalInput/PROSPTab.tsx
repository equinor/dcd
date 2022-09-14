import { Typography } from "@material-ui/core"
import React, {
    Dispatch, MouseEventHandler, SetStateAction, useEffect, useState,
} from "react"
import styled from "styled-components"
import { Button, Input, Label } from "@equinor/eds-core-react"
import { Project } from "../../models/Project"
import PROSPCaseList from "./PROSPCaseList"
import { GetProspService } from "../../Services/ProspService"
import { GetProjectService } from "../../Services/ProjectService"
import { DriveItem } from "../../models/sharepoint/DriveItem"

const ProspFieldWrapper = styled.div`
    margin-bottom: 2.5rem;
    width: 48rem;
    display: flex;
    flex-direction: row;
    justify-content: space-between;
`

const TopWrapper = styled.div`
    display: flex;
    flex-direction: row;
    justify-content: space-between;
`

interface Props {
    setProject: Dispatch<SetStateAction<Project | undefined>>
    project: Project,
}

function PROSPTab({
    project,
    setProject,
}: Props) {
    const [sharepointUrl, setSharepointUrl] = useState<string>()
    const [driveItems, setDriveItems] = useState<DriveItem[]>()

    useEffect(() => {
        (async () => {
            setSharepointUrl(project.sharepointSiteUrl ?? "")
            if (project.sharepointSiteUrl && project.sharepointSiteUrl !== "") {
                try {
                    const result = await (await GetProspService()).getSharePointFileNamesAndId({ url: project.sharepointSiteUrl })
                    setDriveItems(result)
                } catch (error) {
                    console.error("[PROSPTab] error while submitting form data", error)
                }
            }
        })()
    }, [])

    const saveUrl: MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault()
        try {
            const result = await (await GetProspService()).getSharePointFileNamesAndId({ url: sharepointUrl })
            if (sharepointUrl !== project.sharepointSiteUrl) {
                const newProject:Project = { ...project }
                newProject.sharepointSiteUrl = sharepointUrl
                const projectResult = await (await GetProjectService()).updateProject(newProject)
                setProject(projectResult)
                setSharepointUrl(projectResult.sharepointSiteUrl ?? "")
            }
            setDriveItems(result)
        } catch (error) {
            console.error("[PROSPTab] error while submitting form data", error)
        }
    }
    return (
        <div color="yellow">
            <TopWrapper color="danger">
                <Typography variant="h4">PROSP</Typography>
            </TopWrapper>
            <Label htmlFor="textfield-normal" label="Sharepoint Site addresse" />
            <ProspFieldWrapper>
                <Input
                    id="textfield-normal"
                    placeholder="Paste Uri here"
                    onChange={(e) => setSharepointUrl(e.currentTarget.value)}
                    value={sharepointUrl}
                />
                <Button variant="outlined" onClick={saveUrl}>Refresh</Button>
            </ProspFieldWrapper>

            <PROSPCaseList
                project={project}
                setProject={setProject}
                driveItems={driveItems}
                setDriveItems={setDriveItems}
            />

        </div>
    )
}

export default PROSPTab
