import {
    Slider, Autocomplete, TextField, Stack,
} from "@mui/material"

import BaseGantEntry, { getDisplayText } from "./BaseGantEntry"

export interface RangeEntryProps {
    title: string;
    description: string;
    value: number[];
    periodsData: {
        value: number;
        label: string;
        quarter: number;
        year: number;
    }[];
    onChange: (newValue: number[]) => void;
}

const RangeEntry = ({
    title,
    description,
    value,
    periodsData,
    onChange,
}: RangeEntryProps) => {
    const handleChange = (_event: Event, newValue: number | number[]) => {
        onChange(newValue as number[])
    }

    const handleStartDateChange = (_event: React.SyntheticEvent, option: { value: number; quarter: number; year: number } | null) => {
        if (option) {
            const newValue = [...value]

            newValue[0] = option.value
            // Ensure start date is not after end date
            if (newValue[0] <= newValue[1]) {
                onChange(newValue)
            } else {
                onChange([option.value, option.value])
            }
        }
    }

    const handleEndDateChange = (_event: React.SyntheticEvent, option: { value: number; quarter: number; year: number } | null) => {
        if (option) {
            const newValue = [...value]

            newValue[1] = option.value
            // Ensure end date is not before start date
            if (newValue[1] >= newValue[0]) {
                onChange(newValue)
            } else {
                onChange([option.value, option.value])
            }
        }
    }

    const getValueText = (val: number) => getDisplayText(periodsData[val])

    const startPeriod = periodsData[value[0]]
    const endPeriod = periodsData[value[1]]

    const selectComponent = (
        <Stack spacing={2}>
            <Autocomplete
                value={startPeriod}
                onChange={handleStartDateChange}
                options={periodsData}
                getOptionLabel={(option) => `Q${option.quarter} ${option.year}`}
                renderInput={(params) => (
                    <TextField
                        {...params}
                        label="Start date"
                        variant="outlined"
                        size="medium"
                    />
                )}
                disableClearable
                fullWidth
            />
            <Autocomplete
                value={endPeriod}
                onChange={handleEndDateChange}
                options={periodsData}
                getOptionLabel={(option) => `Q${option.quarter} ${option.year}`}
                renderInput={(params) => (
                    <TextField
                        {...params}
                        label="End date"
                        variant="outlined"
                        size="medium"
                    />
                )}
                disableClearable
                fullWidth
            />
        </Stack>
    )

    return (
        <BaseGantEntry
            title={title}
            description={description}
            selectComponent={selectComponent}
        >
            <Slider
                getAriaLabel={() => "Quarter range"}
                value={value}
                onChange={handleChange}
                valueLabelDisplay="on"
                valueLabelFormat={getValueText}
                getAriaValueText={getValueText}
                marks={periodsData}
                min={0}
                max={periodsData.length - 1}
                step={1}
                sx={{
                    "& .MuiSlider-thumb": {
                        width: 16,
                        height: 16,
                    },
                }}
            />
        </BaseGantEntry>
    )
}

export default RangeEntry
