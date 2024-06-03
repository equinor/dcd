import { useState } from "react"
import { __BaseService } from "./__BaseService"
import { config } from "./config"
import { getToken, loginAccessTokenKey } from "../Utils/common"

export class ImageService extends __BaseService {
    public async uploadImage(projectId: string, caseId: string, file: File): Promise<Components.Schemas.ImageDto> {
        const formData = new FormData()
        formData.append("image", file)

        const response = await this.post(`projects/${projectId}/cases/${caseId}/images`, {
            body: formData,
            headers: {
                "Content-Type": "multipart/form-data",
            },
        })
        if (response) {
            return response
        }
        console.error("Response data is undefined:", response)
        throw new Error("Upload image response data is undefined")
    }

    public async getImages(projectId: string, caseId: string): Promise<Components.Schemas.ImageDto[]> {
        const response = await this.get(`projects/${projectId}/cases/${caseId}/images`)
        return response
    }
}

export const getImageService = async () => new ImageService({
    ...config.ImageService,
    accessToken: await getToken(loginAccessTokenKey)!,
})
