import { useState, useEffect } from "react"
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
    settings,
    users_circle,
} from "@equinor/eds-icons"
import Tabs from "@mui/material/Tabs"
import Tab from "@mui/material/Tab"
import { useMediaQuery, Box } from "@mui/material"
import { useQuery } from "@tanstack/react-query"

import { useProjectContext } from "@/Context/ProjectContext"
import useEditDisabled from "@/Hooks/useEditDisabled"
import { useAppContext } from "@/Context/AppContext"
import { formatDateAndTime } from "@/Utils/common"
import { projectTabNames } from "@/Utils/constants"
import RevisionsDropMenu from "./RevisionsDropMenu"
import Classification from "./ClassificationChip"
import FullPageLoading from "../fullPageLoading"
import RevisionChip from "./RevisionChip"
import { projectQueryFn } from "@/Services/QueryFunctions"
import { User } from "@/Models/AccessManagement"

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

const TabContainer = styled.div`
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-top: 8px;
    border-bottom: 1px solid #DCDCDC;
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
    const {
        activeTabProject,
        setActiveTabProject,
        isRevision,
        projectId,
    } = useProjectContext()
    const leftTabs = projectTabNames.filter((name) => name !== "Access Management" && name !== "Settings")
    const rightTabs = projectTabNames.filter((name) => name === "Access Management" || name === "Settings")
    const { isSaving, showEditHistory } = useAppContext()
    const { isEditDisabled, getEditDisabledText } = useEditDisabled()
    const isSmallScreen = useMediaQuery("(max-width: 968px)")

    const [editors, setEditors] = useState<User[] | undefined>([])
    const [viewers, setViewers] = useState<User[] | undefined>([])
    const [isMenuOpen, setIsMenuOpen] = useState(false)
    const [revisionMenuAnchorEl, setRevisionMenuAnchorEl] = useState<any | null>(null)

    const { data: projectApiData } = useQuery({
        queryKey: ["projectApiData", projectId],
        queryFn: () => projectQueryFn(projectId),
        enabled: !!projectId,
    })

    console.log(projectApiData?.projectMembers)

    const handleTabChange = (index: number) => {
        setActiveTabProject(index)
    }

    const getTabIndex = (index: number, isRightTabs: boolean) => {
        if (isRightTabs) {
            return index + leftTabs.length
        }
        return index
    }
    useEffect(() => {
        if (isRevision) {
            setEditMode(false)
        }
    }, [isRevision])

    useEffect(() => {
        const viewersToAdd = projectApiData?.projectMembers?.filter((m) => m.role === 0) as User[]
        const editorsToAdd = projectApiData?.projectMembers?.filter((m) => m.role === 1) as User[]
        setViewers(viewersToAdd)
        setEditors(editorsToAdd)
    }, [projectApiData])

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
                    {/* Uncomment to show project revisions button */}
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
                </div>
            </Header>
            <TabContainer>
                <Tabs
                    value={typeof activeTabProject === "number" && activeTabProject < leftTabs.length ? activeTabProject : false}
                    onChange={(_, index) => handleTabChange(getTabIndex(index, false))}
                    variant="scrollable"
                >
                    {leftTabs.filter((tabName) => showEditHistory || tabName !== "Case edit history").map((tabName) => <Tab key={tabName} label={tabName} />)}
                </Tabs>
                <Box flexGrow={1} />
                <Tabs
                    sx={{ marginRight: "5px" }}
                    value={typeof activeTabProject === "number" && activeTabProject >= leftTabs.length ? activeTabProject - leftTabs.length : false}
                    onChange={(_, index) => handleTabChange(getTabIndex(index, true))}
                    variant="scrollable"
                >
                    {rightTabs.map((tabName) => (
                        <Tab key={tabName} sx={{ minWidth: "48px" }} icon={tabName === "Access Management" ? <Icon data={users_circle} /> : <Icon data={settings} />} />
                    ))}
                </Tabs>
            </TabContainer>
        </>
    )
}

export default ProjectControls
