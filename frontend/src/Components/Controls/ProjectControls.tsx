import styled from "styled-components"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import {
    Typography,
    Icon,
    Button,
} from "@equinor/eds-core-react"
import {
    edit,
    save,
} from "@equinor/eds-icons"
import Tabs from "@mui/material/Tabs"
import Tab from "@mui/material/Tab"
import Classification from "./Classification"
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
    setIsCanceling: (value: boolean) => void
}

const ProjectControls = ({ projectLastUpdated, handleEdit, setIsCanceling }: props) => {
    const { currentContext } = useModuleCurrentContext()
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

                    {editMode && (
                        <Button variant="outlined" onClick={() => setIsCanceling(true)}>
                            Cancel
                        </Button>
                    )}
                    <Button onClick={handleEdit} variant={editMode ? "outlined" : "contained"}>

                        {editMode && (
                            <>
                                <Icon data={save} />
                                <span>Save</span>
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
