import { useEffect, useState, useRef } from "react"

import useTechnicalInputEdits from "@/Hooks/useEditTechnicalInput"
import { useDataFetch } from "@/Hooks"
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
import { Currency } from "@/Models/enums"
import { useAppStore } from "@/Store/AppStore"
import useCanUserEdit from "@/Hooks/useCanUserEdit"

type ExplorationCostsState = Omit<
    Components.Schemas.ExplorationOperationalWellCostsOverviewDto,
    "explorationOperationalWellCostsId" | "projectId"
>

const ExplorationCosts = () => {
    const revisionAndProjectData = useDataFetch()
    const { explorationOperationalWellCostsId } = revisionAndProjectData?.commonProjectAndRevisionData.explorationOperationalWellCosts ?? {}
    const { explorationOperationalWellCosts } = revisionAndProjectData?.commonProjectAndRevisionData ?? {}
    const { currency, fusionProjectId } = revisionAndProjectData?.commonProjectAndRevisionData ?? {}
    const { projectId } = revisionAndProjectData ?? {}
    const { editMode } = useAppStore()
    const { addExplorationWellCostEdit } = useTechnicalInputEdits()
    const { canEdit, isEditDisabled } = useCanUserEdit()

    const [costs, setCosts] = useState<ExplorationCostsState>({
        explorationRigUpgrading: 0,
        explorationRigMobDemob: 0,
        explorationProjectDrillingCosts: 0,
        appraisalRigMobDemob: 0,
        appraisalProjectDrillingCosts: 0,
    })

    const debouncedCosts = useDebounce(costs, 1000)

    const previousCostsRef = useRef<ExplorationCostsState | null>(null)

    useEffect(() => {
        if (explorationOperationalWellCosts) {
            const newCosts = {
                explorationRigUpgrading: explorationOperationalWellCosts.explorationRigUpgrading ?? 0,
                explorationRigMobDemob: explorationOperationalWellCosts.explorationRigMobDemob ?? 0,
                explorationProjectDrillingCosts: explorationOperationalWellCosts.explorationProjectDrillingCosts ?? 0,
                appraisalRigMobDemob: explorationOperationalWellCosts.appraisalRigMobDemob ?? 0,
                appraisalProjectDrillingCosts: explorationOperationalWellCosts.appraisalProjectDrillingCosts ?? 0,
            } as ExplorationCostsState
            setCosts(newCosts)
            previousCostsRef.current = newCosts
        }
    }, [revisionAndProjectData])

    useEffect(() => {
        if (explorationOperationalWellCostsId && projectId && fusionProjectId && debouncedCosts && canEdit()) {
            const hasChanges = !previousCostsRef.current || Object.entries(debouncedCosts).some(
                ([key, value]) => previousCostsRef.current?.[key as keyof ExplorationCostsState] !== value,
            )
            if (hasChanges) {
                previousCostsRef.current = { ...debouncedCosts }
                addExplorationWellCostEdit(projectId, fusionProjectId, explorationOperationalWellCostsId, debouncedCosts)
            }
        }
    }, [debouncedCosts, explorationOperationalWellCostsId, projectId, addExplorationWellCostEdit, editMode, isEditDisabled])

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
                                {`${currency === Currency.NOK ? "(mill NOK)" : "(mill USD)"}`}
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
