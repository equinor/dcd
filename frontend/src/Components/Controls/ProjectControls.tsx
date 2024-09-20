import styled from "styled-components"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import {
    Typography,
    Icon,
    Button,
} from "@equinor/eds-core-react"
import { useQuery, useQueryClient } from "react-query"
import { useNavigate } from "react-router"
import {
    edit,
    visibility,
} from "@equinor/eds-icons"
import Tabs from "@mui/material/Tabs"
import Tab from "@mui/material/Tab"
import Classification from "./Classification"
import { GetProjectService } from "../../Services/ProjectService"
import { useAppContext } from "../../Context/AppContext"

import { formatDateAndTime } from "../../Utils/common"
import { projectTabNames } from "../../Utils/constants"
import { useProjectContext } from "../../Context/ProjectContext"

const Header = styled.div`
    display: flex;
    align-items: center;
    justify-content: space-between;
    gap: 10px;
    padding: 0 13px;

    & div {
        display: flex;
        align-items: center;
        gap: 10px;
    }
`

interface props {
    projectLastUpdated: string
    handleEdit: () => void
}

const ProjectControls = ({ projectLastUpdated, handleEdit }: props) => {
    const { currentContext } = useModuleCurrentContext()
    const queryClient = useQueryClient()
    const { setSnackBarMessage } = useAppContext()
    const navigate = useNavigate()

    const projectId = currentContext?.externalId

    const fetchProjectData = async () => {
        const projectService = await GetProjectService()
        return projectService.getProject(projectId!)
    }

    useQuery(
        ["apiData", { projectId }],
        fetchProjectData,
        {
            enabled: !!projectId,
            onSuccess: (result) => {
                const projectData = result

                queryClient.setQueryData(["apiData", { projectId }], projectData)
            },
            onError: (error: Error) => {
                console.error("Error fetching data:", error)
                setSnackBarMessage("Project data not found")
                navigate("/")
            },
        },
    )
    const { editMode } = useAppContext()
    const { activeTabProject, setActiveTabProject } = useProjectContext()

    const handleTabChange = (index: number) => {
        setActiveTabProject(index)
    }

    return (
        <>
            <Header>
                <div>
                    <Typography variant="h4">
                        {currentContext?.title}
                    </Typography>
                    <Classification />
                </div>
                <div>
                    {!editMode
                        && (
                            <Typography variant="caption">
                                Project last updated
                                {" "}
                                {formatDateAndTime(projectLastUpdated)}
                            </Typography>
                        )}
                    <Button onClick={handleEdit} variant={editMode ? "outlined" : "contained"}>

                        {editMode && (
                            <>
                                <Icon data={visibility} />
                                <span>View</span>
                            </>
                        )}
                        {!editMode && (
                            <>
                                <Icon data={edit} />
                                <span>Edit</span>
                            </>
                        )}
                    </Button>

                </div>
            </Header>
            <Tabs
                value={activeTabProject}
                onChange={(_, index) => handleTabChange(index)}
                variant="scrollable"
            >
                {projectTabNames.map((tabName) => <Tab key={tabName} label={tabName} />)}
            </Tabs>

        </>
    )
}

export default ProjectControls
