export interface EditInstance {
    uuid: string; // unique identifier for the edit
    newValue: string | undefined; // the value after the edit
    previousValue: string | undefined; // the value before the edit
    objectKey: keyof Components.Schemas.CaseDto | Components.Schemas.ProjectDto; // the key of the entry in the object being edited
    inputLabel: string; // the label of the input field being edited
    timeStamp: string; // (hh:mm)
    level: "project" | "case";
    objectId: string; // the id of the object being edited (project id or case id)
}
