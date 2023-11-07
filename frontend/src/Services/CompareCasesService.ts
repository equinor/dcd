import { __BaseService } from "./__BaseService"
import { config } from "./config"
import { GetToken, LoginAccessTokenKey } from "../Utils/common"

export class __CompareCasesService extends __BaseService {
    async calculate(projectId: string) {
        // eslint-disable-next-line max-len
        const res: Components.Schemas.CompareCasesDto[] = await this.post<Components.Schemas.CompareCasesDto[]>(`/${projectId}/calculateCompareCasesTotals`)
        return res
    }
}

export async function GetCompareCasesService() {
    return new __CompareCasesService({
        ...config.CompareCasesService,
        accessToken: await GetToken(LoginAccessTokenKey)!,
    })
}
