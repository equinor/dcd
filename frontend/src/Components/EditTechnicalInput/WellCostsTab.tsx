import { Dispatch, SetStateAction } from "react"
import { Typography } from "@equinor/eds-core-react"
import styled from "styled-components"
import OperationalWellCosts from "./OperationalWellCosts"
import WellListEditTechnicalInput from "./WellListEditTechnicalInput"

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
    project: Components.Schemas.ProjectDto
    developmentOperationalWellCosts: Components.Schemas.DevelopmentOperationalWellCostsDto
    setDevelopmentOperationalWellCosts: Dispatch<SetStateAction<Components.Schemas.DevelopmentOperationalWellCostsDto>>

    explorationOperationalWellCosts: Components.Schemas.ExplorationOperationalWellCostsDto
    setExplorationOperationalWellCosts: Dispatch<SetStateAction<Components.Schemas.ExplorationOperationalWellCostsDto>>

    wellProjectWells: Components.Schemas.WellDto[]
    setWellProjectWells: Dispatch<SetStateAction<Components.Schemas.WellDto[]>>

    explorationWells: Components.Schemas.WellDto[]
    setExplorationWells: Dispatch<SetStateAction<Components.Schemas.WellDto[]>>
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
