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
    YEARS_BEFORE_DG4: 20, // Show 20 years in the past
    YEARS_AFTER_DG4: 40, // Show 40 years in the future
    DEFAULT_DG4_YEAR: new Date().getFullYear(), // Use current year as default
}

/**
 * Generates quarterly periods based on the current year
 * @param _ - Ignored parameter (kept for backward compatibility)
 * @returns Array of quarterly periods
 */
export const generateDynamicQuarterlyPeriods = (_?: Date | null) => {
    const quarters = []
    let index = 0

    // Always use current year as the center point
    const currentYear = new Date().getFullYear()

    // Calculate start and end years based on configuration
    const startYear = currentYear - GANTT_CONFIG.YEARS_BEFORE_DG4
    const endYear = currentYear + GANTT_CONFIG.YEARS_AFTER_DG4

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
 * @param startYear - The start year of the timeline (defaults to current year - YEARS_BEFORE_DG4)
 * @returns Quarter index (0-based)
 */
export const dateToQuarterIndex = (date: Date | undefined | null, startYear?: number): number | undefined => {
    if (!date || Number.isNaN(date.getTime())) {
        return undefined
    }

    const year = date.getFullYear()
    const month = date.getMonth()

    // If no start year provided, use current year - YEARS_BEFORE_DG4
    const currentYear = new Date().getFullYear()
    const fixedStartYear = startYear || (currentYear - GANTT_CONFIG.YEARS_BEFORE_DG4)

    // Only support dates within our calculated range
    const fixedEndYear = fixedStartYear + GANTT_CONFIG.YEARS_BEFORE_DG4 + GANTT_CONFIG.YEARS_AFTER_DG4

    if (year < fixedStartYear || year > fixedEndYear) {
        return undefined
    }

    // Calculate quarter (0-based)
    const quarter = Math.floor(month / 3)

    // Calculate index: 4 quarters per year, starting from our fixed start year
    return (year - fixedStartYear) * 4 + quarter
}

/**
 * Converts a quarter index to the first day of that quarter
 * @param quarterIndex - Quarter index (0-based)
 * @param startYear - The start year of the timeline (defaults to current year - YEARS_BEFORE_DG4)
 * @returns Date object representing the first day of the quarter
 */
export const quarterIndexToStartDate = (quarterIndex: number | undefined, startYear?: number): Date | null => {
    if (quarterIndex === undefined) {
        return null
    }

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
