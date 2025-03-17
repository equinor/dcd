import React from "react"
import {
    Tabs, Tab, Box,
} from "@mui/material"
import { Icon, Tooltip } from "@equinor/eds-core-react"
import { users_circle, settings, assignment } from "@equinor/eds-icons"
import { useParams } from "react-router-dom"

import { projectTabNames } from "@/Utils/constants"
import { useAppNavigation } from "@/Hooks/useNavigate"

type ProjectTabsProps = {
    activeTabProject: number | boolean
    setActiveTabProject: (index: number) => void
}

const ProjectTabs: React.FC<ProjectTabsProps> = ({ activeTabProject, setActiveTabProject }) => {
    const leftTabs = projectTabNames.filter((name) => name !== "Access Management" && name !== "Settings" && name !== "Project change log")
    const rightTabs = projectTabNames.filter((name) => name === "Access Management" || name === "Project change log" || name === "Settings")
    const { revisionId } = useParams()
    const { navigateToProjectTab } = useAppNavigation()

    const handleTabChange = (index: number) => {
        setActiveTabProject(index)
        navigateToProjectTab(index, revisionId)
    }

    const getTabIndex = (index: number, isRightTabs: boolean) => {
        if (isRightTabs) {
            return index + leftTabs.length
        }
        return index
    }

    const getRightTabIcon = (tabName: string) => {
        if (tabName === "Access Management") {
            return users_circle
        }
        if (tabName === "Project change log") {
            return assignment
        }
        if (tabName === "Settings") {
            return settings
        }
        return settings // fallback icon
    }

    return (
        <>
            <Tabs
                value={typeof activeTabProject === "number" && activeTabProject < leftTabs.length ? activeTabProject : false}
                onChange={(_, index) => handleTabChange(getTabIndex(index, false))}
                variant="scrollable"
            >
                {leftTabs.map((tabName) => <Tab key={tabName} label={tabName} />)}
            </Tabs>
            <Box flexGrow={1} />
            <Tabs
                sx={{ marginRight: "5px" }}
                value={
                    typeof activeTabProject === "number"
                        && activeTabProject >= leftTabs.length
                        ? activeTabProject - leftTabs.length
                        : false
                }
                onChange={(_, index) => handleTabChange(getTabIndex(index, true))}
                variant="scrollable"
            >
                {rightTabs.map((tabName) => (
                    <Tab
                        key={tabName}
                        sx={{ minWidth: "48px" }}
                        icon={
                            (
                                <Tooltip title={tabName} placement="left">
                                    <span>
                                        <Icon data={getRightTabIcon(tabName)} />
                                    </span>
                                </Tooltip>
                            )
                        }
                    />
                ))}
            </Tabs>
        </>
    )
}

export default ProjectTabs
