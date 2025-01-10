using api.Context;

using Xunit;

namespace tests.Context;

public class ChangeLogServiceTests
{
    [Fact]
    public void VerifySimpleTypes()
    {
        Assert.False(ChangeLogService.IsSimpleType(typeof(List<string>)));
        Assert.False(ChangeLogService.IsSimpleType(typeof(List<int>)));
        Assert.False(ChangeLogService.IsSimpleType(typeof(Dictionary<int, int>)));
        Assert.False(ChangeLogService.IsSimpleType(typeof(ChangeLogServiceTests)));

        Assert.True(ChangeLogService.IsSimpleType(typeof(int?)));
        Assert.True(ChangeLogService.IsSimpleType(typeof(int)));
        Assert.True(ChangeLogService.IsSimpleType(typeof(string)));
        Assert.True(ChangeLogService.IsSimpleType(typeof(decimal)));
        Assert.True(ChangeLogService.IsSimpleType(typeof(decimal?)));
    }
}
