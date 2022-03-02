import { Project } from "./Project"

export class RecentProject {
    id: string
    name: string
    phase: string
    description: string | null
    createdAt: string | null

    constructor(project: Project) {
        this.id = project.id
        this.name = project.name
        this.phase = project.phase ? project.phase.toString() ?? "" : ""
        this.description = project.description
        this.createdAt = project.createdAt!.toDateString()
    }
}
