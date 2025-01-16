import VideoPlayer from "../VideoPlayer"
import Article from "../Components/Article"

const MakeEdits = () => (
    <Article>
        <Article.Header>How to edit data</Article.Header>
        <Article.Body>
            <VideoPlayer src="https://dcdstorageaccount.blob.core.windows.net/user-guide/claudia%20-%20how%20to%20make%20edits.mp4" />
            <div>
                <p>
                    The project and case dashboards have two primary modes:
                    <strong> &quot;view mode&quot;</strong>
                    {" "}
                    and
                    <strong> &quot;edit mode&quot;</strong>
                    .
                </p>
                <ul>
                    <li>
                        <strong>View Mode:</strong>
                        {" "}
                        In this mode, you can review and present the details of your project or case without making any changes.
                    </li>
                    <li>
                        <strong>Edit Mode:</strong>
                        {" "}
                        To switch to edit mode, click the
                        <em>&quot;Edit&quot;</em>
                        {" "}
                        button located on the toolbar. Once in edit mode:
                        <ul>
                            <li>All previously static data fields transform into input fields, allowing you to make changes.</li>
                            <li>
                                <strong>Autosave:</strong>
                                {" "}
                                Captures your updates instantly, so your progress is always secure.
                            </li>
                            <li>
                                <strong>Undo/Redo:</strong>
                                {" "}
                                The
                                <em>&quot;Undo&quot;</em>
                                {" "}
                                button reverts your last change, while the
                                <em>&quot;Redo&quot;</em>
                                {" "}
                                button reapplies it.
                                These features ensure a smooth, error-free editing experience.
                            </li>
                        </ul>
                    </li>
                </ul>
            </div>

        </Article.Body>
    </Article>
)

export default MakeEdits
