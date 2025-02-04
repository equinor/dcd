import { useState, ChangeEventHandler, useEffect } from "react"
import { Input, NativeSelect } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid2"

import InputSwitcher from "@/Components/Input/Components/InputSwitcher"
import { PROJECT_CLASSIFICATION } from "@/Utils/constants"
import useEditProject from "@/Hooks/useEditProject"
import { useDataFetch } from "@/Hooks/useDataFetch"
import ProjectSkeleton from "../LoadingSkeletons/ProjectSkeleton"

const ProjectSettingsTab = () => {
    const { addProjectEdit } = useEditProject()
    const revisionAndProjectData = useDataFetch()

    const [dummyRole] = useState(0) // TODO: Get role from user
    const [oilPriceUSD, setOilPriceUSD] = useState(revisionAndProjectData?.commonProjectAndRevisionData.oilPriceUSD || 0)
    const [gasPriceNOK, setGasPriceNOK] = useState(revisionAndProjectData?.commonProjectAndRevisionData.gasPriceNOK || 0)
    const [discountRate, setDiscountRate] = useState(revisionAndProjectData?.commonProjectAndRevisionData.discountRate || 0)
    const [exchangeRateUSDToNOK, setExchangeRateUSDToNOK] = useState(revisionAndProjectData?.commonProjectAndRevisionData.exchangeRateUSDToNOK || 0)
    const [npvYear, setNpvYear] = useState(revisionAndProjectData?.commonProjectAndRevisionData.npvYear || 0)

    useEffect(() => {
        if (revisionAndProjectData) {
            setOilPriceUSD(revisionAndProjectData.commonProjectAndRevisionData.oilPriceUSD)
            setGasPriceNOK(revisionAndProjectData.commonProjectAndRevisionData.gasPriceNOK)
            setDiscountRate(revisionAndProjectData.commonProjectAndRevisionData.discountRate)
            setExchangeRateUSDToNOK(revisionAndProjectData.commonProjectAndRevisionData.exchangeRateUSDToNOK)
            setNpvYear(revisionAndProjectData.commonProjectAndRevisionData.npvYear)
        }
    }, [revisionAndProjectData])

    const handlePhysicalUnitChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1].indexOf(Number(e.currentTarget.value)) !== -1 && revisionAndProjectData) {
            const newPhysicalUnit: Components.Schemas.PhysUnit = Number(e.currentTarget.value) as Components.Schemas.PhysUnit
            const newProject: Components.Schemas.UpdateProjectDto = { ...revisionAndProjectData.commonProjectAndRevisionData }
            newProject.physicalUnit = newPhysicalUnit
            addProjectEdit(revisionAndProjectData.projectId, newProject)
        }
    }

    const handleCurrencyChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([1, 2].indexOf(Number(e.currentTarget.value)) !== -1 && revisionAndProjectData) {
            const newCurrency: Components.Schemas.Currency = Number(e.currentTarget.value) as Components.Schemas.Currency
            const newProject: Components.Schemas.UpdateProjectDto = { ...revisionAndProjectData.commonProjectAndRevisionData }
            newProject.currency = newCurrency
            addProjectEdit(revisionAndProjectData.projectId, newProject)
        }
    }

    const handleClassificationChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([1, 2, 3].indexOf(Number(e.currentTarget.value)) !== -1 && revisionAndProjectData) {
            const newClassification: Components.Schemas.ProjectClassification = Number(e.currentTarget.value) as unknown as Components.Schemas.ProjectClassification
            const newProject: Components.Schemas.UpdateProjectDto = { ...revisionAndProjectData.commonProjectAndRevisionData }
            newProject.classification = newClassification
            addProjectEdit(revisionAndProjectData.projectId, newProject)
        }
    }

    const handleOilPriceChange = () => {
        const newOilPrice = oilPriceUSD
        if (!Number.isNaN(newOilPrice) && revisionAndProjectData) {
            const newProject: Components.Schemas.UpdateProjectDto = { ...revisionAndProjectData.commonProjectAndRevisionData, oilPriceUSD: newOilPrice }
            addProjectEdit(revisionAndProjectData.projectId, newProject)
        }
    }

    const handleGasPriceChange = () => {
        const newGasPrice = gasPriceNOK
        if (!Number.isNaN(newGasPrice) && revisionAndProjectData) {
            const newProject: Components.Schemas.UpdateProjectDto = { ...revisionAndProjectData.commonProjectAndRevisionData, gasPriceNOK: newGasPrice }
            addProjectEdit(revisionAndProjectData.projectId, newProject)
        }
    }

    const handleDiscountRateChange = () => {
        const newDiscountRate = discountRate
        if (!Number.isNaN(newDiscountRate) && revisionAndProjectData) {
            const newProject: Components.Schemas.UpdateProjectDto = { ...revisionAndProjectData.commonProjectAndRevisionData, discountRate: newDiscountRate }
            addProjectEdit(revisionAndProjectData.projectId, newProject)
        }
    }

    const handleExchangeRateChange = () => {
        const newExchangeRate = exchangeRateUSDToNOK
        if (!Number.isNaN(newExchangeRate) && revisionAndProjectData) {
            const newProject: Components.Schemas.UpdateProjectDto = { ...revisionAndProjectData.commonProjectAndRevisionData, exchangeRateUSDToNOK: newExchangeRate }
            addProjectEdit(revisionAndProjectData.projectId, newProject)
        }
    }

    const handleNpvYearChange = () => {
        const newNpvYear = npvYear
        if (!Number.isNaN(newNpvYear) && revisionAndProjectData) {
            const newProject: Components.Schemas.UpdateProjectDto = { ...revisionAndProjectData.commonProjectAndRevisionData, npvYear: newNpvYear }
            addProjectEdit(revisionAndProjectData.projectId, newProject)
        }
    }

    const getPhysicalUnit = () => (revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === 0 ? "SI" : "Oil field")
    const getCurrency = () => (revisionAndProjectData?.commonProjectAndRevisionData.currency === 1 ? "NOK" : "USD")

    if (!revisionAndProjectData) {
        return <ProjectSkeleton />
    }

    return (
        <Grid container direction="column" spacing={2}>
            <Grid container size={12} justifyContent="flex-start">
                <Grid container size={{ xs: 12, md: 6, lg: 4 }} spacing={2}>
                    <Grid size={12}>
                        <InputSwitcher
                            value={getPhysicalUnit()}
                            label="Physical unit"
                        >
                            <NativeSelect
                                id="physicalUnit"
                                label=""
                                onChange={handlePhysicalUnitChange}
                                value={revisionAndProjectData.commonProjectAndRevisionData.physicalUnit}
                            >
                                <option key={0} value={0}>SI</option>
                                <option key={1} value={1}>Oil field</option>
                            </NativeSelect>
                        </InputSwitcher>
                    </Grid>
                    <Grid size={12}>
                        <InputSwitcher
                            value={getCurrency()}
                            label="Currency"
                        >
                            <NativeSelect
                                id="currency"
                                label=""
                                onChange={handleCurrencyChange}
                                value={revisionAndProjectData.commonProjectAndRevisionData.currency}
                            >
                                <option key={1} value={1}>NOK</option>
                                <option key={2} value={2}>USD</option>
                            </NativeSelect>
                        </InputSwitcher>
                    </Grid>
                    <Grid size={12}>
                        {dummyRole === 0 && (
                            <InputSwitcher
                                value={PROJECT_CLASSIFICATION[revisionAndProjectData.commonProjectAndRevisionData.classification].label}
                                label="Classification"
                            >
                                <NativeSelect
                                    id="classification"
                                    label=""
                                    onChange={(e) => handleClassificationChange(e)}
                                    value={revisionAndProjectData ? revisionAndProjectData.commonProjectAndRevisionData.classification : 1}
                                >
                                    {Object.entries(PROJECT_CLASSIFICATION).filter(([key]) => key !== "0").map(([key, value]) => (
                                        <option key={key} value={key}>{value.label}</option>
                                    ))}
                                </NativeSelect>
                            </InputSwitcher>
                        )}
                    </Grid>
                    <Grid size={12}>
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
                    <Grid size={12}>
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
                    <Grid size={12}>
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
            </Grid>
            <Grid size={12}>
                <InputSwitcher
                    value={String(exchangeRateUSDToNOK)}
                    label="Exchange Rate (USD to NOK)"
                >
                    <Input
                        type="number"
                        step="0.01"
                        value={exchangeRateUSDToNOK}
                        onChange={(e: any) => setExchangeRateUSDToNOK(Number(e.target.value))}
                        onBlur={handleExchangeRateChange}
                    />
                </InputSwitcher>
            </Grid>
            <Grid size={12}>
                <InputSwitcher
                    value={String(npvYear)}
                    label="NPV Year"
                >
                    <Input
                        type="number"
                        step="1"
                        value={npvYear}
                        onChange={(e: any) => setNpvYear(Number(e.target.value))}
                        onBlur={handleNpvYearChange}
                    />
                </InputSwitcher>
            </Grid>
        </Grid>
    )
}

export default ProjectSettingsTab
