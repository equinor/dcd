import React from "react"
import styled from "styled-components"

const LogoWrapper = styled.div`
    margin-top: 100px;
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 5px;
`;

const D = styled.div`
    width: 30px;
    height: 40px;
    border-width: 7px;
    border-style: solid;
    border-color: #FF1043 #DB002E #990025 #DB002E; 
    border-top-right-radius: 4px;
    border-bottom-right-radius: 4px;
`;

const C = styled.div`
    width: 28px;
    height: 40px;
    border-width: 7px;
    border-style: solid;
    border-color: #FF1043 transparent #990025 #DB002E; 
    border-top-left-radius: 4px;
    border-bottom-left-radius: 4px;
`;

const Logo: React.FC = () => {
    return (
        <LogoWrapper>
            <D />
            <C />
            <D />
        </LogoWrapper>
    );
};

export default Logo; 