import { Project } from "../models/Project"
import { GetDrainageStrategyService } from "../Services/DrainageStrategyService"
import { GetExplorationService } from "../Services/ExplorationService"
import { GetSubstructureService } from "../Services/SubstructureService"
import { GetSurfService } from "../Services/SurfService"
import { GetTopsideService } from "../Services/TopsideService"
import { GetTransportService } from "../Services/TransportService"
import { GetWellProjectService } from "../Services/WellProjectService"

export const CreateAsset = async (assetType: string, caseId: string, projectId: string) => {
    enum AssetType {
        DrainageStrategy = "DrainageStrategy",
        Exploration = "Exploration",
        WellProject = "WellProject",
        SURF = "SURF",
        Topside = "Topside",
        Substructure = "Substructure",
        Transport = "Transport"
    }

    let newProject
    let assetId
    if (assetType === AssetType.DrainageStrategy) {
        newProject = await GetDrainageStrategyService().createDrainageStrategy(
            caseId,
            { name: "New drainage strategy", description: "", projectId },
        )
        assetId = newProject?.cases.find((o) => o.id === caseId)?.drainageStrategyLink
    } else if (assetType === AssetType.Exploration) {
        newProject = await GetExplorationService().createExploration(
            caseId,
            { name: "New exploration", projectId },
        )
        assetId = newProject?.cases.find((o) => o.id === caseId)?.explorationLink
    } else if (assetType === AssetType.WellProject) {
        newProject = await GetWellProjectService().createWellProject(
            caseId,
            { name: "New well project", projectId },
        )
        assetId = newProject?.cases.find((o) => o.id === caseId)?.wellProjectLink
    } else if (assetType === AssetType.SURF) {
        newProject = await GetSurfService().createSurf(
            caseId,
            {
                name: "New SURF",
                projectId,
                costProfile: {
                    values: [0.0], // CostProfile needs a value upon creation
                },
            },
        )
        assetId = newProject?.cases.find((o) => o.id === caseId)?.surfLink
    } else if (assetType === AssetType.Topside) {
        newProject = await GetTopsideService().createTopside(
            caseId,
            {
                name: "New topside",
                projectId,
                costProfile: {
                    values: [0.0],
                },
            },
        )
        assetId = newProject?.cases.find((o) => o.id === caseId)?.topsideLink
    } else if (assetType === AssetType.Substructure) {
        newProject = await GetSubstructureService().createSubstructure(
            caseId,
            {
                name: "New substructure",
                projectId,
                costProfile: {
                    values: [0.0],
                },
            },
        )
        assetId = newProject?.cases.find((o) => o.id === caseId)?.substructureLink
    } else if (assetType === AssetType.Transport) {
        newProject = await GetTransportService().createTransport(
            caseId,
            {
                name: "New transport",
                projectId,
                costProfile: {
                    values: [0.0],
                },
            },
        )
        assetId = newProject?.cases.find((o) => o.id === caseId)?.transportLink
    }
    const result: [Project | undefined, string | undefined] = [newProject, assetId]
    return result
}
