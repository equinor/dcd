<<<<<<< HEAD
import React from 'react'
import styled from 'styled-components'
import { Typography } from '@equinor/eds-core-react'
import { useTranslation } from "react-i18next";
=======
import React from "react"
import styled from "styled-components"
import { Typography } from "@equinor/eds-core-react"
>>>>>>> main

const Wrapper = styled.div`
    display: flex;
    flex-direction: column;
`

<<<<<<< HEAD
const OverviewView = () => {
    const { t } = useTranslation();
=======
function OverviewView() {
>>>>>>> main
    return (
        <Wrapper>
            <Typography variant="h3">{t('OverviewView.Overview')}</Typography>
        </Wrapper>
    )
}

export default OverviewView
