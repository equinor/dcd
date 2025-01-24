import { useEffect, useMemo } from "react"
import { Typography } from "@equinor/eds-core-react"
import { PersonSelectEvent } from "@equinor/fusion-react-person"
import Grid from "@mui/material/Grid"
import { useMediaQuery } from "@mui/material"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"

import { UserRole } from "@/Models/AccessManagement"
import { useProjectContext } from "@/Context/ProjectContext"
import AccessManagementSkeleton from "./Components/AccessManagementSkeleton"
import { EditorViewerContainer } from "./Components/AccessManagement.styles"
import ExternalAccessInfo from "./Components/ExternalAccessInfo"
import RolePanel from "./Components/RolePanel"
import { useDataFetch } from "@/Hooks/useDataFetch"
import { useEditPeople } from "@/Hooks/useEditPeople"

const AccessManagementTab = () => {
    const { projectId } = useProjectContext()
    const isSmallScreen = useMediaQuery("(max-width:960px)", { noSsr: true })
    const { currentContext } = useModuleCurrentContext()
    const revisionAndProjectData = useDataFetch()
    const fusionProjectId = revisionAndProjectData?.commonProjectAndRevisionData?.fusionProjectId

    const {
        addPerson,
        updatePerson,
        deletePerson,
        syncPmtMembers,
    } = useEditPeople()

    const projectData = revisionAndProjectData?.dataType === "project"
        ? (revisionAndProjectData as Components.Schemas.ProjectDataDto)
        : null

    const viewers = useMemo(
        () => projectData?.projectMembers?.filter((m) => m.role === 0) ?? [],
        [projectData],
    )
    const editors = useMemo(
        () => projectData?.projectMembers?.filter((m) => m.role === 1) ?? [],
        [projectData],
    )

    const handleRemovePerson = (userId: string) => {
        if (!projectId) { return }
        deletePerson(projectId, userId)
    }

    const handleAddPerson = (e: PersonSelectEvent, role: UserRole) => {
        const personToAdd = e.nativeEvent.detail.selected?.azureId

        if (
            !personToAdd
            || !projectId
            || projectData?.projectMembers.some((p) => p.userId === personToAdd)
        ) {
            return
        }

        addPerson(projectId, personToAdd, role)
    }

    const handleSwitchPerson = (personId: string, role: UserRole) => {
        if (!personId || !projectId) { return }
        updatePerson(projectId, personId, role)
    }

    // This is used to synchronize PMT members to projects
    useEffect(
        () => {
            if (!projectId || !fusionProjectId || !currentContext?.id || !currentContext?.externalId) { return }
            if (fusionProjectId !== currentContext.externalId) { return }

            syncPmtMembers(projectId, currentContext.id)
        },
        [projectId, currentContext, fusionProjectId],
    )

    if (!revisionAndProjectData) {
        return <AccessManagementSkeleton />
    }

    return (
        <Grid container direction="column" paddingX="10px" maxWidth={1200} spacing={2}>
            <Grid item>
                <Typography variant="h3">Access Management</Typography>
            </Grid>
            <Grid item>
                <Typography variant="body_short">
                    On this page the project admins can add and remove members to the project.
                    If the project classification is set to &quot;restricted&quot; or &quot;confidential&quot;,
                    only the project members and the application admin can access it.
                    Project members from Org chart with &quot;PMT&quot; are automatically added as project viewers. External users can also be added here.
                </Typography>
            </Grid>
            <EditorViewerContainer $isSmallScreen={isSmallScreen}>
                <RolePanel
                    isSmallScreen={isSmallScreen}
                    people={editors}
                    handleAddPerson={handleAddPerson}
                    handleSwitchPerson={handleSwitchPerson}
                    handleRemovePerson={handleRemovePerson}
                />
                <hr />
                <RolePanel
                    isSmallScreen={isSmallScreen}
                    isViewers
                    people={viewers}
                    handleAddPerson={handleAddPerson}
                    handleSwitchPerson={handleSwitchPerson}
                    handleRemovePerson={handleRemovePerson}
                />
            </EditorViewerContainer>
            <ExternalAccessInfo />
        </Grid>
    )
}

export default AccessManagementTab
