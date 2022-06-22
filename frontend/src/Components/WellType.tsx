import {
    Typography,
} from "@equinor/eds-core-react"
import styled from "styled-components"

const WrapperColumn = styled.div`
    display: flex;
    flex-direction: column;
    margin-bottom: 2rem;
`

interface Props {
    wellType: Components.Schemas.WellTypeNew | Components.Schemas.ExplorationWellType | undefined
}

const WellType = ({
    wellType,
}: Props) => (
    <WrapperColumn>
        <Typography variant="h5">
            Well type
        </Typography>
        <Typography>
            Name:
            {wellType?.name}
        </Typography>
        <Typography>
            Description:
            {wellType?.description}
        </Typography>
        <Typography>
            Category:
            {wellType?.category}
        </Typography>
        <Typography>
            Drilling days:
            {wellType?.drillingDays}
        </Typography>
        <Typography>
            Well cost:
            {wellType?.wellCost}
        </Typography>
    </WrapperColumn>
)

export default WellType
