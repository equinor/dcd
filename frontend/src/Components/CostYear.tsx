import {
    Typography,
    Input,
} from "@equinor/eds-core-react"
import {
    ChangeEventHandler,
    Dispatch,
    SetStateAction,
} from "react"
import styled from "styled-components"

const DgField = styled.div`
    margin-bottom: 2.5rem;
    width: 12rem;
    display: flex;
`

interface Props {
    setCostYear: Dispatch<SetStateAction<number | undefined>>
    costYear: number | undefined
    setHasChanges: Dispatch<SetStateAction<boolean>>
}

const CostYear = ({
    setCostYear,
    costYear,
    setHasChanges,
}: Props) => {
    const onApprovedByChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setCostYear(Number(e.target.value))
        if (e.target.value !== undefined && e.target.value !== "") {
            setHasChanges(true)
        } else {
            setHasChanges(false)
        }
    }

    return (
        <>
            <Typography variant="h6">Cost Year</Typography>
            <DgField>
                <Input
                    defaultValue={costYear}
                    id="costYear"
                    name="costYear"
                    type="number"
                    onChange={onApprovedByChange}
                />
            </DgField>
        </>
    )
}

export default CostYear
