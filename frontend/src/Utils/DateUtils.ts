export const defaultDg4Date = "2030-01-01T00:00:00Z"

/**
 * Converts a UTC date string to a UTC Date object.
 * All dates stored in the database are in UTC. We need to make sure the Date objects we create are also in UTC.
 * If we ever need to work with non-UTC dates, we should create a new function for that.
 * @param dateString - The UTC date string to convert.
 * @returns The Date object representing the UTC date.
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

export const getYearFromDateString = (dateString: string): number => new Date(dateString).getFullYear()

export const dateFromTimestamp = (timestamp: number): Date => new Date(timestamp)

export const sortUtcDateStrings = (a: string, b: string): number => {
    const dateA = dateStringToDateUtc(a)
    const dateB = dateStringToDateUtc(b)

    return dateA.getTime() - dateB.getTime()
}

export const toMonthDate = (date?: Date | null): string | undefined => {
    if (Number.isNaN(date?.getTime())) {
        return undefined
    }

    return date?.toISOString().substring(0, 7)
}

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
