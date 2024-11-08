// import styled from "styled-components"
import { CircularProgress, Menu } from "@equinor/eds-core-react"
import styled from "styled-components"

import { PersonDetails } from "@/Models/AccessManagement"
import Person from "./Person"

interface SearchResultsProps {
    isMenuOpen: boolean
    menuAnchorEl: HTMLElement | null
    isSearching: boolean
    searchResults: PersonDetails[],
    isPersonSelected: (azureId: string) => boolean
    handlePersonSelected: (person: PersonDetails) => void
}

const SearchResultsContainer = styled.div<{ $isSearching: boolean }>`
    display: flex;
    flex-direction: column;
    gap: 10px;
    justify-content: ${({ $isSearching }) => ($isSearching ? "center" : "space-between")};
    padding: 20px;
    max-height: 500px;
    min-height: 400px;
    min-width: 400px;
    width: 100%;
    overflow-y: auto;
`

const LoadingContainer = styled.div`
    display: flex;
    height: 100%;
    justify-content: center;
    align-items: center;
`

const SearchResultsMenu = ({
    isMenuOpen,
    menuAnchorEl,
    isSearching,
    searchResults,
    isPersonSelected,
    handlePersonSelected,
}: SearchResultsProps) => {
    console.log()
    return (
        <div>
            <Menu
                id="menu-complex"
                open={isMenuOpen}
                anchorEl={menuAnchorEl}
                placement="bottom-start"
            >
                <SearchResultsContainer $isSearching={isSearching}>
                    {(isSearching)
                        && (
                            <LoadingContainer>
                                <CircularProgress />
                            </LoadingContainer>
                        )}
                    {!isSearching && !!searchResults
                        && searchResults.filter((p) => p.azureId !== null).map((p) => (
                            <Person key={p.azureId} person={p} isPersonSelected={isPersonSelected} handlePersonSelected={handlePersonSelected} />
                        ))}
                </SearchResultsContainer>
            </Menu>
        </div>
    )
}

export default SearchResultsMenu
