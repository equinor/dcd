/**
 * Converts a UTC date string to a UTC Date object.
 * All dates stored in the database is in UTC. We need to make sure the Date objects we create are also in UTC.
 * If we ever need to work with non-UTC dates, we should create a new function for that.
 * @param dateString - The UTC date string to convert.
 * @returns The Date object representing the UTC date.
 */
export const dateStringToDateUtc = (dateString: string): Date => {
    // TODO: Make sure the string is a valid date string.

    // Ensure the date string ends with "Z" or "+00:00" to create the Date object as UTC.
    if (!dateString.endsWith("Z") && !dateString.endsWith("+00:00")) {
        // eslint-disable-next-line no-restricted-syntax
        return new Date(`${dateString}Z`)
    }
    // eslint-disable-next-line no-restricted-syntax
    return new Date(dateString)
}

// eslint-disable-next-line no-restricted-syntax
export const getYearFromDateString = (dateString: string): number => new Date(dateString).getFullYear()

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

export const isDefaultDate = (date?: Date | null): boolean => {
    if (date && (toMonthDate(date) === "0001-01" || date.toLocaleDateString("en-CA") === "1-01-01")) {
        return true
    }
    return false
}

export const isDefaultDateString = (dateString?: string | null): boolean => {
    const date = new Date(dateString ?? "")
    if (date && (toMonthDate(date) === "0001-01" || date.toLocaleDateString("en-CA") === "1-01-01")) {
        return true
    }
    return false
}

export const defaultDate = () => dateStringToDateUtc("0001-01-01T00:00:00+00:00")

export function formatDate(isoDateString: string): string {
    if (isoDateString === "0001-01-01T00:00:00+00:00" || isoDateString === "0001-01-01T00:00:00.000Z") {
        return "_"
    }
    const date = dateStringToDateUtc(isoDateString)
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
