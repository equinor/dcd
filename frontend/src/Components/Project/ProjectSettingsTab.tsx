import { NativeSelect } from "@equinor/eds-core-react"
import { ChangeEventHandler } from "react"
import { useAppContext } from "../../Context/AppContext"
import InputContainer from "../Input/InputContainer"

const ProjectSettingsTab = () => {
    const { project, setProject } = useAppContext()

    const handlePhysicalUnitChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1].indexOf(Number(e.currentTarget.value)) !== -1 && project) {
            const newPhysicalUnit: Components.Schemas.PhysUnit = Number(e.currentTarget.value) as Components.Schemas.PhysUnit
            const newProject: Components.Schemas.ProjectDto = { ...project }
            newProject.physUnit = newPhysicalUnit
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

    if (!project) {
        return <div>Loading project data...</div>
    }

    return (
        <InputContainer mobileColumns={2} desktopColumns={2} breakPoint={850}>
            <NativeSelect
                id="physicalUnit"
                label="Physical unit"
                onChange={handlePhysicalUnitChange}
                value={project.physUnit}
            >
                <option key={0} value={0}>SI</option>
                <option key={1} value={1}>Oil field</option>
            </NativeSelect>
            <NativeSelect
                id="currency"
                label="Currency"
                onChange={handleCurrencyChange}
                value={project.currency}
            >
                <option key={1} value={1}>NOK</option>
                <option key={2} value={2}>USD</option>
            </NativeSelect>
        </InputContainer>
    )
}

export default ProjectSettingsTab
