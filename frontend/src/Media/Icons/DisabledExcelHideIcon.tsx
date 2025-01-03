import React from "react"

interface Props {
  size: number
}
export const DisabledExcelHideIcon = ({ size }: Props) => {
    const color = "#adada5"
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
            stroke: ${color};
          }
          .cls-1, .cls-2 {
            fill: ${color};
            stroke-miterlimit: 10;
            stroke-width: 1.5px;
          }
          .cls-2 {
            stroke: ${color};
          }
        `}
                </style>
            </defs>
            <path fill={color} d="M13.01,1.25L1.01,3.25v14l12,2v-2h7c.55,0,1-.45,1-1V4.25c0-.55-.45-1-1-1h-7V1.25ZM11.01,3.61v13.28l-8-1.33V4.94l8-1.33ZM13.01,5.25h2v2h-2v-2ZM17.01,5.25h2v2h-2v-2ZM4.18,6.54l1.88,3.7-2.06,3.71h1.74l1.12-2.4c.08-.23.13-.4.15-.51h.02c.04.24.09.41.13.49l1.11,2.41h1.73l-1.99-3.73,1.94-3.67h-1.62l-1.03,2.2c-.1.28-.17.51-.2.65h-.03c-.05-.21-.11-.42-.19-.63l-.92-2.21h-1.78ZM13.01,9.25h2v2h-2v-2ZM17.01,9.25h2v2h-2v-2ZM13.01,13.25h2v2h-2v-2ZM17.01,13.25h2v2h-2v-2Z" />
            <line className="cls-1" x1="1.42" y1=".55" x2="21.06" y2="18.95" />
            <line className="cls-2" x1=".51" y1="1.61" x2="20.16" y2="20.01" />
        </svg>
    )
}
