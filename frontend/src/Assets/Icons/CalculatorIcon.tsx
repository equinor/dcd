import React from "react"

interface Props {
  size: number;
}

export const CalculatorIcon = ({ size }: Props) => {
    const color = "var(--eds_interactive_primary__resting, rgba(0, 112, 121, 1));"

    return (
        <svg
            id="Layer_1"
            xmlns="http://www.w3.org/2000/svg"
            viewBox="0 0 21.57 20.56"
            width={size}
            height={size}
            display="block"
        >
            <defs>
                <style>
                    {`
            .cls-1 {
              font-family: BalooBhaijaan-Regular, 'Baloo Bhaijaan';
              font-size: 1.5px;
            }
            .cls-2, .cls-3 {
              stroke: #fff;
            }
            .cls-2, .cls-3, .cls-4, .cls-5, .cls-6, .cls-7 {
              stroke-miterlimit: 10;
            }
            .cls-2, .cls-4, .cls-5, .cls-6 {
              fill: #fff;
            }
            .cls-2, .cls-6, .cls-7 {
              stroke-width: 1.5px;
            }
            .cls-8 {
              letter-spacing: .13em;
            }
            .cls-9 {
              letter-spacing: .13em;
            }
            .cls-10 {
              letter-spacing: .13em;
            }
            .cls-3 {
              fill: none;
              stroke-width: 1.1px;
            }
            .cls-4 {
              stroke-width: .8px;
            }
            .cls-4, .cls-5, .cls-6, .cls-7 {
              stroke: ${color};
            }
            .cls-5 {
              stroke-width: 1.2px;
            }
            .cls-7 {
              fill: ${color};
            }
          `}
                </style>
            </defs>
            <rect className="cls-5" x="3.01" y="1.25" width="15.15" height="18" rx=".61" ry=".61" />
            <rect className="cls-4" x="4.85" y="3" width="11.32" height="2.88" rx=".04" ry=".04" />
            <rect className="cls-7" x="5.27" y="8.23" width="10.62" height="8.7" />
            <line className="cls-3" x1="10.51" y1="7.06" x2="10.51" y2="18.11" />
            <line className="cls-3" x1="7.26" y1="7.15" x2="7.26" y2="18.2" />
            <line className="cls-3" x1="13.83" y1="6.86" x2="13.83" y2="17.92" />
            <line className="cls-3" x1="16.69" y1="11.84" x2="4.45" y2="11.84" />
            <line className="cls-3" x1="16.7" y1="14.97" x2="4.47" y2="14.97" />
            <line className="cls-3" x1="16.7" y1="8.65" x2="4.47" y2="8.65" />
            <text className="cls-1" transform="translate(11.66 4.86)">
                <tspan className="cls-8" x="0" y="0">13</tspan>
                <tspan className="cls-10" x="1.77" y="0">3</tspan>
                <tspan className="cls-9" x="2.73" y="0">7</tspan>
            </text>
        </svg>
    )
}
