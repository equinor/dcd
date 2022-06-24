import styled from "styled-components"
import React from "react"
import LinearDataTable from "../Components/LinearDataTable"

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

const CompareCasesView = ({
    capexYearX,
    capexYearY,
    caseTitles,
}: Props) => (
    <Wrapper>
        <ChartsContainer>
            <LinearDataTable
                capexYearX={capexYearX}
                capexYearY={capexYearY}
                caseTitles={caseTitles}
            />
        </ChartsContainer>
    </Wrapper>
)

export default CompareCasesView
