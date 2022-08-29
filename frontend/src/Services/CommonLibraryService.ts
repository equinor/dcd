import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { GetToken, FusionAccessTokenKey } from "../Utils/common"

export class __CommonLibraryService extends __BaseService {
    async getProjects() {
        const projects: Components.Schemas.CommonLibraryProjectDto[] = await this.get("/projects")
        return projects
    }
}

export async function GetCommonLibraryService() {
    return new __CommonLibraryService({
        ...config.CommonLibraryService,
        accessToken: await GetToken(FusionAccessTokenKey)!,
    })
}
