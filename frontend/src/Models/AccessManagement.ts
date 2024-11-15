export enum UserRole {
    Editor = 1,
    Viewer = 0
}

export interface User {
    UserId: string
    Role: UserRole
}

export interface FusionPersonV1 {
    azureUniqueId: string
    mail: string
    name: string
    accountType: string
    accountClassification: string
}
