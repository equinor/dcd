import { useEffect, useState } from "react"
import Article from "../Components/Article"
import { getVideoService } from "@/Services/VideoService"

const CreateCase = () => {
    const [videoUrl, setVideoUrl] = useState<string>("")

    useEffect(() => {
        const loadVideo = async () => {
            try {
                const videoData = await getVideoService().getVideo("how_to_create_case.mp4")
                setVideoUrl(videoData.base64EncodedData)
            } catch (error) {
                console.error("Error loading video:", error)
            }
        }
        loadVideo()
    }, [])

    return (
        <Article>
            <Article.Header>How to create a case</Article.Header>
            <Article.Body>
                {videoUrl
                    && (
                        <video controls>
                            <source src={videoUrl} type="video/mp4" />
                            Your browser does not support HTML video.
                        </video>
                    )}
                <ol>
                    <li>
                        <p><strong>Go to the Project Overview Page:</strong></p>
                        <ul>
                            <li>
                                Find the table with your existing cases.
                            </li>
                        </ul>
                    </li>
                    <li>
                        <p><strong>Choose How to Create a Case:</strong></p>
                        <ul>
                            <li>
                                <strong>Option 1:</strong>
                                {" "}
                                Click the &apos;Create a Case&apos; button above the case overview table.

                            </li>
                            <li>
                                <strong>Option 2:</strong>
                                {" "}
                                Click the small&nbsp;
                                <strong>plus (+) icon</strong>
                                {" "}
                                on the sidebar (available anywhere in the app).
                            </li>
                        </ul>
                    </li>
                    <li>
                        <p><strong>Fill in the Details:</strong></p>
                        <ul>
                            <li>
                                A modal will appear.
                            </li>
                            <li>

                                Enter the&nbsp;
                                <strong>case name</strong>
                                {" "}
                                (mandatory).

                            </li>
                            <li>
                                Add other details if needed (you can edit them later).
                            </li>
                        </ul>
                    </li>
                    <li>
                        <p><strong>Finalize:</strong></p>
                        <ul>
                            <li>

                                Click&nbsp;
                                <strong>&apos;Create Case&apos;</strong>
                                {" "}
                                to save it.

                            </li>
                            <li>
                                The new case will appear in in the sidebar, and in the case overview table.
                            </li>
                        </ul>
                    </li>
                </ol>
            </Article.Body>
        </Article>
    )
}

export default CreateCase
