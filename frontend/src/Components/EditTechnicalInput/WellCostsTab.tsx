import { Dispatch, SetStateAction } from "react"
import styled from "styled-components"
import { DevelopmentOperationalWellCosts } from "../../models/DevelopmentOperationalWellCosts"
import { ExplorationOperationalWellCosts } from "../../models/ExplorationOperationalWellCosts"
import { Project } from "../../models/Project"
import { Well } from "../../models/Well"
import OperationalWellCosts from "./OperationalWellCosts"
import WellListEditTechnicalInput from "./WellListEditTechnicalInput"

const TopWrapper = styled.div`
    display: flex;
    flex-direction: row;
    justify-content: space-between;
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

    wellProjectWells: Well[] | undefined
    setWellProjectWells: Dispatch<SetStateAction<Well[] | undefined>>

    explorationWells: Well[] | undefined
    setExplorationWells: Dispatch<SetStateAction<Well[] | undefined>>
}

const WellCostsTab = ({
    project,
    developmentOperationalWellCosts, setDevelopmentOperationalWellCosts,
    explorationOperationalWellCosts, setExplorationOperationalWellCosts,
    wellProjectWells, setWellProjectWells,
    explorationWells, setExplorationWells,
}: Props) => (
    <>
        <TopWrapper />
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
                    developmentOperationalWellCosts={developmentOperationalWellCosts}
                    setDevelopmentOperationalWellCosts={setDevelopmentOperationalWellCosts}
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
                    explorationOperationalWellCosts={explorationOperationalWellCosts}
                    setExplorationOperationalWellCosts={setExplorationOperationalWellCosts}
                />
            </ColumnWrapper>
        </RowWrapper>
    </>
)

export default WellCostsTab
