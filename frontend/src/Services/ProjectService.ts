import { config } from "./config"
import { __BaseService } from "./__BaseService"

export class __ProjectService extends __BaseService {
    getProjects() {
        return this.get('')
    }

    getProjectByID(id: string) {
        return this.get(`/${id}`)
    }

    createProject(project: Components.Schemas.ProjectDto) {
        return this.post(``, { body: project })
    }
}

export const projectService = new __ProjectService({
        ...config.ProjectService,
        accessToken: window.sessionStorage.getItem('loginAccessToken')!,
    })
