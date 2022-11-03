import { Typography } from "@material-ui/core"
import { Dispatch, SetStateAction } from "react"
import styled from "styled-components"
import { DevelopmentOperationalWellCosts } from "../../models/DevelopmentOperationalWellCosts"
import { ExplorationOperationalWellCosts } from "../../models/ExplorationOperationalWellCosts"
import { Project } from "../../models/Project"
import { Well } from "../../models/Well"
import CO2ListTechnicalInput from "./CO2ListTechnicalInput"
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
    setProject: Dispatch<SetStateAction<Project | undefined>>
}

const CO2Tab = ({
    project, setProject,
}: Props) => (
    <>
        <TopWrapper color="danger">
            <Typography variant="h4">CO2 Emission</Typography>
        </TopWrapper>
        <CO2ListTechnicalInput project={project} setProject={setProject} />
    </>
)

export default CO2Tab
