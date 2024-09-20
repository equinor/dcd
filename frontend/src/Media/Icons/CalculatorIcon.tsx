import React from 'react';

interface Props {
  size: number;
}

export const CalculatorIcon = ({ size }: Props) => {
  return (
    <svg
      id="Layer_1"
      xmlns="http://www.w3.org/2000/svg"
      viewBox="0 0 16.35 19.2"
      width={size}
      height={size}
    >
      <defs>
        <style>
          {`
            .cls-1 {
              font-family: BalooBhaijaan-Regular, 'Baloo Bhaijaan';
              font-size: 1.5px;
            }
            .cls-2 {
              letter-spacing: .13em;
            }
            .cls-3 {
              letter-spacing: .13em;
            }
            .cls-4 {
              letter-spacing: .13em;
            }
            .cls-5 {
              fill: none;
              stroke: #fff;
              stroke-width: 1.1px;
            }
            .cls-5, .cls-6, .cls-7, .cls-8 {
              stroke-miterlimit: 10;
            }
            .cls-6 {
              stroke-width: .8px;
            }
            .cls-6, .cls-7 {
              fill: #fff;
            }
            .cls-6, .cls-7, .cls-8 {
              stroke: #000;
            }
            .cls-7 {
              stroke-width: 1.2px;
            }
            .cls-8 {
              fill: #020000;
              stroke-width: 1.5px;
            }
          `}
        </style>
      </defs>
      <rect className="cls-7" x=".6" y=".6" width="15.15" height="18" rx=".61" ry=".61" />
      <rect className="cls-6" x="2.44" y="2.35" width="11.32" height="2.88" rx=".04" ry=".04" />
      <rect className="cls-8" x="2.86" y="7.58" width="10.62" height="8.7" />
      <line className="cls-5" x1="8.1" y1="6.41" x2="8.1" y2="17.46" />
      <line className="cls-5" x1="4.85" y1="6.5" x2="4.85" y2="17.56" />
      <line className="cls-5" x1="11.42" y1="6.21" x2="11.42" y2="17.27" />
      <line className="cls-5" x1="14.28" y1="11.19" x2="2.05" y2="11.19" />
      <line className="cls-5" x1="14.29" y1="14.32" x2="2.06" y2="14.32" />
      <line className="cls-5" x1="14.29" y1="8" x2="2.06" y2="8" />
      <text className="cls-1" transform="translate(9.25 4.21)">
        <tspan className="cls-2" x="0" y="0">13</tspan>
        <tspan className="cls-4" x="1.77" y="0">3</tspan>
        <tspan className="cls-3" x="2.73" y="0">7</tspan>
      </text>
    </svg>
  );
};