import { tokens } from "@equinor/eds-tokens"
import React, {
    useState, useEffect, useRef, RefObject,
} from "react"
import { Link } from "react-router-dom"
import styled from "styled-components"

import CreateCase from "./Articles/CreateCase"
import MakeEdits from "./Articles/MakeEdits"
import Article from "./Components/Article"
import Header from "./Components/Header"

const Wrapper = styled.div`
    display: flex;
    align-items: flex-start;
    justify-content: center;
    gap: 40px;
    padding: 40px;
    background-color: white;

    & video {
        max-width: 70%;
    }
`

const ArticleView = styled.div`
  flex: 1;
  margin-top: 20px;
  max-width: 1000px;

    & li {
        margin-bottom: 10px;
    }

`

const MainHeader = styled.div`
  font-family: Equinor;
  font-weight: 400;
  font-size: 32px;
  margin-bottom: 30px;
`
const Button = styled.button`
    font-family: Equinor;
    transition: all 0.3s ease-in-out;
    border: none;
    border-left: 4px solid #e6e6e6;
    background-color: transparent;
    display: flex;
    padding: 10px 20px;
    padding-left: 20px;
    width: 100%;
    font-size: 16px;
    text-align: left;

    &:hover {
        border-left: 4px solid ${tokens.colors.interactive.primary__resting.rgba};
    }

    &:focus {
        outline: none;
    }

    &.active {
        border-left-color: ${tokens.colors.interactive.primary__resting.rgba};
        padding-left: 26px;
    }

    &.header {
        font-size: 25px;
    }
`

const NavigationBar = styled.div`
    position: sticky;
    margin-top: 20px;
    top: 140px;
    width: 300px;

    & > a {
        text-decoration: none;
    }
`

interface ArticleData {
    title: string;
    component: React.FC;
    type?: "header";
}

const placeholder = (title: string) => {
    const PlaceholderComponent: React.FC = () => (
        <Article>
            <Article.Header>{title}</Article.Header>
            <Article.Body>
                <p>Placeholder</p>
            </Article.Body>
        </Article>
    )

    return PlaceholderComponent
}

const articles: ArticleData[] = [
    { type: "header", title: "Cases", component: () => <Header>Cases</Header> },
    { title: "How to create a case", component: CreateCase },
    { title: "How to edit data", component: MakeEdits },
    { title: "How to change the case information/ case page", component: placeholder("How to change the case information/ case page") },
    { title: "How to enter the production profile", component: placeholder("How to enter the production profile") },
    { title: "How to upload Prosp and how it fills out the facilities tab", component: placeholder("How to upload Prosp and how it fills out the facilities tab") },
    { title: "How to add well cost", component: placeholder("How to add well cost") },
    { title: "How to add wells in the well schedule", component: placeholder("How to add wells in the well schedule") },
    { title: "How to give finance and control the numbers – STEA export", component: placeholder("How to give finance and control the numbers – STEA export") },
    { type: "header", title: "Project", component: () => <Header>Project</Header> },
    { title: "How to make a revision", component: placeholder("How to make a revision") },
    { type: "header", title: "Utilities?", component: () => <Header>Utilities?</Header> },
]

const UserGuideView: React.FC = () => {
    const [activeArticleIndex, setActiveArticleIndex] = useState<number>(0)
    const articleRefs = useRef<RefObject<HTMLDivElement>[]>(articles.map(() => React.createRef()))

    const scrollToArticle = (ref: RefObject<HTMLDivElement>, index: number) => {
        setActiveArticleIndex(index)
        ref.current?.scrollIntoView({ behavior: "smooth", block: "start" })
    }

    const handleScroll = () => {
        const newActiveIndex = articleRefs.current.findIndex((ref) => {
            const rect = ref.current?.getBoundingClientRect()

            return rect ? rect.top >= 0 && rect.top < window.innerHeight / 2 : false
        })

        if (newActiveIndex !== -1) {
            setActiveArticleIndex(newActiveIndex)
        }
    }

    useEffect(() => {
        const hash = window.location.hash.substring(1)
        const decodedHash = decodeURIComponent(hash)
        const articleIndex = articles.findIndex((a) => a.title === decodedHash)

        if (articleIndex !== -1 && articleRefs.current[articleIndex]?.current) {
            scrollToArticle(articleRefs.current[articleIndex], articleIndex)
        }

        window.addEventListener("scroll", handleScroll)

        return () => {
            window.removeEventListener("scroll", handleScroll)
        }
    }, [])

    return (
        <Wrapper>
            <NavigationBar>
                <MainHeader>User Guide</MainHeader>
                {articles.map((article, index) => (
                    <Link to={`/guide#${article.title}`} key={article.title}>
                        <Button
                            className={`${index === activeArticleIndex ? "active" : ""} ${article.type === "header" ? "header" : ""}`}
                            onClick={() => scrollToArticle(articleRefs.current[index], index)}
                        >
                            {article.title}
                        </Button>
                    </Link>
                ))}
            </NavigationBar>
            <ArticleView>
                {articles.map((article, index) => (
                    <div key={article.title} ref={articleRefs.current[index]}>
                        <article.component />
                    </div>
                ))}
            </ArticleView>
        </Wrapper>
    )
}

export default UserGuideView
