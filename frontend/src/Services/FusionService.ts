import { __BaseService } from "./__BaseService";

export class __FusionService extends __BaseService {
    getFusionProjects() {
        return this.get('/contexts?$filter=type%20in%20(%27OrgChart%27)')
    }
}
