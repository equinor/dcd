import { useState, ChangeEventHandler, useEffect } from "react"
import { Input, NativeSelect } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import { useProjectContext } from "../../Context/ProjectContext"
import InputSwitcher from "../Input/Components/InputSwitcher"
import { PROJECT_CLASSIFICATION } from "../../Utils/constants"

const ProjectSettingsTab = () => {
    const { project, projectEdited, setProjectEdited } = useProjectContext()
    const [classification, setClassification] = useState<number | undefined>(undefined)
    const [dummyRole, setDummyRole] = useState(0) // TODO: Get role from user
    const [oilPriceUSD, setOilPriceUSD] = useState<number | undefined>(undefined)
    const [gasPriceNOK, setGasPriceNOK] = useState<number | undefined>(undefined)
    const [discountRate, setDiscountRate] = useState<number | undefined>(undefined)

    useEffect(() => {
        if (project) {
            setClassification(project.classification)
            setOilPriceUSD(projectEdited?.oilPriceUSD ?? oilPriceUSD)
            setGasPriceNOK(projectEdited?.gasPriceNOK ?? gasPriceNOK)
            setDiscountRate(projectEdited?.discountRate ?? discountRate)
        }
    }, [project, projectEdited])

    const handlePhysicalUnitChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1].indexOf(Number(e.currentTarget.value)) !== -1 && project) {
            const newPhysicalUnit: Components.Schemas.PhysUnit = Number(e.currentTarget.value) as Components.Schemas.PhysUnit
            const newProject: Components.Schemas.ProjectWithAssetsDto = { ...project }
            newProject.physicalUnit = newPhysicalUnit
            setProjectEdited(newProject)
        }
    }

    const handleCurrencyChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([1, 2].indexOf(Number(e.currentTarget.value)) !== -1 && project) {
            const newCurrency: Components.Schemas.Currency = Number(e.currentTarget.value) as Components.Schemas.Currency
            const newProject: Components.Schemas.ProjectWithAssetsDto = { ...project }
            newProject.currency = newCurrency
            setProjectEdited(newProject)
        }
    }

    const handleClassificationChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1, 2, 3].indexOf(Number(e.currentTarget.value)) !== -1 && project) {
            setClassification(Number(e.currentTarget.value))
            const newClassification: Components.Schemas.ProjectClassification = Number(e.currentTarget.value) as unknown as Components.Schemas.ProjectClassification
            const newProject: Components.Schemas.ProjectWithAssetsDto = { ...project }
            newProject.classification = newClassification
            setProjectEdited(newProject)
        }
    }

    const handleOilPriceChange: ChangeEventHandler<HTMLInputElement> = (e) => {
        const newOilPrice = parseFloat(e.currentTarget.value)
        if (!Number.isNaN(newOilPrice) && project) {
            setOilPriceUSD(newOilPrice)
            const newProject = { ...project, oilPriceUSD: newOilPrice }
            setProjectEdited(newProject)
        }
    }

    const handleGasPriceChange: ChangeEventHandler<HTMLInputElement> = (e) => {
        const newGasPrice = parseFloat(e.currentTarget.value)
        if (!Number.isNaN(newGasPrice) && project) {
            const newProject: Components.Schemas.ProjectWithAssetsDto = { ...project, gasPriceNOK: newGasPrice }
            setGasPriceNOK(newGasPrice)
            setProjectEdited(newProject)
        }
    }

    const handleDiscountRateChange: ChangeEventHandler<HTMLInputElement> = (e) => {
        const newDiscountRate = parseFloat(e.currentTarget.value)
        if (!Number.isNaN(newDiscountRate) && project) {
            const newProject: Components.Schemas.ProjectWithAssetsDto = { ...project, discountRate: newDiscountRate }
            setDiscountRate(newDiscountRate)
            setProjectEdited(newProject)
        }
    }

    if (!project) {
        return <div>Loading project data...</div>
    }

    return (
        <Grid container direction="column" spacing={2}>
            <Grid item>
                <InputSwitcher
                    value={project.physicalUnit === 0 ? "SI" : "Oil field"}
                    label="Physical unit"
                >
                    <NativeSelect
                        id="physicalUnit"
                        label=""
                        onChange={handlePhysicalUnitChange}
                        value={projectEdited ? projectEdited.physicalUnit : project.physicalUnit}
                    >
                        <option key={0} value={0}>SI</option>
                        <option key={1} value={1}>Oil field</option>
                    </NativeSelect>
                </InputSwitcher>
            </Grid>
            <Grid item>
                <InputSwitcher
                    value={project.currency === 1 ? "NOK" : "USD"}
                    label="Currency"
                >
                    <NativeSelect
                        id="currency"
                        label=""
                        onChange={handleCurrencyChange}
                        value={projectEdited ? projectEdited.currency : project.currency}
                    >
                        <option key={1} value={1}>NOK</option>
                        <option key={2} value={2}>USD</option>
                    </NativeSelect>
                </InputSwitcher>
            </Grid>
            <Grid item>
                {dummyRole === 0 && (
                    <InputSwitcher
                        value={classification !== undefined ? PROJECT_CLASSIFICATION[classification].label : "Not set"}
                        label="Classification"
                    >
                        <NativeSelect
                            id="classification"
                            label=""
                            onChange={(e) => handleClassificationChange(e)}
                            value={classification || undefined}
                        >
                            {Object.entries(PROJECT_CLASSIFICATION).map(([key, value]) => (
                                <option key={key} value={key}>{value.label}</option>
                            ))}
                        </NativeSelect>
                    </InputSwitcher>
                )}
            </Grid>
            <Grid item>
                <InputSwitcher
                    value={String(projectEdited?.oilPriceUSD ?? project.oilPriceUSD ?? oilPriceUSD)}
                    label="Oil Price (USD)"
                >
                    <Input
                        type="number"
                        step="0.01"
                        value={oilPriceUSD}
                        onChange={handleOilPriceChange}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item>
                <InputSwitcher
                    value={String(projectEdited?.gasPriceNOK ?? project.gasPriceNOK ?? gasPriceNOK)}
                    label="Gas Price (NOK)"
                >
                    <Input
                        type="number"
                        step="0.01"
                        value={gasPriceNOK}
                        onChange={handleGasPriceChange}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item>
                <InputSwitcher
                    value={String(projectEdited?.discountRate ?? project.discountRate ?? discountRate)}
                    label="Discount Rate (%)"
                >
                    <Input
                        type="number"
                        step="0.01"
                        value={discountRate}
                        onChange={handleDiscountRateChange}
                    />
                </InputSwitcher>
            </Grid>
        </Grid>
    )
}

export default ProjectSettingsTab
