import { Slider } from "@mui/material"
import React from "react"
import styled from "styled-components"

const SliderContainer = styled.div`
    position: relative;
    margin-bottom: 24px;
    padding-top: 20px;
`

const YearTooltip = styled.div<{ position: number; isStart: boolean }>`
    position: absolute;
    top: -20px;
    left: ${(props): number => props.position}%;
    transform: translateX(-50%);
    background-color: #243746;
    color: white;
    padding: 4px 8px;
    border-radius: 4px;
    font-size: 14px;
    font-weight: 500;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
    z-index: 1;
`

interface RangeSliderProps {
    startValue: number
    endValue: number
    minValue: number
    maxValue: number
    step?: number
    onChange: (newValues: [number, number]) => void
    onChangeCommitted?: (event: Event | React.SyntheticEvent<Element, Event>, newValue: number | number[]) => void
    disabled?: boolean
    marks?: { value: number; label: string }[]
    showTooltips?: boolean
    minTooltipDistance?: number
}

const RangeSlider: React.FC<RangeSliderProps> = ({
    startValue,
    endValue,
    minValue,
    maxValue,
    step = 1,
    onChange,
    onChangeCommitted,
    disabled = false,
    marks,
    showTooltips = true,
    minTooltipDistance = 15,
}) => {
    const startValuePosition = ((startValue - minValue) / (maxValue - minValue)) * 100
    const endValuePosition = ((endValue - minValue) / (maxValue - minValue)) * 100

    // Adjust tooltip positions if they're too close together
    const tooltipDistance = endValuePosition - startValuePosition
    let adjustedStartPosition = startValuePosition
    let adjustedEndPosition = endValuePosition

    if (showTooltips && tooltipDistance < minTooltipDistance) {
        const midpoint = (startValuePosition + endValuePosition) / 2

        adjustedStartPosition = midpoint - minTooltipDistance / 2
        adjustedEndPosition = midpoint + minTooltipDistance / 2

        adjustedStartPosition = Math.max(0, adjustedStartPosition)
        adjustedEndPosition = Math.min(100, adjustedEndPosition)
    }

    const handleChange = (_event: Event, newValue: number | number[]): void => {
        if (Array.isArray(newValue)) {
            onChange(newValue as [number, number])
        }
    }

    return (
        <SliderContainer>
            {showTooltips && (
                <>
                    <YearTooltip position={adjustedStartPosition} isStart>{startValue}</YearTooltip>
                    <YearTooltip position={adjustedEndPosition} isStart={false}>{endValue}</YearTooltip>
                </>
            )}
            <Slider
                value={[startValue, endValue]}
                onChange={handleChange}
                onChangeCommitted={onChangeCommitted}
                min={minValue}
                max={maxValue}
                step={step}
                disabled={disabled}
                marks={marks}
                valueLabelDisplay="off"
            />
        </SliderContainer>
    )
}

export default RangeSlider
