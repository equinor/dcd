import FileSaver from "file-saver"
import { __BaseService } from "./__BaseService"

import { loginAccessTokenKey, getToken } from "../Utils/common"

class STEAService extends __BaseService {
    public async excelToSTEA(project: Components.Schemas.ProjectDataDto) {
        const postExcelResponse: any = await this.postExcel(`stea/${project.projectId}`, "blob", { headers: { accept: "text/plain" } })
        FileSaver.saveAs(postExcelResponse, (`${project.commonProjectAndRevisionData.name}.xlsx`))
    }
}

export const GetSTEAService = async () => new STEAService({
    accessToken: await getToken(loginAccessTokenKey)!,
})
