import { __BaseService } from "./__BaseService";

export class __ProjectService extends __BaseService {
    getProjects() {
        return this.get('')
    }
}
