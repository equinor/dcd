import { NativeSelect } from "@equinor/eds-core-react"
import { ChangeEventHandler } from "react"
import styled from "styled-components"
import { useAppContext } from "../../Context/AppContext"

const Wrapper = styled.div`
    margin: 1rem;
    display: flex;
    flex-direction: row;
`

const RowWrapper = styled.div`
    margin: 1rem;
    display: flex;
    flex-direction: row;
    justify-content: space-between;
`

const InputField = styled(NativeSelect)`
    margin-right: 2rem;
    width: 20rem;
    padding-bottom: 2em;
 `

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
        <Wrapper>
            <RowWrapper>
                <InputField
                    id="physicalUnit"
                    label="Physical unit"
                    onChange={handlePhysicalUnitChange}
                    value={project.physUnit}
                >
                    <option key={0} value={0}>SI</option>
                    <option key={1} value={1}>Oil field</option>
                </InputField>
                <InputField
                    id="currency"
                    label="Currency"
                    onChange={handleCurrencyChange}
                    value={project.currency}
                >
                    <option key={1} value={1}>NOK</option>
                    <option key={2} value={2}>USD</option>
                </InputField>
            </RowWrapper>
        </Wrapper>
    )
}

export default ProjectSettingsTab
