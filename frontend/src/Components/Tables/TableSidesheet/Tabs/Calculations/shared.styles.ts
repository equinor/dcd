import { Typography } from "@mui/material"
import styled from "styled-components"
import { tokens } from "@equinor/eds-tokens"

export const Container = styled.div`
    padding: 24px;
    max-width: 100%;
    margin: 0 auto;
`

export const Section = styled.div`
    margin-bottom: 24px;
    background: ${tokens.colors.ui.background__default.rgba};
`

export const Formula = styled.div`
    font-family: 'Roboto Mono', monospace;
    background-color: #c9d5d0;
    padding: 20px;
    border-radius: 4px;
    margin: 16px 0;
    line-height: 1.5;
`

export const MainFormula = styled.div`
    font-weight: 500;
    color: #577865;
    font-size: 1.1em;
    margin-bottom: 20px;
    padding: 12px 16px;
    background: ${tokens.colors.ui.background__light.rgba};
    border-radius: 4px;
    border-left: 4px solid #90c2ab;
`

export const FormulaSection = styled.div`
    margin: 16px 0;
    
    h4 {
        color: #90c2ab;
        font-size: 1em;
        margin: 0 0 8px 0;
        font-weight: 500;
    }
`

export const SubFormula = styled.div`
    margin: 8px 0;
    padding: 8px 12px;
    background: ${tokens.colors.ui.background__light.rgba};
    border-radius: 4px;
    color: ${tokens.colors.text.static_icons__default.rgba};
    font-size: 0.95em;
`

export const FormulaList = styled.ul`
    margin: 8px 0;
    padding: 0;
    list-style-type: none;

    & li {
        margin: 6px 0;
        padding-left: 16px;
        position: relative;
        color: ${tokens.colors.text.static_icons__default.rgba};

        &:before {
            content: "•";
            color: #90c2ab;
            position: absolute;
            left: 0;
        }

        & ul {
            margin: 4px 0 4px 16px;
            padding: 0;
            list-style-type: none;

            & li {
                margin: 4px 0;
                font-size: 0.95em;
                color: ${tokens.colors.text.static_icons__tertiary.rgba};

                &:before {
                    content: "○";
                }
            }
        }
    }
`

export const SectionTitle = styled(Typography)`
    color: #57685c;
    font-weight: 500;
    margin-bottom: 12px !important;
`

export const Note = styled.div`
    font-size: 13px;
    color: ${tokens.colors.text.static_icons__tertiary.rgba};
    margin-top: 20px;
    padding: 16px;
    background: ${tokens.colors.ui.background__light.rgba};
    border-radius: 4px;
    line-height: 1.5;

    & strong {
        color: ${tokens.colors.text.static_icons__default.rgba};
    }

    & ul {
        margin: 8px 0;
        padding-left: 20px;
    }

    & li {
        margin: 4px 0;
    }
`

export const SpecialNote = styled(Typography)`
    margin-top: 16px !important;
    padding: 12px 16px;
    background-color: #fff3e0;
    border-left: 4px solid #ff9800;
    border-radius: 4px;
`
