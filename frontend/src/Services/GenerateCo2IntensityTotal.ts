import { __BaseService } from "./__BaseService"
import { config } from "./config"
import { GetToken, LoginAccessTokenKey } from "../Utils/common"

export class __GenerateCo2IntensityTotal extends __BaseService {
    async calculate(caseId: string) {
        // eslint-disable-next-line max-len
        const res: number = await this.post<number>(`/${caseId}/generateCo2IntensityTotal`)
        return res
    }
}

export async function GetCo2IntensityTotal() {
    return new __GenerateCo2IntensityTotal({
        ...config.GenerateCo2IntensityTotal,
        accessToken: await GetToken(LoginAccessTokenKey)!,
    })
}
