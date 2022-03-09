import {
    ChangeEventHandler,
    FormEventHandler,
    MouseEventHandler,
    useState,
    VoidFunctionComponent,
} from "react"
import { Button, Input, Label } from "@equinor/eds-core-react"

import { AssetFormActionsContainer, AssetFormContainer } from "./styles/shared"

type Props = {
    onCancel: MouseEventHandler<HTMLButtonElement>
}

export const CreateDrainageStrategyForm: VoidFunctionComponent<Props> = ({ onCancel }) => {
    const [fieldValues, setFieldValues] = useState<Record<string, any>>({})

    const handleFieldChange: ChangeEventHandler<HTMLInputElement> = (e) => {
        setFieldValues({
            ...fieldValues,
            [e.target.name]: e.target.value,
        })
    }

    const onCancelClick: MouseEventHandler<HTMLButtonElement> = (e) => {
        setFieldValues({})
        onCancel(e)
    }

    const onSubmit: FormEventHandler<HTMLFormElement> = (e) => {
        e.preventDefault()
        console.log(fieldValues)
    }

    return (
        <AssetFormContainer onSubmit={onSubmit}>
            <div>
                <Label htmlFor="productionProfileOil" label="Production profile oil" />
                <Input
                    name="productionProfileOil"
                    id="productionProfileOil"
                    placeholder="Production profile oil"
                    onChange={handleFieldChange}
                    type="number"
                />
            </div>
            <div>
                <Label htmlFor="netSalesGas" label="Net sales gas" />
                <Input
                    name="netSalesGas"
                    id="netSalesGas"
                    placeholder="Net sales gas"
                    onChange={handleFieldChange}
                    type="number"
                />
            </div>
            <div>
                <Label htmlFor="co2Emissions" label={"CO\u2082 emissions"} />
                <Input
                    name="co2Emissions"
                    id="co2Emissions"
                    placeholder={"CO\u2082 emissions"}
                    onChange={handleFieldChange}
                    type="number"
                />
            </div>

            <AssetFormActionsContainer>
                <Button type="submit">Create asset</Button>
                <Button variant="ghost" color="secondary" onClick={onCancelClick}>Cancel</Button>
            </AssetFormActionsContainer>
        </AssetFormContainer>
    )
}
