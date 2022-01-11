import { __BaseService } from "./__BaseService";

class __ProjectService extends __BaseService {
    constructor() {
        super('ProjectsService')
    }

    getProjects() {
        return this.get('')
    }
}

export const ProjectService = new __ProjectService()
