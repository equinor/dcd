import { useMutation, useQueryClient } from "@tanstack/react-query"
import { getImageService } from "@/Services/ImageService"
import { useAppStore } from "@/Store/AppStore"

type UpdateImageVariables = {
    projectId: string
    imageId: string
    description: string
    caseId?: string
}

type DeleteImageVariables = {
    projectId: string
    imageId: string
    caseId?: string
}

export const useEditGallery = () => {
    const queryClient = useQueryClient()
    const { setSnackBarMessage, setIsSaving } = useAppStore()

    const updateImageMutationFn = async ({
        projectId,
        imageId,
        description,
        caseId,
    }: UpdateImageVariables) => {
        const imageService = getImageService()
        const updateImageDto = { description }

        if (caseId) {
            return imageService.updateCaseImage(projectId, caseId, imageId, updateImageDto)
        }
        return imageService.updateProjectImage(projectId, imageId, updateImageDto)
    }

    const deleteImageMutationFn = async ({ projectId, imageId, caseId }: DeleteImageVariables) => {
        const imageService = getImageService()

        if (caseId) {
            return imageService.deleteCaseImage(projectId, caseId, imageId)
        }
        return imageService.deleteProjectImage(projectId, imageId)
    }

    const updateImageMutation = useMutation({
        mutationFn: updateImageMutationFn,
        onMutate: () => {
            setIsSaving(true)
        },
        onSuccess: (_, variables) => {
            queryClient.invalidateQueries({
                queryKey: ["gallery", variables.projectId, variables.caseId],
            })
            setIsSaving(false)
        },
        onError: (error) => {
            console.error("Error updating image description:", error)
            setSnackBarMessage("Error updating description")
            setIsSaving(false)
        },
    })

    const deleteImageMutation = useMutation({
        mutationFn: deleteImageMutationFn,
        onMutate: () => {
            setIsSaving(true)
        },
        onSuccess: (_, variables) => {
            queryClient.invalidateQueries({
                queryKey: ["gallery", variables.projectId, variables.caseId],
            })
            setIsSaving(false)
        },
        onError: (error) => {
            console.error("Error deleting image:", error)
            setSnackBarMessage("Error deleting image")
            setIsSaving(false)
        },
    })

    const updateImageDescription = (
        projectId: string,
        imageId: string,
        description: string,
        caseId?: string,
    ) => {
        updateImageMutation.mutate({
            projectId,
            imageId,
            description,
            caseId,
        })
    }

    const deleteImage = (
        projectId: string,
        imageId: string,
        caseId?: string,
    ) => {
        deleteImageMutation.mutate({ projectId, imageId, caseId })
    }

    return {
        updateImageDescription,
        deleteImage,
        isUpdating: updateImageMutation.isPending,
        isDeleting: deleteImageMutation.isPending,
    }
}
