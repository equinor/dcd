export type ResourceName = "case" | "topside" | "surf" | "substructure" | "drainageStrategy" | "transport"
export type ResourcePropertyKey =
    keyof Components.Schemas.TopsideDto |
    keyof Components.Schemas.SurfDto |
    keyof Components.Schemas.SubstructureDto |
    keyof Components.Schemas.TransportDto |
    keyof Components.Schemas.CaseDto |
    keyof Components.Schemas.DrainageStrategyDto

export interface EditInstance {
    uuid: string; // unique identifier for the edit
    timeStamp: number; // the time the edit was made
    newValue: string | number | undefined; // the value after the edit
    previousValue: string | number | undefined; // the value before the edit
    inputLabel: string; // the label of the input field being edited
    projectId: string; // the project id
    resourceName: ResourceName; // the asset being edited
    resourcePropertyKey: ResourcePropertyKey; // the key of the asset being edited
    resourceId?: string; // the id of the asset being edited
    caseId?: string; // the case id
    newDisplayValue?: string | number | undefined; // the displayed new value in case of when the value submitted is not what the user should see
    previousDisplayValue?: string | number | undefined; // the displayed previous value in case of when the value submitted is not what the user should see

}

export interface EditEntry {
    caseId: string;
    currentEditId: string;
}
