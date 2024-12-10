import { Table } from "@equinor/eds-core-react"
import {
    Dispatch,
    SetStateAction,
    useEffect,
    useState,
} from "react"
import styled from "styled-components"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useQuery } from "@tanstack/react-query"
import OperationalWellCost from "./OperationalWellCost"
import { projectQueryFn } from "../../Services/QueryFunctions"

const {
    Head, Body, Row, Cell,
} = Table

const FullwidthTable = styled(Table)`
    width: 100%;
`

const CostWithCurrency = styled.div`
    display: flex;
    align-items: baseline;
    gap: 5px;

    div {
        font-weight: normal;
        font-size: 10px;
        margin-top: -4px;
        color: #6F6F6F;
        letter-spacing: 0.5px;
    }
`

interface Props {
    title: string
    developmentOperationalWellCosts?: Components.Schemas.DevelopmentOperationalWellCostsOverviewDto
    setDevelopmentOperationalWellCosts?: Dispatch<SetStateAction<Components.Schemas.DevelopmentOperationalWellCostsOverviewDto | undefined>>

    explorationOperationalWellCosts?: Components.Schemas.ExplorationOperationalWellCostsOverviewDto
    setExplorationOperationalWellCosts?: Dispatch<SetStateAction<Components.Schemas.ExplorationOperationalWellCostsOverviewDto | undefined>>
}

