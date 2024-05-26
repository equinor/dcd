import React from "react"
import Grid from "@mui/material/Grid"
import { Typography } from "@equinor/eds-core-react"
import RangeButton from "../Buttons/RangeButton"
import CaseNumberInput from "./Components/NumberInputWithValidation"

interface DateRangePickerProps {
    setStartYear: (startYear: number) => void
    setEndYear: (endYear: number) => void
    startYear: number
    endYear: number
    labelText: string
    labelValue: string | undefined | number
    handleTableYearsClick: () => void
}

const DateRangePicker: React.FC<DateRangePickerProps> = ({
    setStartYear,
    setEndYear,
    startYear,
    endYear,
    labelText,
    labelValue,
    handleTableYearsClick,
}) => {
    const handleStartYearStateChange = (value: number): void => {
        const newStartYear = value
        if (newStartYear < 2010) {
            setStartYear(2010)
            return
        }
        setStartYear(newStartYear)
    }

    const handleEndYearStateChange = (value: number): void => {
        const newEndYear = value
        if (newEndYear > 2100) {
            setEndYear(2100)
            return
        }
        setEndYear(newEndYear)
    }

    return (
        <Grid item xs={12} container spacing={1} justifyContent="flex-end" alignItems="baseline" marginTop={6}>
            <Grid item>
                <Typography>
                    {labelText}
                    {" "}
                    {labelValue}
                </Typography>
            </Grid>
            <Grid item>
                <Typography variant="caption">Start year</Typography>
                <CaseNumberInput
                    onSubmit={(value) => handleStartYearStateChange(value)}
                    defaultValue={startYear}
                    integer
                    min={2010}
                    max={2110}
                />
            </Grid>
            <Grid item>
                <Typography variant="caption">End year</Typography>
                <CaseNumberInput
                    onSubmit={(value) => handleEndYearStateChange(value)}
                    defaultValue={endYear}
                    integer
                    min={2010}
                    max={2110}
                />
            </Grid>
            <Grid item>
                <RangeButton onClick={handleTableYearsClick} />
            </Grid>
        </Grid>
    )
}

export default DateRangePicker
