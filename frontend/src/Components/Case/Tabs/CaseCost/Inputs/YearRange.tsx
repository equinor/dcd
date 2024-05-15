import React from "react"
import { NativeSelect, Typography } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import RangeButton from "../../../../Buttons/RangeButton"
import CaseNumberInput from "../../../../Input/CaseNumberInput"
import { useProjectContext } from "../../../../../Context/ProjectContext"
import { handleStartYearStateChange, handleEndYearStateChange } from "../../../../../Utils/common"

interface RearRangeProps {
    startYear: number
    setStartYear: (startYear: number) => void
    endYear: number
    setEndYear: (endYear: number) => void
    setTableYears: (years: [number, number]) => void
}

const RearRange: React.FC<RearRangeProps> = ({
    startYear,
    setStartYear,
    endYear,
    setEndYear,
    setTableYears,
}) => {
    const { project } = useProjectContext()

    const handleTableYearsClick = () => {
        setTableYears([startYear, endYear])
    }

    return (
        <>
            <Grid item>
                <NativeSelect
                    id="currency"
                    label="Currency"
                    onChange={() => { }}
                    value={project?.currency}
                    disabled
                >
                    <option key="1" value={1}>MNOK</option>
                    <option key="2" value={2}>MUSD</option>
                </NativeSelect>
            </Grid>
            <Grid item>
                <Typography variant="caption">Start year</Typography>
                <CaseNumberInput
                    onChange={(value) => handleStartYearStateChange(value, setStartYear)}
                    defaultValue={startYear}
                    integer
                    min={2010}
                    max={2100}
                />
            </Grid>
            <Grid item>
                <Typography variant="caption">End year</Typography>
                <CaseNumberInput
                    onChange={(value) => handleEndYearStateChange(value, setEndYear)}
                    defaultValue={endYear}
                    integer
                    min={2010}
                    max={2100}
                />
            </Grid>
            <Grid item>
                <RangeButton onClick={handleTableYearsClick} />
            </Grid>
        </>
    )
}

export default RearRange
