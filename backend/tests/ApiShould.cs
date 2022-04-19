// using System.Reflection;
// using System.Security.Claims;
// using System.Text.Encodings.Web;
// using System.Text.Json;
// using System.Web;

// using api.Adapters;
// using api.Dtos;
// using api.Models;
// using api.SampleData.Generators;

// using Microsoft.AspNetCore.Authentication;
// using Microsoft.AspNetCore.Mvc.Testing;
// using Microsoft.AspNetCore.TestHost;
// using Microsoft.Extensions.Options;

// using Xunit;

// namespace tests;

// public class DummyAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
// {
//     public DummyAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions>
//         options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock
//         clock) : base(options, logger, encoder, clock)
//     {
//     }

//     protected override Task<AuthenticateResult> HandleAuthenticateAsync()
//     {
//         var claims = new[] { new Claim(ClaimTypes.Name, "Test user") };
//         var identity = new ClaimsIdentity(claims, "Test");
//         var principal = new ClaimsPrincipal(identity);
//         var ticket = new AuthenticationTicket(principal, "Test");

//         var result = AuthenticateResult.Success(ticket);

//         return Task.FromResult(result);
//     }
// }

// public class ApiShould : IClassFixture<WebApplicationFactory<Program>>
// {
//     private readonly WebApplicationFactory<Program> _factory;
//     private static readonly string _projectsApiPath = "/projects";
//     private static readonly string _casesApiPath = "/cases";
//     private static readonly string _drainageStrategiesApiPath = "/drainage-strategies";
//     private static readonly string _substructuresApiPath = "/substructures";
//     private static readonly string _wellProjectsApiPath = "/well-projects";
//     private static readonly string _explorationsApiPath = "/explorations";

//     public ApiShould(WebApplicationFactory<Program> factory)
//     {
//         _factory = factory;
//     }

//     private string currentDirFromRuntime()
//     {
//         var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().Location);
//         var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
//         return Path.GetDirectoryName(codeBasePath);
//     }

//     private HttpClient getNoAuthApiClient()
//     {
//         return _factory.WithWebHostBuilder(builder =>
//         {
//             builder.ConfigureAppConfiguration((_, configBuilder) =>
//             {
//                 configBuilder.AddJsonFile(Path.Combine(currentDirFromRuntime(),
//                             "appsettings.json"));
//             });
//             builder.ConfigureTestServices(services =>
//             {
//                 services.AddAuthentication("Test")
//                 .AddScheme<AuthenticationSchemeOptions, DummyAuthHandler>(
//                     "Test", options => { });
//             });
//         }).CreateClient();
//     }

//     private async Task<TResponse> doJsonRequest<TResponse, TReqBody>(
//             HttpClient client, string url, string method,
//             TReqBody body)
//     {
//         HttpResponseMessage response;
//         switch (method)
//         {
//             case "GET":
//                 response = await client.GetAsync(url);
//                 break;
//             case "POST":
//                 response = await client.PostAsJsonAsync<TReqBody>(url, body);
//                 break;
//             default:
//                 throw new Exception($"Unimplemented method {method}");
//         }
//         try
//         {
//             response.EnsureSuccessStatusCode(); // Status Code 200-299
//         }
//         catch (Exception e)
//         {
//             Console.WriteLine($"got exception: {e}");
//             Console.WriteLine(await response.Content.ReadAsStringAsync());
//         }
//         Assert.Equal("application/json; charset=utf-8",
//             response.Content.Headers.ContentType.ToString());
//         return await response.Content.ReadFromJsonAsync<TResponse>();
//     }

//     private async Task<TResponse> getJson<TResponse>(
//             HttpClient client, string url)
//     {
//         return await doJsonRequest<TResponse, Object>(client, url, "GET",
//                 null);
//     }

//     [Fact]
//     public async Task GetProjects()
//     {
//         var client = getNoAuthApiClient();
//         var responseProjects = await getJson<List<ProjectDto>>(client, _projectsApiPath);
//         foreach (var responseProject in responseProjects)
//         {
//             Assert.Equal(responseProject.CreateDate,
//                     DateTimeOffset.UtcNow.Date);
//             Assert.NotNull(responseProject.DrainageStrategies);
//         }
//     }

//     [Fact]
//     public async Task CreateProjectWithCaseAndAssetsAndRetrieveAllData()
//     {
//         var client = getNoAuthApiClient();

