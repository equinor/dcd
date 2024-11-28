using api.Models;

namespace api.Features.BackgroundServices.ProjectMaster.Services.EnumConverters;

public static class ProjectCategoryEnumConverter
{
    public static ProjectCategory? ConvertCategory(string category)
    {
        return category switch
        {
            "" => ProjectCategory.Null,
            "BROWNFIELD" => ProjectCategory.Brownfield,
            "CESSATION" => ProjectCategory.Cessation,
            "DRILLING_UPGRADE" => ProjectCategory.DrillingUpgrade,
            "ONSHORE" => ProjectCategory.Onshore,
            "PIPELINE" => ProjectCategory.Pipeline,
            "PLATFORM_FPSO" => ProjectCategory.PlatformFpso,
            "SUBSEA" => ProjectCategory.Subsea,
            "SOLAR" => ProjectCategory.Solar,
            "CO2 STORAGE" => ProjectCategory.Co2Storage,
            "EFUEL" => ProjectCategory.Efuel,
            "NUCLEAR" => ProjectCategory.Nuclear,
            "CO2 CAPTURE" => ProjectCategory.Co2Capture,
            "FPSO" => ProjectCategory.Fpso,
            "HYDROGEN" => ProjectCategory.Hydrogen,
            "HSE" => ProjectCategory.Hse,
            "OFFSHORE_WIND" => ProjectCategory.OffshoreWind,
            "PLATFORM" => ProjectCategory.Platform,
            "POWER_FROM_SHORE" => ProjectCategory.PowerFromShore,
            "TIE-IN" => ProjectCategory.TieIn,
            "RENEWABLE_OTHER" => ProjectCategory.RenewableOther,
            "CCS" => ProjectCategory.Ccs,
            _ => null
        };
    }
}
