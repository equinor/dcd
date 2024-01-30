import { Dispatch, SetStateAction } from "react"
import styled from "styled-components"
import { DevelopmentOperationalWellCosts } from "../../models/DevelopmentOperationalWellCosts"
import { ExplorationOperationalWellCosts } from "../../models/ExplorationOperationalWellCosts"
import { Project } from "../../models/Project"
import { Well } from "../../models/Well"
import OperationalWellCosts from "./OperationalWellCosts"
import WellListEditTechnicalInput from "./WellListEditTechnicalInput"
import { Typography } from "@equinor/eds-core-react"

const TopWrapper = styled.div`
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    margin-bottom: 12px;
    margin-top: 12px;
`

const RowWrapper = styled.div`
    display: flex;
    flex-direction: row;
`

const ColumnWrapper = styled.div`
    width: 50%;
    display: flex;
    flex-direction: column;
`

const WellListWrapper = styled.div`
    margin-bottom: 20px;
    margin-right: 50px;
`

interface Props {
    project: Project
    developmentOperationalWellCosts: DevelopmentOperationalWellCosts
    setDevelopmentOperationalWellCosts: Dispatch<SetStateAction<DevelopmentOperationalWellCosts | undefined>>

    explorationOperationalWellCosts: ExplorationOperationalWellCosts
    setExplorationOperationalWellCosts: Dispatch<SetStateAction<ExplorationOperationalWellCosts | undefined>>

    wellProjectWells: Well[]
    setWellProjectWells: Dispatch<SetStateAction<Well[]>>

    explorationWells: Well[]
    setExplorationWells: Dispatch<SetStateAction<Well[]>>
}

const WellCostsTab = ({
    project,
    developmentOperationalWellCosts, setDevelopmentOperationalWellCosts,
    explorationOperationalWellCosts, setExplorationOperationalWellCosts,
    wellProjectWells, setWellProjectWells,
    explorationWells, setExplorationWells,
}: Props) => (
    <>
        <TopWrapper color="danger">
            <Typography variant="h1">Well Costs</Typography>
        </TopWrapper>
        <TopWrapper>
            <Typography variant="body_long">
                This input is used to calculate each case&apos;s well costs based on their drilling schedules.
            </Typography>

        </TopWrapper>
        <RowWrapper>
            <ColumnWrapper>
                <WellListWrapper>
                    <WellListEditTechnicalInput
                        project={project}
                        explorationWells
                        setWells={setExplorationWells}
                        wells={explorationWells}
                    />
                </WellListWrapper>
            </ColumnWrapper>
            <ColumnWrapper>
                <OperationalWellCosts
                    title="Exploration costs"
                    project={project}
                    explorationOperationalWellCosts={explorationOperationalWellCosts}
                    setExplorationOperationalWellCosts={setExplorationOperationalWellCosts}
                />
            </ColumnWrapper>
        </RowWrapper>
        <RowWrapper>
            <ColumnWrapper>
                <WellListWrapper>
                    <WellListEditTechnicalInput
                        project={project}
                        explorationWells={false}
                        setWells={setWellProjectWells}
                        wells={wellProjectWells}
                    />
                </WellListWrapper>
            </ColumnWrapper>
            <ColumnWrapper>
                <OperationalWellCosts
                    title="Development costs"
                    project={project}
                    developmentOperationalWellCosts={developmentOperationalWellCosts}
                    setDevelopmentOperationalWellCosts={setDevelopmentOperationalWellCosts}
                />
            </ColumnWrapper>
        </RowWrapper>
    </>
)

export default WellCostsTab
