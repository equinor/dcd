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

export const dateFromString = (dateString?: string | null): Date => new Date(dateString ?? "")

export const defaultDate = () => new Date("0001-01-01")

export function formatDate(isoDateString: string): string {
    if (isoDateString === "0001-01-01T00:00:00+00:00" || isoDateString === "0001-01-01T00:00:00.000Z") {
        return "_"
    }
    const date = new Date(isoDateString)
    const options: Intl.DateTimeFormatOptions = {
        month: "long",
        year: "numeric",
    }
    return new Intl.DateTimeFormat("no-NO", options).format(date)
}

export const formatDateAndTime = (dateString: string | undefined | null) => {
    if (!dateString) { return "" }
    const date = new Date(dateString)
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
    const date = new Date(dateString)
    const options: Intl.DateTimeFormatOptions = {
        day: "2-digit",
        month: "short",
        year: "numeric",
    }
    return new Intl.DateTimeFormat("en-GB", options)
        .format(date)
        .replace(",", "")
}
