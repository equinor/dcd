import { __BaseService } from "./__BaseService"

class WellService extends __BaseService {
    public async isWellInUse(
        projectId: string,
        wellId: string,
    ): Promise<boolean> {
        const res = await this.get(`projects/${projectId}/wells/${wellId}/is-in-use`)

        return res
    }
}

export const GetWellService = () => new WellService()
