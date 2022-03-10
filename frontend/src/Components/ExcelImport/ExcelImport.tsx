import React, { useState } from "react"
import styled from "styled-components"
import { Button, Dialog, Typography } from "@equinor/eds-core-react"

import { useTranslation } from "react-i18next"

import { tsvToJson } from "./helpers"

const StyledDialog = styled(Dialog)`
    width: 50rem;
`

const TextArea = styled.textarea`
    flex-grow: 1;
    resize: none;
    white-space: pre;
    overflow: scroll;
    tab-size: 20;
`

const Bold = styled.em`
    font-style: normal;
    font-weight: bold;
`

const Label = styled.label`
    margin-top: 2rem;
`

const ImportButton = styled(Button)`
    margin-right: 0.5rem;
`

const Main = styled.div`
    display: flex;
    flex-direction: column;
    padding: 1rem;
`

interface Props {
    onClose: () => void
    onImport: (obj: { [key: string]: string }[]) => void
}

const ExcelImport = ({ onClose, onImport }: Props) => {
    const { t } = useTranslation()
    const [dataInput, setDataInput] = useState<string>("")

    const example = "E.g\n"
        + "forecastYear\tforecastMonth\toilProduction\trichGasProduction\n"
        + "2019\t1\t1000000\t1200000\n"
        + "2019\t2\t1000000\t1200000\n"
        + "2019\t3\t1000000\t1200000\n"
        + "2019\t4\t1000000\t1200000\n"
        + "2019\t5\t1000000\t1200000"

    const onChangeInput = (event: any) => {
        setDataInput(event.currentTarget.value)
    }

    const onClickImport = () => {
        if (!dataInput) {
            return
        }
        const inputAsJson = tsvToJson(dataInput)
        onImport(inputAsJson)
    }

    return (
        <StyledDialog>
            <Dialog.Title>{t("ExcelImport.ImportDataFromExcel")}</Dialog.Title>
            <Main>
                <Typography>
                    {t("ExcelImport.ToPasteValuesCorrectly")}
                    {" "}
                    <Bold>{t("ExcelImport.ColumnTitles")}</Bold>
                    {" "}
                    {t("ExcelImport.ArePartOfPaste")}
                </Typography>
                <Label>{t("ExcelImport.PasteInformationHere")}</Label>
                <TextArea
                    cols={30}
                    rows={10}
                    placeholder={example}
                    value={dataInput}
                    onChange={onChangeInput}
                />
            </Main>
            <Dialog.Actions>
                <ImportButton onClick={onClickImport}>{t("ExcelImport.Import")}</ImportButton>
                <Button variant="outlined" onClick={onClose}>
                    {t("ExcelImport.Cancel")}
                </Button>
            </Dialog.Actions>
        </StyledDialog>
    )
}

export default ExcelImport
