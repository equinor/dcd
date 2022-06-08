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
// import DGEnum from "../models/DGEnum"
import { WrapperInherited } from "../Views/Asset/StyledAssetComponents"

const DgField = styled.div`
    margin-bottom: 2.5rem;
    width: 12rem;
    display: flex;
`

const ActionsContainer = styled.div`
`

interface Props {
    setHasChanges?: Dispatch<SetStateAction<boolean>>
    setValue?: Dispatch<SetStateAction<Date | undefined>>
    value: Date | undefined
    // dGType: DGEnum,
    dGName: string,
    caseValue: Date | null | undefined
}

const DGDateInherited = ({
    setHasChanges,
    setValue,
    value,
    // dGType,
    dGName,
    caseValue,
}: Props) => {
    const [isMismatchedToCase, setIsMismatchedToCase] = useState<boolean | undefined>()

    useEffect(() => {
        (async () => {
            if (caseValue?.toLocaleDateString("en-CA") !== value?.toLocaleDateString("en-CA")) {
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

    const dgReturnDate = () => value?.toLocaleDateString("en-CA")

    // const limitDateToNextDGDate = () => {
    //     if (dGType === DGEnum.DG3) {
    //         return DG4Date?.toLocaleDateString("en-CA")
    //     }
    //     return undefined
    // }

    return (
        <>
            <Typography variant="h6">{dGName}</Typography>
            <DgField>
                <Input
                    defaultValue={dgReturnDate()}
                    key={dgReturnDate()}
                    id="dgDate"
                    type="date"
                    name="dgDate"
                    onChange={onChange}
                    // max={limitDateToNextDGDate()}
                />
            </DgField>
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
        </>
    )
}

export default DGDateInherited
