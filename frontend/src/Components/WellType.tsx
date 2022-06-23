import {
    NativeSelect,
    Typography,
} from "@equinor/eds-core-react"
import styled from "styled-components"
import { Wrapper } from "../Views/Asset/StyledAssetComponents"

const WrapperColumn = styled.div`
    display: flex;
    flex-direction: column;
    margin-bottom: 2rem;
`

const WellTypeDropdown = styled(NativeSelect)`
width: 20rem;
margin-top: -0.5rem;
margin-left: 1rem;
`

interface Props {
    wellType: Components.Schemas.WellType | Components.Schemas.ExplorationWellType | undefined
}

const WellType = ({
    wellType,
}: Props) => (
    <WrapperColumn>
        <Wrapper>
            <Typography variant="h5">
                Well type
            </Typography>
            <WellTypeDropdown
                label=""
                id="WellType"
                placeholder="Choose a well type"
                // onChange={(event: ChangeEvent<HTMLSelectElement>) => onChange(event)}
                // value={currentValue}
                disabled={false}
            >
                <option key="0" value={0}>Placeholder well type name</option>
            </WellTypeDropdown>
        </Wrapper>
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
