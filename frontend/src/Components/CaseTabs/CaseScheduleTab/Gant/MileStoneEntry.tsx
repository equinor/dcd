import {
    Slider,
} from "@mui/material"
import { useState, useEffect } from "react"
import styled from "styled-components"

import BaseGantEntry, { getDisplayText } from "./BaseGantEntry"

export interface MileStoneEntryProps {
    title: string;
    value: number;
    periodsData: {
        value: number;
        label: string;
        quarter: number;
        year: number;
    }[];
    onChange: (newValue: number) => void;
    onClear?: () => void;
    isRequired?: boolean;
    disabled?: boolean;
    readOnly?: boolean;
}

const StyledSlider = styled(Slider)<{ $readOnly?: boolean }>`
  & .MuiSlider-thumb {
    background-color: #d32f2f;
    border-radius: 2px;
    width: ${({ $readOnly }): string => ($readOnly ? "12px" : "20px")};
    height: ${({ $readOnly }): string => ($readOnly ? "12px" : "20px")};
  }
  
  & .MuiSlider-markLabel {
    font-size: 0.75rem;
  }
  
  ${({ $readOnly }): string => ($readOnly ? `
    pointer-events: none;
    height: 32px;
    margin-top: 0;
    
    & .MuiSlider-valueLabel {
      background-color: transparent;
      color: #000;
      font-size: 0.75rem;
      font-weight: bold;
      top: -2px;
      padding: 0 4px;
    }
    
    & .MuiSlider-rail {
      opacity: 0.5;
      height: 4px;
    }
    
    & .MuiSlider-mark {
      opacity: 0.4;
      height: 8px;
    }
  ` : "")}
`

const MileStoneEntry = ({
    title,
    value,
    periodsData,
    onChange,
    onClear,
    isRequired = false,
    disabled = false,
    readOnly = false,
}: MileStoneEntryProps): JSX.Element => {
    const [localValue, setLocalValue] = useState<number>(value)

    useEffect(() => {
        setLocalValue(value)
    }, [value])

    const handleDragChange = (_event: Event, newValue: number | number[]): void => {
        if (!readOnly) {
            setLocalValue(newValue as number)
        }
    }

    const handleChangeCommitted = (_event: React.SyntheticEvent | Event, newValue: number | number[]): void => {
        if (!readOnly) {
            onChange(newValue as number)
        }
    }

    const getValueText = (val: number): string => getDisplayText(periodsData[val])

    return (
        <BaseGantEntry
            title={title}
            onClear={!readOnly ? onClear : undefined}
            isRequired={isRequired}
            disabled={disabled || readOnly}
            readOnly={readOnly}
        >
            <StyledSlider
                $readOnly={readOnly}
                getAriaLabel={(): string => "Milestone date"}
                value={localValue}
                onChange={handleDragChange}
                onChangeCommitted={handleChangeCommitted}
                valueLabelDisplay="on"
                valueLabelFormat={getValueText}
                getAriaValueText={getValueText}
                marks={periodsData.filter((p) => p.label !== "")}
                min={0}
                max={periodsData.length - 1}
                step={1}
                track={false}
                disabled={disabled || readOnly}
            />
        </BaseGantEntry>
    )
}

export default MileStoneEntry
