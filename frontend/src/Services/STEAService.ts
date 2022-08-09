import FileSaver from "file-saver"
import { __BaseService } from "./__BaseService"

import { config } from "./config"

import { Project } from "../models/Project"
import { LoginAccessTokenKey, GetToken } from "../Utils/common"

class __STEAService extends __BaseService {
    public async excelToSTEA(project: Project) {
        const postExcelResponse: any = await this.postExcel(project.id, "blob", { headers: { accept: "text/plain" } })
        FileSaver.saveAs(postExcelResponse, (`${project.name}.xlsx`))
    }
}

export async function GetSTEAService() {
    return new __STEAService({
        ...config.STEAService,
        accessToken: await GetToken(LoginAccessTokenKey)!,
    })
}
