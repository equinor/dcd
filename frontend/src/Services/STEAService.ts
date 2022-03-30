import FileSaver from "file-saver"
import { __BaseService } from "./__BaseService"

import { GetProjectService } from "../Services/ProjectService"

import { config } from "./config"

import { Project } from "../models/Project"
import { LoginAccessTokenKey, GetToken } from "../Utils/common"

class __STEAService extends __BaseService {
    public async excelToSTEA(project: Project) {
        const postExcelResponse = await this.postExcel(project.id, "blob", { headers: { accept: "text/plain" } })
        FileSaver.saveAs(postExcelResponse, (`${project.name}.xlsx`))
    }
}

export function GetSTEAService() {
    return new __STEAService({
        ...config.STEAService,
        accessToken: GetToken(LoginAccessTokenKey)!,
    })
}
