import {
    Slider, Autocomplete, TextField,
} from "@mui/material"
import { useState, useEffect, useCallback } from "react"

import BaseGantEntry, { getDisplayText } from "./BaseGantEntry"

export interface MileStoneEntryProps {
    title: string;
    description: string;
    value: number;
    periodsData: {
        value: number;
        label: string;
        quarter: number;
        year: number;
    }[];
    onChange: (newValue: number) => void;
    onClear?: () => void;
    disabled?: boolean;
}

const MileStoneEntry = ({
    title,
    description,
    value,
    periodsData,
    onChange,
    onClear,
    disabled = false,
}: MileStoneEntryProps) => {
    // Local state to track the slider value during dragging
    const [localValue, setLocalValue] = useState<number>(value)

    // Update local value when prop value changes
    useEffect(() => {
        setLocalValue(value)
    }, [value])

    // Handle local state updates during dragging (doesn't trigger API call)
    const handleDragChange = (_event: Event, newValue: number | number[]) => {
        setLocalValue(newValue as number)
    }

    // Only commit the change to the API when the slider drag is complete
    const handleChangeCommitted = (_event: React.SyntheticEvent | Event, newValue: number | number[]) => {
        onChange(newValue as number)
    }

    const handleSelectChange = (_event: React.SyntheticEvent, option: { value: number; quarter: number; year: number } | null) => {
        if (option) {
            onChange(option.value)
        }
    }

    const getValueText = (val: number) => getDisplayText(periodsData[val])

    const selectedPeriod = periodsData[value]

    const selectComponent = (
        <Autocomplete
            value={selectedPeriod}
            onChange={handleSelectChange}
            options={periodsData}
            getOptionLabel={(option) => `Q${option.quarter} ${option.year}`}
            renderInput={(params) => (
                <TextField
                    {...params}
                    label="Select date"
                    variant="outlined"
                    size="medium"
                />
            )}
            disableClearable
            fullWidth
            disabled={disabled}
        />
    )

    return (
        <BaseGantEntry
            title={title}
            description={description}
            selectComponent={selectComponent}
            onClear={onClear}
            canClear={Boolean(onClear)}
            disabled={disabled}
        >
            <Slider
                getAriaLabel={() => "Milestone date"}
                value={localValue}
                onChange={handleDragChange}
                onChangeCommitted={handleChangeCommitted}
                valueLabelDisplay="on"
                valueLabelFormat={getValueText}
                getAriaValueText={getValueText}
                marks={periodsData}
                min={0}
                max={periodsData.length - 1}
                step={1}
                track={false}
                disabled={disabled}
                sx={{
                    "& .MuiSlider-thumb": {
                        backgroundColor: "#d32f2f",
                        borderRadius: "2px",
                        width: 20,
                        height: 20,
                    },
                }}
            />
        </BaseGantEntry>
    )
}

export default MileStoneEntry
