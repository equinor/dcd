import VideoPlayer from "../VideoPlayer"
import Article from "../Components/Article"

const CreateCase = () => (
    <Article>
        <Article.Header>How to create a case</Article.Header>
        <Article.Body>
            <VideoPlayer src="How%20t%20o%20create%20case.mp4" />
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

export default CreateCase
