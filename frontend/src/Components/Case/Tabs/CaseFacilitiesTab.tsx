import {
    Dispatch,
    SetStateAction,
    ChangeEventHandler,
    useState,
} from "react"
import { Typography, Input } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import { debounce } from "lodash"
import { useMutation, useQueryClient } from "react-query"
import SwitchableNumberInput from "../../Input/SwitchableNumberInput"
import InputSwitcher from "../../Input/Components/InputSwitcher"
import { useProjectContext } from "../../../Context/ProjectContext"
import { useCaseContext } from "../../../Context/CaseContext"
import { setNonNegativeNumberState } from "../../../Utils/common"
import SwitchableDropdownInput from "../../Input/SwitchableDropdownInput"
import { GetTopsideService } from "../../../Services/TopsideService"

interface Props {
    topside: Components.Schemas.TopsideWithProfilesDto,
    setTopside: Dispatch<SetStateAction<Components.Schemas.TopsideWithProfilesDto | undefined>>,
    surf: Components.Schemas.SurfWithProfilesDto,
    setSurf: Dispatch<SetStateAction<Components.Schemas.SurfWithProfilesDto | undefined>>,
    substructure: Components.Schemas.SubstructureWithProfilesDto,
    setSubstrucutre: Dispatch<SetStateAction<Components.Schemas.SubstructureWithProfilesDto | undefined>>,
    transport: Components.Schemas.TransportWithProfilesDto,
    setTransport: Dispatch<SetStateAction<Components.Schemas.TransportWithProfilesDto | undefined>>,
}

