import {
    Typography,
    Button,
    Icon,
} from "@equinor/eds-core-react"
import { undo } from "@equinor/eds-icons"
import Grid from "@mui/material/Grid2"
import React, { useState, useEffect } from "react"
import styled from "styled-components"

import RangeSlider from "./RangeSlider"

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

const ButtonContainer = styled.div`
    display: flex;
    justify-content: flex-end;
    gap: 8px;
    margin-top: 16px;
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
    const [isResetting, setIsResetting] = useState(false)

    const hasChanges = startYear !== initialStart || endYear !== initialEnd

    useEffect(() => {
        if (isResetting && startYear === initialStart && endYear === initialEnd) {
            handleTableYearsClick()
            setIsResetting(false)
        }
    }, [startYear, endYear, initialStart, initialEnd, isResetting, handleTableYearsClick])

    const handleReset = (): void => {
        setIsResetting(true)
        setStartYear(initialStart)
        setEndYear(initialEnd)
    }

    const handleYearChange = (newValues: [number, number]): void => {
        const [newStart, newEnd] = newValues

        if (newStart <= newEnd) {
            setStartYear(newStart)
            setEndYear(newEnd)
        }
    }

    const onApplyButtonClick = (): void => {
        handleTableYearsClick()
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
                <RangeSlider
                    startValue={startYear}
                    endValue={endYear}
                    minValue={MIN_YEAR}
                    maxValue={MAX_YEAR}
                    step={1}
                    onChange={handleYearChange}
                    marks={[
                        { value: MIN_YEAR, label: MIN_YEAR.toString() },
                        { value: MAX_YEAR, label: MAX_YEAR.toString() },
                    ]}
                    showTooltips
                />
                <ButtonContainer>
                    <ResetButton
                        variant="outlined"
                        onClick={handleReset}
                        disabled={!hasChanges}
                    >
                        <Icon data={undo} size={24} />
                    </ResetButton>
                    <Button
                        variant="contained"
                        onClick={onApplyButtonClick}
                    >
                        Apply
                    </Button>
                </ButtonContainer>
            </RangeContainer>
        </StyledContainer>
    )
}

export default DateRangePicker
