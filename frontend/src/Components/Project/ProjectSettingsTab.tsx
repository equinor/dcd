import { useState, ChangeEventHandler } from "react"
import { NativeSelect } from "@equinor/eds-core-react"
import { useAppContext } from "../../Context/AppContext"
import InputContainer from "../Input/Containers/InputContainer"
import InputSwitcher from "../Input/InputSwitcher"

const ProjectSettingsTab = () => {
    const { project, setProject } = useAppContext()
    const [classification, setClassification] = useState(0) // TODO: Get classification from project

    const handlePhysicalUnitChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1].indexOf(Number(e.currentTarget.value)) !== -1 && project) {
            const newPhysicalUnit: Components.Schemas.PhysUnit = Number(e.currentTarget.value) as Components.Schemas.PhysUnit
            const newProject: Components.Schemas.ProjectDto = { ...project }
            newProject.physicalUnit = newPhysicalUnit
            setProject(newProject)
        }
    }

    const handleCurrencyChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([1, 2].indexOf(Number(e.currentTarget.value)) !== -1 && project) {
            const newCurrency: Components.Schemas.Currency = Number(e.currentTarget.value) as Components.Schemas.Currency
            const newProject: Components.Schemas.ProjectDto = { ...project }
            newProject.currency = newCurrency
            setProject(newProject)
        }
    }

    const classificationOptions: { [key: number]: string } = {
        0: "Internal",
        1: "Open",
        2: "Restricted",
        3: "Confidential",
    }

    if (!project) {
        return <div>Loading project data...</div>
    }

    return (
        <InputContainer mobileColumns={2} desktopColumns={2} breakPoint={850}>

            <InputSwitcher
                value={project.physicalUnit === 0 ? "SI" : "Oil field"}
                label="Physical unit"
            >
                <NativeSelect
                    id="physicalUnit"
                    label="Physical unit"
                    onChange={handlePhysicalUnitChange}
                    value={project.physicalUnit}
                >
                    <option key={0} value={0}>SI</option>
                    <option key={1} value={1}>Oil field</option>
                </NativeSelect>
            </InputSwitcher>
            <InputSwitcher
                value={project.currency === 1 ? "NOK" : "USD"}
                label="Currency"
            >
                <NativeSelect
                    id="currency"
                    label="Currency"
                    onChange={handleCurrencyChange}
                    value={project.currency}
                >
                    <option key={1} value={1}>NOK</option>
                    <option key={2} value={2}>USD</option>
                </NativeSelect>
            </InputSwitcher>
            <InputSwitcher
                value={classificationOptions[classification]}
                label="Classification"
            >
                <NativeSelect
                    id="classification"
                    label="Classification"
                    onChange={(e) => setClassification(Number(e.currentTarget.value))}
                    value={classification}
                >
                    {Object.entries(classificationOptions).map(([key, value]) => (
                        <option key={key} value={key}>{value}</option>
                    ))}
                </NativeSelect>
            </InputSwitcher>
        </InputContainer>
    )
}

export default ProjectSettingsTab
