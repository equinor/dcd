import { Button } from "@equinor/eds-core-react"
import { useState } from "react"

const ExcelUpload = () => {
    const [selectedFile, setSelectedFile] = useState<any>()
    const [isFilePicked, setIsFilePicked] = useState(false)
    const changeHandler = (event: any) => {
        setSelectedFile(event.target.files[0])
        setIsFilePicked(true)
    }

    const handleSubmission = () => {
        const formData = new FormData()

        formData.append("File", selectedFile)

        fetch(
            "http://localhost:5000/api/Upload",
            {
                method: "POST",
                body: formData,
            },
        )
            .then((response) => response.json())
            .then((result) => {
                console.log("Success:", result)
            })
            .catch((error) => {
                console.error("Error:", error)
            })
    }
    return (
        <label htmlFor="file-upload">
            <input type="file" id="file-upload" style={{ display: "none" }} multiple onChange={changeHandler} />
            {isFilePicked ? (
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
            )}
            <Button onClick={handleSubmission} as="span">Upload file</Button>
        </label>
    )
}

export default ExcelUpload
