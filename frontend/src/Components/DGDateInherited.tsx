/* eslint-disable camelcase */
import {
    Typography,
    Input,
    EdsProvider,
    Button,
    Icon,
    Tooltip,
} from "@equinor/eds-core-react"
import { info_circle } from "@equinor/eds-icons"
import {
    useState,
    useEffect,
    ChangeEventHandler,
    Dispatch,
    SetStateAction,
} from "react"
import styled from "styled-components"
import { ToMonthDate } from "../Utils/common"
import { WrapperInherited } from "../Views/Asset/StyledAssetComponents"

const DgField = styled.div`
margin-right: 1rem;
margin-left: 1rem;
margin-bottom: 2rem;
width: 10rem;
display: flex;
`

const ActionsContainer = styled.div`
`

interface Props {
    setHasChanges?: Dispatch<SetStateAction<boolean>>
    setValue?: Dispatch<SetStateAction<Date | undefined>>
    value: Date | undefined
    dGName: string,
    caseValue: Date | null | undefined
    disabled?: boolean
}

const DGDateInherited = ({
    setHasChanges,
    setValue,
    value,
    dGName,
    caseValue,
    disabled,
}: Props) => {
    const [isMismatchedToCase, setIsMismatchedToCase] = useState<boolean | undefined>()

    useEffect(() => {
        (async () => {
            if (ToMonthDate(caseValue) !== ToMonthDate(value)) {
                return setIsMismatchedToCase(true)
            }
            return setIsMismatchedToCase(false)
        })()
    })

    const onChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        if (setValue) {
            setValue(new Date(e.target.value))
        }
        if (setHasChanges) {
            if (e.target.value !== undefined && e.target.value !== "") {
                setHasChanges(true)
            } else {
                setHasChanges(false)
            }
        }
    }

    const dgReturnDate = () => ToMonthDate(value)

    return (
        <>
            <Typography variant="h4">{dGName}</Typography>
            <WrapperInherited>
                <EdsProvider density="compact">
                    <ActionsContainer hidden={!isMismatchedToCase}>
                        <Tooltip
                            title="Data does not match data on case. Using this data will overwrite asset data."
                            hidden={!isMismatchedToCase}
                        >
                            <Button
                                variant="ghost_icon"
                                aria-label="Case mismatch"
                                color="danger"
                            >
                                <Icon data={info_circle} />
                            </Button>
                        </Tooltip>
                    </ActionsContainer>
                </EdsProvider>
            </WrapperInherited>
            <DgField>
                <Input
                    defaultValue={dgReturnDate()}
                    id="dgDate"
                    type="month"
                    name="dgDate"
                    onChange={onChange}
                    disabled={disabled}
                />
            </DgField>
        </>
    )
}

export default DGDateInherited
