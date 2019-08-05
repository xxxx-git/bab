using System.Net.Http;
using bab;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using System.Collections.Generic;

public class WebApi_IntegrationTest : IClassFixture<WebApplicationFactory<Startup>>
{
  private readonly HttpClient _client;
  private readonly string FAKE_PAYLOD_AS_STR = "{\"Key\":\"Value\"}";
  private readonly object FAKE_PAYLOD_AS_OBJ = new { Key = "Value"};
  private readonly string GENERATE_URL = "/api/token/generate";
  private readonly string VERIFY_URL = "/api/token/verify";
  private readonly string COOKIE_NAME = "Authorization";

  public WebApi_IntegrationTest(WebApplicationFactory<Startup> factory)
  {
    
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task GenerateToken_PostContent_ReturensTokenAndSetCookie()
  {
    //Act
    HttpResponseMessage httpResponse = await _client.PostAsJsonAsync(GENERATE_URL, FAKE_PAYLOD_AS_OBJ);

    //Assert
    //check success response
    httpResponse.EnsureSuccessStatusCode();//the method will throw exaption if not

    //check token returned
    string stringResponse = await httpResponse.Content.ReadAsStringAsync();
    Assert.NotNull(stringResponse);
    Assert.NotEqual("", stringResponse);

    //check cookie was set
    foreach (var header in  httpResponse.Headers.GetValues("Set-Cookie"))
    {
      if (header.Trim().StartsWith(COOKIE_NAME))
      {
        Assert.True(true);
        return;
      }         
    }
    Assert.True(false);
  }

  [Fact]
  public async Task VerifyToken_SendTokenAsCookie_ReturensContent()
  {
    //Arrange
    HttpResponseMessage res = await _client.PostAsJsonAsync(GENERATE_URL, FAKE_PAYLOD_AS_OBJ);
    string token = await res.Content.ReadAsStringAsync();
    _client.DefaultRequestHeaders.Add("Set-Cookie",new List<string>{token});

    //Act
    HttpResponseMessage httpResponse = await _client.GetAsync(VERIFY_URL);

    //Assert
    httpResponse.EnsureSuccessStatusCode();
    string acttualContent = await httpResponse.Content.ReadAsStringAsync();
    Assert.Equal(FAKE_PAYLOD_AS_STR, acttualContent);
  }
}