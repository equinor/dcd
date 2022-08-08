import { obtainAccessToken } from "../../auth/tokenProvider"

interface DefaultHeaders {
    headers: {
        "Content-Type": string;
        Authorization: string;
    };
}
/**
 * Returns a default header to be used with Pitstop related API calls
 *
 * @param {string[]} scopes
 * @return {*}  {Promise<HeaderContainer>}
 */
export const getDefaultHeader = async (scopes: string[]): Promise<DefaultHeaders> => {
    const token = await obtainAccessToken(scopes)
    return {
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
        },
    }
}
