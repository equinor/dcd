import { Typography } from "@material-ui/core"
import React, { Dispatch, SetStateAction } from "react"
import styled from "styled-components"
import { DevelopmentOperationalWellCosts } from "../../models/DevelopmentOperationalWellCosts"
import { ExplorationOperationalWellCosts } from "../../models/ExplorationOperationalWellCosts"
import { Project } from "../../models/Project"
import { Well } from "../../models/Well"
// import DevelopmentOperationalWellCost from "./DevelopmentOperationalWellCost"
// import DevelopmentOperationalWellCosts from "./DevelopmentOperationalWellCosts"
import OperationalWellCosts from "./OperationalWellCosts"
import WellListEditTechnicalInput from "./WellListEditTechnicalInput"
import WellListEditTechnicalInputNew from "./WellListEditTechnicalInputNew"

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
    display: flex;
    flex-direction: column;
`

const WellListWrapper = styled.div`
    margin-bottom: 20px;
    width: 500px;
`

interface Props {
    setProject: Dispatch<SetStateAction<Project | undefined>>
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
    setProject,
    developmentOperationalWellCosts,
    setDevelopmentOperationalWellCosts,
    explorationOperationalWellCosts,
    setExplorationOperationalWellCosts,
    wellProjectWells, setWellProjectWells,
    explorationWells, setExplorationWells,
}: Props) => (

    <div>
        <TopWrapper color="danger" />
        <RowWrapper>
            <ColumnWrapper>
                <WellListWrapper>
                    <WellListEditTechnicalInput
                        project={project}
                        setProject={setProject}
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
                    <WellListEditTechnicalInputNew
                        project={project}
                        setProject={setProject}
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
        {/* <DevelopmentOperationalWellCosts project={project} setProject={setProject} title="Exploration costs" /> */}

    </div>

)

export default WellCostsTab
