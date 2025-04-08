import { Icon } from "@equinor/eds-core-react"
import { calendar } from "@equinor/eds-icons"
import {
    Box,
    Slider,
    Typography,
} from "@mui/material"
import { DatePicker } from "@mui/x-date-pickers/DatePicker"
import dayjs, { Dayjs } from "dayjs"
import {
    useState, useEffect, useCallback, useMemo,
} from "react"
import styled from "styled-components"

import BaseGantEntry, { getDisplayText } from "./BaseGantEntry"

import { dateToQuarterIndex } from "@/Utils/DateUtils"

export interface MilestoneEntryProps {
    title: string;
    value: number;
    periodsData: {
        value: number;
        label: string;
        quarter: number;
        year: number;
    }[];
    dateValue: Date | undefined;
    rangeStartYear: number;
    rangeEndYear: number;
    onSliderChange: (newValue: number) => void;
    onDateChange: (newDate: Date | null) => void;
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
  ` : `
    /* Custom tooltip styling to match RangeSlider - only in edit mode */
    & .MuiSlider-valueLabel {
      background-color: #243746;
      color: white;
      padding: 4px 8px;
      border-radius: 4px;
      font-size: 14px;
      font-weight: 500;
      box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
    }

    & .MuiSlider-valueLabel::before {
      display: none;
    }
  `)}
`

const DatePickerContainer = styled(Box)`
  width: 160px;
  
  & .MuiInputBase-root {
    height: 40px;
  }
`

const ReadOnlyDateDisplay = styled(Box)`
  display: flex;
  align-items: center;
  padding: 8px ;
  gap: 8px;
  color: #333;
  background-color: #edf3f1;
  border-radius: 4px;
  border: 1px solid #dcdcdc;
  
  & .calendar-icon {
    color: #007079;
  }
  
  & .date-text {
    font-weight: 500;
  }
`

const MilestoneEntry = ({
    title,
    value,
    periodsData,
    dateValue,
    rangeStartYear,
    rangeEndYear,
    onSliderChange,
    onDateChange,
    onClear,
    isRequired = false,
    disabled = false,
    readOnly = false,
}: MilestoneEntryProps): JSX.Element => {
    const [localValue, setLocalValue] = useState<number>(value)

    // Update local value when prop value changes
    useEffect(() => {
        setLocalValue(value)
    }, [value])

    const { minDate, maxDate } = useMemo(() => {
        const startDate = dayjs().year(rangeStartYear).month(0).date(1)
        const endDate = dayjs().year(rangeEndYear).month(11).date(31)

        return {
            minDate: startDate,
            maxDate: endDate,
        }
    }, [rangeStartYear, rangeEndYear])

    const handleDragChange = (_event: Event, newValue: number | number[]): void => {
        if (!readOnly) {
            setLocalValue(newValue as number)
        }
    }

    const handleChangeCommitted = (_event: React.SyntheticEvent | Event, newValue: number | number[]): void => {
        if (!readOnly) {
            onSliderChange(newValue as number)
        }
    }

    const handleDateChange = useCallback((newDate: Dayjs | null) => {
        if (!readOnly) {
            if (newDate) {
                const jsDate = newDate.toDate()

                const dateQuarterIndex = dateToQuarterIndex(jsDate, rangeStartYear)

                if (dateQuarterIndex !== undefined && dateQuarterIndex !== localValue) {
                    setLocalValue(dateQuarterIndex)
                    onSliderChange(dateQuarterIndex)
                }

                onDateChange(jsDate)
            } else if (!isRequired) {
                onDateChange(null)
            }
        }
    }, [isRequired, onDateChange, readOnly, rangeStartYear, localValue, onSliderChange])

    const getValueText = (val: number): string => getDisplayText(periodsData[val])

    const formattedDate = useMemo(() => {
        if (!dateValue) { return "" }

        return dayjs(dateValue).format("DD MMM YYYY")
    }, [dateValue])

    const leftControls = readOnly ? (
        <ReadOnlyDateDisplay>
            <Icon data={calendar} size={18} className="calendar-icon" />
            <Typography variant="body2" className="date-text">
                {formattedDate}
            </Typography>
        </ReadOnlyDateDisplay>
    ) : (
        <DatePickerContainer>
            <DatePicker
                disabled={disabled}
                value={dateValue ? dayjs(dateValue) : null}
                onChange={handleDateChange}
                slotProps={{
                    textField: {
                        size: "small",
                        fullWidth: true,
                        required: isRequired,
                    },
                }}
                format="DD MMM YYYY"
                minDate={minDate}
                maxDate={maxDate}
            />
        </DatePickerContainer>
    )

    return (
        <BaseGantEntry
            title={title}
            onClear={!readOnly ? onClear : undefined}
            isRequired={isRequired}
            disabled={disabled || readOnly}
            readOnly={readOnly}
            leftControls={leftControls}
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

export default MilestoneEntry
