import styled from "styled-components"

const CellWrapper = styled.div`
    line-height: 100%;
    margin-top: 7px;
`

const UnitWrapper = styled.span`
    font-weight: normal;
    font-size: 10px;
    margin-top: -4px;
    color: #6F6F6F;
`

/**
 * Sets timeseries profile name and unit in same cell.
 */
const profileAndUnitInSameCell = (params: any, rowData: any) => {
    const rowUnits = rowData.map((data: any) => data.unit)
    const checkAllUnitsAreSame = rowUnits.every((unit: any) => unit === rowUnits[0])
    const totalUnit = checkAllUnitsAreSame && params.value !== undefined ? rowUnits[0] : ""

    return (
        <CellWrapper>
            {params.value}
            <br />
            <UnitWrapper>
                {params.data?.unit ? params.data.unit : totalUnit}
            </UnitWrapper>
        </CellWrapper>
    )
}

export default profileAndUnitInSameCell
