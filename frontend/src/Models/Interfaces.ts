export interface EditInstance {
    uuid: string;
    newValue: string | undefined;
    previousValue: string | undefined;
    objectKey: keyof Components.Schemas.CaseDto | Components.Schemas.ProjectDto;
    inputLabel: string;
    timeStamp: string;
    level: "project" | "case"
}
