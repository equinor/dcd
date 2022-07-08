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
        const scopes = ["api://9b125a0c-4907-43b9-8db2-ff405d6b0524/.default"]
        console.log("Before getDefaultHeader()")
        const response = await axios.get(url, await getDefaultHeader(scopes))
        console.log("After getDefaultHeader: ", response.status)
        if (response.status !== HttpStatusCode.Ok) { throw new Error(`Unexpected return code (${response.status}) returned from /user/me endpoint`) }
        return
    } catch (error) {
        console.error("Failed to get user details", error)
        throw error
    }
}
