import { Button, Checkbox } from "@equinor/eds-core-react"
import { useCurrentContext } from "@equinor/fusion"
import {
    Dispatch, SetStateAction, useRef, useState,
} from "react"
import { useParams } from "react-router"
import { Case } from "../models/case/Case"
import { Project } from "../models/Project"
import { GetProspService } from "../Services/ProspService"
import { unwrapCase } from "../Utils/common"
import {
    Wrapper,
} from "../Views/Asset/StyledAssetComponents"

interface Props {
    setProject: Dispatch<SetStateAction<Project | undefined>>
    setCase: Dispatch<SetStateAction<Case | undefined>>
}

const ExcelUpload = ({
    setProject,
    setCase,
}: Props) => {
    const { fusionContextId, caseId } = useParams<Record<string, string | undefined>>()
    const currentProject = useCurrentContext()

    const [surf, setSurf] = useState<boolean>(false)
    const [topside, setTopside] = useState<boolean>(false)
    const [substructure, setSubstructure] = useState<boolean>(false)
    const [transport, setTransport] = useState<boolean>(false)
    const fileInputRef = useRef(document.createElement("input"))

    const generateFormData = (file: File): FormData => {
        const data = new FormData()
        data.append("File", file)
        data.append("Surf", surf.toString())
        data.append("Topside", topside.toString())
        data.append("Substructure", substructure.toString())
        data.append("Transport", transport.toString())
        return data
    }

    const onFileUpload = async (event: React.ChangeEvent<HTMLInputElement>): Promise<void> => {
        const currentFiles = event.currentTarget.files
        if (!currentFiles) return

        const data = generateFormData(currentFiles[0])

        try {
            const uploadService = await GetProspService()
            const response = await uploadService.create(caseId!, currentProject?.externalId!, data)
            setProject(response)
            const caseResult = unwrapCase(response.cases.find((o) => o.id === caseId))
            setCase(caseResult)
        } catch {
            console.error("Error uploading Excel document: ")
        }

        fileInputRef.current.files = null
    }

    const disabled = () => !(surf || substructure || topside || transport)
    return (
        <>
            <Wrapper>
                <Checkbox label="Surf" onChange={() => { setSurf(!surf) }} checked={surf} />
                <Checkbox
                    label="Substructure"
                    onChange={() => { setSubstructure(!substructure) }}
                    checked={substructure}
                />
                <Checkbox label="Topside" onChange={() => { setTopside(!topside) }} checked={topside} />
                <Checkbox label="Transport" onChange={() => { setTransport(!transport) }} checked={transport} />
            </Wrapper>
            <Wrapper>
                <Button disabled={disabled()} onClick={(): void => fileInputRef.current.click()}>Upload file</Button>
                <input
                    type="file"
                    id="file-upload"
                    style={{ display: "none" }}
                    ref={fileInputRef}
                    onChange={onFileUpload}
                    value=""
                />
            </Wrapper>
        </>
    )
}

export default ExcelUpload