const CaseFacilitiesTab = ({
    topside, setTopside,
    surf, setSurf,
    substructure, setSubstrucutre,
    transport, setTransport,
}: Props) => {
    const queryClient = useQueryClient()
    const { project } = useProjectContext()
    const { projectCase, setProjectCaseEdited } = useCaseContext()
    const [topsideCostProfileOverrideUpdate, setTopsideCostProfileOverrideUpdate] = useState<Components.Schemas.UpdateTopsideCostProfileOverrideDto | undefined>(undefined)
    if (!projectCase) { return null }

    const [topsideData, setTopsideData] = useState<Components.Schemas.APIUpdateTopsideDto | undefined>(undefined)

    const mutation = useMutation(
        async (updatedData: Components.Schemas.APIUpdateTopsideDto) => {
            const topsideService = await GetTopsideService()
            return topsideService.updateTopside(project!.id, projectCase.id, topside.id, updatedData)
        },
        {
            onSuccess: (data) => {
                queryClient.setQueryData(["topsideData", project!.id, projectCase.id, topside.id], data)
                console.log("API Response:", data)
                // Set topsideData here for immediate UI update (optimistic)
                setTopsideData(data)
            },
            onError: (error) => {
                console.error("Failed to update topside:", error)
                setTopsideData((prevData) => prevData) // Retains previous data
            },
        },
    )

    const updateTopside = (key: keyof Components.Schemas.APIUpdateTopsideDto, value: any) => {
        // Create a copy of topsideData to avoid mutation
        const updatedData = { ...topsideData, [key]: value }

        setTopsideData(updatedData)

        mutation.mutate(updatedData)
    }

    /*
    let isApiCallInProgress = false
    const updateTopside = async (key: keyof Components.Schemas.APIUpdateTopsideDto, value: any) => {
        try {
            setTopsideData((prevState) => ({
                ...prevState,
                [key]: value,
            }))

            const topsideService = await GetTopsideService()

            const debouncedUpdate = debounce(async (updatedData) => {
                if (isApiCallInProgress) { return } // Prevent new call if one is already in progress
                isApiCallInProgress = true

                try {
                    const response: Components.Schemas.APIUpdateTopsideDto = await topsideService.updateTopside(project!.id, projectCase.id, topside.id, updatedData)
                    console.log("API Response:", response)
                    setTopsideData(response)
                } catch (error) {
                    console.error("Failed to update topside:", error)
                } finally {
                    isApiCallInProgress = false
                }
            }, 300)

            debouncedUpdate({ ...topsideData, [key]: value })
        } catch (error) {
            console.error("Failed to update topside:", error)
            setTopsideData((prevState) => ({
                ...prevState,
                [key]: topside[key],
            }))
        }
    } */

    const updateTopsideCostProfileOverride = async (key: keyof Components.Schemas.UpdateTopsideCostProfileOverrideDto, value: any) => {
        if (topsideCostProfileOverrideUpdate) {
            setTopsideCostProfileOverrideUpdate({ ...topsideCostProfileOverrideUpdate, [key]: value })
        } else {
            setTopsideCostProfileOverrideUpdate({ [key]: value })
        }
    }

    const platformConceptValues: { [key: number]: string } = {
        0: "No Concept",
        1: "Tie-back to existing offshore platform",
        2: "JACKET - Fixed Steel Jacket",
        3: "GBS - Fixed Concrete Platform - Gravity Based Structure",
        4: "TLP - Tension Leg Platform - Steel",
        5: "SPAR Platform - Steel",
        6: "SEMI - Semi Submersible - Steel",
        7: "CIRCULAR BARGE - Sevan type FPSO",
        8: "BARGE - Barge shaped - Spread Moored FPSO",
        9: "FPSO - Ship shaped - TUrret Moored",
        10: "TANKER - converted tanker FPSO - Turret Moored",
        11: "JACK-UP Platform",
        12: "Subsea to shore",
    }
    const productionFlowlineValues: { [key: number]: string } = {
        0: "No production flowline",
        1: "Carbon",
        2: "SS Clad",
        3: "Cr13",
        4: "Carbon + insulation",
        5: "SS Clad + insulation",
        6: "Cr13 + insulation",
        7: "Carbon + insulation + DEH",
        8: "SS Clad + insulation + DEH",
        9: "Cr13 + insulation + DEH",
        10: "Carbon + PIP",
        11: "SS Clad + PIP",
        12: "Cr13 + PIP",
        13: "HDPE lined CS (Water injection only)",
    }

    const handleProductionFlowlineChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13].indexOf(Number(e.currentTarget.value)) !== -1) {
            const newProductionFlowline: Components.Schemas.ProductionFlowline = Number(e.currentTarget.value) as Components.Schemas.ProductionFlowline
            const newSurf = { ...surf }
            newSurf.productionFlowline = newProductionFlowline
            setSurf(newSurf)
        }
    }

    const handleSubstructureDryweightChange = (value: number): void => {
        const newSubstructure = { ...substructure }
        newSubstructure.dryWeight = value > 0 ? Math.max(value, 0) : 0
        setSubstrucutre(newSubstructure)
    }

    const handleSubstructureConceptChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12].indexOf(Number(e.currentTarget.value)) !== -1) {
            const newConcept: Components.Schemas.Concept = Number(e.currentTarget.value) as Components.Schemas.Concept
            const newSubstructure = { ...substructure }
            newSubstructure.concept = newConcept
            if (newConcept !== 1) {
                const newCase = { ...projectCase }
                newCase.host = ""
                setProjectCaseEdited(newCase)
            }
            setSubstrucutre(newSubstructure)
        }
    }

    const handleHostChange = (value: number): void => {
        const newCase = { ...projectCase }
        newCase.host = value > 0 ? value.toString() : ""
        setProjectCaseEdited(newCase)
    }

    return (
        <Grid container spacing={2}>
            <Grid item xs={12} md={4}>
                <SwitchableDropdownInput
                    value={substructure?.concept}
                    options={platformConceptValues}
                    objectKey={substructure?.concept}
                    label="Platform concept"
                    onSubmit={handleSubstructureConceptChange}
                />
            </Grid>
            {substructure.concept === 1 && (
                <Grid item xs={12} md={4}>
                    <InputSwitcher
                        value={projectCase.host ?? ""}
                        label="Host"
                    >
                        <Input
                            id="NumberInput"
                            value={projectCase.host ?? ""}
                            disabled={false}
                            onChange={handleHostChange}
                        />
                    </InputSwitcher>
                </Grid>
            )}
            <Grid item xs={12} md={4}>

                <SwitchableNumberInput
                    objectKey={topside.facilityOpex}
                    label="Facility opex"
                    onSubmit={(value: number) => {
                        setNonNegativeNumberState(value, "facilityOpex", topside, setTopside)
                        updateTopside("facilityOpex", value)
                    }}
                    value={Math.round(Number(topside?.facilityOpex) * 10) / 10}
                    integer={false}
                    unit={`${project?.currency === 1 ? "MNOK" : "MUSD"}`}
                />

            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    objectKey={surf.cessationCost}
                    label="Cessation cost"
                    onSubmit={(value: number) => setNonNegativeNumberState(value, "cessationCost", surf, setSurf)}
                    value={Math.round(Number(surf?.cessationCost) * 10) / 10}
                    integer={false}
                    unit={`${project?.currency === 1 ? "MNOK" : "MUSD"}`}
                />
            </Grid>
            <Grid item xs={12}>
                <Typography variant="h4">Topside</Typography>
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    objectKey={topside.dryWeight}
                    label="Topside dry weight"
                    onSubmit={(value: number) => {
                        setNonNegativeNumberState(value, "dryWeight", topside, setTopside)
                        updateTopside("dryWeight", value)
                    }}
                    value={Math.round(Number(topside?.dryWeight) * 1) / 1}
                    integer
                    unit="tonnes"
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    label="Facilities availability"
                    value={projectCase.facilitiesAvailability ?? 0 * 100}
                    integer
                    disabled
                    unit="%"
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    objectKey={topside.peakElectricityImported}
                    label="Peak electricity imported"
                    onSubmit={(value: number) => {
                        setNonNegativeNumberState(value, "peakElectricityImported", topside, setTopside)
                        updateTopside("peakElectricityImported", value)
                    }}
                    value={Math.round(Number(topside?.peakElectricityImported) * 10) / 10}
                    integer={false}
                    unit="MW"
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={4}>

                <SwitchableNumberInput
                    objectKey={topside.oilCapacity}
                    label="Oil capacity"
                    onSubmit={(value: number) => {
                        setNonNegativeNumberState(value, "oilCapacity", topside, setTopside)
                        updateTopside("oilCapacity", value)
                    }}
                    value={Math.round(Number(topside?.oilCapacity) * 1) / 1}
                    integer
                    unit="Sm³/sd"
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    objectKey={topside.gasCapacity}
                    label="Gas capacity"
                    onSubmit={(value: number) => {
                        setNonNegativeNumberState(value, "gasCapacity", topside, setTopside)
                        updateTopside("gasCapacity", value)
                    }}
                    value={Math.round(Number(topside?.gasCapacity) * 10) / 10}
                    integer={false}
                    unit="MSm³/sd"
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    objectKey={topside.waterInjectionCapacity}
                    label="Water injection capacity"
                    onSubmit={(value: number) => {
                        setNonNegativeNumberState(value, "waterInjectionCapacity", topside, setTopside)
                        updateTopside("waterInjectionCapacity", value)
                    }}
                    value={Math.round(Number(topside?.waterInjectionCapacity) * 1) / 1}
                    integer
                    unit="MSm³/sd"
                />
            </Grid>
            <Grid item xs={12}>
                <Typography variant="h4">Platform wells</Typography>
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    objectKey={topside.producerCount}
                    label="Producer count"
                    onSubmit={(value: number) => {
                        setNonNegativeNumberState(value, "producerCount", topside, setTopside)
                        updateTopside("producerCount", value)
                    }}
                    value={topside?.producerCount}
                    integer
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    objectKey={topside.gasInjectorCount}
                    label="Gas injector count"
                    onSubmit={(value: number) => {
                        setNonNegativeNumberState(value, "gasInjectorCount", topside, setTopside)
                        updateTopside("gasInjectorCount", value)
                    }}
                    value={topside?.gasInjectorCount}
                    integer
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    objectKey={topside.waterInjectorCount}
                    label="Water injector count"
                    onSubmit={(value: number) => {
                        setNonNegativeNumberState(value, "waterInjectorCount", topside, setTopside)
                        updateTopside("waterInjectorCount", value)
                    }}
                    value={topside?.waterInjectorCount}
                    integer
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12}>
                <Typography variant="h4">SURF</Typography>
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    objectKey={surf.templateCount}
                    label="Templates"
                    onSubmit={(value: number) => setNonNegativeNumberState(value, "templateCount", surf, setSurf)}
                    value={surf?.templateCount}
                    integer
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    objectKey={surf.riserCount}
                    label="Risers"
                    onSubmit={(value: number) => setNonNegativeNumberState(value, "riserCount", surf, setSurf)}
                    value={surf?.riserCount}
                    integer
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    objectKey={surf.infieldPipelineSystemLength}
                    label="Production lines length"
                    onSubmit={(value: number) => setNonNegativeNumberState(value, "infieldPipelineSystemLength", surf, setSurf)}
                    value={Math.round(Number(surf?.infieldPipelineSystemLength) * 10) / 10}
                    integer={false}
                    unit="km"
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    objectKey={surf.umbilicalSystemLength}
                    label="Umbilical system length"
                    onSubmit={(value: number) => setNonNegativeNumberState(value, "umbilicalSystemLength", surf, setSurf)}
                    value={Math.round(Number(surf?.umbilicalSystemLength) * 10) / 10}
                    integer={false}
                    unit="km"
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableDropdownInput
                    value={surf.productionFlowline}
                    options={productionFlowlineValues}
                    objectKey={surf.productionFlowline}
                    label="Production flowline"
                    onSubmit={handleProductionFlowlineChange}
                />
            </Grid>
            <Grid item xs={12}>
                <Typography variant="h4">Subsea wells</Typography>
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    objectKey={surf.producerCount}
                    label="Producer count"
                    onSubmit={(value: number) => setNonNegativeNumberState(value, "producerCount", surf, setSurf)}
                    value={surf?.producerCount}
                    integer
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    objectKey={surf.gasInjectorCount}
                    label="Gas injector count"
                    onSubmit={(value: number) => setNonNegativeNumberState(value, "gasInjectorCount", surf, setSurf)}
                    value={surf?.gasInjectorCount}
                    integer
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    objectKey={surf.waterInjectorCount}
                    label="Water injector count"
                    onSubmit={(value: number) => setNonNegativeNumberState(value, "waterInjectorCount", surf, setSurf)}
                    value={surf?.waterInjectorCount}
                    integer
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12}>
                <Typography variant="h4">Transport</Typography>
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    objectKey={transport.oilExportPipelineLength}
                    label="Oil export pipeline length"
                    onSubmit={(value: number) => setNonNegativeNumberState(value, "oilExportPipelineLength", transport, setTransport)}
                    value={Math.round(Number(transport?.oilExportPipelineLength) * 10) / 10}
                    integer={false}
                    unit="km"
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    objectKey={transport.gasExportPipelineLength}
                    label="Gas export pipeline length"
                    onSubmit={(value: number) => setNonNegativeNumberState(value, "gasExportPipelineLength", transport, setTransport)}
                    value={Math.round(Number(transport?.gasExportPipelineLength) * 10) / 10}
                    integer={false}
                    unit="km"
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12}>
                <Typography variant="h4">Substructure</Typography>
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    objectKey={substructure.dryWeight}
                    label="Substructure dry weight"
                    onSubmit={handleSubstructureDryweightChange}
                    value={Math.round(Number(substructure?.dryWeight) * 1) / 1}
                    integer
                    unit="tonnes"
                    min={0}
                    max={1000000}
                />
            </Grid>
        </Grid>
    )
}

export default CaseFacilitiesTab
