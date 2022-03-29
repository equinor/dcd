import { __BaseService } from "./__BaseService"

// import { Project } from "../models/Project"
import { config } from "./config"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"

class __STEAService extends __BaseService {
    public async excelToSTEA(projectId: string) {
         // const postResponse = await this.post(projectId, { headers: { 'accept' : "text/plain"} })
        const postExcelResponse = await this.postExcel(projectId, "blob", { headers: { 'accept' : "text/plain"} } )
        
        const asd = this.post(postExcelResponse)
        console.log("Manni")
        console.log(asd)
        return asd
    }
}

export function GetSTEAService() {
    return new __STEAService({
        ...config.STEAService,
        accessToken: GetToken(LoginAccessTokenKey)!,
    })
}