import React from "react"
import { Banner, Icon } from "@equinor/eds-core-react"
import { info_circle } from "@equinor/eds-icons"
import styled from "styled-components"

const StickyBanner = styled(Banner)`
    position: sticky;
    top: 0;
    z-index: 1;
`

const IndexView: React.FC = () => (
    <div>
        <StickyBanner>
            <Banner.Icon variant="info">
                <Icon data={info_circle} />
            </Banner.Icon>
            <Banner.Message>
                Select a project to view
            </Banner.Message>
        </StickyBanner>
    </div>
)

export default IndexView
