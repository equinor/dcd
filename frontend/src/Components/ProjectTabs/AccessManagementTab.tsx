import { useEffect, useMemo, useState } from "react"
import { Typography } from "@equinor/eds-core-react"
import { PersonSelectEvent } from "@equinor/fusion-react-person"
import Grid from "@mui/material/Grid2"
import { useMediaQuery } from "@mui/material"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useCurrentUser } from "@equinor/fusion-framework-react/hooks"

import { useProjectContext } from "@/Store/ProjectContext"
import AccessManagementSkeleton from "../LoadingSkeletons/AccessManagementSkeleton"
import { EditorViewerContainer } from "./Components/AccessManagement.styles"
import ExternalAccessInfo from "./Components/ExternalAccessInfo"
import RolePanel from "./Components/RolePanel"
import { useDataFetch } from "@/Hooks"
import { useEditPeople } from "@/Hooks/useEditPeople"
import { ProjectMemberRole } from "@/Models/enums"
import RemoveCurrentUserAccess from "@/Components/Modal/RemoveCurrentUserAccess"

const AccessManagementTab = () => {
    const user = useCurrentUser()
    const { projectId } = useProjectContext()
    const isSmallScreen = useMediaQuery("(max-width:960px)", { noSsr: true })
    const { currentContext } = useModuleCurrentContext()
    const revisionAndProjectData = useDataFetch()
    const fusionProjectId = revisionAndProjectData?.commonProjectAndRevisionData?.fusionProjectId

    const [userActionModal, setUserActionModal] = useState<{
        isOpen: boolean;
        isSwitch: boolean;
        azureAdUserId: string;
        role?: ProjectMemberRole;
    }>({
        isOpen: false,
        isSwitch: false,
        azureAdUserId: "",
    })

    const {
        addPerson,
        updatePerson,
        deletePerson,
        syncPmtMembers,
    } = useEditPeople()

    const projectData = revisionAndProjectData?.dataType === "project"
        ? (revisionAndProjectData as Components.Schemas.ProjectDataDto)
        : null

    const currentUserAzureId = useMemo(() => user?.localAccountId, [user])

    const viewers = useMemo(
        () => projectData?.projectMembers?.filter((m) => m.role === ProjectMemberRole.Observer) ?? [],
        [projectData],
    )
    const editors = useMemo(
        () => projectData?.projectMembers?.filter((m) => m.role === ProjectMemberRole.Editor) ?? [],
        [projectData],
    )

    const handleRemovePerson = (azureAdUserId: string) => {
        if (!projectId || !fusionProjectId) { return }

        // If current user is trying to remove themselves, show confirmation modal
        if (azureAdUserId === currentUserAzureId) {
            setUserActionModal({
                isOpen: true,
                isSwitch: false,
                azureAdUserId,
            })
            return
        }

        deletePerson(projectId, fusionProjectId, azureAdUserId)
    }

    const handleAddPerson = (e: PersonSelectEvent, role: ProjectMemberRole) => {
        const personToAdd = e.nativeEvent.detail.selected?.azureId

        if (
            !personToAdd
            || !projectId
            || !fusionProjectId
            || projectData?.projectMembers.some((p) => p.azureAdUserId === personToAdd)
        ) { return }

        addPerson(projectId, fusionProjectId, personToAdd, role)
    }

    const handleSwitchPerson = (azureAdUserId: string, role: ProjectMemberRole) => {
        if (
            !azureAdUserId
            || !projectId
            || !fusionProjectId
        ) { return }

        // If current user is trying to switch themselves from editor to viewer, show confirmation modal
        if (azureAdUserId === currentUserAzureId && role === ProjectMemberRole.Observer) {
            setUserActionModal({
                isOpen: true,
                isSwitch: true,
                azureAdUserId,
                role,
            })
            return
        }

        updatePerson(projectId, fusionProjectId, azureAdUserId, role)
    }

    const handleConfirmUserAction = () => {
        if (!projectId || !fusionProjectId) { return }

        if (userActionModal.isSwitch && userActionModal.role !== undefined) {
            updatePerson(projectId, fusionProjectId, userActionModal.azureAdUserId, userActionModal.role)
        } else {
            deletePerson(projectId, fusionProjectId, userActionModal.azureAdUserId)
        }

        setUserActionModal({
            isOpen: false,
            isSwitch: false,
            azureAdUserId: "",
        })
    }

    // This is used to synchronize PMT members to projects
    useEffect(
        () => {
            if (
                !projectId
                || !fusionProjectId
                || !currentContext?.id
                || !currentContext?.externalId
                || fusionProjectId !== currentContext.externalId
            ) { return }

            syncPmtMembers(projectId, fusionProjectId, currentContext.id)
        },
        [projectId, currentContext, fusionProjectId],
    )

    if (!revisionAndProjectData) {
        return <AccessManagementSkeleton />
    }

    return (
        <Grid container direction="column" paddingX="10px" maxWidth={1200} spacing={2}>
            <Grid size={12}>
                <Typography variant="h3">Access Management</Typography>
            </Grid>
            <Grid size={12}>
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

            <RemoveCurrentUserAccess
                isOpen={userActionModal.isOpen}
                isSwitch={userActionModal.isSwitch}
                onClose={() => setUserActionModal({ isOpen: false, isSwitch: false, azureAdUserId: "" })}
                onConfirm={handleConfirmUserAction}
            />
        </Grid>
    )
}

export default AccessManagementTab
