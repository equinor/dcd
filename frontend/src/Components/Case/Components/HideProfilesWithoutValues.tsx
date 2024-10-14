import { ITimeSeriesTableData } from "@/Models/ITimeSeries"

/**
 * Used to hide certain profiles in the read-only table if they have no value, but will be viewed when in edit mode.
 */
const hideProfilesWithoutValues = (editMode: boolean, profilesToHide: string[], tableRows: ITimeSeriesTableData[]) => {
    if (!editMode) {
        const matchProfileName = (profile: ITimeSeriesTableData) => profilesToHide.some((name) => name !== profile?.profileName)
        const hideNullValueProfile = (profile: ITimeSeriesTableData) => profilesToHide.some((name) => name === profile.profileName && profile.profile === null)
        return tableRows.filter((profile) => matchProfileName(profile) && !hideNullValueProfile(profile))
    }
    return tableRows
}

export default hideProfilesWithoutValues
