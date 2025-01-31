import { Typography } from "@mui/material"
import styled from "styled-components"

export const Container = styled.div`
    padding: 24px;
    max-width: 100%;
    margin: 0 auto;
`

export const Section = styled.div`
    margin-bottom: 24px;
    background: #fff;
    border-radius: 4px;
    box-shadow: 0 1px 2px rgba(0, 0, 0, 0.1);
    padding: 24px;
`

export const Formula = styled.div`
    font-family: 'Roboto Mono', monospace;
    background-color: #f5f6f7;
    padding: 20px;
    border-radius: 4px;
    margin: 16px 0;
    line-height: 1.5;
`

export const MainFormula = styled.div`
    font-weight: 500;
    color: #0D47A1;
    font-size: 1.1em;
    margin-bottom: 20px;
    padding: 12px 16px;
    background: #E3F2FD;
    border-radius: 4px;
    border-left: 4px solid #1976D2;
`

export const FormulaSection = styled.div`
    margin: 16px 0;
    
    h4 {
        color: #1976D2;
        font-size: 1em;
        margin: 0 0 8px 0;
        font-weight: 500;
    }
`

export const SubFormula = styled.div`
    margin: 8px 0;
    padding: 8px 12px;
    background: #FAFAFA;
    border-radius: 4px;
    color: #424242;
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
        color: #424242;

        &:before {
            content: "•";
            color: #1976D2;
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
                color: #616161;

                &:before {
                    content: "○";
                }
            }
        }
    }
`

export const SectionTitle = styled(Typography)`
    color: #1976d2;
    font-weight: 500;
    margin-bottom: 12px !important;
`

export const Note = styled.div`
    font-size: 13px;
    color: #757575;
    margin-top: 20px;
    padding: 16px;
    background: #FAFAFA;
    border-radius: 4px;
    line-height: 1.5;
    word-wrap: break-word;
    overflow-wrap: break-word;
    word-break: break-word;
    hyphens: auto;

    & strong {
        color: #424242;
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
    background-color: #FFF3E0;
    border-left: 4px solid #FB8C00;
    border-radius: 4px;
` 