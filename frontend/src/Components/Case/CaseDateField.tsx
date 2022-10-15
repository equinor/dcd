import {
    Input,
    Label,
} from "@equinor/eds-core-react"
import {
    ChangeEventHandler,
} from "react"
import styled from "styled-components"
import { IsInvalidDate, ToMonthDate } from "../../Utils/common"

const ColumnWrapper = styled.div`
    display: flex;
    flex-direction: column;
`
const DgField = styled.div`
    margin-bottom: 2.5rem;
    width: 12rem;
    display: flex;
`

interface Props {
    onChange: ChangeEventHandler<HTMLInputElement>
    value: Date
    label: string
}

const CaseDateField = ({
    onChange,
    value,
    label,
}: Props) => {
    console.log("CaseDGDateNew")
    // const dgReturnDate = () => (IsInvalidDate(caseItem?.[dGType])
    //     ? undefined
    //     : ToMonthDate(caseItem?.[dGType]))

    // const inputMaxDate = () => {
    //     if (dGType === DGEnum.DG0) {
    //         return IsInvalidDate(caseItem?.DG1Date) ? undefined : ToMonthDate(caseItem?.DG1Date)
    //     }
    //     if (dGType === DGEnum.DG1) {
    //         return IsInvalidDate(caseItem?.DG2Date) ? undefined : ToMonthDate(caseItem?.DG2Date)
    //     }
    //     if (dGType === DGEnum.DG2) {
    //         return IsInvalidDate(caseItem?.DG3Date) ? undefined : ToMonthDate(caseItem?.DG3Date)
    //     }
    //     if (dGType === DGEnum.DG3) {
    //         return IsInvalidDate(caseItem?.DG4Date) ? undefined : ToMonthDate(caseItem?.DG4Date)
    //     }
    //     return undefined
    // }

    // const inputMinDate = () => {
    //     if (dGType === DGEnum.DG1) {
    //         return IsInvalidDate(caseItem?.DG0Date) ? undefined : ToMonthDate(caseItem?.DG0Date)
    //     }
    //     if (dGType === DGEnum.DG2) {
    //         return IsInvalidDate(caseItem?.DG1Date) ? undefined : ToMonthDate(caseItem?.DG1Date)
    //     }
    //     if (dGType === DGEnum.DG3) {
    //         return IsInvalidDate(caseItem?.DG2Date) ? undefined : ToMonthDate(caseItem?.DG2Date)
    //     }
    //     if (dGType === DGEnum.DG4) {
    //         return IsInvalidDate(caseItem?.DG3Date) ? undefined : ToMonthDate(caseItem?.DG3Date)
    //     }
    //     return undefined
    // }

    return (
        <ColumnWrapper>
            <Label htmlFor="NumberInput" label={label} />
            <DgField>
                <Input
                    type="month"
                    id="dgDate"
                    name="dgDate"
                    value={IsInvalidDate(value) ? undefined : ToMonthDate(value)}
                    onChange={onChange}
                // max={inputMaxDate()}
                // min={inputMinDate()}
                />
            </DgField>
        </ColumnWrapper>
    )
}

export default CaseDateField
