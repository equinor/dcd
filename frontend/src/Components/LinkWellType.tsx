/* eslint-disable max-len */
import {
    NativeSelect,
    Typography,
} from "@equinor/eds-core-react"
import {
    ChangeEvent, useEffect,
} from "react"
import styled from "styled-components"
import { Case } from "../models/Case"
import { EMPTY_GUID } from "../Utils/constants"
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
    caseItem: Case | undefined,
    linkWellType: (event: React.ChangeEvent<HTMLSelectElement>, link: any) => void,
    link: string,
    // currentValue: Components.Schemas.WellType | Components.Schemas.ExplorationWellType | undefined,
    values: JSX.Element[] | undefined
    currentValue: string | undefined,
}

const LinkWellType = ({
    caseItem,
    linkWellType,
    link,
    currentValue,
    values,
}: Props) => {
    const wellTypeCollection = () => {
        if (caseItem?.wells !== (null || undefined)) {
            const wellsCollection = caseItem?.wells
            const wellTypes = wellsCollection?.filter((obj) => obj.wellType === true)
            return wellTypes
        }
        return []
    }

    useEffect(() => {
        wellTypeCollection()
    })

    return (
        <WrapperColumn>
            <Wrapper>
                <Typography variant="h5">
                    Well type
                </Typography>
                <WellTypeDropdown
                    label=""
                    id="WellType"
                    placeholder="Choose a well type"
                    onChange={(event: ChangeEvent<HTMLSelectElement>) => linkWellType(event, link)}
                    // onChange={(event: ChangeEvent<HTMLSelectElement>) => onChange(event)}
                    value={currentValue}
                    disabled={false}
                >
                    {/* {wellTypeCollection} */}
                    {/* eslint-disable-next-line jsx-a11y/control-has-associated-label */}
                    {/* <option value={currentValue?.name ?? ""}>{currentValue?.name ?? ""}</option>
                    {wellTypeCollection().map((wellType) => (
                        <option value={wellType.id} key={wellType.id}>{wellType.name}</option>
                    ))} */}
                    {values}
                    <option key={EMPTY_GUID} value={EMPTY_GUID}> </option>
                </WellTypeDropdown>
                {/* <Typography>
                    Description:
                    {values?.description}
                </Typography>
                <Typography>
                    Category:
                    {currentValue?.category}
                </Typography>
                <Typography>
                    Drilling days:
                    {currentValue?.drillingDays}
                </Typography>
                <Typography>
                    Well cost:
                    {currentValue?.wellCost}
                </Typography> */}
            </Wrapper>
        </WrapperColumn>
    )
}

export default LinkWellType
