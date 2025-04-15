import { __BaseService } from "./__BaseService"

class TableRangesService extends __BaseService {
    public async updateTableRanges(
        projectId: string,
        caseId: string,
        tableRanges: Components.Schemas.UpdateTableRangesDto,
    ): Promise<void> {
        console.log("TableRangesService - updateTableRanges called", { projectId, caseId, tableRanges })
        try {
            await this.put(`projects/${projectId}/cases/${caseId}/table-ranges`, { body: tableRanges })
            console.log("TableRangesService - updateTableRanges completed successfully")
        } catch (error) {
            console.error("TableRangesService - updateTableRanges error", error)
            throw error
        }
    }
}

export const GetTableRangesService = () => new TableRangesService()

export default GetTableRangesService
