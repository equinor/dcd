import { __BaseService } from "./__BaseService"
import { config } from "./config"
import { getToken, loginAccessTokenKey } from "../Utils/common"

const projectUrl = (projectId: string) => `projects/${projectId}/images`
const caseUrl = (projectId: string, caseId: string) => `projects/${projectId}/cases/${caseId}/images`

export class ImageService extends __BaseService {
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

    public async getImages(projectId: string, caseId?: string): Promise<Components.Schemas.ImageDto[]> {
        console.log("level:", caseId ? "case" : "project")
        console.log("url:", caseId ? caseUrl(projectId, caseId) : projectUrl(projectId))
        const response = await this.get(caseId ? caseUrl(projectId, caseId) : projectUrl(projectId))
        return response
    }

    public async deleteImage(projectId: string, imageId: string, caseId?: string): Promise<void> {
        console.log("level:", caseId ? "case" : "project")
        console.log("url:", caseId ? `${caseUrl(projectId, caseId)}/${imageId}` : `${projectUrl(projectId)}/${imageId}`)
        await this.delete(caseId ? `${caseUrl(projectId, caseId)}/${imageId}` : `${projectUrl(projectId)}/${imageId}`)
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

    public async getProjectImages(projectId: string): Promise<Components.Schemas.ImageDto[]> {
        const response = await this.get(projectUrl(projectId))
        return response
    }

    public async deleteProjectImage(projectId: string, imageId: string): Promise<void> {
        await this.delete(`${projectUrl(projectId)}/${imageId}`)
    }
}

export const getImageService = async () => new ImageService({
    ...config.ImageService,
    accessToken: await getToken(loginAccessTokenKey)!,
})
