export interface EditInstance {
    uuid: string; // unique identifier for the edit
    newValue: string | number | undefined; // the value after the edit
    previousValue: string | number| undefined; // the value before the edit
    objectKey: string | number; // the key of the entry in the object being edited
    inputLabel: string; // the label of the input field being edited
    timeStamp: number;
    level: "project" | "case";
    objectId: string; // the id of the object being edited (project id or case id)
}
