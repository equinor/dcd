import { ProjectMemberRole } from "./enums"

export interface User {
    UserId: string
    Role: ProjectMemberRole
}
