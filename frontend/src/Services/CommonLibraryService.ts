import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"

export class __CommonLibraryService extends __BaseService {
    async getProjects() {
        const projects = await this.get("/projects")
        return projects
    }
}

export function GetCommonLibraryService() {
    return new __CommonLibraryService({
        ...config.CommonLibraryService,
        accessToken: GetToken(LoginAccessTokenKey)!,
    })
}
