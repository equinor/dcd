import styled from "styled-components"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import {
    Typography,
    Icon,
    Button,
    Tooltip,
    CircularProgress,
} from "@equinor/eds-core-react"
import {
    edit,
    visibility,
    check_circle_outlined,
} from "@equinor/eds-icons"
import Tabs from "@mui/material/Tabs"
import Tab from "@mui/material/Tab"
import Classification from "./Classification"
import { useAppContext } from "../../Context/AppContext"
import { formatDateAndTime } from "../../Utils/common"
import { projectTabNames } from "../../Utils/constants"
import { useProjectContext } from "../../Context/ProjectContext"
import FullPageLoading from "../fullPageLoading"

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
const Status = styled.div`
   display: flex;
   align-items: center;
   gap: 5px;
`
interface props {
    projectLastUpdated: string
    handleEdit: () => void
}

const ProjectControls = ({ projectLastUpdated, handleEdit }: props) => {
    const { currentContext } = useModuleCurrentContext()
    const { editMode } = useAppContext()
    const { activeTabProject, setActiveTabProject } = useProjectContext()
    const { isSaving } = useAppContext()

    const handleTabChange = (index: number) => {
        setActiveTabProject(index)
    }

    return (
        <>
            {isSaving && <FullPageLoading />}
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
                        isSaving ? (
                            <Status>
                                <CircularProgress value={0} size={16} />
                                <Typography variant="caption">saving...</Typography>
                            </Status>
                        ) : (
                            <Tooltip title="All changes are saved">
                                <Status>
                                    <Icon data={check_circle_outlined} size={16} />
                                    <Typography variant="caption">up to date</Typography>
                                </Status>
                            </Tooltip>
                        )
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
