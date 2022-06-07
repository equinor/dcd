import { Button, Checkbox } from "@equinor/eds-core-react"
import { useState } from "react"
import { useParams } from "react-router"
import { GetUploadService } from "../Services/UploadService"

const ExcelUpload = () => {
    const [selectedFile, setSelectedFile] = useState<any>()
    const [isFilePicked, setIsFilePicked] = useState(false)
    const params = useParams()
    const [surf, setSurf] = useState<boolean>(false)
    const [topside, setTopside] = useState<boolean>(false)
    const [substructure, setSubstructure] = useState<boolean>(false)
    const [transport, setTransport] = useState<boolean>(false)
    const changeHandler = (event: any) => {
        setSelectedFile(event.target.files[0])
        setIsFilePicked(true)
    }

    const handleSubmission = () => {
        const formData = new FormData()

        formData.append("File", selectedFile)
        formData.append("Surf", surf.toString())
        formData.append("Topside", topside.toString())
        formData.append("Substructure", substructure.toString())
        formData.append("Transport", transport.toString())
        const uploadService = GetUploadService()
        uploadService.create(params.caseId!, params.projectId!, formData)
    }

    const disabled = () => {
        console.log("Surf: ", surf)
        console.log("Substructure: ", substructure)
        console.log("Topside: ", topside)
        return !(surf || substructure || topside)
    }
    return (
        <label htmlFor="file-upload">
            <input type="file" id="file-upload" style={{ display: "none" }} multiple onChange={changeHandler} />
            {/* {isFilePicked ? (
                <div>
                    <p>
                        Filename:
                        {selectedFile?.name}
                    </p>
                    <p>
                        Filetype:
                        {selectedFile?.type}
                    </p>
                    <p>
                        Size in bytes:
                        {selectedFile.size}
                    </p>
                    <p>
                        lastModifiedDate:
                        {selectedFile.lastModifiedDate.toLocaleDateString()}
                    </p>
                </div>
            ) : (
                <p>Select a file to show details</p>
            )} */}
            <Checkbox label="Surf" onChange={() => { setSurf(!surf) }} checked={surf} />
            <Checkbox label="Substructure" onChange={() => { setSubstructure(!substructure) }} checked={substructure} />
            <Checkbox label="Topside" onChange={() => { setTopside(!topside) }} checked={topside} />
            <Checkbox label="Transport" onChange={() => { setTransport(!transport) }} checked={transport} />
            <Button onClick={handleSubmission} as="span">Upload file</Button>
        </label>
    )
}

export default ExcelUpload
