import {
    Dispatch,
    SetStateAction,
    ChangeEventHandler,
    useEffect,
} from "react"
import { Typography, Input } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import { useQueryClient } from "react-query"
import SwitchableNumberInput from "../../Input/SwitchableNumberInput"
import InputSwitcher from "../../Input/Components/InputSwitcher"
import { useProjectContext } from "../../../Context/ProjectContext"
import { useCaseContext } from "../../../Context/CaseContext"
import { setNonNegativeNumberState } from "../../../Utils/common"
import SwitchableDropdownInput from "../../Input/SwitchableDropdownInput"
import { GetTopsideService } from "../../../Services/TopsideService"
import useOptimisticMutation from "../../../Hooks/useOptimisticMutation"
import { GetSurfService } from "../../../Services/SurfService"
import { GetSubstructureService } from "../../../Services/SubstructureService"
import { GetCaseService } from "../../../Services/CaseService"
import { GetTransportService } from "../../../Services/TransportService"

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
    const { project } = useProjectContext()
    const { projectCase, setProjectCaseEdited } = useCaseContext()

    if (!projectCase) { return null }
    const queryClient = useQueryClient()

    const { updateData: updateTopside } = useOptimisticMutation({
        queryKey: ["topsideData", project!.id, projectCase.id],
        mutationFn: async (updatedData: Components.Schemas.APIUpdateTopsideDto) => {
            const topsideService = await GetTopsideService()
            return topsideService.updateTopside(project!.id, projectCase.id, topside.id, updatedData)
        },
    })

    const { updateData: updateSurf } = useOptimisticMutation({
        queryKey: ["surfData", project!.id, projectCase.id],
        mutationFn: async (updatedData: Components.Schemas.APIUpdateSurfDto) => {
            const surfService = await GetSurfService()
            return surfService.updateSurf(project!.id, projectCase.id, surf.id, updatedData)
        },
    })

    const { updateData: updateSubstructure } = useOptimisticMutation({
        queryKey: ["substructureData", project!.id, projectCase.id],
        mutationFn: async (updatedData: Components.Schemas.APIUpdateSubstructureDto) => {
            const substructureService = await GetSubstructureService()
            return substructureService.updateSubstructure(project!.id, projectCase.id, substructure.id, updatedData)
        },
    })

    const { updateData: updateCase } = useOptimisticMutation({
        queryKey: ["caseData", project!.id],
        mutationFn: async (updatedData: Components.Schemas.CaseDto) => {
            const caseService = await GetCaseService()
            return caseService.updateCase(project!.id, projectCase.id, updatedData)
        },
    })

    const { updateData: updateTransport } = useOptimisticMutation({
        queryKey: ["transportData", project!.id, projectCase.id],
        mutationFn: async (updatedData: Components.Schemas.APIUpdateTransportDto) => {
            const transportService = await GetTransportService()
            return transportService.updateTransport(project!.id, projectCase.id, transport.id, updatedData)
        },
    })

    useEffect(() => {
        const topsideData = queryClient.getQueryData<Components.Schemas.APIUpdateTopsideDto>(["topsideData", project!.id, projectCase.id])
        if (topsideData) {
            console.log("Updated topsideData:", topsideData)
        }
    }, [queryClient.getQueryData(["topsideData", project!.id, projectCase.id])])

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
            updateSurf("productionFlowline", newProductionFlowline)
        }
    }

    const handleSubstructureDryweightChange = (value: number): void => {
        const newSubstructure = { ...substructure }
        newSubstructure.dryWeight = value > 0 ? Math.max(value, 0) : 0
        setSubstrucutre(newSubstructure)
        updateSubstructure("dryWeight", value)
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
            updateSubstructure("concept", newConcept)
        }
    }

    const handleHostChange = (value: number): void => {
        const newCase = { ...projectCase }
        newCase.host = value > 0 ? value.toString() : ""
        setProjectCaseEdited(newCase)
        updateCase("host", value)
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
                    onSubmit={(value: number) => {
                        setNonNegativeNumberState(value, "cessationCost", surf, setSurf)
                        updateSurf("cessationCost", value)
                    }}
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
                    onSubmit={(value: number) => {
                        setNonNegativeNumberState(value, "templateCount", surf, setSurf)
                        updateSurf("templateCount", value)
                    }}
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
                    onSubmit={(value: number) => {
                        setNonNegativeNumberState(value, "riserCount", surf, setSurf)
                        updateSurf("riserCount", value)
                    }}
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
                    onSubmit={(value: number) => {
                        setNonNegativeNumberState(value, "infieldPipelineSystemLength", surf, setSurf)
                        updateSurf("infieldPipelineSystemLength", value)
                    }}
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
                    onSubmit={(value: number) => {
                        setNonNegativeNumberState(value, "umbilicalSystemLength", surf, setSurf)
                        updateSurf("umbilicalSystemLength", value)
                    }}
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
                    onSubmit={(value: number) => {
                        setNonNegativeNumberState(value, "producerCount", surf, setSurf)
                        updateSurf("producerCount", value)
                    }}
                    value={surf?.producerCount}
                    integer
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    objectKey={surf.gasInjectorCount}
                    label="Gas injector count"
                    onSubmit={(value: number) => {
                        setNonNegativeNumberState(value, "gasInjectorCount", surf, setSurf)
                        updateSurf("gasInjectorCount", value)
                    }}
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
                    onSubmit={(value: number) => {
                        setNonNegativeNumberState(value, "waterInjectorCount", surf, setSurf)
                        updateSurf("waterInjectorCount", value)
                    }}
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
                    onSubmit={(value: number) => {
                        setNonNegativeNumberState(value, "oilExportPipelineLength", transport, setTransport)
                        updateTransport("oilExportPipelineLength", value)
                    }}
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
                    onSubmit={(value: number) => {
                        setNonNegativeNumberState(value, "gasExportPipelineLength", transport, setTransport)
                        updateTransport("gasExportPipelineLength", value)
                    }}
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
