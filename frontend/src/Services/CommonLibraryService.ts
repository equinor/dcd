import { config } from "./config"
import { __BaseService } from "./__BaseService"

export class __CommonLibraryService extends __BaseService {
    async getProjects() {
        const projects = await this.get("/projects")
        return projects
    }
}

export const CommonLibraryService = new __CommonLibraryService({
    ...config.CommonLibraryService,
    accessToken: window.sessionStorage.getItem("loginAccessToken")!,
})
