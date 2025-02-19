import { __BaseService } from "./__BaseService"

export class __ProjectService extends __BaseService {
    async getProject(id: string) {
        const project: Components.Schemas.ProjectDataDto = await this.get<Components.Schemas.ProjectDataDto>(`projects/${id}`)
        return project
    }

    async getRevision(projectId: string, revisionId: string) {
        const revision: Components.Schemas.RevisionDataDto = await this.get<Components.Schemas.RevisionDataDto>(`projects/${projectId}/revisions/${revisionId}`)
        return revision
    }

    public async projectExists(projectId: string): Promise<Components.Schemas.ProjectExistsDto> {
        return this.get(`projects/exists?contextId=${projectId}`)
    }

    public async createProject(contextId: string): Promise<Components.Schemas.ProjectDataDto> {
        const res: Components.Schemas.ProjectDataDto = await this.postWithParams(
            "projects",
            {},
            { params: { contextId } },
        )
        return res
    }

    public async createRevision(projectId: string, body: Components.Schemas.CreateRevisionDto): Promise<Components.Schemas.RevisionDataDto> {
        const res: Components.Schemas.RevisionDataDto = await this.post(
            `projects/${projectId}/revisions`,
            { body },
        )
        return res
    }

    public async updateRevision(projectId: string, revisionId: string, body: Components.Schemas.UpdateRevisionDto): Promise<Components.Schemas.RevisionDataDto> {
        const res = await this.put(`projects/${projectId}/revisions/${revisionId}`, { body })
        return res
    }

    public async updateProject(projectId: string, body: Components.Schemas.UpdateProjectDto): Promise<Components.Schemas.ProjectDataDto> {
        const res = await this.put(`projects/${projectId}`, { body })
        return res
    }

    public async compareCases(projectId: string) {
        const res: Components.Schemas.CompareCasesDto[] = await this.get<Components.Schemas.CompareCasesDto[]>(`projects/${projectId}/case-comparison`)
        return res
    }
}

export const GetProjectService = () => new __ProjectService()
