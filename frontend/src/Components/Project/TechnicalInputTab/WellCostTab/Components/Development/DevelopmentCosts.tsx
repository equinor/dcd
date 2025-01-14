import {
    Dispatch,
    SetStateAction,
    useEffect,
    useState,
} from "react"

import { useDataFetch } from "@/Hooks/useDataFetch"
import OperationalWellCost from "../Shared/CostCell"
import {
    FullwidthTable,
    Head,
    Body,
    Row,
    Cell,
    CostWithCurrency,
} from "../Shared/SharedWellStyles"

interface Props {
    setDevelopmentOperationalWellCosts: Dispatch<SetStateAction<Components.Schemas.DevelopmentOperationalWellCostsOverviewDto | undefined>>
}

const DevelopmentCosts = ({
    setDevelopmentOperationalWellCosts,
}: Props) => {
    const revisionAndProjectData = useDataFetch()

    const [costs, setCosts] = useState<Components.Schemas.DevelopmentOperationalWellCostsOverviewDto>({
        rigUpgrading: 0,
        rigMobDemob: 0,
        annualWellInterventionCostPerWell: 0,
        pluggingAndAbandonment: 0,
    })

    useEffect(() => {
        if (revisionAndProjectData?.commonProjectAndRevisionData.developmentOperationalWellCosts) {
            setCosts({
                rigUpgrading: revisionAndProjectData.commonProjectAndRevisionData.developmentOperationalWellCosts.rigUpgrading ?? 0,
                rigMobDemob: revisionAndProjectData.commonProjectAndRevisionData.developmentOperationalWellCosts.rigMobDemob ?? 0,
                annualWellInterventionCostPerWell: revisionAndProjectData.commonProjectAndRevisionData.developmentOperationalWellCosts.annualWellInterventionCostPerWell ?? 0,
                pluggingAndAbandonment: revisionAndProjectData.commonProjectAndRevisionData.developmentOperationalWellCosts.pluggingAndAbandonment ?? 0,
            })
        }
    }, [revisionAndProjectData])

    useEffect(() => {
        setDevelopmentOperationalWellCosts(costs)
    }, [costs])

    return (
        <FullwidthTable>
            <Head>
                <Row>
                    <Cell>
                        Development costs
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
                    title="Rig upgrading"
                    setValue={(value: number | undefined) => setCosts((prev) => ({ ...prev, rigUpgrading: value ?? 0 }))}
                    value={costs.rigUpgrading ?? 0}
                />
                <OperationalWellCost
                    title="Rig mob/demob"
                    setValue={(value: number | undefined) => setCosts((prev) => ({ ...prev, rigMobDemob: value ?? 0 }))}
                    value={costs.rigMobDemob ?? 0}
                />
                <OperationalWellCost
                    title="Annual well intervention cost per well"
                    setValue={(value: number | undefined) => setCosts((prev) => ({ ...prev, annualWellInterventionCostPerWell: value ?? 0 }))}
                    value={costs.annualWellInterventionCostPerWell ?? 0}
                />
                <OperationalWellCost
                    title="Plugging and abandonment"
                    setValue={(value: number | undefined) => setCosts((prev) => ({ ...prev, pluggingAndAbandonment: value ?? 0 }))}
                    value={costs.pluggingAndAbandonment ?? 0}
                />
            </Body>
        </FullwidthTable>
    )
}

export default DevelopmentCosts
