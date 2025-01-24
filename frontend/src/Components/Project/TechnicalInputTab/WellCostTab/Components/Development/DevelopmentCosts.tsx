import {
    useEffect,
    useRef,
    useState,
} from "react"

import useTechnicalInputEdits from "@/Hooks/useEditTechnicalInput"
import { useDataFetch } from "@/Hooks/useDataFetch"
import { useDebounce } from "@/Hooks/useDebounce"
import OperationalWellCost from "../Shared/CostCell"
import {
    FullwidthTable,
    Head,
    Body,
    Row,
    Cell,
    CostWithCurrency,
} from "../Shared/SharedWellStyles"
import { useAppContext } from "@/Context/AppContext"

type DevelopmentCostsState = Omit<
    Components.Schemas.DevelopmentOperationalWellCostsOverviewDto,
    "developmentOperationalWellCostsId" | "projectId"
>

const DevelopmentCosts = () => {
    const revisionAndProjectData = useDataFetch()
    const { developmentOperationalWellCostsId } = revisionAndProjectData?.commonProjectAndRevisionData.developmentOperationalWellCosts ?? {}
    const { developmentOperationalWellCosts } = revisionAndProjectData?.commonProjectAndRevisionData ?? {}
    const { currency } = revisionAndProjectData?.commonProjectAndRevisionData ?? {}
    const { projectId } = revisionAndProjectData ?? {}
    const { addDevelopmentWellCostEdit } = useTechnicalInputEdits()
    const { editMode } = useAppContext()
    const previousCostsRef = useRef<DevelopmentCostsState | null>(null)

    const [costs, setCosts] = useState<DevelopmentCostsState>({
        rigUpgrading: 0,
        rigMobDemob: 0,
        annualWellInterventionCostPerWell: 0,
        pluggingAndAbandonment: 0,
    })

    const debouncedCosts = useDebounce(costs, 1000)

    useEffect(() => {
        if (developmentOperationalWellCosts) {
            const newCosts = {
                rigUpgrading: developmentOperationalWellCosts?.rigUpgrading ?? 0,
                rigMobDemob: developmentOperationalWellCosts?.rigMobDemob ?? 0,
                annualWellInterventionCostPerWell: developmentOperationalWellCosts?.annualWellInterventionCostPerWell ?? 0,
                pluggingAndAbandonment: developmentOperationalWellCosts?.pluggingAndAbandonment ?? 0,
            } as DevelopmentCostsState
            setCosts(newCosts)
            previousCostsRef.current = newCosts
        }
    }, [revisionAndProjectData])

    useEffect(() => {
        if (developmentOperationalWellCostsId && projectId && debouncedCosts && editMode) {
            const hasChanges = !previousCostsRef.current || Object.entries(debouncedCosts).some(
                ([key, value]) => previousCostsRef.current?.[key as keyof DevelopmentCostsState] !== value,
            )
            if (hasChanges) {
                previousCostsRef.current = { ...debouncedCosts }
                addDevelopmentWellCostEdit(projectId, developmentOperationalWellCostsId, debouncedCosts)
            }
        }
    }, [debouncedCosts, developmentOperationalWellCostsId, projectId, addDevelopmentWellCostEdit, editMode])

    return (
        <FullwidthTable>
            <Head>
                <Row>
                    <Cell>
                        Development costs
                    </Cell>
                    <Cell>
                        <CostWithCurrency>
                            Cost
                            <div>
                                {`${currency === 1 ? "(mill NOK)" : "(mill USD)"}`}
                            </div>
                        </CostWithCurrency>
                    </Cell>
                </Row>
            </Head>
            <Body>
                <OperationalWellCost
                    title="Rig upgrading"
                    setValue={(value: number | undefined) => setCosts((prev) => ({ ...prev, rigUpgrading: value ?? 0 }))}
                    value={costs.rigUpgrading ?? 0}
                />
                <OperationalWellCost
                    title="Rig mob/demob"
                    setValue={(value: number | undefined) => setCosts((prev) => ({ ...prev, rigMobDemob: value ?? 0 }))}
                    value={costs.rigMobDemob ?? 0}
                />
                <OperationalWellCost
                    title="Annual well intervention cost per well"
                    setValue={(value: number | undefined) => setCosts((prev) => ({ ...prev, annualWellInterventionCostPerWell: value ?? 0 }))}
                    value={costs.annualWellInterventionCostPerWell ?? 0}
                />
                <OperationalWellCost
                    title="Plugging and abandonment"
                    setValue={(value: number | undefined) => setCosts((prev) => ({ ...prev, pluggingAndAbandonment: value ?? 0 }))}
                    value={costs.pluggingAndAbandonment ?? 0}
                />
            </Body>
        </FullwidthTable>
    )
}

export default DevelopmentCosts
