import { useEffect, useMemo } from "react"
import { Typography } from "@equinor/eds-core-react"
import { PersonSelectEvent } from "@equinor/fusion-react-person"
import Grid from "@mui/material/Grid"
import { useQuery, useQueryClient } from "@tanstack/react-query"
import { useMediaQuery } from "@mui/material"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"

import { peopleQueryFn, projectQueryFn } from "@/Services/QueryFunctions"
import { useAppContext } from "@/Context/AppContext"
import { UserRole } from "@/Models/AccessManagement"
import { GetProjectMembersService } from "@/Services/ProjectMembersService"
import { useProjectContext } from "@/Context/ProjectContext"
import { GetOrgChartMembersService } from "@/Services/OrgChartMembersService"
import AccessManagementSkeleton from "./Components/AccessManagementSkeleton"
import { EditorViewerContainer } from "./Components/AccessManagement.styles"
import ExternalAccessInfo from "./Components/ExternalAccessInfo"
import RolePanel from "./Components/RolePanel"
import { useDataFetch } from "@/Hooks/useDataFetch"

const AccessManagementTab = () => {
    const { projectId } = useProjectContext()
    const queryClient = useQueryClient()
    const isSmallScreen = useMediaQuery("(max-width:960px)", { noSsr: true })
    const { setSnackBarMessage } = useAppContext()
    const { currentContext } = useModuleCurrentContext()
    const data = useDataFetch()

    console.log(data)

    const { data: projectApiData } = useQuery({
        queryKey: ["projectApiData", projectId],
        queryFn: () => projectQueryFn(projectId),
        enabled: !!projectId,
    })

    const { data: peopleApiData } = useQuery({
        queryKey: ["peopleApiData", projectId],
        queryFn: () => peopleQueryFn(projectId),
        enabled: !!projectId,
    })

    const viewers = useMemo(() => peopleApiData?.filter((m) => m.role === 0), [peopleApiData])
    const editors = useMemo(() => peopleApiData?.filter((m) => m.role === 1), [peopleApiData])

    const handleRemovePerson = async (userId: string) => {
        await (await GetProjectMembersService()).deletePerson(projectId, userId).then(() => {
            queryClient.invalidateQueries(
                { queryKey: ["peopleApiData", projectId] },
            )
        })
    }

    const handleAddPerson = async (e: PersonSelectEvent, role: UserRole) => {
        const personToAdd = e.nativeEvent.detail.selected?.azureId
        if ((!personToAdd || !projectId) || projectApiData?.projectMembers.some((p) => p.userId === personToAdd)) { return }
        const addPerson = await (await GetProjectMembersService()).addPerson(projectId, { userId: personToAdd || "", role })
        if (addPerson) {
            queryClient.invalidateQueries(
                { queryKey: ["peopleApiData", projectId] },
            )
        }
    }

    const handleSwitchPerson = async (personId: string, role: UserRole) => {
        if (!personId && !projectId) { return }

        const switchRoles = await (await GetProjectMembersService()).updatePerson(projectId, { userId: personId, role })
        if (switchRoles) {
            queryClient.invalidateQueries(
                { queryKey: ["peopleApiData", projectId] },
            )
        }
    }

    // This is used to synchronize PMT members to projects
    useEffect(() => {
        (async () => {
            if (!projectId || !currentContext) { return }
            const projectMembersService = await GetOrgChartMembersService()
            const syncPmt = await projectMembersService.getOrgChartPeople(projectId, currentContext?.id)
            if (syncPmt) {
                queryClient.invalidateQueries(
                    { queryKey: ["peopleApiData", projectId] },
                )
            }
        })()
    }, [])

    if (!projectApiData) {
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
                    If the project classification is set to “restricted” or “confidential”,
                    only the project members and the application admin can access it.
                    {/* Project members from Org chart with “PMT” are automatically added as project viewers after DG0. External users can also be added here. */}
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