//         // create PROJECT dto, send it, save the id
//         var projectDto = TestDataGenerator.SpreadSheetProject();
//         var responseProject = await
//             doJsonRequest<ProjectDto, ProjectDto>(client, _projectsApiPath,
//                     "POST", projectDto);
//         var projectId = responseProject.ProjectId;

//         // create CASE, send it, save id as query for asset requests
//         var caseDto = TestDataGenerator.Case2Case();
//         caseDto.ProjectId = projectId;
//         var responseCaseProject = await doJsonRequest<ProjectDto,
//             CaseDto>(client, _casesApiPath, "POST", caseDto);
//         var responseCase = responseCaseProject.Cases.First(_ => true);
//         var query = HttpUtility.ParseQueryString(string.Empty);
//         query["sourceCaseId"] = responseCase.Id.ToString();
//         var caseIdQueryString = "?" + query.ToString();

//         // create dtos for other ASSETS, add project id, add case id as query,
//         // submit
//         // DRAINAGE STRATEGY
//         var drainageStrategyDto = TestDataGenerator.Case2DrainageStrategy();
//         drainageStrategyDto.ProjectId = projectId;
//         var drainageStrategyUrl = _drainageStrategiesApiPath +
//             caseIdQueryString;
//         await doJsonRequest<ProjectDto, DrainageStrategyDto>(client,
//                 drainageStrategyUrl, "POST", drainageStrategyDto);
//         // SUBSTRUCTURE
//         var substructureDto = TestDataGenerator.Case2Substructure();
//         substructureDto.ProjectId = projectId;
//         var substructuresUrl = _substructuresApiPath + caseIdQueryString;
//         await doJsonRequest<ProjectDto, SubstructureDto>(client,
//                 substructuresUrl, "POST", substructureDto);
//         // WELLPROJECT
//         var wellProjectDto = TestDataGenerator.Case2WellProject();
//         wellProjectDto.ProjectId = projectId;
//         var wellProjectsUrl = _wellProjectsApiPath + caseIdQueryString;
//         await doJsonRequest<ProjectDto, WellProjectDto>(client,
//                 wellProjectsUrl, "POST", wellProjectDto);
//         // EXPLORATION
//         var explorationDto = TestDataGenerator.Case2Exploration();
//         explorationDto.ProjectId = projectId;
//         var explorationsUrl = _explorationsApiPath + caseIdQueryString;
//         await doJsonRequest<ProjectDto, ExplorationDto>(client,
//                 explorationsUrl, "POST", explorationDto);

//         // retrieve projectDto
//         var fetchedProjectDto = await getJson<ProjectDto>(client,
//                 _projectsApiPath + "/" + projectId);

//         // establish correctness of received fetchedProjectDto  
//         // basic comparisons of project data
//         // set expected date on test project instance, for comparison
//         projectDto.CreateDate = DateTimeOffset.UtcNow.Date;
//         TestHelper.CompareProjectsData(projectDto, fetchedProjectDto);
//         // check that case on project is expected case
//         var fetchedCaseDto = fetchedProjectDto.Cases.First();
//         TestHelper.CompareCases(caseDto, fetchedCaseDto);
//         // check that all assets on project are as expected assets
//         var fetchedDrainageStrategyDto =
//             fetchedProjectDto.DrainageStrategies.First();
//         TestHelper.CompareDrainageStrategies(drainageStrategyDto, fetchedDrainageStrategyDto);
//         var fetchedSubstructureDto = fetchedProjectDto.Substructures.First();
//         TestHelper.CompareSubstructures(substructureDto, fetchedSubstructureDto);
//         var fetchedWellProjectDto = fetchedProjectDto.WellProjects.First();
//         TestHelper.CompareWellProjects(wellProjectDto, fetchedWellProjectDto);
//         var fetchedExplorationDto = fetchedProjectDto.Explorations.First();
//         TestHelper.CompareExplorations(explorationDto, fetchedExplorationDto);
//         // check that assets are linked onto case
//         Assert.Equal(fetchedCaseDto.DrainageStrategyLink, fetchedDrainageStrategyDto.Id);
//         Assert.Equal(fetchedCaseDto.SubstructureLink, fetchedSubstructureDto.Id);
//         Assert.Equal(fetchedCaseDto.WellProjectLink, fetchedWellProjectDto.Id);
//         Assert.Equal(fetchedCaseDto.ExplorationLink, fetchedExplorationDto.Id);
//     }
// }
