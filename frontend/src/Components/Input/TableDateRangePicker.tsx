import {
    Typography,
    Button,
} from "@equinor/eds-core-react"
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

interface DateRangePickerProps {
    startYear: number
    endYear: number
    labelText?: string | undefined
    labelValue?: string | undefined | number
    handleTableYearsClick: (startYear: number, endYear: number) => void
}

const MIN_YEAR = 2010
const MAX_YEAR = 2100

const DateRangePicker: React.FC<DateRangePickerProps> = ({
    startYear: tableStartYear,
    endYear: tableEndYear,
    labelText,
    labelValue,
    handleTableYearsClick,
}) => {
    const { editMode } = useAppStore()

    const [startYear, setStartYear] = useState<number>(tableStartYear)
    const [endYear, setEndYear] = useState<number>(tableEndYear)

    useEffect(() => {
        setStartYear(tableStartYear)
        setEndYear(tableEndYear)
    }, [tableStartYear, tableEndYear])

    const handleYearChange = (newValues: [number, number]): void => {
        const [newStart, newEnd] = newValues

        if (newStart <= newEnd) {
            setStartYear(newStart)
            setEndYear(newEnd)
        }
    }

    const handleApplyClick = (): void => {
        handleTableYearsClick(startYear, endYear)
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
                    <Button
                        variant="contained"
                        onClick={handleApplyClick}
                    >
                        Apply
                    </Button>
                </ButtonContainer>
            </ContentContainer>
        </Container>
    )
}

export default DateRangePicker
