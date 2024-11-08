export enum UserRole {
    Editor = 1,
    Viewer = 0
}

export interface User {
    UserId?: string
    Role?: UserRole
}

export type Position = {
    id: string;
    name: string;
    project: {
        id: string;
        name: string;
    };
};
export type Manager = {
    azureUniqueId: string;
    name?: string;
    pictureSrc?: string;
    department?: string;
    accountType?: PersonAccountType[keyof PersonAccountType];
};

export enum PersonAccountType {
    Employee = "Employee",
    Consultant = "Consultant",
    Enterprise = "Enterprise",
    External = "External",
    ExternalHire = "External Hire"
}

export type PersonInfo = {
    azureId: string;
    name?: string;
    jobTitle?: string;
    department?: string;
    mail?: string;
    upn?: string;
    mobilePhone?: string;
    accountType?: PersonAccountType[keyof PersonAccountType];
    officeLocation?: string;
    managerAzureUniqueId?: string;
};
export type PersonDetails = PersonInfo & {
    positions?: Position[];
    manager?: Manager;
};
