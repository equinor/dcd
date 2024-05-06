import React, { ChangeEventHandler } from "react"
import { NativeSelect, Typography } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import RangeButton from "../../../../Buttons/RangeButton"
import CaseNumberInput from "../../../../Input/CaseNumberInput"
import { useProjectContext } from "../../../../../Context/ProjectContext"

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

    const handleStartYearChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newStartYear = Number(e.currentTarget.value)
        if (newStartYear < 2010) {
            setStartYear(2010)
            return
        }
        setStartYear(newStartYear)
    }

    const handleEndYearChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newEndYear = Number(e.currentTarget.value)
        if (newEndYear > 2100) {
            setEndYear(2100)
            return
        }
        setEndYear(newEndYear)
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
                    onChange={handleStartYearChange}
                    defaultValue={startYear}
                    integer
                    min={2010}
                    max={2100}
                />
            </Grid>
            <Grid item>
                <Typography variant="caption">End year</Typography>
                <CaseNumberInput
                    onChange={handleEndYearChange}
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
