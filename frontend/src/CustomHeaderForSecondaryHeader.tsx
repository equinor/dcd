import { Typography } from "@equinor/eds-core-react"
import styled from "styled-components"

interface CustomHeaderProps {
    unit: string
    columnHeader: string
}

const UnitWrapper = styled.div`
    font-size: 10px;
    font-weight: normal;
    color: #6F6F6F;
`

const Wrapper = styled.div`
    flex-direction: column;
`

const CustomHeaderForSecondaryHeader = ({ unit, columnHeader }: CustomHeaderProps) => (
    <Wrapper>
        <Typography group="table" variant="cell_header">
            {columnHeader}
        </Typography>
        <UnitWrapper>
            {unit}
        </UnitWrapper>
    </Wrapper>
)

export default CustomHeaderForSecondaryHeader
