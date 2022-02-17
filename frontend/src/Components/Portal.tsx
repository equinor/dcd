import { FunctionComponent } from 'react';
import ReactDom from 'react-dom';

const portalElement = document.getElementById('portal')!

type Props = {}

export const Portal: FunctionComponent<Props> = ({ children }) => {
    return ReactDom.createPortal(children, portalElement);
}
