import React from "react"
import styled from "styled-components"

const Container = styled.div`
    display: flex;
    flex-direction: row;
    justify-content: flex-end;
    margin: 20px 0;
`

const Child = styled.div`
    flex: 1;
`

const PushToRight = styled.div`
    width: 400px;
    display: flex;
    flex-direction: row;
    align-items: end;
     gap: 10px;
`

interface FilterContainerProps {
    children: React.ReactNode
}

const FilterContainer: React.FC<FilterContainerProps> = ({ children }) => (
    <Container>
        <PushToRight>
            {React.Children.map(children, (child) => (
                <Child>{child}</Child>
            ))}
        </PushToRight>
    </Container>
)

export default FilterContainer
