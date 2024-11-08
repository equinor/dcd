import { useCallback } from "react"
import { useCurrentUser } from "@equinor/fusion-framework-react-app/framework"
import { usePeopleClient, peopleClientHeaders } from "./usePeopleClient"

export const usePeopleApi = () => {
    const httpClient = usePeopleClient()
    const user = useCurrentUser()

    const getById = useCallback(async (id: string): Promise<any> => httpClient.json(`/persons/${id}`, {
        headers: peopleClientHeaders,
    }), [httpClient])

    const search = useCallback(async (query: string): Promise<any> => httpClient.json(`/persons?query=${query}&api-version=1.0`, {
        headers: peopleClientHeaders,
    }), [httpClient])

    const getPersonRoles = useCallback(async (): Promise<any> => httpClient.json(
        `/persons/${user?.localAccountId}?$api-version=3.0&$expand=roles`,
        {
            headers: peopleClientHeaders,
        },
    ), [httpClient, user?.localAccountId])

    return { getPersonRoles, getById, search }
}
