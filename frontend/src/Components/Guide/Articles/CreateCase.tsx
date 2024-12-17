import VideoPlayer from "../VideoPlayer"
import Article from "../Components/Article"
import HowCreateCase from "../Components/howCreateCase.mp4"

const CreateCase = () => (
    <Article>
        <Article.Header>How to create a case</Article.Header>
        <Article.Body>
            <VideoPlayer src={HowCreateCase} />
            <ol data-spread="true" start={1} data-pm-slice="3 5 []">
                <li>
                    <p><strong>Go to the Project Overview Page:</strong></p>
                    <ul data-spread="false">
                        <li>
                            <p>Find the table with your existing cases.</p>
                        </li>
                    </ul>
                </li>
                <li>
                    <p><strong>Choose How to Create a Case:</strong></p>
                    <ul data-spread="false">
                        <li>
                            <p>
                                <strong>Option 1:</strong>
                                {" "}
                                Click the &apos;Create a Case&apos; button above the table.
                            </p>
                        </li>
                        <li>
                            <p>
                                <strong>Option 2:</strong>
                                {" "}
                                Click the small&nbsp;
                                <strong>plus (+) icon</strong>
                                {" "}
                                on the sidebar (available anywhere in the app).
                            </p>
                        </li>
                    </ul>
                </li>
                <li>
                    <p><strong>Fill in the Details:</strong></p>
                    <ul data-spread="false">
                        <li>
                            <p>A modal will appear.</p>
                        </li>
                        <li>
                            <p>
                                Enter the&nbsp;
                                <strong>case name</strong>
                                {" "}
                                (mandatory).
                            </p>
                        </li>
                        <li>
                            <p>Add other details if needed (you can edit them later).</p>
                        </li>
                    </ul>
                </li>
                <li>
                    <p><strong>Finalize:</strong></p>
                    <ul data-spread="false">
                        <li>
                            <p>
                                Click&nbsp;
                                <strong>&apos;Create Case&apos;</strong>
                                {" "}
                                to save it.
                            </p>
                        </li>
                    </ul>
                </li>
                <li>
                    <p><strong>You&rsquo;re Done:</strong></p>
                    <ul data-spread="false">
                        <li>
                            <p>The new case will appear in your list.</p>
                        </li>
                    </ul>
                </li>
            </ol>
        </Article.Body>
    </Article>
)

export default CreateCase
