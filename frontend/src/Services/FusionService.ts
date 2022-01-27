import { useEffect, useMemo, useState } from "react"
import { useMsal } from "@azure/msal-react"

import { fusionApiScope } from "../config"
import { config } from "./config"
import { __BaseService } from "./__BaseService"

export class __FusionService extends __BaseService {
    getFusionProjects() {
        return this.get('/contexts?$filter=type%20in%20(%27OrgChart%27)')
    }
}

export const useFusionService = () => {
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

    const fusionService = useMemo(() => {
        return new __FusionService({
            ...config.FusionService,
            accessToken: fusionAccessToken,
        })
    }, [])

    return fusionService
}
