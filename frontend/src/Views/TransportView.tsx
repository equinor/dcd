import {
    Typography,
} from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import {
    useParams,
} from "react-router-dom"
import { useCurrentContext } from "@equinor/fusion"
import Save from "../Components/Save"
import AssetName from "../Components/AssetName"
import TimeSeries from "../Components/TimeSeries"
import { Transport } from "../models/assets/transport/Transport"
import { Case } from "../models/case/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetTransportService } from "../Services/TransportService"
import { IsDefaultDate, unwrapCase, unwrapProjectId } from "../Utils/common"
import { initializeFirstAndLastYear } from "./Asset/AssetHelper"
import {
    AssetViewDiv, Wrapper,
} from "./Asset/StyledAssetComponents"
import AssetTypeEnum from "../models/assets/AssetTypeEnum"
import NumberInput from "../Components/NumberInput"
import Maturity from "../Components/Maturity"
import { TransportCostProfile } from "../models/assets/transport/TransportCostProfile"
import { TransportCessationCostProfile } from "../models/assets/transport/TransportCessationCostProfile"
import AssetCurrency from "../Components/AssetCurrency"
import DGDateInherited from "../Components/DGDateInherited"

import { IAssetService } from "../Services/IAssetService"

const TransportView = () => {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [transport, setTransport] = useState<Transport>()
    const [hasChanges, setHasChanges] = useState(false)
    const [transportName, setTransportName] = useState<string>("")
    const { fusionContextId, caseId, transportId } = useParams<Record<string, string | undefined>>()
    const currentProject = useCurrentContext()

    const [firstTSYear, setFirstTSYear] = useState<number>()
    const [lastTSYear, setLastTSYear] = useState<number>()
    const [gasExportPipelineLength, setGasExportPipelineLength] = useState<number | undefined>()
    const [oilExportPipelineLength, setOilExportPipelineLength] = useState<number | undefined>()
    const [maturity, setMaturity] = useState<Components.Schemas.Maturity | undefined>()
    const [costProfile, setCostProfile] = useState<TransportCostProfile>()
    const [cessationCostProfile, setCessationCostProfile] = useState<TransportCessationCostProfile>()
    const [currency, setCurrency] = useState<Components.Schemas.Currency>(1)
    const [costYear, setCostYear] = useState<number | undefined>()
    const [dG3Date, setDG3Date] = useState<Date>()
    const [dG4Date, setDG4Date] = useState<Date>()
    const [transportService, setTransportService] = useState<IAssetService>()

    useEffect(() => {
        (async () => {
            try {
                const projectId = unwrapProjectId(currentProject?.externalId)
                const projectResult = await (await GetProjectService()).getProjectByID(projectId)
                setProject(projectResult)
                const service = await GetTransportService()
                setTransportService(service)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${currentProject?.externalId}`, error)
            }
        })()
    }, [])

    useEffect(() => {
        (async () => {
            if (project !== undefined) {
                const caseResult = unwrapCase(project.cases.find((o) => o.id === caseId))
                setCase(caseResult)
                let newTransport = project.transports.find((s) => s.id === transportId)
                if (newTransport !== undefined) {
                    if (newTransport.DG3Date === null
                        || IsDefaultDate(newTransport.DG3Date)) {
                        newTransport.DG3Date = caseResult?.DG3Date
                    }
                    if (newTransport.DG4Date === null
                        || IsDefaultDate(newTransport.DG4Date)) {
                        newTransport.DG4Date = caseResult?.DG4Date
                    }
                    setTransport(newTransport)
                } else {
                    newTransport = new Transport()
                    newTransport.currency = project.currency
                    newTransport.DG3Date = caseResult?.DG3Date
                    newTransport.DG4Date = caseResult?.DG4Date
                    setTransport(newTransport)
                }
                setTransportName(newTransport?.name!)
                setGasExportPipelineLength(newTransport?.gasExportPipelineLength)
                setOilExportPipelineLength(newTransport?.oilExportPipelineLength)
                setCostYear(newTransport?.costYear)
                setMaturity(newTransport?.maturity ?? undefined)
                setCurrency(newTransport.currency ?? 1)
                setDG3Date(newTransport.DG3Date ?? undefined)
                setDG4Date(newTransport.DG4Date ?? undefined)

                setCostProfile(newTransport.costProfile)
                setCessationCostProfile(newTransport.cessationCostProfile)

                if (caseResult?.DG4Date) {
                    const dg4 = newTransport?.source === 1 ? newTransport.DG4Date?.getFullYear()
                        : caseResult.DG4Date.getFullYear()
                    initializeFirstAndLastYear(
                        dg4!,
                        [newTransport.costProfile, newTransport.cessationCostProfile],
                        setFirstTSYear,
                        setLastTSYear,
                    )
                }
            }
        })()
    }, [project])

    useEffect(() => {
        if (transport !== undefined) {
            const newTransport: Transport = { ...transport }
            newTransport.gasExportPipelineLength = gasExportPipelineLength
            newTransport.oilExportPipelineLength = oilExportPipelineLength
            newTransport.costYear = costYear
            newTransport.maturity = maturity
            newTransport.costProfile = costProfile
            newTransport.cessationCostProfile = cessationCostProfile
            newTransport.currency = currency
            newTransport.DG3Date = dG3Date
            newTransport.DG4Date = dG4Date

            if (caseItem?.DG4Date) {
                const dg4 = newTransport?.source === 1 ? newTransport.DG4Date?.getFullYear()
                    : caseItem.DG4Date.getFullYear()
                initializeFirstAndLastYear(
                    dg4!,
                    [costProfile, cessationCostProfile],
                    setFirstTSYear,
                    setLastTSYear,
                )
            }
            setTransport(newTransport)
        }
    }, [gasExportPipelineLength, oilExportPipelineLength, maturity,
        costProfile, cessationCostProfile, currency, costYear, dG3Date, dG4Date])

    const setAllStates = (timeSeries: any) => {
        if (timeSeries) {
            if (timeSeries.name === "Cost profile") {
                setCostProfile(timeSeries)
            }
            if (timeSeries.name === "Cessation cost profile") {
                setCessationCostProfile(timeSeries)
            }
        }
    }

    if (!transport || !caseItem) {
        return null
    }

    return (
        <AssetViewDiv>
            <Wrapper>
                <Typography variant="h2">Transport</Typography>
                <Save
                    name={transportName}
                    setHasChanges={setHasChanges}
                    hasChanges={hasChanges}
                    setAsset={setTransport}
                    setProject={setProject}
                    asset={transport!}
                    assetService={transportService!}
                    assetType={AssetTypeEnum.transports}
                />
                <Typography variant="h6">
                    {transport?.LastChangedDate?.toLocaleString()
                        ? `Last changed: ${transport?.LastChangedDate?.toLocaleString()}` : ""}
                </Typography>
            </Wrapper>
            <AssetName
                setName={setTransportName}
                name={transportName}
                setHasChanges={setHasChanges}
            />
            <Wrapper>
                <DGDateInherited
                    setHasChanges={setHasChanges}
                    setValue={setDG3Date}
                    dGName="DG3"
                    value={dG3Date}
                    caseValue={caseItem?.DG3Date}
                    disabled={transport?.source === 1}
                />
                <DGDateInherited
                    setHasChanges={setHasChanges}
                    setValue={setDG4Date}
                    dGName="DG4"
                    value={dG4Date}
                    caseValue={caseItem?.DG4Date}
                    disabled={transport?.source === 1}
                />
            </Wrapper>
            <AssetCurrency
                setCurrency={setCurrency}
                setHasChanges={setHasChanges}
                currentValue={currency}
            />
            <Typography>
                {`Prosp version: ${transport?.ProspVersion
                    ? transport?.ProspVersion.toLocaleDateString() : "N/A"}`}
            </Typography>
            <Typography>
                {`Source: ${transport?.source === 0 || transport?.source === undefined ? "ConceptApp" : "Prosp"}`}
            </Typography>
            <Wrapper>
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setCostYear}
                    value={costYear ?? 0}
                    integer
                    label="Cost year"
                />
            </Wrapper>
            <Wrapper>
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setGasExportPipelineLength}
                    value={gasExportPipelineLength ?? 0}
                    integer
                    label="Length of gas export pipeline"
                />
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setOilExportPipelineLength}
                    value={oilExportPipelineLength ?? 0}
                    integer
                    label="Length of oil export pipeline"
                />
            </Wrapper>
            <Maturity
                setMaturity={setMaturity}
                currentValue={maturity}
                setHasChanges={setHasChanges}
            />
            <TimeSeries
                dG4Year={transport.source === 1 ? transport.DG4Date!.getFullYear() : caseItem.DG4Date!.getFullYear()}
                setTimeSeries={setAllStates}
                setHasChanges={setHasChanges}
                timeSeries={[costProfile, cessationCostProfile]}
                firstYear={firstTSYear!}
                lastYear={lastTSYear!}
                profileName={["Cost profile", "Cessation cost profile"]}
                profileEnum={project?.currency!}
                profileType="Cost"
            />
        </AssetViewDiv>
    )
}

export default TransportView
