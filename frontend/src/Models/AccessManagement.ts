export enum UserRole {
    Editor = 1,
    Viewer = 0
}

export interface User {
    UserId?: string
    Role?: UserRole
}
