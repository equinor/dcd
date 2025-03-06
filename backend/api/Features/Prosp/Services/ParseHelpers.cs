using api.Models.Enums;

using DocumentFormat.OpenXml.Spreadsheet;

namespace api.Features.Prosp.Services;

public static class ParseHelpers
{
    public static double ReadDoubleValue(IEnumerable<Cell> cellData, string coordinate)
    {
        return double.TryParse(cellData.FirstOrDefault(c => c.CellReference == coordinate)?.CellValue?.InnerText, out var value)
            ? Math.Round(value, 3)
            : 0;
    }

    public static int ReadIntValue(IEnumerable<Cell> cellData, string coordinate)
    {
        return int.TryParse(cellData.FirstOrDefault(c => c.CellReference == coordinate)?.CellValue?.InnerText, out var value)
            ? value
            : 0;
    }

    public static DateTime ReadDateValue(IEnumerable<Cell> cellData, string coordinate)
    {
        return double.TryParse(cellData.FirstOrDefault(c => c.CellReference == coordinate)?.CellValue?.InnerText, out var value)
            ? DateTime.FromOADate(value)
            : new DateTime(1900, 1, 1);
    }

    public static double[] ReadDoubleValues(IEnumerable<Cell> cellData, List<string> coordinates)
    {
        var values = new List<double>();

        foreach (var cell in cellData.Where(c => c.CellReference != null && coordinates.Contains(c.CellReference!)))
        {
            if (double.TryParse(cell.CellValue?.InnerText.Replace(',', '.'), out var value))
            {
                values.Add(value);
            }
        }

        return values.ToArray();
    }

    public static Concept MapSubstructureConcept(int importValue)
    {
        return importValue switch
        {
            0 => Concept.TieBack,
            1 => Concept.Jacket,
            2 => Concept.Gbs,
            3 => Concept.Tlp,
            4 => Concept.Spar,
            5 => Concept.Semi,
            6 => Concept.CircularBarge,
            7 => Concept.Barge,
            8 => Concept.Fpso,
            9 => Concept.Tanker,
            10 => Concept.JackUp,
            11 => Concept.SubseaToShore,
            _ => Concept.NoConcept
        };
    }

    public static ArtificialLift MapArtificialLift(int importValue)
    {
        return importValue switch
        {
            0 => ArtificialLift.NoArtificialLift,
            1 => ArtificialLift.GasLift,
            2 => ArtificialLift.ElectricalSubmergedPumps,
            3 => ArtificialLift.SubseaBoosterPumps,
            _ => ArtificialLift.NoArtificialLift
        };
    }

    public static ProductionFlowline MapProductionFlowLine(int importValue)
    {
        return importValue switch
        {
            1 => ProductionFlowline.Carbon,
            2 => ProductionFlowline.SsClad,
            3 => ProductionFlowline.Cr13,
            11 => ProductionFlowline.CarbonInsulation,
            12 => ProductionFlowline.SsCladInsulation,
            13 => ProductionFlowline.Cr13Insulation,
            21 => ProductionFlowline.CarbonInsulationDeh,
            22 => ProductionFlowline.SsCladInsulationDeh,
            23 => ProductionFlowline.Cr13InsulationDeh,
            31 => ProductionFlowline.CarbonPip,
            32 => ProductionFlowline.SsCladPip,
            33 => ProductionFlowline.Cr13Pip,
            41 => ProductionFlowline.HdpeLinedCs,
            _ => ProductionFlowline.NoProductionFlowline
        };
    }
}
