import { __BaseService } from "./__BaseService"

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

    public async deleteProjectImage(projectId: string, imageId: string): Promise<void> {
        try {
            await this.delete(`projects/${projectId}/images/${imageId}`)
        } catch (error) {
            throw new Error(`Error deleting image: ${error}`)
        }
    }

    public async deleteCaseImage(projectId: string, caseId: string, imageId: string): Promise<void> {
        try {
            await this.delete(`projects/${projectId}/cases/${caseId}/images/${imageId}`)
        } catch (error) {
            throw new Error(`Error deleting image: ${error}`)
        }
    }

    public async updateProjectImage(
        projectId: string,
        imageId: string,
        dto: Components.Schemas.UpdateImageDto,
    ): Promise<Components.Schemas.ImageDto> {
        const response: Components.Schemas.ImageDto = await this.put(`projects/${projectId}/images/${imageId}`, { body: dto })

        return response
    }

    public async updateCaseImage(
        projectId: string,
        caseId: string,
        imageId: string,
        dto: Components.Schemas.UpdateImageDto,
    ): Promise<Components.Schemas.ImageDto> {
        const response: Components.Schemas.ImageDto = await this.put(`projects/${projectId}/cases/${caseId}/images/${imageId}`, { body: dto })

        return response
    }

    public async uploadCaseImage(projectId: string, file: File, caseId: string): Promise<Components.Schemas.ImageDto> {
        const formData = new FormData()

        formData.append("image", file)
        const response = await this.post(caseUrl(projectId, caseId), {
            body: formData,
        })

        if (response) {
            return response
        }
        throw new Error("Upload image response data is undefined")
    }

    public async uploadProjectImage(projectId: string, file: File): Promise<Components.Schemas.ImageDto> {
        const formData = new FormData()

        formData.append("image", file)
        const response = await this.post(projectUrl(projectId), {
            body: formData,
        })

        if (response) {
            return response
        }
        throw new Error("Upload image response data is undefined")
    }
}

export const getImageService = () => new ImageService()
