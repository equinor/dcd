import { __BaseService } from "./__BaseService"

class ProjectMembersService extends __BaseService {
    public async getPeople(projectId: string): Promise<Components.Schemas.ProjectMemberDto[]> {
        const res: Components.Schemas.ProjectMemberDto[] = await this.get<Components.Schemas.ProjectMemberDto[]>(
            `projects/${projectId}/members`,
        )

        return res
    }

    async deletePerson(projectId: string, userId: string) {
        const res: Components.Schemas.ProjectMemberDto = await this.delete<Components.Schemas.ProjectMemberDto>(`projects/${projectId}/members/${userId}`)

        return res
    }

    public async addPerson(projectId: string, body: Components.Schemas.CreateProjectMemberDto): Promise<Components.Schemas.ProjectMemberDto> {
        const res: Components.Schemas.ProjectMemberDto = await this.post<Components.Schemas.ProjectMemberDto>(
            `projects/${projectId}/members`,
            { body },
        )

        return res
    }

    public async updatePerson(projectId: string, body: Components.Schemas.UpdateProjectMemberDto): Promise<Components.Schemas.ProjectMemberDto> {
        const res: Components.Schemas.ProjectMemberDto = await this.put<Components.Schemas.ProjectMemberDto>(
            `projects/${projectId}/members`,
            { body },
        )

        return res
    }
}

export const GetProjectMembersService = () => new ProjectMembersService()
