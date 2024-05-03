export interface CaseEditInstance {
    value: string;
    caseObjectKey: keyof Components.Schemas.CaseDto;
    validationRule: string;
    isValid: boolean;
}
