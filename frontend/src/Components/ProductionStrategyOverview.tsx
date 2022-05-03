import {
    NativeSelect,
} from "@equinor/eds-core-react"
import {
    ChangeEvent, Dispatch, SetStateAction,
} from "react"
import styled from "styled-components"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetCaseService } from "../Services/CaseService"

const ProductionStrategyOverviewDropdown = styled(NativeSelect)`
width: 20rem;
padding-bottom: 2em;
`

interface Props {
    caseItem: Case | undefined,
    setProductionStrategyOverview: Dispatch<SetStateAction<Components.Schemas.ProductionStrategyOverview>>,
    setProject: Dispatch<SetStateAction<Project | undefined>>,
    currentValue: Components.Schemas.ProductionStrategyOverview,
}

const ProductionStrategyOverview = ({
    caseItem,
    setProductionStrategyOverview,
    setProject,
    currentValue,
}: Props) => {
    const onChange = async (event: ChangeEvent<HTMLSelectElement>) => {
        let al:Components.Schemas.ProductionStrategyOverview
        switch (event.currentTarget.selectedOptions[0].value) {
        case "0":
            setProductionStrategyOverview(0)
            al = 0
            break
        case "1":
            setProductionStrategyOverview(1)
            al = 1
            break
        case "2":
            setProductionStrategyOverview(2)
            al = 2
            break
        case "3":
            setProductionStrategyOverview(3)
            al = 3
            break
        case "4":
            setProductionStrategyOverview(4)
            al = 4
            break
        default:
            al = 0
            setProductionStrategyOverview(0)
            break
        }
        if (caseItem !== undefined) {
            const newCase = Case.Copy(caseItem)
            newCase.productionStrategyOverview = al
            const newProject = await GetCaseService().updateCase(newCase)
            setProject(newProject)
        }
    }

    return (
        <ProductionStrategyOverviewDropdown
            label="Production Strategy Overview"
            id="ProductionStrategyOverview"
            placeholder="Choose a production strategy overview"
            onChange={(event: ChangeEvent<HTMLSelectElement>) => onChange(event)}
            value={currentValue}
            disabled={false}
        >
            <option key="0" value={0}>Depletion</option>
            <option key="1" value={1}>Water Injection</option>
            <option key="2" value={2}>Gas Injection</option>
            <option key="3" value={3}>WAG</option>
            <option key="4" value={4}>Mixed</option>
        </ProductionStrategyOverviewDropdown>
    )
}

export default ProductionStrategyOverview
