using Microsoft.AspNetCore.Mvc.Testing;
using Project_for_Aceleration_Csharp_Tryitter.DTO;
using Project_for_Aceleration_Csharp_Tryitter.Utils;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Project_for_Aceleration_Csharp_Tryitter.Models;

namespace Project_for_Aceleration_Csharp_Tryitter.Test
{
    public class TestUsersController : IClassFixture<TestContext<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        public class UserId
        {
            public int id;
        }

        public TestUsersController(TestContext<Program> factory)
        {
            _factory = factory;
        }

        public readonly static TheoryData<UserDTO, UserDTO> TestGetAllUsersData =
        new()
        {
            {
                new UserDTO ()
                {
                    Name = "pessoa1",
                    Email = "pessoa1@mail.com",
                    Password = "pessoa1",
                    Admin = false,
                },
                new UserDTO ()
                {
                    Name = "pessoa2",
                    Email = "pessoa2@mail.com",
                    Password = "pessoa2",
                    Admin = false,
                }
            },
            {
                new UserDTO ()
                {
                    Name = "pessoa3",
                    Email = "pessoa3@mail.com",
                    Password = "pessoa3",
                    Admin = false,
                },
                new UserDTO ()
                {
                    Name = "pessoa4",
                    Email = "pessoa4@mail.com",
                    Password = "pessoa4",
                    Admin = false,
                }
            },
        };

        [Theory]
        [MemberData(nameof(TestGetAllUsersData))]
        public async Task TestGetAllUsers(UserDTO user1, UserDTO user2)
        {
            var client = _factory.CreateClient();

            await client.PostAsJsonAsync("register", user1);
            await client.PostAsJsonAsync("register", user2);

            using HttpResponseMessage response = await client.GetAsync("users");
            var responseBody = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseBody.Contains(user1.Name).Should().BeTrue();
            responseBody.Contains(user2.Name).Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestGetAllUsersData))]
        public async Task TestGetUserById(UserDTO user1, UserDTO user2)
        {
            var client = _factory.CreateClient();

            await client.PostAsJsonAsync("register", user1);
            await client.PostAsJsonAsync("register", user2);

            var instance = new TokenGenerator();
            var token = instance.CreateToken(user1.Email!, user1.Admin);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using HttpResponseMessage response = await client.GetAsync($"users/1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [MemberData(nameof(TestGetAllUsersData))]
        public async Task TestUpdateUsersByAdmin(UserDTO user1, UserDTO user2)
        {
            var client = _factory.CreateClient();

            await client.PostAsJsonAsync("register", user1);

            var instance = new TokenGenerator();
            var token = instance.CreateToken(user1.Email!, true);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using HttpResponseMessage response = await client.PutAsJsonAsync("users/1", user2);
            var responseBody = await response.Content.ReadFromJsonAsync<User>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseBody.Name.Should().Be(user2.Name);
        }

        [Theory]
        [MemberData(nameof(TestGetAllUsersData))]
        public async Task TestUpdateUsersByUser(UserDTO user1, UserDTO user2)
        {
            var client = _factory.CreateClient();

            await client.PostAsJsonAsync("register", user1);

            var instance = new TokenGenerator();
            var token = instance.CreateToken(user1.Email!, user1.Admin);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using HttpResponseMessage response = await client.PutAsJsonAsync("users/me", user2);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [MemberData(nameof(TestGetAllUsersData))]
        public async Task TestDeleteUsersByAdmin(UserDTO user1, UserDTO user2)
        {
            var client = _factory.CreateClient();

            user1.Email = Faker.Internet.Email("pessoa1");
            user2.Email = Faker.Internet.Email("pessoa2");

            await client.PostAsJsonAsync("register", user1);
            using HttpResponseMessage responsePost = await client.PostAsJsonAsync("register", user2);
            var resultPost = await responsePost.Content.ReadFromJsonAsync<UserDTO>();

            var instance = new TokenGenerator();
            var token = instance.CreateToken("test@gmail.com", true);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using HttpResponseMessage response = await client.DeleteAsync($"users/{resultPost.id}");

            var getUserResponse = await client.GetAsync($"users/{resultPost.id}");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            getUserResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [MemberData(nameof(TestGetAllUsersData))]
        public async Task Test6DeleteUsersByUser(UserDTO user1, UserDTO user2)
        {
            var client = _factory.CreateClient();

            await client.PostAsJsonAsync("register", user1);
            await client.PostAsJsonAsync("register", user2);

            var instance = new TokenGenerator();
            var token = instance.CreateToken(user1.Email!, user1.Admin);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using HttpResponseMessage response = await client.DeleteAsync("users/me");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
