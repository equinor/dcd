import React from "react"
import NumberInputWithValidation from "./Components/NumberInputWithValidation"
import InputSwitcher from "./Components/InputSwitcher"
import useDataEdits from "../../Hooks/useDataEdits"
import { useCaseContext } from "../../Context/CaseContext"
import { useProjectContext } from "../../Context/ProjectContext"
import { ServiceKey, ServiceName } from "../../Models/Interfaces"

interface CaseEditInputProps {
    label: string;
    onSubmit?: (value: number) => void;
    value: number | undefined;
    serviceName: ServiceName;
    serviceKey: ServiceKey;
    serviceId?: string;
    integer: boolean;
    disabled?: boolean;
    unit?: string;
    allowNegative?: boolean;
    min?: number;
    max?: number;
}

const SwitchableNumberInput: React.FC<CaseEditInputProps> = ({
    label,
    onSubmit, // this will be obsolete when we introduce autosave.
    value,
    serviceKey,
    serviceName,
    serviceId,
    integer,
    disabled,
    unit,
    allowNegative,
    min,
    max,
}: CaseEditInputProps) => {
    const { addEdit } = useDataEdits()
    const { projectCase } = useCaseContext()
    const { project } = useProjectContext()

    if (!projectCase || !project) { return null }

    const addToEditsAndSubmit = (insertedValue: number) => {
        if (onSubmit) {
            onSubmit(insertedValue) // this will be obsolete when we introduce autosave.
        }

        addEdit(
            insertedValue, // newValue
            value, // previousValue
            label, // inputLabel
            project.id, // projectId
            serviceName, // serviceName
            serviceKey, // serviceKey
            serviceId, // serviceId
            projectCase.id, // caseId
        )
    }

    return (
        <InputSwitcher
            label={label}
            value={`${value}`}
        >
            <NumberInputWithValidation
                onSubmit={addToEditsAndSubmit}
                defaultValue={value}
                integer={integer}
                disabled={disabled}
                unit={unit}
                allowNegative={allowNegative}
                min={min}
                max={max}
            />
        </InputSwitcher>
    )
}

export default SwitchableNumberInput
