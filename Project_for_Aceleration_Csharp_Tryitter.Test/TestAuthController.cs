
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Project_for_Aceleration_Csharp_Tryitter.DTO;
using Project_for_Aceleration_Csharp_Tryitter.Models;
using Project_for_Aceleration_Csharp_Tryitter.Utils;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Project_for_Aceleration_Csharp_Tryitter.Test;

public class TestAuthController : IClassFixture<TestContext<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    public TestAuthController(TestContext<Program> factory)
    {
        _factory = factory;
    }

    [Fact(DisplayName = "Test register user")]
    public async Task TestRegisterCommomUserSuccess()
    {
        var client = _factory.CreateClient();
        var userDTO = new UserDTO
        {
            Name = "Test",
            Email = "test@123.com",
            Password = "test123",
            Admin = false,
        };

        using HttpResponseMessage response = await client
            .PostAsJsonAsync("register", userDTO);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact(DisplayName = "test duplicated user")]
    public async Task TestRegisterDuplicateEmailSuccess()
    {
        var client = _factory.CreateClient();
        var userDTO = new UserDTO
        {
            Name = "Test",
            Email = "test@123.com",
            Password = "test123",
            Admin = false,
        };

        await client.PostAsJsonAsync("register", userDTO);

        using HttpResponseMessage response = await client
            .PostAsJsonAsync("register", userDTO);

        var responseBody = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        responseBody.Should().Be("The email is already registered");
    }

    [Fact(DisplayName = "Test register admin")]
    public async Task TestRegisterAdminUserSuccess()
    {
        var client = _factory.CreateClient();
        var user = new User
        {
            Name = "Admin User",
            Email = "Admin@Admin.com",
            Password = "passwordAdmin",
            Admin = true,
        };

        var instance = new TokenGenerator();
        var token = instance.CreateToken(user.Email, user.Admin);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using HttpResponseMessage response = await client
            .PostAsJsonAsync("register", user);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact(DisplayName = "Teste 'Unauthorized' para cadastro de admin")]
    public async Task TestRegisterAdminUserFail()
    {
        var client = _factory.CreateClient();
        var newUser = new User
        {
            Name = "testAdmin",
            Email = "testAdmin@admin.com",
            Password = "passwordtestAdmin",
            Admin = true,
        };

        var userNotAdmin = new User
        {
            Name = "userNottestAdmin",
            Email = "userNottestAdmin@userNotAdmin.com",
            Password = "userNottestAdmin",
            Admin = false,
        };

        var instance = new TokenGenerator();
        var token = instance.CreateToken(userNotAdmin.Email, userNotAdmin.Admin);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using HttpResponseMessage response = await client
            .PostAsJsonAsync("register", newUser);

        var responseBody = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        responseBody.Should().Be("Acesso negado");
    }

    [Fact(DisplayName = "Teste login")]
    public async Task TestLoginSuccess()
    {
        var client = _factory.CreateClient();
        var user = new User
        {
            Name = "Test",
            Email = "aloha@321.com",
            Password = "password",
            Admin = false,
        };

        await client.PostAsJsonAsync("register", user);

        using HttpResponseMessage response = await client
            .PostAsJsonAsync("login", user);

        var responseBody = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseBody.Split(".").Should().HaveCount(3);
    }
}