const OperationalWellCosts = ({
    title,
    developmentOperationalWellCosts,
    explorationOperationalWellCosts,
    setDevelopmentOperationalWellCosts,
    setExplorationOperationalWellCosts,
}: Props) => {
    const { currentContext } = useModuleCurrentContext()
    const externalId = currentContext?.externalId

    const [developmentRigUpgrading, setDevelopmentRigUpgrading] = useState<number | undefined>(developmentOperationalWellCosts?.rigUpgrading)
    const [developmentRigMobDemob, setDevelopmentRigMobDemob] = useState<number | undefined>(developmentOperationalWellCosts?.rigMobDemob)
    const [developmentAnnualWellInterventionCost, setDevelopmentAnnualWellInterventionCost] = useState<number | undefined>(developmentOperationalWellCosts?.annualWellInterventionCostPerWell)
    const [developmentPluggingAndAbandonment, setDevelopmentPluggingAndAbandonment] = useState<number | undefined>(developmentOperationalWellCosts?.pluggingAndAbandonment)
    const [explorationRigUpgrading, setExplorationRigUpgrading] = useState<number | undefined>(explorationOperationalWellCosts?.explorationRigUpgrading)
    const [explorationRigMobDemob, setExplorationRigMobDemob] = useState<number | undefined>(explorationOperationalWellCosts?.explorationRigMobDemob)
    const [explorationProjectDrillingCosts, setExplorationProjectDrillingCosts] = useState<number | undefined>(explorationOperationalWellCosts?.explorationProjectDrillingCosts)
    const [appraisalRigMobDemob, setAppraisalRigMobDemob] = useState<number | undefined>(explorationOperationalWellCosts?.appraisalRigMobDemob)
    const [appraisalProjectDrillingCosts, setAppraisalProjectDrillingCosts] = useState<number | undefined>(explorationOperationalWellCosts?.appraisalProjectDrillingCosts)

    const { data: apiData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })

    useEffect(() => {
        if (developmentRigUpgrading === undefined
            || developmentRigMobDemob === undefined
            || developmentAnnualWellInterventionCost === undefined
            || developmentPluggingAndAbandonment === undefined) {
            return
        }
        if (developmentOperationalWellCosts && setDevelopmentOperationalWellCosts) {
            const newDevelopmentOperationalWellCosts: Components.Schemas.DevelopmentOperationalWellCostsOverviewDto = { ...developmentOperationalWellCosts }
            newDevelopmentOperationalWellCosts.rigUpgrading = developmentRigUpgrading
            newDevelopmentOperationalWellCosts.rigMobDemob = developmentRigMobDemob
            newDevelopmentOperationalWellCosts.annualWellInterventionCostPerWell = developmentAnnualWellInterventionCost
            newDevelopmentOperationalWellCosts.pluggingAndAbandonment = developmentPluggingAndAbandonment
            setDevelopmentOperationalWellCosts(newDevelopmentOperationalWellCosts)
        }
    }, [developmentRigUpgrading, developmentRigMobDemob,
        developmentAnnualWellInterventionCost, developmentPluggingAndAbandonment])

    useEffect(() => {
        if (explorationRigUpgrading === undefined
            || explorationRigMobDemob === undefined
            || explorationProjectDrillingCosts === undefined
            || appraisalRigMobDemob === undefined
            || appraisalProjectDrillingCosts === undefined) {
            return
        }
        if (explorationOperationalWellCosts && setExplorationOperationalWellCosts) {
            const newExplorationOperationalWellCosts: Components.Schemas.ExplorationOperationalWellCostsOverviewDto = { ...explorationOperationalWellCosts }
            newExplorationOperationalWellCosts.explorationRigUpgrading = explorationRigUpgrading
            newExplorationOperationalWellCosts.explorationRigMobDemob = explorationRigMobDemob
            newExplorationOperationalWellCosts.explorationProjectDrillingCosts = explorationProjectDrillingCosts
            newExplorationOperationalWellCosts.appraisalRigMobDemob = appraisalRigMobDemob
            newExplorationOperationalWellCosts.appraisalProjectDrillingCosts = appraisalProjectDrillingCosts
            setExplorationOperationalWellCosts(newExplorationOperationalWellCosts)
        }
    }, [explorationRigUpgrading, explorationRigMobDemob,
        explorationProjectDrillingCosts, appraisalRigMobDemob, appraisalProjectDrillingCosts])

    return (
        <FullwidthTable>
            <Head>
                <Row>
                    <Cell>
                        {title}
                    </Cell>
                    <Cell>
                        <CostWithCurrency>
                            Cost
                            <div>
                                {`${apiData?.commonProjectAndRevisionData.currency === 1 ? "(mill NOK)" : "(mill USD)"}`}
                            </div>
                        </CostWithCurrency>
                    </Cell>
                </Row>
            </Head>
            <Body>
                {developmentOperationalWellCosts ? (
                    <>
                        <OperationalWellCost
                            title="Rig upgrading"
                            setValue={setDevelopmentRigUpgrading}
                            value={developmentRigUpgrading ?? 0}
                        />
                        <OperationalWellCost
                            title="Rig mob/demob"
                            setValue={setDevelopmentRigMobDemob}
                            value={developmentRigMobDemob ?? 0}
                        />
                        <OperationalWellCost
                            title="Annual well intervention cost per well"
                            setValue={setDevelopmentAnnualWellInterventionCost}
                            value={developmentAnnualWellInterventionCost ?? 0}
                        />
                        <OperationalWellCost
                            title="Plugging and abandonment"
                            setValue={setDevelopmentPluggingAndAbandonment}
                            value={developmentPluggingAndAbandonment ?? 0}
                        />
                    </>

                ) : (
                    <>
                        <OperationalWellCost
                            title="Rig upgrading - exploration"
                            setValue={setExplorationRigUpgrading}
                            value={explorationRigUpgrading ?? 0}
                        />
                        <OperationalWellCost
                            title="Rig mob/demob - exploration"
                            setValue={setExplorationRigMobDemob}
                            value={explorationRigMobDemob ?? 0}
                        />
                        <OperationalWellCost
                            title="Project spesific drilling costs - exploration"
                            setValue={setExplorationProjectDrillingCosts}
                            value={explorationProjectDrillingCosts ?? 0}
                        />
                        <OperationalWellCost
                            title="Rig mob/demob - appraisal"
                            setValue={setAppraisalRigMobDemob}
                            value={appraisalRigMobDemob ?? 0}
                        />
                        <OperationalWellCost
                            title="Project spesific drilling costs - appraisal"
                            setValue={setAppraisalProjectDrillingCosts}
                            value={appraisalProjectDrillingCosts ?? 0}
                        />
                    </>
                )}

            </Body>
        </FullwidthTable>
    )
}

export default OperationalWellCosts
