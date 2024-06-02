export interface EditInstance {
    uuid: string; // unique identifier for the edit
    newValue: string | number | undefined; // the value after the edit
    previousValue: string | number | undefined; // the value before the edit
    newDisplayValue?: string | number | undefined; // the displayed new value in case of when the value submitted is not what the user should see
    previousDisplayValue?: string | number | undefined; // the displayed previous value in case of when the value submitted is not what the user should see
    objectKey: string | number; // the key of the entry in the object being edited
    inputLabel: string; // the label of the input field being edited
    timeStamp: number;
    level: "project" | "case";
    objectId: string; // the id of the object being edited (project id or case id)
}

export interface EditEntry {
    caseId: string;
    currentEditId: string;
}

export type ServiceName = "case" | "topside" | "surf" | "substructure" | "transport";
