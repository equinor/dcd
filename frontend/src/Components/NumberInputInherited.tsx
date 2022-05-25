import { Input, Label, Typography } from "@equinor/eds-core-react"
import {
    ChangeEventHandler, Dispatch, SetStateAction, useEffect, useState,
} from "react"
import {
    WrapperColumn,
} from "../Views/Asset/StyledAssetComponents"

interface Props {
    setValue?: Dispatch<SetStateAction<number | undefined>>
    value: number | undefined
    setHasChanges?: Dispatch<SetStateAction<boolean>>
    integer: boolean,
    disabled?: boolean
    label: string
    caseValue: number | undefined,
    name: string | undefined,
}

const NumberInputInherited = ({
    setValue,
    value,
    setHasChanges,
    integer,
    disabled,
    label,
    caseValue,
    name,
}: Props) => {
    const [warning, setWarning] = useState<string>("")

    const resetWarning = () => {
        setWarning("")
    }

    useEffect(() => {
        (async () => {
            if (caseValue !== null && caseValue !== undefined && caseValue !== value) {
                return setWarning(`${name} does not match case ${name}`)
            }
            return resetWarning()
        })()
    })

    const onChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        if (setValue) {
            setValue(Number(e.target.value))
        }
        if (setHasChanges) {
            if (e.target.value !== undefined && e.target.value !== "") {
                setHasChanges(true)
            } else {
                setHasChanges(false)
            }
        }
    }

    return (
        <WrapperColumn style={{ paddingLeft: 10 }}>
            <Typography variant="h6" color="red">{warning}</Typography>
            <Label htmlFor="NumberInput" label={label} />
            <Input
                id="NumberInput"
                type="number"
                value={value}
                disabled={disabled}
                onChange={onChange}
                onKeyPress={(event) => {
                    if (integer && !/\d/.test(event.key)) {
                        event.preventDefault()
                    }
                }}
            />
        </WrapperColumn>
    )
}

export default NumberInputInherited
