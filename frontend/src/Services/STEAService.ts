import FileSaver from "file-saver"
import { __BaseService } from "./__BaseService"

import { config } from "./config"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"

class STEAService extends __BaseService {
    public async excelToSTEA(project: Components.Schemas.ProjectDto) {
        const postExcelResponse: any = await this.postExcel(project.id, "blob", { headers: { accept: "text/plain" } })
        FileSaver.saveAs(postExcelResponse, (`${project.name}.xlsx`))
    }
}

export const GetSTEAService = async () => {
    return new STEAService({
        ...config.STEAService,
        accessToken: await GetToken(LoginAccessTokenKey)!,
    })
}
