import { useContext, useMemo } from "react"

import { __ProjectService } from "./ProjectService"

import { ServicesContext } from "./ServicesContext"
import { config } from "./config"

export const useService = (namespace: keyof typeof config) => {
    const { accessToken } = useContext(ServicesContext)

    const service = useMemo(() => {
        switch (namespace) {
            case 'ProjectService':
                return new __ProjectService({
                    ...config.ProjectService,
                    accessToken,
                })
            default:
                break;
        }
    }, [])

    return service
}
