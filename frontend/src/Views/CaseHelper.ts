import { Case } from "../models/Case"
import { EMPTY_GUID } from "../Utils/constants"

export const isDisabled = (
    property: string,
    caseItem?: Case,
): boolean => {
    if (caseItem && caseItem.drainageStrategyLink !== EMPTY_GUID) {
        if (["producerCount", "gasInjectorCount", "waterInjectorCount"].includes(property)) {
            return true
        }
    }
    if (caseItem && caseItem.explorationLink !== EMPTY_GUID) {
        if (["rigMobDemob"].includes(property)) {
            return true
        }
    }
    if (caseItem && caseItem.wellProjectLink !== EMPTY_GUID) {
        if (["producerCount", "gasInjectorCount", "waterInjectorCount", "rigMobDemob"].includes(property)) {
            return true
        }
    }
    if (caseItem && caseItem.surfLink !== EMPTY_GUID) {
        if (["riserCount", "templateCount"].includes(property)) {
            return true
        }
    }
    if (caseItem && caseItem.topsideLink !== EMPTY_GUID) {
        if ([""].includes(property)) {
            return true
        }
    }
    if (caseItem && caseItem.substructureLink !== EMPTY_GUID) {
        if ([""].includes(property)) {
            return true
        }
    }
    if (caseItem && caseItem.transportLink !== EMPTY_GUID) {
        if ([""].includes(property)) {
            return true
        }
    }
    return false
}
