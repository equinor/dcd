import { tokens } from "@equinor/eds-tokens"
import { Button, NativeSelect, Table } from "@equinor/eds-core-react"
import { MouseEventHandler, ReactElement } from "react"
import { Well } from "../../models/Well"
import { WellProjectWell } from "../../models/WellProjectWell"
import { GetWellProjectWellService } from "../../Services/WellProjectWellService"
import { Case } from "../../models/Case"
import { WellProject } from "../../models/assets/wellproject/WellProject"

interface Props {
    wells: Well[]
    wellProjectWells: WellProjectWell[] | undefined | null
    wellProject: WellProject
}

function WellTableRow({ wells, wellProjectWells, wellProject }: Props) {
    const IncreaseWellCase = (well: Well, wellProjectWell: WellProjectWell | undefined) => {
        if (!wellProjectWell) {
            const newWellCase = new WellProjectWell()
            newWellCase.wellId = well.id
            newWellCase.wellProjectId = wellProject.id
            newWellCase.count = 1
            const newProject = GetWellProjectWellService().createWellProjectWell(newWellCase)
        } else {
            const newWellCase = { ...wellProjectWell }
            newWellCase.count! += 1
            const newProject = GetWellProjectWellService().updateWellProjectWell(newWellCase)
        }
    }

    const DecreaseWellCase = (well: Well, wellProjectWell: WellProjectWell | undefined) => {
        if (!wellProjectWell) {
            const newWellCase = new WellProjectWell()
            newWellCase.wellId = well.id
            newWellCase.wellProjectId = wellProject.id
            newWellCase.count = 1
            const newProject = GetWellProjectWellService().createWellProjectWell(newWellCase)
        } else {
            const newWellCase = { ...wellProjectWell }
            newWellCase.count! -= 1
            const newProject = GetWellProjectWellService().updateWellProjectWell(newWellCase)
        }
    }

    const GenerateWellTableRows = (): ReactElement[] => {
        const tableRows: JSX.Element[] = []
        wells?.forEach((w) => {
            const wc = wellProjectWells?.find((x) => x.wellId === w.id && x.wellProjectId === wellProject.id)
            // tableRows.push((<WellTableRow key={w.id} well={w} wellCase={wc} />))
            tableRows.push((
                <Table.Row key={w.id}>
                    <Table.Cell>
                        {wc?.count ?? 0}
                        <Button onClick={() => IncreaseWellCase(w, wc)}>Increase</Button>
                        <Button onClick={() => DecreaseWellCase(w, wc)}>Decrease</Button>
                        {console.log("WellProjectWell.WellId: ", wc?.wellId)}
                        {wc?.wellId}
                    </Table.Cell>
                    <Table.Cell>
                        {w.name}
                    </Table.Cell>
                    <Table.Cell>
                        {w.wellCategory}
                        <NativeSelect id="wellCategory" label="Well category" value={w.wellCategory}>
                            <option key="0" value={0}>Oil producer</option>
                            <option key="1" value={1}>Gas producer</option>
                            <option key="2" value={2}>Water injector</option>
                            <option key="3" value={3}>Gas Injector</option>
                            <option key="4" value={4}>Exploration well</option>
                            <option key="5" value={5}>Appraisal well</option>
                            <option key="6" value={6}>Sidetrack</option>
                            <option key="7" value={7}>Rig mob/demob</option>
                        </NativeSelect>
                    </Table.Cell>
                    <Table.Cell>
                        {w.drillingDays}
                    </Table.Cell>
                    <Table.Cell>
                        {w.wellCost}
                    </Table.Cell>
                </Table.Row>))
        })
        return tableRows
    }

    return (<>{GenerateWellTableRows()}</>)
}

export default WellTableRow
