/* eslint-disable max-len */
import { Table } from "@equinor/eds-core-react"
import {
    Dispatch, SetStateAction, useEffect, useState,
} from "react"
import styled from "styled-components"
import { DevelopmentOperationalWellCosts } from "../../models/DevelopmentOperationalWellCosts"
import { ExplorationOperationalWellCosts } from "../../models/ExplorationOperationalWellCosts"
import OperationalWellCost from "./OperationalWellCost"

const TableWrapper = styled(Table)`
    width: 100%;
`

interface Props {
    title: string
    developmentOperationalWellCosts?: DevelopmentOperationalWellCosts
    setDevelopmentOperationalWellCosts?: Dispatch<SetStateAction<DevelopmentOperationalWellCosts | undefined>>

    explorationOperationalWellCosts?: ExplorationOperationalWellCosts
    setExplorationOperationalWellCosts?: Dispatch<SetStateAction<ExplorationOperationalWellCosts | undefined>>

}

function OperationalWellCosts({
    title,
    developmentOperationalWellCosts,
    explorationOperationalWellCosts,
    setDevelopmentOperationalWellCosts,
    setExplorationOperationalWellCosts,
}: Props) {
    const [developmentRigUpgrading, setDevelopmentRigUpgrading] = useState<number | undefined>(developmentOperationalWellCosts?.rigUpgrading)
    const [developmentRigMobDemob, setDevelopmentRigMobDemob] = useState<number | undefined>(developmentOperationalWellCosts?.rigMobDemob)
    const [developmentAnnualWellInterventionCost, setDevelopmentAnnualWellInterventionCost] = useState<number | undefined>(developmentOperationalWellCosts?.annualWellInterventionCostPerWell)
    const [developmentPluggingAndAbandonment, setDevelopmentPluggingAndAbandonment] = useState<number | undefined>(developmentOperationalWellCosts?.pluggingAndAbandonment)

    const [explorationRigUpgrading, setExplorationRigUpgrading] = useState<number | undefined>(explorationOperationalWellCosts?.rigUpgrading)
    const [explorationRigMobDemob, setExplorationRigMobDemob] = useState<number | undefined>(explorationOperationalWellCosts?.explorationRigMobDemob)
    const [explorationProjectDrillingCosts, setExplorationProjectDrillingCosts] = useState<number | undefined>(explorationOperationalWellCosts?.explorationProjectDrillingCosts)

    const [appraisalRigMobDemob, setAppraisalRigMobDemob] = useState<number | undefined>(explorationOperationalWellCosts?.appraisalRigMobDemob)
    const [appraisalProjectDrillingCosts, setAppraisalProjectDrillingCosts] = useState<number | undefined>(explorationOperationalWellCosts?.appraisalProjectDrillingCosts)

    useEffect(() => {
        if (developmentOperationalWellCosts && setDevelopmentOperationalWellCosts) {
            const newDevelopmentOperationalWellCosts: DevelopmentOperationalWellCosts = { ...developmentOperationalWellCosts }
            newDevelopmentOperationalWellCosts.rigUpgrading = developmentRigUpgrading
            newDevelopmentOperationalWellCosts.rigMobDemob = developmentRigMobDemob
            newDevelopmentOperationalWellCosts.annualWellInterventionCostPerWell = developmentAnnualWellInterventionCost
            newDevelopmentOperationalWellCosts.pluggingAndAbandonment = developmentPluggingAndAbandonment
            setDevelopmentOperationalWellCosts(newDevelopmentOperationalWellCosts)
        }
    }, [developmentRigUpgrading, developmentRigMobDemob,
        developmentAnnualWellInterventionCost, developmentPluggingAndAbandonment])

    useEffect(() => {
        if (explorationOperationalWellCosts && setExplorationOperationalWellCosts) {
            const newExplorationOperationalWellCosts: ExplorationOperationalWellCosts = { ...explorationOperationalWellCosts }
            newExplorationOperationalWellCosts.rigUpgrading = explorationRigUpgrading
            newExplorationOperationalWellCosts.explorationRigMobDemob = explorationRigMobDemob
            newExplorationOperationalWellCosts.explorationProjectDrillingCosts = explorationProjectDrillingCosts
            newExplorationOperationalWellCosts.appraisalRigMobDemob = appraisalRigMobDemob
            newExplorationOperationalWellCosts.appraisalProjectDrillingCosts = appraisalProjectDrillingCosts
            setExplorationOperationalWellCosts(newExplorationOperationalWellCosts)
        }
    }, [explorationRigUpgrading, explorationRigMobDemob,
        explorationProjectDrillingCosts, appraisalRigMobDemob, appraisalProjectDrillingCosts])

    return (
        <TableWrapper>
            <Table.Head>
                <Table.Row>
                    <Table.Cell>
                        {title}
                    </Table.Cell>
                    <Table.Cell>
                        Cost (MUSD)
                    </Table.Cell>
                </Table.Row>
            </Table.Head>
            <Table.Body>
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

            </Table.Body>
        </TableWrapper>
    )
}

export default OperationalWellCosts
