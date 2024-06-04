import { __BaseService } from "./__BaseService"
import { config } from "./config"
import { getToken, loginAccessTokenKey } from "../Utils/common"

export class ImageService extends __BaseService {
    public async uploadImage(projectId: string, projectName: string, caseId: string, file: File): Promise<Components.Schemas.ImageDto> {
        const formData = new FormData()
        formData.append("image", file)
        // Assuming projectId and projectName should be in the form data
        formData.append("projectId", projectId)
        formData.append("projectName", projectName)

        const response = await this.post(`projects/${projectId}/cases/${caseId}/images`, {
            body: formData,
            // Remove the 'Content-Type' header to let the browser set it with the correct boundary
        })
        if (response) {
            return response
        }
        console.error("Response data is undefined:", response)
        throw new Error("Upload image response data is undefined")
    }

    public async getImages(projectId: string, caseId: string): Promise < Components.Schemas.ImageDto[] > {
    const response = await this.get(`projects/${projectId}/cases/${caseId}/images`)
        return response
}
}

export const getImageService = async () => new ImageService({
    ...config.ImageService,
    accessToken: await getToken(loginAccessTokenKey)!,
})
