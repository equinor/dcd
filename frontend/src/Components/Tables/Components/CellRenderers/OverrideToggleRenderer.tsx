import { ICellRendererParams } from "@ag-grid-community/core"
import { CircularProgress } from "@equinor/eds-core-react"
import styled from "styled-components"

import CalculationSourceToggle from "../CalculationToggle/CalculationSourceToggle"

import useCanUserEdit from "@/Hooks/useCanUserEdit"
import { ITimeSeriesTableDataOverrideWithSet } from "@/Models/ITimeSeries"

const CenterGridIcons = styled.div`
    padding-top: 0px;
    padding-left: 0px;
    height: 100%;
    display: flex;
    align-items: center;
    gap: 8px;
`

interface OverrideToggleRendererProps {
    params: ICellRendererParams<ITimeSeriesTableDataOverrideWithSet>
    calculatedFields?: string[]
    ongoingCalculation?: boolean
    isProsp?: boolean
    sharepointFileId?: string
}

const OverrideToggleRenderer = ({
    params,
    calculatedFields,
    ongoingCalculation,
    isProsp,
    sharepointFileId,
}: OverrideToggleRendererProps) => {
    const { canEdit } = useCanUserEdit()

    if (!params.data) { return null }

    const isUnlocked = params.data.overrideProfile?.override

    if (!isUnlocked && calculatedFields?.includes(params.data.resourceName) && ongoingCalculation) {
        return <CircularProgress size={24} />
    }

    return (
        <CenterGridIcons>
            <CalculationSourceToggle
                editAllowed={canEdit()}
                isProsp={isProsp}
                sharepointFileId={sharepointFileId}
                clickedElement={params}
            />
        </CenterGridIcons>
    )
}

export default OverrideToggleRenderer
