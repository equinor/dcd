import { Typography } from "@material-ui/core"
import {
    ChangeEvent, ChangeEventHandler, Dispatch, MouseEventHandler, SetStateAction, useEffect, useState,
} from "react"
import styled from "styled-components"
import {
    Button, Input, Label, Progress, Switch,
} from "@equinor/eds-core-react"
import { Project } from "../../models/Project"
import { GetProspService } from "../../Services/ProspService"
import { GetProjectService } from "../../Services/ProjectService"
import { DriveItem } from "../../models/sharepoint/DriveItem"
import PROSPCaseList from "./PROSPCaseList"

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

interface Props {
    setProject: Dispatch<SetStateAction<Project | undefined>>
    project: Project,
}

function PROSPTab({
    project,
    setProject,
}: Props) {
    const [sharepointUrl, setSharepointUrl] = useState<string>()
    const [check, setCheck] = useState(false)
    const [driveItems, setDriveItems] = useState<DriveItem[]>()

    const [isRefreshing, setIsRefreshing] = useState<boolean>()

    useEffect(() => {
        (async () => {
            setSharepointUrl(project.sharepointSiteUrl ?? "")
            if (project.sharepointSiteUrl && project.sharepointSiteUrl !== "") {
                try {
                    const result = await (await GetProspService())
                        .getSharePointFileNamesAndId({ url: project.sharepointSiteUrl })
                    setDriveItems(result)
                } catch (error) {
                    console.error("[PROSPTab] error while submitting form data", error)
                }
            }
        })()
    }, [])

    const saveUrl: MouseEventHandler<HTMLButtonElement> = async (e) => {
        setIsRefreshing(true)
        e.preventDefault()
        try {
            const result = await (await GetProspService()).getSharePointFileNamesAndId({ url: sharepointUrl })
            if (sharepointUrl !== project.sharepointSiteUrl) {
                const newProject: Project = { ...project }
                newProject.sharepointSiteUrl = sharepointUrl
                const projectResult = await (await GetProjectService()).updateProject(newProject)
                setProject(projectResult)
                setSharepointUrl(projectResult.sharepointSiteUrl ?? "")
            }
            setDriveItems(result)
            setIsRefreshing(false)
        } catch (error) {
            setIsRefreshing(false)
            console.error("[PROSPTab] error while submitting form data", error)
        }
    }

    const handleSharePointUrl: ChangeEventHandler<HTMLInputElement> = (e) => {
        setSharepointUrl(e.currentTarget.value)
    }

    return (
        <div color="yellow">
            <TopWrapper color="danger">
                <Typography variant="h4">PROSP</Typography>
            </TopWrapper>
            <Label htmlFor="textfield-normal" label="Sharepoint Site addresse" />
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
                            <Progress.Circular size={16} color="primary" />
                        </Button>
                    )}
            </ProspFieldWrapper>
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
                project={project}
                setProject={setProject}
                driveItems={driveItems}
                check={check}
            />

        </div>
    )
}

export default PROSPTab
