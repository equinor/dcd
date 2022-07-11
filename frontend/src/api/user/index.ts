/* eslint-disable max-len */
import axios from "axios"
import apiConfig from "../apiConfig"
import { getDefaultHeader } from "../common/defaultHeader"
import { HttpStatusCode } from "../common/httpStatusCodes"

/**
 * Returns user privileges for the logged in user.
 */
export const getMe = async (): Promise<any> => {
    const config = apiConfig()
    const url = `${config.baseApiUrl}/projects`
    try {
        const scopes = [config.scopes[0]]
        const response = await axios.get(url, await getDefaultHeader(scopes))
        if (response.status !== HttpStatusCode.Ok) { throw new Error(`Unexpected return code (${response.status}) returned from /user/me endpoint`) }
        return
    } catch (error) {
        console.error("Failed to get user details", error)
        throw error
    }
}
