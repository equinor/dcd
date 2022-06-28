using api.Models;

namespace api.SampleData.Builders;

public class WellTypeBuilder : WellType
{
    public WellTypeBuilder()
    {

    }

    public WellTypeBuilder WithWellType(WellTypeBuilder d)
    {
        return this;
    }
}
