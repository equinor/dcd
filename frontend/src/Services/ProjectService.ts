import { useContext, useMemo } from "react"
import { config } from "./config"

import { ServicesContext } from "./ServicesContext"
import { __BaseService } from "./__BaseService"

export class __ProjectService extends __BaseService {
    getProjects() {
        return this.get('')
    }

    getProjectByID(id: string) {
        return this.get(`/${id}`)
    }
}

export const useProjectService = () => {
    const { accessToken } = useContext(ServicesContext)

    const projectService = useMemo(() => {
        return new __ProjectService({
            ...config.ProjectService,
            accessToken,
        })
    }, [])

    return projectService
}
