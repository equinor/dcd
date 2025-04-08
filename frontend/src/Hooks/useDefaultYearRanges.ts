import { getYearFromDateString } from "../Utils/DateUtils"

import { useCaseApiData } from "./useCaseApiData"

/**
 * Returns default year ranges based on case data. used for default view of tables if they are not set from profile data
 * @returns Object containing various default year range constants
 */
export const useDefaultYearRanges = (): {
    DEFAULT_CO2_EMISSIONS_YEARS: [number, number],
    DEFAULT_DRILLING_SCHEDULE_YEARS: [number, number],
    DEFAULT_CASE_COST_YEARS: [number, number],
    DEFAULT_SUMMARY_YEARS: [number, number],
    DEFAULT_PRODUCTION_PROFILES_YEARS: [number, number],
} => {
    const { apiData } = useCaseApiData()
    const dg4Date = apiData?.case.dg4Date
    const currentYear = new Date().getFullYear()
    const dg4Year = dg4Date ? getYearFromDateString(dg4Date) : currentYear

    return {
        DEFAULT_CO2_EMISSIONS_YEARS: [dg4Year - 1, dg4Year + 15],
        DEFAULT_DRILLING_SCHEDULE_YEARS: [currentYear, dg4Year + 1],
        DEFAULT_CASE_COST_YEARS: [2023, 2033],
        DEFAULT_SUMMARY_YEARS: [2023, 2033],
        DEFAULT_PRODUCTION_PROFILES_YEARS: [dg4Year - 1, dg4Year + 15],
    }
}

export default useDefaultYearRanges
