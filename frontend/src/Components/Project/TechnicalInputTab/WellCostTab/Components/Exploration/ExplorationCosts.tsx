import { useEffect, useState } from "react"

import useTechnicalInputEdits from "@/Hooks/useEditTechnicalInput"
import { useDataFetch } from "@/Hooks/useDataFetch"
import { useDebounce } from "@/Hooks/useDebounce"
import CostCell from "../Shared/CostCell"
import {
    FullwidthTable,
    Head,
    Body,
    Row,
    Cell,
    CostWithCurrency,
} from "../Shared/SharedWellStyles"

type ExplorationCostsState = Omit<
    Components.Schemas.ExplorationOperationalWellCostsOverviewDto,
    "explorationOperationalWellCostsId" | "projectId"
>

const ExplorationCosts = () => {
    const revisionAndProjectData = useDataFetch()
    const { explorationOperationalWellCostsId } = revisionAndProjectData?.commonProjectAndRevisionData.explorationOperationalWellCosts ?? {}
    const { projectId } = revisionAndProjectData ?? {}
    const { addExplorationWellCostEdit } = useTechnicalInputEdits()

    const [costs, setCosts] = useState<ExplorationCostsState>({
        explorationRigUpgrading: 0,
        explorationRigMobDemob: 0,
        explorationProjectDrillingCosts: 0,
        appraisalRigMobDemob: 0,
        appraisalProjectDrillingCosts: 0,
    })

    const debouncedCosts = useDebounce(costs, 1000)

    useEffect(() => {
        if (revisionAndProjectData?.commonProjectAndRevisionData.explorationOperationalWellCosts) {
            setCosts({
                explorationRigUpgrading: revisionAndProjectData.commonProjectAndRevisionData.explorationOperationalWellCosts.explorationRigUpgrading ?? 0,
                explorationRigMobDemob: revisionAndProjectData.commonProjectAndRevisionData.explorationOperationalWellCosts.explorationRigMobDemob ?? 0,
                explorationProjectDrillingCosts: revisionAndProjectData.commonProjectAndRevisionData.explorationOperationalWellCosts.explorationProjectDrillingCosts ?? 0,
                appraisalRigMobDemob: revisionAndProjectData.commonProjectAndRevisionData.explorationOperationalWellCosts.appraisalRigMobDemob ?? 0,
                appraisalProjectDrillingCosts: revisionAndProjectData.commonProjectAndRevisionData.explorationOperationalWellCosts.appraisalProjectDrillingCosts ?? 0,
            } as ExplorationCostsState)
        }
    }, [revisionAndProjectData])

    useEffect(() => {
        if (explorationOperationalWellCostsId && projectId) {
            addExplorationWellCostEdit(projectId, explorationOperationalWellCostsId, debouncedCosts)
        }
    }, [debouncedCosts])

    return (
        <FullwidthTable>
            <Head>
                <Row>
                    <Cell>
                        Exploration costs
                    </Cell>
                    <Cell>
                        <CostWithCurrency>
                            Cost
                            <div>
                                {`${revisionAndProjectData?.commonProjectAndRevisionData.currency === 1 ? "(mill NOK)" : "(mill USD)"}`}
                            </div>
                        </CostWithCurrency>
                    </Cell>
                </Row>
            </Head>
            <Body>
                <CostCell
                    title="Rig upgrading - exploration"
                    setValue={(value: number) => setCosts((prev) => ({ ...prev, explorationRigUpgrading: value ?? 0 }))}
                    value={costs.explorationRigUpgrading ?? 0}
                />
                <CostCell
                    title="Rig mob/demob - exploration"
                    setValue={(value: number) => setCosts((prev) => ({ ...prev, explorationRigMobDemob: value ?? 0 }))}
                    value={costs.explorationRigMobDemob ?? 0}
                />
                <CostCell
                    title="Project spesific drilling costs - exploration"
                    setValue={(value: number) => setCosts((prev) => ({ ...prev, explorationProjectDrillingCosts: value ?? 0 }))}
                    value={costs.explorationProjectDrillingCosts ?? 0}
                />
                <CostCell
                    title="Rig mob/demob - appraisal"
                    setValue={(value: number) => setCosts((prev) => ({ ...prev, appraisalRigMobDemob: value ?? 0 }))}
                    value={costs.appraisalRigMobDemob ?? 0}
                />
                <CostCell
                    title="Project spesific drilling costs - appraisal"
                    setValue={(value: number) => setCosts((prev) => ({ ...prev, appraisalProjectDrillingCosts: value ?? 0 }))}
                    value={costs.appraisalProjectDrillingCosts ?? 0}
                />
            </Body>
        </FullwidthTable>
    )
}

export default ExplorationCosts
