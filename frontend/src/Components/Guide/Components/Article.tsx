import React, { ReactNode } from "react"
import styled from "styled-components"
import { Typography } from "@equinor/eds-core-react"

const StyledArticle = styled.div`
    padding: 30px 0;
    line-height: 1.7;
    letter-spacing: 0.4px;
    border-bottom: 1px solid #e6e6e6;
`

const Header = styled(Typography).attrs({ variant: "h2" })`
    font-size: 22px;
    padding-bottom: 15px;
`

const Spacer = styled.div`
    padding-bottom: 20px;
`

const SubHeader = styled(Typography).attrs({ variant: "h3" })`
    font-size: 18px;
    padding-bottom: 15px;
`

const Body = styled.div`
    & p {
        margin-bottom: 20px;
    }

    & table {
        width: 100%;

        & span {
        color: red;
        font-weight: bold;
    }
    }

    & a {
        color: #007079;
        text-decoration: underline;
    }    

`

const Gallery = styled.div`
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    gap: 10px;
    margin: 40px 0;
    & > img {
        width: 100%;
        height: auto;
        object-fit: cover;
    }

    & > img:only-child {
        grid-column: span 2;
    }
`

const Section = styled.div`
    margin-bottom: 35px;
    width: 100%;
`

const Caption = styled.div`
    text-align: center;
    font-style: italic;
    margin-top: 10px;
    margin-bottom: 30px;
`

interface ArticleProps {
    children: ReactNode;
}

const Article: React.FC<ArticleProps> & {
    Header: typeof Header;
    Body: typeof Body;
    Gallery: typeof Gallery;
    Caption: typeof Caption;
    SubHeader: typeof SubHeader;
    Section: typeof Section;
    Spacer: typeof Spacer;
} = ({ children }) => <StyledArticle>{children}</StyledArticle>

Article.Header = Header
Article.Body = Body
Article.Gallery = Gallery
Article.Caption = Caption
Article.SubHeader = SubHeader
Article.Section = Section
Article.Spacer = Spacer

export default Article
