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

    public async getImageData(url: string): Promise<Components.Schemas.ImageContentDto> {
        const response = await this.get(url)
        return response
    }

    // eslint-disable-next-line class-methods-use-this
    public async fetchImage(projectId: string, caseId: string | null, imageId: string): Promise<string> {
        let ImageContent: Components.Schemas.ImageContentDto
        if (caseId) {
            ImageContent = await this.getImageData(`/api/projects/${projectId}/cases/${caseId}/images/${imageId}/raw`)
        } else {
            ImageContent = await this.getImageData(`/api/projects/${projectId}/images/${imageId}/raw`)
        }

        const base64String = ImageContent.base64EncodedData
        const byteCharacters = atob(base64String!)
        const byteNumbers = new Array(byteCharacters.length)
        for (let i = 0; i < byteCharacters.length; i += 1) {
            byteNumbers[i] = byteCharacters.charCodeAt(i)
        }
        const byteArray = new Uint8Array(byteNumbers)
        const blob = new Blob([byteArray], { type: "image/jpeg" })
        return URL.createObjectURL(blob)
    }
}

export const getImageService = async () => new ImageService({
    ...config.ImageService,
    accessToken: await getToken(loginAccessTokenKey)!,
})
