import styled from "styled-components"
import React from "react"
import {
    Typography,
} from "@equinor/eds-core-react"
import LinearDataTable from "../../Components/LinearDataTable"

interface Props {
    capexYearX: number[]
    capexYearY: number[][]
    caseTitles: string[]
}

const Wrapper = styled.div`
    display: flex;
    flex-direction: column;
`

const ChartsContainer = styled.div`
    display: flex;
`

const ProjectCompareCasesTab = ({
    capexYearX,
    capexYearY,
    caseTitles,
}: Props) => (
    <Wrapper>
        <ChartsContainer>
            {capexYearX.length !== 0
                ? (
                    <LinearDataTable
                        capexYearX={capexYearX}
                        capexYearY={capexYearY}
                        caseTitles={caseTitles}
                    />
                )
                : <Typography> No cases containing CapEx to display data for</Typography> }
        </ChartsContainer>
    </Wrapper>

)

export default ProjectCompareCasesTab
