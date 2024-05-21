/**
 * Sets timeseries profile name and unit in same cell.
 */
const profileAndUnitInSameCell = (params: any, rowData: any) => {
    const rowUnits = rowData.map((data: any) => data.unit)
    const checkAllUnitsAreSame = rowUnits.every((unit: any) => unit === rowUnits[0])
    const totalUnit = checkAllUnitsAreSame && !params.data?.group ? rowUnits[0] : ""

    return (
        <div style={{ lineHeight: "100%", marginTop: "7px" }}>
            {params.value}
            <br />
            <span style={{
                fontWeight: "normal", fontSize: "10px", marginTop: "-4px", color: "#6F6F6F",
            }}
            >
                {params.data?.unit ? params.data.unit : totalUnit}
            </span>
        </div>
    )
}

export default profileAndUnitInSameCell
