import React from 'react'
import styled from 'styled-components'
import { Typography } from '@equinor/eds-core-react'

const Wrapper = styled.div`
    display: flex;
    flex-direction: column;
`

const OverviewView = () => {
    return (
        <Wrapper>
            <Typography variant="h3">Overview</Typography>
        </Wrapper>
    )
}

export default OverviewView
