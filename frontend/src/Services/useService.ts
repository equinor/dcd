import { useContext, useEffect, useMemo, useState } from "react"
import { useMsal } from "@azure/msal-react"

import { __ProjectService } from "./ProjectService"
import { __FusionService } from "./FusionService"

import { ServicesContext } from "./ServicesContext"
import { config } from "./config"
import { fusionApiScope } from "../config"

export const useService = (namespace: keyof typeof config) => {
    const { accessToken } = useContext(ServicesContext)

    const { instance, accounts } = useMsal()
    const [fusionAccessToken, setFusionAccessToken] = useState<string>('')
    useEffect(() => {
        instance
            .acquireTokenSilent({
                scopes: fusionApiScope,
                account: accounts[0],
            })
            .then(response => {
                setFusionAccessToken(response.accessToken)
            })
    }, [])

    const service = useMemo(() => {
        switch (namespace) {
            case 'ProjectService':
                return new __ProjectService({
                    ...config.ProjectService,
                    accessToken,
                })
            case 'FusionService':
                return new __FusionService({
                    ...config.FusionService,
                    accessToken: fusionAccessToken,
                })
            default:
                break;
        }
    }, [])

    return service
}
