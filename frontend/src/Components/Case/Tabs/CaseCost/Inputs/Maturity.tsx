import React, { ChangeEventHandler } from "react"
import { NativeSelect } from "@equinor/eds-core-react"
import InputSwitcher from "../../../../Input/Components/InputSwitcher"
import { useCaseContext } from "../../../../../Context/CaseContext"
import useQuery from "../../../../../Hooks/useQuery"
import { GetSurfService } from "../../../../../Services/SurfService"
import { useProjectContext } from "../../../../../Context/ProjectContext"

const Maturity: React.FC = () => {
    const {
        surf,
        setSurf,
        surfCost,
        projectCase,
    } = useCaseContext()
    const { project } = useProjectContext()

    if (!projectCase || !surf) { return null }

    const maturityOptions: { [key: string]: string } = {
        0: "A",
        1: "B",
        2: "C",
        3: "D",
    }

    const { updateData: updateSurf } = useQuery({
        queryKey: ["surfData", project!.id, projectCase.id],
        mutationFn: async (updatedData: Components.Schemas.APIUpdateSurfDto) => {
            const surfService = await GetSurfService()
            return surfService.updateSurf(project!.id, projectCase.id, surf.id, updatedData)
        },
    })

    const updatedAndSetSurf = (surfItem: Components.Schemas.SurfWithProfilesDto) => {
        const newSurf: Components.Schemas.SurfWithProfilesDto = { ...surfItem }
        if (surfCost) {
            newSurf.costProfile = surfCost
        }
        setSurf(newSurf)
    }

    const handleSurfMaturityChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1, 2, 3].indexOf(Number(e.currentTarget.value)) !== -1) {
            const newMaturity: Components.Schemas.Maturity = Number(e.currentTarget.value) as Components.Schemas.Maturity
            const newSurf = { ...surf }
            newSurf.maturity = newMaturity
            updatedAndSetSurf(newSurf as Components.Schemas.SurfWithProfilesDto)
            updateSurf("maturity", newMaturity)
        }
    }

    return (
        <InputSwitcher value={maturityOptions[surf?.maturity ?? "defaultKey"]} label="Maturity">
            <NativeSelect
                id="maturity"
                label=""
                onChange={handleSurfMaturityChange}
                value={surf?.maturity ?? ""}
            >
                {Object.keys(maturityOptions).map((key) => (
                    <option key={key} value={key}>{maturityOptions[key]}</option>
                ))}
            </NativeSelect>
        </InputSwitcher>
    )
}

export default Maturity
