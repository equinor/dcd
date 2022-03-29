import FileSaver from "file-saver"
import { __BaseService } from "./__BaseService"

import { GetProjectService } from "../Services/ProjectService"

import { config } from "./config"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"

class __STEAService extends __BaseService {
    public async excelToSTEA(projectId: string) {
        const postExcelResponse = await this.postExcel(projectId, "blob", { headers: { accept: "text/plain" } })
        const projectResult = await GetProjectService().getProjectByID(projectId!)
        FileSaver.saveAs(postExcelResponse, (`${projectResult.name}.xlsx`))
    }
}

export function GetSTEAService() {
    return new __STEAService({
        ...config.STEAService,
        accessToken: GetToken(LoginAccessTokenKey)!,
    })
}
