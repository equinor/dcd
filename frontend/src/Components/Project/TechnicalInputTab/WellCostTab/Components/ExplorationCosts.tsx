import {
    Dispatch,
    SetStateAction,
    useEffect,
    useState,
} from "react"

import { useDataFetch } from "@/Hooks/useDataFetch"
import OperationalWellCost from "./Shared/CostCell"
import {
    FullwidthTable,
    Head,
    Body,
    Row,
    Cell,
    CostWithCurrency,
} from "./Shared/SharedWellStyles"

interface Props {
    setExplorationOperationalWellCosts: Dispatch<SetStateAction<Components.Schemas.ExplorationOperationalWellCostsOverviewDto | undefined>>
}

const ExplorationCosts = ({
    setExplorationOperationalWellCosts,
}: Props) => {
    const revisionAndProjectData = useDataFetch()

    const [costs, setCosts] = useState<Components.Schemas.ExplorationOperationalWellCostsOverviewDto>({
        explorationRigUpgrading: 0,
        explorationRigMobDemob: 0,
        explorationProjectDrillingCosts: 0,
        appraisalRigMobDemob: 0,
        appraisalProjectDrillingCosts: 0,
    })

    useEffect(() => {
        if (revisionAndProjectData?.commonProjectAndRevisionData.explorationOperationalWellCosts) {
            setCosts({
                explorationRigUpgrading: revisionAndProjectData.commonProjectAndRevisionData.explorationOperationalWellCosts.explorationRigUpgrading ?? 0,
                explorationRigMobDemob: revisionAndProjectData.commonProjectAndRevisionData.explorationOperationalWellCosts.explorationRigMobDemob ?? 0,
                explorationProjectDrillingCosts: revisionAndProjectData.commonProjectAndRevisionData.explorationOperationalWellCosts.explorationProjectDrillingCosts ?? 0,
                appraisalRigMobDemob: revisionAndProjectData.commonProjectAndRevisionData.explorationOperationalWellCosts.appraisalRigMobDemob ?? 0,
                appraisalProjectDrillingCosts: revisionAndProjectData.commonProjectAndRevisionData.explorationOperationalWellCosts.appraisalProjectDrillingCosts ?? 0,
            })
        }
    }, [revisionAndProjectData])

    useEffect(() => {
        setExplorationOperationalWellCosts(costs)
    }, [costs])

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
                <OperationalWellCost
                    title="Rig upgrading - exploration"
                    setValue={(value: number | undefined) => setCosts((prev) => ({ ...prev, explorationRigUpgrading: value ?? 0 }))}
                    value={costs.explorationRigUpgrading}
                />
                <OperationalWellCost
                    title="Rig mob/demob - exploration"
                    setValue={(value: number | undefined) => setCosts((prev) => ({ ...prev, explorationRigMobDemob: value ?? 0 }))}
                    value={costs.explorationRigMobDemob}
                />
                <OperationalWellCost
                    title="Project spesific drilling costs - exploration"
                    setValue={(value: number | undefined) => setCosts((prev) => ({ ...prev, explorationProjectDrillingCosts: value ?? 0 }))}
                    value={costs.explorationProjectDrillingCosts}
                />
                <OperationalWellCost
                    title="Rig mob/demob - appraisal"
                    setValue={(value: number | undefined) => setCosts((prev) => ({ ...prev, appraisalRigMobDemob: value ?? 0 }))}
                    value={costs.appraisalRigMobDemob}
                />
                <OperationalWellCost
                    title="Project spesific drilling costs - appraisal"
                    setValue={(value: number | undefined) => setCosts((prev) => ({ ...prev, appraisalProjectDrillingCosts: value ?? 0 }))}
                    value={costs.appraisalProjectDrillingCosts}
                />
            </Body>
        </FullwidthTable>
    )
}

export default ExplorationCosts
