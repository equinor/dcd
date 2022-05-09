import { IAsset } from "../models/assets/IAsset"
import { Project } from "../models/Project"

export interface IAssetService {
    create: (caseId: string, asset: IAsset) => Promise<Project>
    update: (asset: IAsset) => Promise<Project>
}
