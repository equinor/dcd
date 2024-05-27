/**
 * Used to hide certain profiles in the read-only table if they have no value, but will be viewed when in edit mode.
 */
const hideProfilesWithoutValues = (editMode: boolean, profilesToHide: string[], tableRows: any[]) => {
    if (!editMode) {
        const matchProfileName = (profile: any) => profilesToHide.some((name) => name !== profile.profileName)
        const hideNullValueProfile = (profile: any) => profilesToHide.some((name) => name === profile.profileName && profile.profile === null)
        return tableRows.filter((profile) => matchProfileName(profile) && !hideNullValueProfile(profile))
    }
    return tableRows
}

export default hideProfilesWithoutValues
