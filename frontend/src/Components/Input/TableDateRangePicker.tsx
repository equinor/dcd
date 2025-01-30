import React, { useState, useEffect } from "react"
import { default as Grid } from "@mui/material/Grid2"
import { Typography, TextField, Button, Icon } from "@equinor/eds-core-react"
import { undo } from "@equinor/eds-icons"
import Slider from "@mui/material/Slider"
import styled from "styled-components"

const StyledContainer = styled(Grid)`
    width: 100%;
    margin-top: 48px;
    margin-right: 32px;
    display: flex;
    justify-content: flex-end;
    align-items: baseline;
`

const HelperContainer = styled(Grid)`
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    position: relative;
    top: 11px;
    margin-right: 20px;
`

const RangeContainer = styled.div`
    width: 380px;
`

const InputGroup = styled.div`
    display: flex;
    align-items: center;
    margin-bottom: 24px;
    gap: 10px;
`

const YearInput = styled(TextField)`
    width: 100px;
    input {
        text-align: center;
    }
`

const YearSeparator = styled(Typography)`
    color: #6F6F6F;
`

const ResetButton = styled(Button)`
    min-width: 40px !important;
    width: 40px;
    padding: 0;
`

interface DateRangePickerProps {
    setStartYear: (startYear: number) => void
    setEndYear: (endYear: number) => void
    startYear: number
    endYear: number
    labelText?: string | undefined
    labelValue?: string | undefined | number
    handleTableYearsClick: () => void
}

const MIN_YEAR = 2010
const MAX_YEAR = 2100

const DateRangePicker: React.FC<DateRangePickerProps> = ({
    setStartYear,
    setEndYear,
    startYear,
    endYear,
    labelText,
    labelValue,
    handleTableYearsClick,
}) => {
    const [initialStart] = useState(startYear)
    const [initialEnd] = useState(endYear)
    const [localStartYear, setLocalStartYear] = useState(startYear.toString())
    const [localEndYear, setLocalEndYear] = useState(endYear.toString())
    const [isResetting, setIsResetting] = useState(false)

    const hasChanges = startYear !== initialStart || endYear !== initialEnd

    useEffect(() => {
        if (isResetting && startYear === initialStart && endYear === initialEnd) {
            handleTableYearsClick()
            setIsResetting(false)
        }
    }, [startYear, endYear, initialStart, initialEnd, isResetting, handleTableYearsClick])

    const handleReset = () => {
        setIsResetting(true)
        setStartYear(initialStart)
        setEndYear(initialEnd)
        setLocalStartYear(initialStart.toString())
        setLocalEndYear(initialEnd.toString())
    }

    useEffect(() => {
        setLocalStartYear(startYear.toString())
        setLocalEndYear(endYear.toString())
    }, [startYear, endYear])

    const handleYearChange = (_event: Event, newValue: number | number[]) => {
        if (Array.isArray(newValue)) {
            const [newStart, newEnd] = newValue
            if (newStart <= newEnd) {
                setStartYear(newStart)
                setEndYear(newEnd)
            }
        }
    }

    const handleStartYearInput = (e: React.ChangeEvent<HTMLInputElement>) => {
        const inputValue = e.target.value
        setLocalStartYear(inputValue)
        
        const value = parseInt(inputValue, 10)
        if (!isNaN(value) && value >= MIN_YEAR && value <= MAX_YEAR) {
            if (value <= endYear) {
                setStartYear(value)
            }
        }
    }

    const handleEndYearInput = (e: React.ChangeEvent<HTMLInputElement>) => {
        const inputValue = e.target.value
        setLocalEndYear(inputValue)
        
        const value = parseInt(inputValue, 10)
        if (!isNaN(value) && value >= MIN_YEAR && value <= MAX_YEAR) {
            if (value >= startYear) {
                setEndYear(value)
            }
        }
    }

    const handleStartYearBlur = () => {
        const value = parseInt(localStartYear, 10)
        if (isNaN(value)) {
            setStartYear(MIN_YEAR)
            setLocalStartYear(MIN_YEAR.toString())
        } else if (value > endYear) {
            setStartYear(endYear)
            setLocalStartYear(endYear.toString())
        } else if (value < MIN_YEAR) {
            setStartYear(MIN_YEAR)
            setLocalStartYear(MIN_YEAR.toString())
        } else if (value > MAX_YEAR) {
            setStartYear(MAX_YEAR)
            setLocalStartYear(MAX_YEAR.toString())
        } else {
            setStartYear(value)
            setLocalStartYear(value.toString())
        }
    }

    const handleEndYearBlur = () => {
        const value = parseInt(localEndYear, 10)
        if (isNaN(value)) {
            setEndYear(MIN_YEAR)
            setLocalEndYear(MIN_YEAR.toString())
        } else if (value < startYear) {
            setEndYear(startYear)
            setLocalEndYear(startYear.toString())
        } else if (value < MIN_YEAR) {
            setEndYear(MIN_YEAR)
            setLocalEndYear(MIN_YEAR.toString())
        } else if (value > MAX_YEAR) {
            setEndYear(MAX_YEAR)
            setLocalEndYear(MAX_YEAR.toString())
        } else {
            setEndYear(value)
            setLocalEndYear(value.toString())
        }
    }

    return (
        <StyledContainer container spacing={2}>
            {labelText && labelValue && (
                <HelperContainer>
                    <Typography variant="meta">
                        {labelText}
                    </Typography>
                    <Typography variant="caption">
                        {labelValue}
                    </Typography>
                </HelperContainer>
            )}
            <RangeContainer>
                <InputGroup>
                    <YearInput
                        id="start-year"
                        type="number"
                        value={localStartYear}
                        onChange={handleStartYearInput}
                        onBlur={handleStartYearBlur}
                        min={MIN_YEAR}
                        max={MAX_YEAR}
                    />
                    <YearSeparator variant="h6">-</YearSeparator>
                    <YearInput
                        id="end-year"
                        type="number"
                        value={localEndYear}
                        onChange={handleEndYearInput}
                        onBlur={handleEndYearBlur}
                        min={MIN_YEAR}
                        max={MAX_YEAR}
                    />
                    <ResetButton
                        variant="outlined"
                        onClick={handleReset}
                        disabled={!hasChanges}
                    >
                        <Icon data={undo} size={24} />
                    </ResetButton>
                    <Button
                        variant="contained"
                        onClick={handleTableYearsClick}
                    >
                        Apply
                    </Button>
                </InputGroup>
                <Slider
                    value={[startYear, endYear]}
                    onChange={handleYearChange}
                    min={MIN_YEAR}
                    max={MAX_YEAR}
                    step={1}
                    marks={[
                        { value: MIN_YEAR, label: MIN_YEAR.toString() },
                        { value: MAX_YEAR, label: MAX_YEAR.toString() },
                    ]}
                    valueLabelDisplay="auto"
                />
            </RangeContainer>
        </StyledContainer>
    )
}

export default DateRangePicker
