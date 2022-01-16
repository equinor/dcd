import { useContext, useMemo } from "react"

import { __ProjectService } from "./ProjectService"

import { ServicesContext } from "./ServicesContext"
import { config } from "./config"

export const useService = (service: keyof typeof config) => {
    const { accessToken } = useContext(ServicesContext)

    const serviceConfig = useMemo(() => config[service], [service])

    switch (service) {
        case 'ProjectService':
            return new __ProjectService({ ...serviceConfig, accessToken })
        default:
            console.warn(`[useService] No config found for ${service}`)
            break
    }
}
