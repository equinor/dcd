export const defaultDg4Date = "2030-01-01T00:00:00Z"

/**
 * Converts a UTC date string to a Date object
 * Handles various UTC date string formats and ensures proper conversion
 * @param dateString - UTC date string to convert
 * @returns Date object in UTC
 */
export const dateStringToDateUtc = (dateString: string): Date => {
    // Ensure the date string ends with "Z" or "+00:00" to create the Date object as UTC.
    if (!dateString.endsWith("Z") && !dateString.endsWith("+00:00")) {
        if (dateString.length === 10) {
            // Handle "0001-01-01" format
            return new Date(`${dateString}T00:00:00Z`)
        }
        if (dateString.length === 16) {
            // Handle "0001-01-01T00:00" format
            return new Date(`${dateString}:00Z`)
        }
        if (dateString.length >= 19) {
            // Handle "0001-01-01T00:00:00.xx" format
            return new Date(`${dateString}Z`)
        }
    }

    return new Date(dateString)
}

export const dateToUtcDateStringWithZeroTimePart = (date: Date): string => dateStringToDateUtc(date.toString()).toISOString()

/**
 * Extracts the year from a date string
 * @param dateString - Date string to extract year from
 * @returns Year as number
 */
export const getYearFromDateString = (dateString: string): number => new Date(dateString).getFullYear()

/**
 * Creates a Date object from a timestamp
 * @param timestamp - Milliseconds since Unix epoch
 * @returns Date object
 */
export const dateFromTimestamp = (timestamp: number): Date => new Date(timestamp)

/**
 * Compares two UTC date strings for sorting
 * @param a - First UTC date string
 * @param b - Second UTC date string
 * @returns Negative if a < b, positive if a > b, zero if equal
 */
export const sortUtcDateStrings = (a: string, b: string): number => {
    const dateA = dateStringToDateUtc(a)
    const dateB = dateStringToDateUtc(b)

    return dateA.getTime() - dateB.getTime()
}

/**
 * Formats date to YYYY-MM format for month selection
 * @param date - Date to format
 * @returns Formatted string or undefined if date is invalid
 */
export const toMonthDate = (date?: Date | null): string | undefined => {
    if (Number.isNaN(date?.getTime())) {
        return undefined
    }

    return date?.toISOString().substring(0, 7)
}

/**
 * Formats ISO date string to localized month and year
 * @param isoDateString - ISO date string to format
 * @returns Formatted date string or placeholder for invalid dates
 */
export function formatDate(isoDateString: string): string {
    if (!isoDateString || isoDateString === "0001-01-01T00:00:00+00:00" || isoDateString === "0001-01-01T00:00:00.000Z") {
        return "_"
    }
    const date = new Date(isoDateString)

    if (Number.isNaN(date.getTime())) {
        console.error(`Invalid date string: ${isoDateString}`)

        return "Unavailable"
    }
    const options: Intl.DateTimeFormatOptions = {
        month: "long",
        year: "numeric",
    }

    return new Intl.DateTimeFormat("no-NO", options).format(date)
}

/**
 * Formats date string to day, month, year, hour and minute
 * @param dateString - Date string to format
 * @returns Formatted date and time string or empty string if no date
 */
export const formatDateAndTime = (dateString: string | undefined | null) => {
    if (!dateString) { return "" }
    const date = dateStringToDateUtc(dateString)
    const options: Intl.DateTimeFormatOptions = {
        day: "2-digit",
        month: "short",
        year: "numeric",
        hour: "2-digit",
        minute: "2-digit",
        hour12: false,
    }

    return new Intl.DateTimeFormat("en-GB", options)
        .format(date)
        .replace(",", "")
}

/**
 * Formats date string to day, month and year
 * @param dateString - Date string to format
 * @returns Formatted date string or empty string if no date
 */
export const formatFullDate = (dateString: string | undefined | null) => {
    if (!dateString) { return "" }
    const date = dateStringToDateUtc(dateString)
    const options: Intl.DateTimeFormatOptions = {
        day: "2-digit",
        month: "short",
        year: "numeric",
    }

    return new Intl.DateTimeFormat("en-GB", options)
        .format(date)
        .replace(",", "")
}

// Configurable time ranges for Gantt chart (in years)
export const GANTT_CONFIG = {
    YEARS_BEFORE_DG4: 9, // Show 5 years in the past
    YEARS_AFTER_DG4: 10, // Show 15 years in the future
    DEFAULT_DG4_YEAR: new Date().getFullYear(), // Use current year as default
}

interface QuarterPeriod {
    value: number
    label: string
    quarter: number
    year: number
}

/**
 * Generates quarterly periods based on specified parameters
 * @param referenceDate - Reference date for calculations (defaults to current date)
 * @param customRangeYears - Optional custom range in years (overrides GANTT_CONFIG)
 * @returns Array of quarterly periods
 */
