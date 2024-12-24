import React from "react"
import {
    Tabs, Tab, Box,
} from "@mui/material"
import { Icon } from "@equinor/eds-core-react"
import { users_circle, settings } from "@equinor/eds-icons"

import { projectTabNames } from "@/Utils/constants"
import { useAppContext } from "@/Context/AppContext"

type ProjectTabsProps = {
    activeTabProject: number | boolean
    setActiveTabProject: (index: number) => void
}

const ProjectTabs: React.FC<ProjectTabsProps> = ({ activeTabProject, setActiveTabProject }) => {
    const leftTabs = projectTabNames.filter((name) => name !== "Access Management" && name !== "Settings")
    const rightTabs = projectTabNames.filter((name) => name === "Access Management" || name === "Settings")
    const { showEditHistory } = useAppContext()

    const handleTabChange = (index: number) => {
        setActiveTabProject(index)
    }

    const getTabIndex = (index: number, isRightTabs: boolean) => {
        if (isRightTabs) {
            return index + leftTabs.length
        }
        return index
    }

    return (
        <>
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
                        icon={tabName === "Access Management"
                            ? <Icon data={users_circle} />
                            : <Icon data={settings} />}
                    />
                ))}
            </Tabs>
        </>
    )
}

export default ProjectTabs
