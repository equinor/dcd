/* eslint-disable @typescript-eslint/no-unused-vars */
import { Button, Checkbox } from "@equinor/eds-core-react"
import {
    Dispatch, SetStateAction, useEffect, useRef, useState,
} from "react"
import { useParams } from "react-router"
import { Case } from "../../models/case/Case"
import { Project } from "../../models/Project"
import { GetUploadService } from "../../Services/UploadService"
import { unwrapCase } from "../../Utils/common"
import { DriveItem } from "../../models/sharepoint/DriveItem"

interface Props {
    setProject: Dispatch<SetStateAction<Project | undefined>>
    setCase: Dispatch<SetStateAction<Case | undefined>>
    project: Project
    caseItem: Case
}

const ExcelUpload = ({
    setProject,
    setCase,
    project,
    caseItem,
}: Props) => {
    const { fusionProjectId, caseId } = useParams<Record<string, string | undefined>>()
    const [surf, setSurf] = useState<boolean>(false)
    const [topside, setTopside] = useState<boolean>(false)
    const [substructure, setSubstructure] = useState<boolean>(false)
    const [transport, setTransport] = useState<boolean>(false)
    const fileInputRef = useRef(document.createElement("input"))
    const [files, setFiles] = useState<DriveItem[]>()

    useEffect(() => {
        (async () => {
            // eslint-disable-next-line max-len
            const fileList = await (await GetUploadService()).create(project.id, caseItem.id!, { webUrl: files![0].webUrl, Surf: "true"})
            console.log(fileList)
        })()
    }, [files])

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
            const uploadService = await GetUploadService()
            const response = await uploadService.create(caseId!, fusionProjectId!, data)
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
            {files ? console.log(files) : []}
            <p>BS</p>
        </>
    )
}

export default ExcelUpload
