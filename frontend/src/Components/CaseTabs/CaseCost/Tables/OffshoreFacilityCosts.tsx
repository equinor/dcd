import React, { useEffect, useMemo, useState } from "react"

import CaseBaseTable from "@/Components/Tables/CaseBaseTable"
import { useDataFetch } from "@/Hooks"
import { ITimeSeriesTableData } from "@/Models/ITimeSeries"
import { ProfileTypes } from "@/Models/enums"
import { getYearFromDateString } from "@/Utils/DateUtils"
import { getUnitByProfileName } from "@/Utils/FormatingUtils"

interface OffshoreFacillityCostsProps {
    tableYears: [number, number];
    capexGridRef: React.MutableRefObject<any>;
    alignedGridsRef: any[];
    apiData: Components.Schemas.CaseWithAssetsDto;
}

const OffshoreFacillityCosts: React.FC<OffshoreFacillityCostsProps> = ({
    tableYears,
    capexGridRef,
    alignedGridsRef,
    apiData,
}) => {
    const revisionAndProjectData = useDataFetch()
    const [capexTimeSeriesData, setCapexTimeSeriesData] = useState<ITimeSeriesTableData[]>([])

    const calculatedFields = useMemo(() => [
        ProfileTypes.SurfCostProfileOverride,
        ProfileTypes.TopsideCostProfileOverride,
        ProfileTypes.SubstructureCostProfileOverride,
        ProfileTypes.TransportCostProfileOverride,
        ProfileTypes.OnshorePowerSupplyCostProfileOverride,
    ], [])

    useEffect(() => {
        const surf = apiData?.surf
        const topside = apiData?.topside
        const transport = apiData?.transport
        const substructure = apiData?.substructure
        const onshorePowerSupply = apiData?.onshorePowerSupply
        const surfCostData = apiData.surfCostProfile
        const surfCostOverrideData = apiData.surfCostProfileOverride
        const topsideCostData = apiData.topsideCostProfile
        const topsideCostOverrideData = apiData.topsideCostProfileOverride
        const substructureCostData = apiData.substructureCostProfile
        const substructureCostOverrideData = apiData.substructureCostProfileOverride
        const transportCostData = apiData.transportCostProfile
        const transportCostOverrideData = apiData.transportCostProfileOverride
        const onshorePowerSupplyCostData = apiData.onshorePowerSupplyCostProfile
        const onshorePowerSupplyCostOverrideData = apiData.onshorePowerSupplyCostProfileOverride

        if (!surf || !topside || !substructure || !transport || !onshorePowerSupply) {
            console.error("Missing offshore facility data")

            return
        }

        const currency = revisionAndProjectData?.commonProjectAndRevisionData.currency
        const physUnit = revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit

        interface CreateProfileDataParams {
            profileName: string;
            profile: any;
            resourceName: ProfileTypes;
            resourceId: string;
            overrideProfile?: any;
            editable?: boolean;
        }

        const createProfileData = ({
            profileName,
            profile,
            resourceName,
            resourceId,
            overrideProfile,
            editable = true,
        }: CreateProfileDataParams): ITimeSeriesTableData => {
            const isCalculatedField = calculatedFields.includes(resourceName)

            return ({
                profileName,
                unit: getUnitByProfileName(profileName, physUnit, currency),
                profile,
                resourceName,
                resourceId,
                resourcePropertyKey: resourceName,
                editable,
                overridable: isCalculatedField,
                ...(overrideProfile && { overrideProfile }),
            })
        }

        const newCapexTimeSeriesData: ITimeSeriesTableData[] = [
            createProfileData({
                profileName: "Subsea production system",
                profile: surfCostData,
                resourceName: ProfileTypes.SurfCostProfileOverride,
                resourceId: surf.id,
                overrideProfile: surfCostOverrideData,
            }),
            createProfileData({
                profileName: "Topside",
                profile: topsideCostData,
                resourceName: ProfileTypes.TopsideCostProfileOverride,
                resourceId: topside.id,
                overrideProfile: topsideCostOverrideData,
            }),
            createProfileData({
                profileName: "Substructure",
                profile: substructureCostData,
                resourceName: ProfileTypes.SubstructureCostProfileOverride,
                resourceId: substructure.id,
                overrideProfile: substructureCostOverrideData,
            }),
            createProfileData({
                profileName: "Transport system",
                profile: transportCostData,
                resourceName: ProfileTypes.TransportCostProfileOverride,
                resourceId: transport.id,
                overrideProfile: transportCostOverrideData,
            }),
            createProfileData({
                profileName: "Onshore (Power from shore)",
                profile: onshorePowerSupplyCostData,
                resourceName: ProfileTypes.OnshorePowerSupplyCostProfileOverride,
                resourceId: onshorePowerSupply.id,
                overrideProfile: onshorePowerSupplyCostOverrideData,
            }),
        ]

        setCapexTimeSeriesData(newCapexTimeSeriesData)
    }, [apiData, revisionAndProjectData, tableYears, calculatedFields])

    return (
        <CaseBaseTable
            timeSeriesData={capexTimeSeriesData}
            dg4Year={getYearFromDateString(apiData.case.dg4Date)}
            tableYears={tableYears}
            tableName="Offshore facility cost"
            gridRef={capexGridRef}
            alignedGridsRef={alignedGridsRef}
            includeFooter
            totalRowName="Total"
            calculatedFields={calculatedFields}
            isProsp
            sharepointFileId={apiData.case.sharepointFileId ?? undefined}
            decimalPrecision={1}
        />
    )
}

export default OffshoreFacillityCosts
