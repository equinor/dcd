import { __BaseService } from "./__BaseService"

export class VideoService extends __BaseService {
    public async getVideo(videoName: string): Promise<Components.Schemas.VideoDto> {
        const response = await this.get(`videos/${videoName}`)

        return response
    }
}

export const getVideoService = () => new VideoService()
