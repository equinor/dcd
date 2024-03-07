import {
    ChangeEventHandler,
    FormEventHandler,
} from "react"
import { NativeSelect } from "@equinor/eds-core-react"
import TextArea from "@equinor/fusion-react-textarea/dist/TextArea"
import Grid from "@mui/material/Grid"
import CaseNumberInput from "../../Input/CaseNumberInput"
import InputSwitcher from "../../Input/InputSwitcher"
import Gallery from "../../Gallery/Gallery"
import { useCaseContext } from "../../../Context/CaseContext"

const CaseDescriptionTab = () => {
    const { projectCase, projectCaseEdited, setProjectCaseEdited } = useCaseContext()

    if (!projectCase) return null

    const productionStrategyOptions = {
        0: "Depletion",
        1: "Water injection",
        2: "Gas injection",
        3: "WAG",
        4: "Mixed",
    }

    const artificialLiftOptions = {
        0: "No lift",
        1: "Gas lift",
        2: "Electrical submerged pumps",
        3: "Subsea booster pumps",
    }

    const handleDescriptionChange: FormEventHandler<any> = async (e) => {
        const newCase = { ...projectCase }
        newCase.description = e.currentTarget.value
        setProjectCaseEdited(newCase)
    }

    const handleFacilitiesAvailabilityChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...projectCase }
        const newfacilitiesAvailability = e.currentTarget.value.length > 0
            ? Math.min(Math.max(Number(e.currentTarget.value), 0), 100) : undefined
        if (newfacilitiesAvailability !== undefined) {
            newCase.facilitiesAvailability = newfacilitiesAvailability / 100
        } else { newCase.facilitiesAvailability = 0 }
        setProjectCaseEdited(newCase)
    }

    const handleProducerCountChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...projectCase }
        newCase.producerCount = e.currentTarget.value.length > 0 ? Math.max(Number(e.currentTarget.value), 0) : 0
        setProjectCaseEdited(newCase)
    }

    const handleGasInjectorCountChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...projectCase }
        newCase.gasInjectorCount = e.currentTarget.value.length > 0 ? Math.max(Number(e.currentTarget.value), 0) : 0
        setProjectCaseEdited(newCase)
    }

    const handletWaterInjectorCountChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...projectCase }
        newCase.waterInjectorCount = e.currentTarget.value.length > 0 ? Math.max(Number(e.currentTarget.value), 0) : 0
        setProjectCaseEdited(newCase)
    }

    const handleProductionStrategyChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1, 2, 3, 4].indexOf(Number(e.currentTarget.value)) !== -1) {
            const newProductionStrategy: Components.Schemas.ProductionStrategyOverview = Number(e.currentTarget.value) as Components.Schemas.ProductionStrategyOverview
            const newCase = { ...projectCase }
            newCase.productionStrategyOverview = newProductionStrategy
            setProjectCaseEdited(newCase)
        }
    }

    const handleArtificialLiftChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1, 2, 3].indexOf(Number(e.currentTarget.value)) !== -1) {
            const newArtificialLift: Components.Schemas.ArtificialLift = Number(e.currentTarget.value) as Components.Schemas.ArtificialLift
            const newCase = { ...projectCase }
            newCase.artificialLift = newArtificialLift
            setProjectCaseEdited(newCase)
        }
    }

    const getFacilitiesAvailabilityDefaultValue = () => {
        if (projectCaseEdited) {
            return projectCaseEdited.facilitiesAvailability !== undefined
                ? projectCaseEdited.facilitiesAvailability * 100
                : undefined
        }
        return projectCase?.facilitiesAvailability !== undefined
            ? projectCase.facilitiesAvailability * 100
            : undefined
    }

    const defaultValue = getFacilitiesAvailabilityDefaultValue()

    return (
        <Grid container spacing={2}>
            <Grid item xs={12}>
                <Gallery />
                <InputSwitcher label="Description" value={projectCaseEdited ? projectCaseEdited.description : projectCase?.description ?? ""}>
                    <TextArea
                        id="description"
                        placeholder="Enter a description"
                        onInput={handleDescriptionChange}
                        value={projectCaseEdited ? projectCaseEdited.description : projectCase?.description ?? ""}
                        cols={10000}
                        rows={8}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher
                    label="Production wells"
                    value={projectCaseEdited ? projectCaseEdited.producerCount.toString() : projectCase.producerCount.toString()}
                >
                    <CaseNumberInput
                        onChange={handleProducerCountChange}
                        defaultValue={projectCaseEdited ? projectCaseEdited.producerCount : projectCase?.producerCount}
                        integer
                        min={0}
                        max={100000}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher
                    label="Water injector wells"
                    value={projectCaseEdited ? projectCaseEdited.producerCount.toString() : projectCase.waterInjectorCount.toString()}
                >
                    <CaseNumberInput
                        onChange={handletWaterInjectorCountChange}
                        defaultValue={projectCaseEdited ? projectCaseEdited.waterInjectorCount : projectCase?.waterInjectorCount}
                        integer
                        disabled={false}
                        min={0}
                        max={100000}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher
                    label="Gas injector wells"
                    value={projectCaseEdited ? projectCaseEdited.gasInjectorCount.toString() : projectCase.gasInjectorCount.toString()}
                >
                    <CaseNumberInput
                        onChange={handleGasInjectorCountChange}
                        defaultValue={projectCaseEdited ? projectCaseEdited.gasInjectorCount : projectCase?.gasInjectorCount}
                        integer
                        min={0}
                        max={100000}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher
                    value={productionStrategyOptions[projectCase?.productionStrategyOverview]}
                    label="Production strategy overview"
                >
                    <NativeSelect
                        id="productionStrategy"
                        label=""
                        onChange={handleProductionStrategyChange}
                        value={projectCaseEdited ? projectCaseEdited.productionStrategyOverview : projectCase.productionStrategyOverview}
                    >
                        {Object.entries(productionStrategyOptions).map(([value, label]) => (
                            <option key={value} value={value}>{label}</option>
                        ))}
                    </NativeSelect>
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher
                    value={artificialLiftOptions[projectCase?.artificialLift]}
                    label="Artificial lift"
                >
                    <NativeSelect
                        id="artificialLift"
                        label=""
                        onChange={handleArtificialLiftChange}
                        value={projectCaseEdited ? projectCaseEdited.artificialLift : projectCase.artificialLift}
                    >
                        {Object.entries(artificialLiftOptions).map(([value, label]) => (
                            <option key={value} value={value}>{label}</option>
                        ))}
                    </NativeSelect>
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher
                    label="Facilities availability"
                    value={`${projectCase?.facilitiesAvailability !== undefined ? (projectCase?.facilitiesAvailability * 100).toFixed(2) : ""}%`}
                >
                    <CaseNumberInput
                        onChange={handleFacilitiesAvailabilityChange}
                        defaultValue={defaultValue}
                        integer={false}
                        unit="%"
                        min={0}
                        max={100}
                    />
                </InputSwitcher>
            </Grid>
        </Grid>
    )
}

export default CaseDescriptionTab
