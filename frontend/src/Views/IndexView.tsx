import React from "react"
import { Banner, Icon } from "@equinor/eds-core-react"
import { info_circle } from "@equinor/eds-icons"
import UserGuideView from "../Components/Guide/UserGuide"

const IndexView: React.FC = () => (
    <div>
        <Banner>
            <Banner.Icon variant="info">
                <Icon data={info_circle} />
            </Banner.Icon>
            <Banner.Message>
                Select a project to view
            </Banner.Message>
        </Banner>
        <UserGuideView />
    </div>
)

export default IndexView
