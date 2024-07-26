import styled from "styled-components"
import { v4 as uuidv4 } from "uuid"
import { useAppContext } from "../../../Context/AppContext"

const Wrapper = styled.div`
    position: absolute;
    top: 0;
    right: 0;
    background: pink;
    width: 100%;
`

const Entry = styled.div`
    padding: 10px;
    border-bottom: 1px solid black;
`

export const QueueTracker = () => {
    const { apiQueue } = useAppContext()
    return (
        <Wrapper>
            {apiQueue.map((item, index) => (
                <Entry key={uuidv4()}>
                    previous value:
                    {" "}
                    {item.previousDisplayValue}
                    {"   "}
                    ---------
                    {"   "}
                    new value:
                    {" "}
                    {item.newDisplayValue}
                </Entry>
            ))}
        </Wrapper>
    )
}
