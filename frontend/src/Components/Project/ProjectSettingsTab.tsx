import { useState, ChangeEventHandler, useEffect } from "react"
import { Input, NativeSelect } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import { useQuery } from "@tanstack/react-query"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import InputSwitcher from "../Input/Components/InputSwitcher"
import { PROJECT_CLASSIFICATION } from "../../Utils/constants"
import { projectQueryFn } from "../../Services/QueryFunctions"
import useEditProject from "../../Hooks/useEditProject"

const ProjectSettingsTab = () => {
    const { currentContext } = useModuleCurrentContext()
    const { addProjectEdit } = useEditProject()
    const externalId = currentContext?.externalId
    const { data: apiData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })

    const [dummyRole, setDummyRole] = useState(0) // TODO: Get role from user
    const [oilPriceUSD, setOilPriceUSD] = useState(apiData?.oilPriceUSD || 0)
    const [gasPriceNOK, setGasPriceNOK] = useState(apiData?.gasPriceNOK || 0)
    const [discountRate, setDiscountRate] = useState(apiData?.discountRate || 0)
    const [internalProjectPhase, setInternalProjectPhase] = useState(apiData?.internalProjectPhase || 0)

    useEffect(() => {
        if (apiData) {
            setOilPriceUSD(apiData.oilPriceUSD)
            setGasPriceNOK(apiData.gasPriceNOK)
            setDiscountRate(apiData.discountRate)
            setInternalProjectPhase(apiData.internalProjectPhase)
        }
    }, [apiData])

    const handlePhysicalUnitChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1].indexOf(Number(e.currentTarget.value)) !== -1 && apiData) {
            const newPhysicalUnit: Components.Schemas.PhysUnit = Number(e.currentTarget.value) as Components.Schemas.PhysUnit
            const newProject: Components.Schemas.ProjectWithAssetsDto = { ...apiData }
            newProject.physicalUnit = newPhysicalUnit
            addProjectEdit(apiData.id, newProject)
        }
    }

    const handleCurrencyChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([1, 2].indexOf(Number(e.currentTarget.value)) !== -1 && apiData) {
            const newCurrency: Components.Schemas.Currency = Number(e.currentTarget.value) as Components.Schemas.Currency
            const newProject: Components.Schemas.ProjectWithAssetsDto = { ...apiData }
            newProject.currency = newCurrency
            addProjectEdit(apiData.id, newProject)
        }
    }

    const handleClassificationChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1, 2, 3].indexOf(Number(e.currentTarget.value)) !== -1 && apiData) {
            const newClassification: Components.Schemas.ProjectClassification = Number(e.currentTarget.value) as unknown as Components.Schemas.ProjectClassification
            const newProject: Components.Schemas.ProjectWithAssetsDto = { ...apiData }
            newProject.classification = newClassification
            addProjectEdit(apiData.id, newProject)
        }
    }

    const handleOilPriceChange = () => {
        const newOilPrice = oilPriceUSD
        if (!Number.isNaN(newOilPrice) && apiData) {
            const newProject = { ...apiData, oilPriceUSD: newOilPrice }
            addProjectEdit(apiData.id, newProject)
        }
    }

    const handleGasPriceChange = () => {
        const newGasPrice = gasPriceNOK
        if (!Number.isNaN(newGasPrice) && apiData) {
            const newProject = { ...apiData, gasPriceNOK: newGasPrice }
            addProjectEdit(apiData.id, newProject)
        }
    }

    const handleDiscountRateChange = () => {
        const newDiscountRate = discountRate
        if (!Number.isNaN(newDiscountRate) && apiData) {
            const newProject = { ...apiData, discountRate: newDiscountRate }
            addProjectEdit(apiData.id, newProject)
        }
    }

    if (!apiData) {
        return <div>Loading project data...</div>
    }

    return (
        <Grid container direction="column" spacing={2}>
            <Grid item>
                <InputSwitcher
                    value={apiData.physicalUnit === 0 ? "SI" : "Oil field"}
                    label="Physical unit"
                >
                    <NativeSelect
                        id="physicalUnit"
                        label=""
                        onChange={handlePhysicalUnitChange}
                        value={apiData.physicalUnit}
                    >
                        <option key={0} value={0}>SI</option>
                        <option key={1} value={1}>Oil field</option>
                    </NativeSelect>
                </InputSwitcher>
            </Grid>
            <Grid item>
                <InputSwitcher
                    value={apiData.currency === 1 ? "NOK" : "USD"}
                    label="Currency"
                >
                    <NativeSelect
                        id="currency"
                        label=""
                        onChange={handleCurrencyChange}
                        value={apiData.currency}
                    >
                        <option key={1} value={1}>NOK</option>
                        <option key={2} value={2}>USD</option>
                    </NativeSelect>
                </InputSwitcher>
            </Grid>
            <Grid item>
                {dummyRole === 0 && (
                    <InputSwitcher
                        value={PROJECT_CLASSIFICATION[apiData.classification].label}
                        label="Classification"
                    >
                        <NativeSelect
                            id="classification"
                            label=""
                            onChange={(e) => handleClassificationChange(e)}
                            value={apiData ? apiData.classification : undefined}
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
                    value={String(oilPriceUSD)}
                    label="Oil Price (USD)"
                >
                    <Input
                        type="number"
                        step="0.01"
                        value={oilPriceUSD}
                        onChange={(e: any) => setOilPriceUSD(Number(e.target.value))}
                        onBlur={handleOilPriceChange}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item>
                <InputSwitcher
                    value={String(gasPriceNOK)}
                    label="Gas Price (NOK)"
                >
                    <Input
                        type="number"
                        step="0.01"
                        value={gasPriceNOK}
                        onChange={(e: any) => setGasPriceNOK(Number(e.target.value))}
                        onBlur={handleGasPriceChange}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item>
                <InputSwitcher
                    value={String(discountRate)}
                    label="Discount Rate (%)"
                >
                    <Input
                        type="number"
                        step="0.01"
                        value={discountRate}
                        onChange={(e: any) => setDiscountRate(Number(e.target.value))}
                        onBlur={handleDiscountRateChange}
                    />
                </InputSwitcher>
            </Grid>
        </Grid>
    )
}

export default ProjectSettingsTab
