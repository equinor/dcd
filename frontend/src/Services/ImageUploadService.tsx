import { __BaseService } from "./__BaseService"
import { config } from "./config"
import { getToken, loginAccessTokenKey } from "../Utils/common"

export class ImageUploadService extends __BaseService {
    public async uploadImage(projectId: string, caseId: string, file: File): Promise<string> {
        const formData = new FormData()
        formData.append("image", file)

        const response = await this.post(`projects/${projectId}/cases/${caseId}/blob-storage/upload`, {
            body: formData,
            headers: {
                "Content-Type": "multipart/form-data",
            },
        })
        console.log("url", response.imageUrl)
        return response.imageUrl
    }

    public async getImages(projectId: string, caseId: string): Promise<string[]> {
        const response = await this.get(`projects/${projectId}/cases/${caseId}/blob-storage/images`)
        return response.map((img: any) => img.url)
    }
}

export const getImageUploadService = async () => new ImageUploadService({
    ...config.ImageUploadService,
    accessToken: await getToken(loginAccessTokenKey)!,
})
