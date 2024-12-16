import { __BaseService } from "./__BaseService"
import { config } from "./config"
import { getToken, loginAccessTokenKey } from "../Utils/common"

const projectUrl = (projectId: string) => `projects/${projectId}/images`
const caseUrl = (projectId: string, caseId: string) => `projects/${projectId}/cases/${caseId}/images`

export class ImageService extends __BaseService {
    public async getCaseImages(projectId: string, caseId?: string): Promise<Components.Schemas.ImageDto[]> {
        const response = await this.get(caseId ? caseUrl(projectId, caseId) : projectUrl(projectId))
        return response
    }

    public async getProjectImages(projectId: string): Promise<Components.Schemas.ImageDto[]> {
        const response = await this.get(projectUrl(projectId))
        return response
    }

    public async deleteImage(projectId: string, imageId: string, caseId?: string): Promise<void> {
        try {
            if (caseId) {
                await this.delete(`projects/${projectId}/cases/${caseId}/images/${imageId}`)
            } else {
                await this.delete(`projects/${projectId}/images/${imageId}`)
            }
        } catch (error) {
            throw new Error(`Error deleting image: ${error}`)
        }
    }

    public async uploadImage(projectId: string, projectName: string, file: File, caseId?: string): Promise<Components.Schemas.ImageDto> {
        const formData = new FormData()
        formData.append("image", file)
        formData.append("projectName", projectName)

        console.log("level:", caseId ? "case" : "project")
        console.log("url:", caseId ? caseUrl(projectId, caseId) : projectUrl(projectId))
        const response = await this.post(caseId ? caseUrl(projectId, caseId) : projectUrl(projectId), {
            body: formData,
        })
        if (response) {
            return response
        }
        console.error("Response data is undefined:", response)
        throw new Error("Upload image response data is undefined")
    }

    public async uploadProjectImage(projectId: string, projectName: string, file: File): Promise<Components.Schemas.ImageDto> {
        const formData = new FormData()
        formData.append("image", file)
        formData.append("projectName", projectName)

        const response = await this.post(projectUrl(projectId), {
            body: formData,
        })

        if (response) {
            return response
        }

        throw new Error("Upload project image response data is undefined")
    }
    // http://localhost:3000/api/projects/e43b2b9a-79bc-4d70-8f0b-5b6d4b8855c2/projects/e43b2b9a-79bc-4d70-8f0b-5b6d4b8855c2/images/863b88ae-b9f2-4005-9298-f7e39e99785b
    // "https://dcdstorageaccount.blob.core.windows.net/ci-image-storage/Johan-Castberg/projects/e43b2b9a-79bc-4d70-8f0b-5b6d4b8855c2/863b88ae-b9f2-4005-9298-f7e39e99785b?sp=r&st=2024-12-16T07:18:21Z&se=2025-03-16T15:18:21Z&spr=https&sv=2022-11-02&sr=c&sig=p4BvYw3TzHuucpRhVilXP0%2BFEQ3IrXiRkMB1DjQJGgc%3D"
    // https://dcdstorageaccount.blob.core.windows.net/ci-image-storage/Johan-Castberg/projects/e43b2b9a-79bc-4d70-8f0b-5b6d4b8855c2/863b88ae-b9f2-4005-9298-f7e39e99785b

    // http://localhost:3000/api/projects/e43b2b9a-79bc-4d70-8f0b-5b6d4b8855c2/cases/b0d06ed4-3ba7-46a2-8731-a6ea0f21c5a0/images/83f3adde-3e9c-4761-9c67-f48b35585154
    // https://dcdstorageaccount.blob.core.windows.net/ci-image-storage/Johan-Castberg/cases/b0d06ed4-3ba7-46a2-8731-a6ea0f21c5a0/83f3adde-3e9c-4761-9c67-f48b35585154?sp=r&st=2024-12-16T07:18:21Z&se=2025-03-16T15:18:21Z&spr=https&sv=2022-11-02&sr=c&sig=p4BvYw3TzHuucpRhVilXP0%2BFEQ3IrXiRkMB1DjQJGgc%3D
    // eslint-disable-next-line class-methods-use-this
    public async fetchImage(projectId: string, caseId: string, imageId: string): Promise<string> {
        let response: Response
        if (caseId) {
            response = await fetch(`/api/projects/${projectId}/cases/${caseId}/images/${imageId}`)
        } else {
            response = await fetch(`/api/projects/${projectId}/images/${imageId}`)
        }
        if (!response.ok) {
            throw new Error("Failed to fetch image")
        }
        const blob = await response.blob()
        return URL.createObjectURL(blob)
    }
}

export const getImageService = async () => new ImageService({
    ...config.ImageService,
    accessToken: await getToken(loginAccessTokenKey)!,
})
