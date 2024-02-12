import { Typography } from "@material-ui/core"
import { Dispatch, SetStateAction } from "react"
import styled from "styled-components"
import CO2ListTechnicalInput from "./CO2ListTechnicalInput"

const TopWrapper = styled.div`
    display: flex;
    flex-direction: row;
    justify-content: space-between;
`
interface Props {
    project: Components.Schemas.ProjectDto
    setProject: Dispatch<SetStateAction<Components.Schemas.ProjectDto | undefined>>
}

const CO2Tab = ({
    project, setProject,
}: Props) => (
    <>
        <TopWrapper color="danger">
            <Typography variant="h4">CO2 Emission</Typography>
        </TopWrapper>
        <TopWrapper>
            <Typography>
                You can override these default assumption to customize the calculation made in CO2 emissions.
            </Typography>
        </TopWrapper>
        <CO2ListTechnicalInput project={project} setProject={setProject} />
    </>
)

export default CO2Tab