export const generateDynamicQuarterlyPeriods = (referenceDate?: Date | null, customRangeYears?: number): QuarterPeriod[] => {
    const quarters: QuarterPeriod[] = []
    let index = 0

    // Get reference year from parameter or use current year
    const referenceYear = referenceDate ? referenceDate.getFullYear() : new Date().getFullYear()

    let startYear: number
    let endYear: number

    if (customRangeYears !== undefined) {
        // If custom range is provided, use the reference year as start and calculate end year based on the custom range
        startYear = referenceYear
        endYear = referenceYear + customRangeYears - 1
    } else {
        // Otherwise use the default config values
        startYear = referenceYear - GANTT_CONFIG.YEARS_BEFORE_DG4
        endYear = referenceYear + GANTT_CONFIG.YEARS_AFTER_DG4
    }

    for (let year = startYear; year <= endYear; year += 1) {
        for (let quarter = 1; quarter <= 4; quarter += 1) {
            quarters.push({
                value: index,
                // Only show label for Q1 of years divisible by 5 (e.g., 2020, 2025, 2030...)
                label: quarter === 1 && year % 5 === 0 ? `${year}` : "",
                quarter,
                year,
            })
            index += 1
        }
    }

    return quarters
}

/**
 * Converts a date to a quarter period index based on a fixed timeline
 * @param date - Date to convert to quarter index
 * @param startYear - The start year of the timeline
 * @returns Quarter index (0-based)
 */
export const dateToQuarterIndex = (date: Date, startYear: number): number => {
    const year = date.getFullYear()
    const month = date.getMonth()

    const quarter = Math.floor(month / 3)

    // Calculate index: 4 quarters per year, starting from our fixed start year
    return (year - startYear) * 4 + quarter
}

/**
 * Converts a quarter index to the first day of that quarter
 * @param quarterIndex - Quarter index (0-based)
 * @param startYear - The start year of the timeline (defaults to current year - YEARS_BEFORE_DG4)
 * @returns Date object representing the first day of the quarter
 */
export const quarterIndexToStartDate = (quarterIndex: number, startYear?: number): Date => {
    // If no start year provided, use current year - YEARS_BEFORE_DG4
    const currentYear = new Date().getFullYear()
    const fixedStartYear = startYear || (currentYear - GANTT_CONFIG.YEARS_BEFORE_DG4)

    const year = Math.floor(quarterIndex / 4) + fixedStartYear
    const quarter = quarterIndex % 4
    const month = quarter * 3

    // Create a UTC timestamp for the first day of the quarter
    const timestamp = Date.UTC(year, month, 1, 0, 0, 0, 0)

    return dateFromTimestamp(timestamp)
}

/**
 * Converts a quarter index to the last day of that quarter
 * @param quarterIndex - Quarter index (0-based)
 * @param startYear - The start year of the timeline (defaults to current year - YEARS_BEFORE_DG4)
 * @returns Date object representing the last day of the quarter
 */
export const quarterIndexToEndDate = (quarterIndex: number | undefined, startYear?: number): Date | null => {
    if (quarterIndex === undefined) {
        return null
    }

    // If no start year provided, use current year - YEARS_BEFORE_DG4
    const currentYear = new Date().getFullYear()
    const fixedStartYear = startYear || (currentYear - GANTT_CONFIG.YEARS_BEFORE_DG4)

    const year = Math.floor(quarterIndex / 4) + fixedStartYear
    const quarter = quarterIndex % 4
    const month = quarter * 3 + 2

    // Create date for the first day of the next month (using UTC)
    const nextMonthTimestamp = Date.UTC(year, month + 1, 1, 0, 0, 0, 0)
    const firstDayNextMonth = dateFromTimestamp(nextMonthTimestamp)

    // Subtract one day to get the last day of the current month
    firstDayNextMonth.setDate(firstDayNextMonth.getDate() - 1)

    return firstDayNextMonth
}

/**
 * Gets the current quarter index based on the current date
 * @param startYear - The start year of the timeline (defaults to current year - YEARS_BEFORE_DG4)
 * @returns Current quarter index (0-based)
 */
export const getCurrentQuarterIndex = (startYear?: number): number => {
    const currentDate = dateFromTimestamp(Date.now()) // Get current date using utility
    const currentYear = currentDate.getFullYear()
    const currentMonth = currentDate.getMonth()
    const currentQuarter = Math.floor(currentMonth / 3) // Calculate current quarter (0-based)

    // If no start year provided, use current year - YEARS_BEFORE_DG4
    const fixedStartYear = startYear || (currentYear - GANTT_CONFIG.YEARS_BEFORE_DG4)

    // Calculate index: 4 quarters per year, starting from our fixed start year
    const quarterIndex = (currentYear - fixedStartYear) * 4 + currentQuarter

    // Ensure the index is within valid bounds for the chart
    return Math.max(0, Math.min(quarterIndex, (GANTT_CONFIG.YEARS_BEFORE_DG4 + GANTT_CONFIG.YEARS_AFTER_DG4) * 4))
}

/**
 * Creates a Date object from a year and quarter
 * @param year - Year for the date
 * @param quarter - Quarter (1-4)
 * @returns Date object for the first day of the specified quarter
 */
export const createDateFromYearAndQuarter = (year: number, quarter: number): Date => {
    const month = (quarter - 1) * 3 // Convert quarter to month (0-based)

    return dateFromTimestamp(Date.UTC(year, month, 1, 0, 0, 0, 0))
}

/**
 * Creates a Date object from an ISO string
 * @param isoString - ISO date string
 * @returns Date object from the ISO string
 */
export const createDateFromISOString = (isoString: string): Date => dateStringToDateUtc(isoString)
