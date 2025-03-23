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
