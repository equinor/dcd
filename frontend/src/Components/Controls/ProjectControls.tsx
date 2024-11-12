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
    history,
} from "@equinor/eds-icons"
import Tabs from "@mui/material/Tabs"
import Tab from "@mui/material/Tab"
import { useEffect, useState } from "react"
import { useMediaQuery } from "@mui/material"

import { useProjectContext } from "@/Context/ProjectContext"
import useEditDisabled from "@/Hooks/useEditDisabled"
import { useAppContext } from "@/Context/AppContext"
import { formatDateAndTime } from "@/Utils/common"
import { projectTabNames } from "@/Utils/constants"
import RevisionsDropMenu from "./RevisionsDropMenu"
import Classification from "./ClassificationChip"
import FullPageLoading from "../fullPageLoading"
import RevisionChip from "./RevisionChip"
import { useFeatureContext } from "@/Context/FeatureContext"

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

const LastUpdated = styled.div`
    flex-direction: column;
    gap: 0!important;
`

const ChipsContainer = styled.div<{ $isSmallScreen: boolean }>`
    flex-direction: ${({ $isSmallScreen }) => ($isSmallScreen ? "column" : "row")};
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
    const { editMode, setEditMode } = useAppContext()
    const { activeTabProject, setActiveTabProject, isRevision } = useProjectContext()
    const { isSaving, showEditHistory } = useAppContext()
    const { isEditDisabled, getEditDisabledText } = useEditDisabled()
    const isSmallScreen = useMediaQuery("(max-width: 968px)")

    const { Features } = useFeatureContext()
    const [isMenuOpen, setIsMenuOpen] = useState(false)
    const [revisionMenuAnchorEl, setRevisionMenuAnchorEl] = useState<any | null>(null)

    const handleTabChange = (index: number) => {
        setActiveTabProject(index)
    }

    useEffect(() => {
        if (isRevision) {
            setEditMode(false)
        }
    }, [isRevision])

    return (
        <>
            {isSaving && <FullPageLoading />}
            <Header>
                <div>
                    <Typography variant="h4">
                        {currentContext?.title}
                    </Typography>
                    <ChipsContainer $isSmallScreen={isSmallScreen}>
                        <Classification />
                        {isRevision && (
                            <RevisionChip />
                        )}
                    </ChipsContainer>
                </div>
                <div>
                    {!editMode
                        && (
                            <LastUpdated>
                                <Typography variant="caption">
                                    Project last updated
                                </Typography>
                                <Typography variant="caption">
                                    {formatDateAndTime(projectLastUpdated)}
                                </Typography>
                            </LastUpdated>
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
                    <Tooltip title={getEditDisabledText()}>
                        <Button
                            onClick={handleEdit}
                            variant={editMode ? "outlined" : "contained"}
                            disabled={isEditDisabled}
                        >
                            {editMode && (
                                <>
                                    <Icon data={visibility} />
                                    {!isSmallScreen && <span>View</span>}
                                </>
                            )}
                            {!editMode && (
                                <>
                                    <Icon data={edit} />
                                    {!isSmallScreen && <span>Edit</span>}
                                </>
                            )}
                        </Button>
                    </Tooltip>
                    {Features?.revisionEnabled
                        && (
                            <div>
                                <Tooltip title="This is a revision">
                                    <Button variant="outlined" onClick={() => setIsMenuOpen(!isMenuOpen)} ref={setRevisionMenuAnchorEl}>
                                        <Icon data={history} />
                                        {!isSmallScreen && "Project revisions"}
                                    </Button>
                                </Tooltip>
                                <RevisionsDropMenu
                                    isMenuOpen={isMenuOpen}
                                    setIsMenuOpen={setIsMenuOpen}
                                    menuAnchorEl={revisionMenuAnchorEl}
                                    isCaseMenu={false}
                                />
                            </div>
                        )}
                </div>
            </Header>
            <Tabs
                value={activeTabProject}
                onChange={(_, index) => handleTabChange(index)}
                variant="scrollable"
            >
                {projectTabNames
                    .filter((tabName) => showEditHistory || tabName !== "Case edit history")
                    .map((tabName) => <Tab key={tabName} label={tabName} />)}
            </Tabs>

        </>
    )
}

export default ProjectControls
