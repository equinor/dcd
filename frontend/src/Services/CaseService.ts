import { __BaseService } from "./__BaseService";

import { Project } from "../models/Project";
import { config } from "./config";

class __CaseService extends __BaseService {
    public async createCase(data: Components.Schemas.CaseDto): Promise<Project> {
        const res = await this.post("", { body: data })
        return Project.fromJSON(res)
    }
}

export const CaseService = new __CaseService({
    ...config.CaseService,
    accessToken: window.sessionStorage.getItem("loginAccessToken")!,
})
