import { useState, useEffect } from "react"
import styled from "styled-components"
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
import { useMediaQuery } from "@mui/material"
import { useProjectContext } from "@/Context/ProjectContext"
import useEditDisabled from "@/Hooks/useEditDisabled"
import { useAppContext } from "@/Context/AppContext"
import { formatDateAndTime } from "@/Utils/common"
import RevisionsDropMenu from "./Revision/RevisionsDropMenu"
import FullPageLoading from "../fullPageLoading"
import { useFeatureContext } from "@/Context/FeatureContext"
import ProjectTabs from "./TabNavigators/ProjectTabs"

const Header = styled.div`
    grid-area: top-right;
    padding: 0 13px;

    & div {
        display: flex;
        justify-content: end;
        gap: 10px;
    }
`

const TabContainer = styled.div`
    grid-area: bottom;
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-top: 8px;
    border-bottom: 1px solid #DCDCDC;
`

const LastUpdated = styled.div`
    flex-direction: column;
    && {
        gap: 0;
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
    const { editMode, setEditMode } = useAppContext()
    const {
        activeTabProject,
        setActiveTabProject,
        isRevision,
    } = useProjectContext()
    const { isSaving } = useAppContext()
    const { isEditDisabled, getEditDisabledText } = useEditDisabled()
    const isSmallScreen = useMediaQuery("(max-width: 968px)")

    const { Features } = useFeatureContext()
    const [isMenuOpen, setIsMenuOpen] = useState(false)
    const [revisionMenuAnchorEl, setRevisionMenuAnchorEl] = useState<HTMLElement | null>(null)

    const handleMenuToggle = (event: React.MouseEvent<HTMLElement>) => {
        if (isMenuOpen) {
            setRevisionMenuAnchorEl(null)
            setIsMenuOpen(false)
        } else {
            setRevisionMenuAnchorEl(event.currentTarget)
            setIsMenuOpen(true)
        }
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
                                    <Button
                                        variant="outlined"
                                        onClick={handleMenuToggle}
                                    >
                                        <Icon data={history} />
                                        {!isSmallScreen && "Project revisions"}
                                    </Button>
                                </Tooltip>
                                {isMenuOpen && (
                                    <RevisionsDropMenu
                                        isMenuOpen={isMenuOpen}
                                        setIsMenuOpen={setIsMenuOpen}
                                        menuAnchorEl={revisionMenuAnchorEl}
                                        isCaseMenu={false}
                                    />
                                )}

                            </div>
                        )}
                </div>
            </Header>
            <TabContainer>
                <ProjectTabs
                    activeTabProject={activeTabProject}
                    setActiveTabProject={setActiveTabProject}
                />
            </TabContainer>
        </>
    )
}

export default ProjectControls
