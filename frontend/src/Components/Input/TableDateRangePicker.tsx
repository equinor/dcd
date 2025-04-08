import {
    Typography,
    Button,
    Icon,
} from "@equinor/eds-core-react"
import { undo } from "@equinor/eds-icons"
import React, { useState, useEffect } from "react"
import styled from "styled-components"

import RangeSlider from "./RangeSlider"

import { useAppStore } from "@/Store/AppStore"

const Container = styled.div`
    display: flex;
    align-items: center;
    gap: 16px;
    margin: 16px 0;
    width: 100%;
    justify-content: flex-end;
`

const ContentContainer = styled.div`
    display: flex;
    align-items: center;
    gap: 16px;
    padding: 25px 36px;
    border: 1px solid #e0e0e0;
    border-radius: 4px;
    background-color: #f9f9f9;
`

const LabelContainer = styled.div`
    display: flex;
    flex-direction: column;
    align-items: center;
    min-width: 60px;
`

const SliderContainer = styled.div`
    width: 300px;
    margin-right: 24px;
    position: relative;
    top: 15px;
`

const ButtonContainer = styled.div`
    display: flex;
    gap: 8px;
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
    const { editMode } = useAppStore()

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

    if (!editMode) {
        return null
    }

    return (
        <Container>
            <ContentContainer>
                {labelText && labelValue && (
                    <LabelContainer>
                        <Typography variant="meta">
                            {labelText}
                        </Typography>
                        <Typography variant="caption">
                            {labelValue}
                        </Typography>
                    </LabelContainer>
                )}
                <SliderContainer>
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
                        minTooltipDistance={18}
                    />
                </SliderContainer>
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
                        onClick={handleTableYearsClick}
                    >
                        Apply
                    </Button>
                </ButtonContainer>
            </ContentContainer>
        </Container>
    )
}

export default DateRangePicker
