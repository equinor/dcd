import React from "react"
import styled from "styled-components"

interface ResponsiveGridProps {
    mobileColumns: number;
    desktopColumns: number;
    breakPoint: number;
    children: React.ReactNode;
}

const GridContainer = styled.div<ResponsiveGridProps>`
    display: grid;
    gap: 10px;
    margin: 20px 0;

    grid-template-columns: repeat(${(props) => props.mobileColumns}, 1fr);

    @media (min-width: ${(props) => props.breakPoint}px) {
        grid-template-columns: repeat(${(props) => props.desktopColumns}, 1fr);
    }
`

const ResponsiveGrid: React.FC<ResponsiveGridProps> = ({
    mobileColumns,
    desktopColumns,
    breakPoint,
    children,
}) => (
    <GridContainer
        mobileColumns={mobileColumns}
        desktopColumns={desktopColumns}
        breakPoint={breakPoint}
    >
        {children}
    </GridContainer>
)

export default ResponsiveGrid
