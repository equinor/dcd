using System.Reflection;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using Newtonsoft.Json;

using System.Web;

using api.Dtos;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

using Xunit;

namespace tests;

public class DummyAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public DummyAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions>
        options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock
        clock) : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[] { new Claim(ClaimTypes.Name, "Test user") };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}

public class ApiShould : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private static readonly string _projectsApiPath = "/projects";
    private static readonly string _casesApiPath = "/cases";
    private static readonly string _drainageStrategiesApiPath = "/drainage-strategies";
    private static readonly string _substructuresApiPath = "/substructures";
    private static readonly string _wellProjectsApiPath = "/well-projects";
    private static readonly string _explorationsApiPath = "/explorations";
    private static readonly string _surfsApiPath = "/surfs";
    private static readonly string _topsidesApiPath = "/topsides";
    private static readonly string _transportsApiPath = "/transports";

    private HttpClient client;
    public ApiShould(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        client = getNoAuthApiClient();
    }

    private static string currentDirFromRuntime()
    {
        var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().Location);
        var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
        return Path.GetDirectoryName(codeBasePath);
    }

    public HttpClient getNoAuthApiClient()
    {
        return _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((_, configBuilder) =>
            {
                configBuilder.AddJsonFile(Path.Combine(currentDirFromRuntime(),
                            "appsettings.json"));
            });
            builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, DummyAuthHandler>(
                    "Test", options => { });
            });
        }).CreateClient();
    }

    public async Task<TResponse> doJsonRequest<TResponse, TReqBody>(
            HttpClient client, string url, string method,
            TReqBody body)
    {
        var myContent = JsonConvert.SerializeObject(body);

        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
        var byteContent = new ByteArrayContent(buffer);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        HttpResponseMessage response;
        switch (method)
        {
            case "GET":
                response = await client.GetAsync(url);
                break;
            case "POST":
                response = await client.PostAsync(url, byteContent);
                break;
            case "PUT":
                response = await client.PutAsync(url, byteContent);
                break;
            default:
                throw new Exception($"Unimplemented method {method}");
        }
        try
        {
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }
        catch (Exception e)
        {
            Console.WriteLine($"got exception: {e}");
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        Assert.Equal("application/json; charset=utf-8",
            response.Content.Headers.ContentType.ToString());
        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    public async Task<TResponse> getJson<TResponse>(
            HttpClient client, string url)
    {
        return await doJsonRequest<TResponse, Object>(client, url, "GET",
                null);
    }

    [Fact]
    public async Task GetProjects()
    {
        var client = getNoAuthApiClient();
        var responseProjects = await getJson<List<ProjectDto>>(client, _projectsApiPath);
        foreach (var responseProject in responseProjects)
        {
            Assert.Equal(responseProject.CreateDate,
                    DateTimeOffset.UtcNow.Date);
            Assert.NotNull(responseProject.DrainageStrategies);
        }
    }
    [Fact]
    public async Task CreateProjectWithCase1_BCCST_Spreadsheet()
    {
        var client = getNoAuthApiClient();

        // create PROJECT dto, send it, save the id
        var projectDto = BCCST_Example_Case2_TestDataGenerator.SpreadSheetProject();
        var responseProject = await
            doJsonRequest<ProjectDto, ProjectDto>(client, _projectsApiPath,
                    "POST", projectDto);
        var projectId = responseProject.ProjectId;

        // create CASE, send it, save id as query for asset requests
        var caseDto = BCCST_Example_Case2_TestDataGenerator.Case2Case();
        caseDto.ProjectId = projectId;
        var responseCaseProject = await doJsonRequest<ProjectDto,
            CaseDto>(client, _casesApiPath, "POST", caseDto);
        var responseCase = responseCaseProject.Cases.First(_ => true);
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["sourceCaseId"] = responseCase.Id.ToString();
        var caseIdQueryString = "?" + query.ToString();

        // Not all assets are created (user specifies which assets are required at leisure)
        // DRAINAGE STRATEGY
        var drainageStrategyDto = BCCST_Example_Case2_TestDataGenerator.Case2DrainageStrategy();
        drainageStrategyDto.ProjectId = projectId;
        var drainageStrategyUrl = _drainageStrategiesApiPath +
            caseIdQueryString;
        await doJsonRequest<ProjectDto, DrainageStrategyDto>(client,
                drainageStrategyUrl, "POST", drainageStrategyDto);
        // SUBSTRUCTURE
        var substructureDto = BCCST_Example_Case2_TestDataGenerator.Case2Substructure();
        substructureDto.ProjectId = projectId;
        var substructuresUrl = _substructuresApiPath + caseIdQueryString;
        await doJsonRequest<ProjectDto, SubstructureDto>(client,
                substructuresUrl, "POST", substructureDto);
        // WELLPROJECT
        var wellProjectDto = BCCST_Example_Case2_TestDataGenerator.Case2WellProject();
        wellProjectDto.ProjectId = projectId;
        var wellProjectsUrl = _wellProjectsApiPath + caseIdQueryString;
        await doJsonRequest<ProjectDto, WellProjectDto>(client,
                wellProjectsUrl, "POST", wellProjectDto);
        // EXPLORATION
        var explorationDto = BCCST_Example_Case2_TestDataGenerator.Case2Exploration();
        explorationDto.ProjectId = projectId;
        var explorationsUrl = _explorationsApiPath + caseIdQueryString;
        await doJsonRequest<ProjectDto, ExplorationDto>(client,
                explorationsUrl, "POST", explorationDto);

        // retrieve projectDto
        var fetchedProjectDto = await getJson<ProjectDto>(client,
                _projectsApiPath + "/" + projectId);

        // establish correctness of received fetchedProjectDto
        // basic comparisons of project data
        // set expected date on test project instance, for comparison
        // projectDto.CreateDate = DateTimeOffset.UtcNow.Date;
        TestHelper.CompareProjectsData(projectDto, fetchedProjectDto);
        // check that case on project is expected case
        var fetchedCaseDto = fetchedProjectDto.Cases.First();
        TestHelper.CompareCases(caseDto, fetchedCaseDto);
        // check that all assets on project are as expected assets
        var fetchedDrainageStrategyDto =
            fetchedProjectDto.DrainageStrategies.First();
        TestHelper.CompareDrainageStrategies(drainageStrategyDto, fetchedDrainageStrategyDto);
        var fetchedSubstructureDto = fetchedProjectDto.Substructures.First();
        TestHelper.CompareSubstructures(substructureDto, fetchedSubstructureDto);
        var fetchedWellProjectDto = fetchedProjectDto.WellProjects.First();
        TestHelper.CompareWellProjects(wellProjectDto, fetchedWellProjectDto);
        var fetchedExplorationDto = fetchedProjectDto.Explorations.First();
        TestHelper.CompareExplorations(explorationDto, fetchedExplorationDto);
        // check that assets are linked onto case
        Assert.Equal(fetchedCaseDto.DrainageStrategyLink, fetchedDrainageStrategyDto.Id);
        Assert.Equal(fetchedCaseDto.SubstructureLink, fetchedSubstructureDto.Id);
        Assert.Equal(fetchedCaseDto.WellProjectLink, fetchedWellProjectDto.Id);
        Assert.Equal(fetchedCaseDto.ExplorationLink, fetchedExplorationDto.Id);
        Assert.Equal(fetchedCaseDto.SurfLink, Guid.Empty);
        Assert.Equal(fetchedCaseDto.TopsideLink, Guid.Empty);
        Assert.Equal(fetchedCaseDto.TransportLink, Guid.Empty);
    }

    [Fact]
    public async Task CreateProjectWithAllAssetsAndTwoCasesWithSomeAssets()
    {
        var client = getNoAuthApiClient();

        // create PROJECT dto, send it, save the id
        var projectDto = TestDataGenerator_AllAssets.OneProject();
        var responseProject = await
            doJsonRequest<ProjectDto, ProjectDto>(client, _projectsApiPath,
                    "POST", projectDto);
        var projectId = responseProject.ProjectId;

        // create two CASE, send it, save id as query for asset requests
        var caseADto = TestDataGenerator_AllAssets.CaseA();
        caseADto.ProjectId = projectId;
        var responseProjectWithCaseA = await doJsonRequest<ProjectDto,
            CaseDto>(client, _casesApiPath, "POST", caseADto);
        var responseCaseA = responseProjectWithCaseA.Cases.Where(n => n.Name == caseADto.Name).First();
        //First(_ => true);
        var queryA = HttpUtility.ParseQueryString(string.Empty);
        queryA["sourceCaseId"] = responseCaseA.Id.ToString();
        var caseAIdQueryString = "?" + queryA.ToString();

        var caseBDto = TestDataGenerator_AllAssets.CaseB();
        caseBDto.ProjectId = projectId;
        var responseProjectWithCaseB = await doJsonRequest<ProjectDto,
            CaseDto>(client, _casesApiPath, "POST", caseBDto);
        var responseCaseB = responseProjectWithCaseB.Cases.Where(n => n.Name == caseBDto.Name).First();

        var queryB = HttpUtility.ParseQueryString(string.Empty);
        queryB["sourceCaseId"] = responseCaseB.Id.ToString();
        var caseBIdQueryString = "?" + queryB.ToString();

        // DRAINAGE STRATEGY ON CASE A

        responseProject = await AddDrainageStrategyToCase(TestDataGenerator_AllAssets.Case2DrainageStrategy(projectId), responseCaseA.Id);

        // SUBSTRUCTURE ON CASE A
        var substructureDto = TestDataGenerator_AllAssets.Case2Substructure();
        substructureDto.ProjectId = projectId;
        var substructuresUrl = _substructuresApiPath + caseAIdQueryString;
        await doJsonRequest<ProjectDto, SubstructureDto>(client,
                substructuresUrl, "POST", substructureDto);
        // WELLPROJECT ON CASE A
        var wellProjectDto = TestDataGenerator_AllAssets.Case2WellProject();
        wellProjectDto.ProjectId = projectId;
        var wellProjectsUrl = _wellProjectsApiPath + caseAIdQueryString;
        await doJsonRequest<ProjectDto, WellProjectDto>(client,
                wellProjectsUrl, "POST", wellProjectDto);
        // EXPLORATION ON CASE A
        responseProject = await AddExplorationToCase(TestDataGenerator_AllAssets.Case2Exploration(projectId), responseCaseA.Id);

        // SURF IN CASE B
        responseProject = await AddSurfToCase(TestDataGenerator_AllAssets.Case2Surf(projectId), responseCaseB.Id);

        // TRANSPORT IN CASE B
        responseProject = await AddTransportToCase(TestDataGenerator_AllAssets.Case2Transport(projectId), responseCaseB.Id);

        // TOPSIDE IN CASE B

        responseProject = await AddTopsideToCase(TestDataGenerator_AllAssets.Case2Topside(projectId), responseCaseB.Id);

        projectDto.CreateDate = DateTimeOffset.UtcNow.Date;
        TestHelper.CompareProjectsData(projectDto, responseProject);
        // check that both cases are present on project
        var fetchedCaseADto = responseProject.Cases.FirstOrDefault(c => c.Name == caseADto.Name);
        var fetchedCaseBDto = responseProject.Cases.FirstOrDefault(c => c.Name == caseBDto.Name);
        TestHelper.CompareCases(caseADto, fetchedCaseADto);
        TestHelper.CompareCases(caseBDto, fetchedCaseBDto);
        // check that all assets on project are as expected assets
        var fetchedDrainageStrategyDto =
            responseProject.DrainageStrategies.First();
        TestHelper.CompareDrainageStrategies(TestDataGenerator_AllAssets.Case2DrainageStrategy(projectId), fetchedDrainageStrategyDto);
        var fetchedSubstructureDto = responseProject.Substructures.First();
        TestHelper.CompareSubstructures(substructureDto, fetchedSubstructureDto);
        var fetchedWellProjectDto = responseProject.WellProjects.First();
        TestHelper.CompareWellProjects(wellProjectDto, fetchedWellProjectDto);
        var fetchedExplorationDto = responseProject.Explorations.First();
        TestHelper.CompareExplorations(TestDataGenerator_AllAssets.Case2Exploration(projectId), fetchedExplorationDto);
        var fetchedSurfDto = responseProject.Surfs.First();
        TestHelper.CompareSurfs(TestDataGenerator_AllAssets.Case2Surf(projectId), fetchedSurfDto);
        var fetchedTopsideDto = responseProject.Topsides.First();
        TestHelper.CompareTopsides(TestDataGenerator_AllAssets.Case2Topside(projectId), fetchedTopsideDto);
        var fetchedTransportDto = responseProject.Transports.First();
        TestHelper.CompareTransports(TestDataGenerator_AllAssets.Case2Transport(projectId), fetchedTransportDto);
        // check that assets are linked to case A
        Assert.Equal(fetchedCaseADto.DrainageStrategyLink, fetchedDrainageStrategyDto.Id);
        Assert.Equal(fetchedCaseADto.SubstructureLink, fetchedSubstructureDto.Id);
        Assert.Equal(fetchedCaseADto.WellProjectLink, fetchedWellProjectDto.Id);
        Assert.Equal(fetchedCaseADto.ExplorationLink, fetchedExplorationDto.Id);
        Assert.Equal(fetchedCaseADto.SurfLink, Guid.Empty);
        Assert.Equal(fetchedCaseADto.TopsideLink, Guid.Empty);
        Assert.Equal(fetchedCaseADto.TransportLink, Guid.Empty);
        // check that assets are linked to case B
        Assert.Equal(fetchedCaseBDto.DrainageStrategyLink, Guid.Empty);
        Assert.Equal(fetchedCaseBDto.SubstructureLink, Guid.Empty);
        Assert.Equal(fetchedCaseBDto.WellProjectLink, Guid.Empty);
        Assert.Equal(fetchedCaseBDto.ExplorationLink, Guid.Empty);
        Assert.Equal(fetchedCaseBDto.SurfLink, fetchedSurfDto.Id);
        Assert.Equal(fetchedCaseBDto.TopsideLink, fetchedTopsideDto.Id);
        Assert.Equal(fetchedCaseBDto.TransportLink, fetchedTransportDto.Id);
    }

    [Fact]
    public async Task UpdateDrainageStrategy()
    {

        var responseProject = await CreateProject(TestDataGenerator_AllAssets.OneProject());

        responseProject = await AddCase2Project(responseProject.ProjectId, TestDataGenerator_AllAssets.CaseA());
        var responseCaseA = responseProject.Cases.Where(n => n.Name == TestDataGenerator_AllAssets.CaseA().Name).First();

        responseProject = await AddDrainageStrategyToCase(TestDataGenerator_AllAssets.Case2DrainageStrategy(responseProject.ProjectId), responseCaseA.Id);

        var fetchedDrainageStrategyDto = responseProject.DrainageStrategies.First();

        responseProject = await UppdateDrainageStrategy(TestDataGenerator_AllAssets.UpdatedDrainageStrategy(), fetchedDrainageStrategyDto.Id);

        var fetchedUpdatedDrainageStrategyDto = responseProject.DrainageStrategies.First();
        TestHelper.CompareDrainageStrategies(TestDataGenerator_AllAssets.UpdatedDrainageStrategy(), fetchedUpdatedDrainageStrategyDto);
    }

    private async Task<ProjectDto> CreateProject(ProjectDto projectDto)
    {
        var responseProject = await
            doJsonRequest<ProjectDto, ProjectDto>(client, _projectsApiPath,
                    "POST", projectDto);

        return await doJsonRequest<ProjectDto, ProjectDto>(client, _projectsApiPath + "/" + responseProject.ProjectId, "GET",
                null);
    }

    private async Task<ProjectDto> AddCase2Project(Guid projectId, CaseDto caseDto)
    {
        caseDto.ProjectId = projectId;
        return await doJsonRequest<ProjectDto,
            CaseDto>(client, _casesApiPath, "POST", caseDto);
    }

    private async Task<ProjectDto> AddDrainageStrategyToCase(DrainageStrategyDto drainageStrategyDto, Guid caseID)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["sourceCaseId"] = caseID.ToString();
        var caseIdQueryString = "?" + query.ToString();
        var drainageStrategyUrl = _drainageStrategiesApiPath + caseIdQueryString;
        return await doJsonRequest<ProjectDto, DrainageStrategyDto>(client,
                drainageStrategyUrl, "POST", drainageStrategyDto);
    }

    private async Task<ProjectDto> AddSurfToCase(SurfDto surfDto, Guid caseID)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["sourceCaseId"] = caseID.ToString();
        var caseIdQueryString = "?" + query.ToString();
        var surfUrl = _surfsApiPath + caseIdQueryString;
        return await doJsonRequest<ProjectDto, SurfDto>(client,
                surfUrl, "POST", surfDto);
    }

    private async Task<ProjectDto> AddTransportToCase(TransportDto transportDto, Guid caseID)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["sourceCaseId"] = caseID.ToString();
        var caseIdQueryString = "?" + query.ToString();
        var surfUrl = _transportsApiPath + caseIdQueryString;
        return await doJsonRequest<ProjectDto, TransportDto>(client,
                surfUrl, "POST", transportDto);
    }

    private async Task<ProjectDto> AddExplorationToCase(ExplorationDto explorationDto, Guid caseID)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["sourceCaseId"] = caseID.ToString();
        var caseIdQueryString = "?" + query.ToString();
        var surfUrl = _explorationsApiPath + caseIdQueryString;
        return await doJsonRequest<ProjectDto, ExplorationDto>(client,
                surfUrl, "POST", explorationDto);
    }

    private async Task<ProjectDto> AddWellProjectToCase(WellProjectDto wellProjectDto, Guid caseID)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["sourceCaseId"] = caseID.ToString();
        var caseIdQueryString = "?" + query.ToString();
        var surfUrl = _wellProjectsApiPath + caseIdQueryString;
        return await doJsonRequest<ProjectDto, WellProjectDto>(client,
                surfUrl, "POST", wellProjectDto);
    }

    private async Task<ProjectDto> AddTopsideToCase(TopsideDto topsideDto, Guid caseID)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["sourceCaseId"] = caseID.ToString();
        var caseIdQueryString = "?" + query.ToString();
        var surfUrl = _topsidesApiPath + caseIdQueryString;
        return await doJsonRequest<ProjectDto, TopsideDto>(client,
                surfUrl, "POST", topsideDto);
    }

    private async Task<ProjectDto> AddSubstructureToCase(SubstructureDto substructureDto, Guid caseID)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["sourceCaseId"] = caseID.ToString();
        var caseIdQueryString = "?" + query.ToString();
        var surfUrl = _substructuresApiPath + caseIdQueryString;
        return await doJsonRequest<ProjectDto, SubstructureDto>(client,
                surfUrl, "POST", substructureDto);
    }
    private async Task<ProjectDto> UppdateDrainageStrategy(DrainageStrategyDto updatedDrainageStrategyDto, Guid DrainageStrategyId)
    {
        var updatedDrainageStrategyUrl = _drainageStrategiesApiPath + "/" + DrainageStrategyId;
        return await doJsonRequest<ProjectDto, DrainageStrategyDto>(client,
               updatedDrainageStrategyUrl, "PUT", updatedDrainageStrategyDto);
    }
}
