import { Button, Checkbox } from "@equinor/eds-core-react"
import {
 Dispatch, SetStateAction, useRef, useState,
} from "react"
import { useParams } from "react-router"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetUploadService } from "../Services/UploadService"
import { unwrapCase } from "../Utils/common"

interface Props {
    setProject: Dispatch<SetStateAction<Project | undefined>>
    setCase: Dispatch<SetStateAction<Case | undefined>>
}

const ExcelUpload = ({
    setProject,
    setCase,
}: Props) => {
    const params = useParams()
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
            const uploadService = GetUploadService()
            const response = await uploadService.create(params.caseId!, params.projectId!, data)
            setProject(response)
            const caseResult: Case = unwrapCase(response.cases.find((o) => o.id === params.caseId))
            setCase(caseResult)
        } catch {
            console.error("Error uploading Excel document: ")
        }

        fileInputRef.current.files = null
    }

    const disabled = () => !(surf || substructure || topside || transport)
    return (
        <>
            <Checkbox label="Surf" onChange={() => { setSurf(!surf) }} checked={surf} />
            <Checkbox label="Substructure" onChange={() => { setSubstructure(!substructure) }} checked={substructure} />
            <Checkbox label="Topside" onChange={() => { setTopside(!topside) }} checked={topside} />
            <Checkbox label="Transport" onChange={() => { setTransport(!transport) }} checked={transport} />
            <label htmlFor="file-upload">
                <Button disabled={disabled()} onClick={(): void => fileInputRef.current.click()}>Upload file</Button>
                <input
                    type="file"
                    id="file-upload"
                    style={{ display: "none" }}
                    ref={fileInputRef}
                    onChange={onFileUpload}
                    value=""
                />
            </label>
        </>
    )
}

export default ExcelUpload
