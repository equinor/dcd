import { useMutation, useQueryClient } from "@tanstack/react-query"
import { GetProjectMembersService } from "../Services/ProjectMembersService"
import { GetOrgChartMembersService } from "../Services/OrgChartMembersService"
import { useAppStore } from "../Store/AppStore"
import { ProjectMemberRole } from "@/Models/enums"

type AddPersonVariables = {
    projectId: string;
    body: {
        userId: string;
        role: ProjectMemberRole;
    };
};

type UpdatePersonVariables = {
    projectId: string;
    body: {
        userId: string;
        role: ProjectMemberRole;
    };
};

type DeletePersonVariables = {
    projectId: string;
    userId: string;
};

export const useEditPeople = () => {
    const queryClient = useQueryClient()
    const { setSnackBarMessage, setIsSaving } = useAppStore()

    const syncPmtMembers = async (projectId: string, contextId: string) => {
        try {
            const syncPmt = await GetOrgChartMembersService().getOrgChartPeople(projectId, contextId)
            if (syncPmt) {
                queryClient.invalidateQueries(
                    { queryKey: ["projectApiData", projectId] },
                )
            }
        } catch (error) {
            console.error("Error syncing PMT members:", error)
            setSnackBarMessage((error as Error).message || "An unknown error occurred while syncing PMT members")
        }
    }

    const addPersonMutationFn = async ({ projectId, body }: AddPersonVariables) => GetProjectMembersService().addPerson(projectId, body)

    const updatePersonMutationFn = async ({ projectId, body }: UpdatePersonVariables) => GetProjectMembersService().updatePerson(projectId, body)

    const deletePersonMutationFn = async ({ projectId, userId }: DeletePersonVariables) => GetProjectMembersService().deletePerson(projectId, userId)

    const addPersonMutation = useMutation({
        mutationFn: addPersonMutationFn,
        onSuccess: (_, variables) => {
            queryClient.invalidateQueries(
                { queryKey: ["projectApiData", variables.projectId] },
            )
            setIsSaving(false)
        },
        onError: (error) => {
            console.error("Error adding person", error)
            setSnackBarMessage(error.message)
            setIsSaving(false)
        },
    })

    const updatePersonMutation = useMutation({
        mutationFn: updatePersonMutationFn,
        onSuccess: (_, variables) => {
            queryClient.invalidateQueries(
                { queryKey: ["projectApiData", variables.projectId] },
            )
            setIsSaving(false)
        },
        onError: (error) => {
            console.error("Error updating person role", error)
            setSnackBarMessage(error.message)
            setIsSaving(false)
        },
    })

    const deletePersonMutation = useMutation({
        mutationFn: deletePersonMutationFn,
        onSuccess: (_, variables) => {
            queryClient.invalidateQueries(
                { queryKey: ["projectApiData", variables.projectId] },
            )
            setIsSaving(false)
        },
        onError: (error) => {
            console.error("Error removing person", error)
            setSnackBarMessage(error.message)
            setIsSaving(false)
        },
    })

    const addPerson = (
        projectId: string,
        userId: string,
        role: ProjectMemberRole,
    ) => {
        setIsSaving(true)
        addPersonMutation.mutate({ projectId, body: { userId, role } })
    }

    const updatePerson = (
        projectId: string,
        userId: string,
        role: ProjectMemberRole,
    ) => {
        setIsSaving(true)
        updatePersonMutation.mutate({ projectId, body: { userId, role } })
    }

    const deletePerson = (
        projectId: string,
        userId: string,
    ) => {
        setIsSaving(true)
        deletePersonMutation.mutate({ projectId, userId })
    }

    return {
        addPerson,
        updatePerson,
        deletePerson,
        syncPmtMembers,
    }
}

export default useEditPeople
