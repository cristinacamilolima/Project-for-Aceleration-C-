using Microsoft.AspNetCore.Mvc.Testing;
using Project_for_Aceleration_Csharp_Tryitter.DTO;
using Project_for_Aceleration_Csharp_Tryitter.Models;
using Project_for_Aceleration_Csharp_Tryitter.Utils;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;

namespace Project_for_Aceleration_Csharp_Tryitter.Test
{
    public class TestPostsController : IClassFixture<TestContext<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public UserDTO user1 = new UserDTO()
        {
            Name = "Test",
            Email = "test@testmail.com",
            Password = "test123",
            Admin = false,
        };
        public TestPostsController(TestContext<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task TestGetAllPosts()
        {
            var client = _factory.CreateClient();

            using HttpResponseMessage response = await client
                .GetAsync("posts");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }       
        
        [Fact]
        public async Task TestGetPostById()
        {
            var client = _factory.CreateClient();
            var post = new Post() {
                UserId = Guid.NewGuid(),
                Title = Faker.Lorem.Sentence(),
                ImageUrl = Faker.Lorem.Sentence(),
            };

            user1.Name = Faker.Name.First();
            user1.Name = Faker.Internet.Email(user1.Name);

            var instance = new TokenGenerator();
            var token = instance.CreateToken(user1.Email!, false);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await client.PostAsJsonAsync("register", user1);

            await client.PostAsJsonAsync("posts", post);

            using HttpResponseMessage response = await client
                .GetAsync("posts/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Fact]
        public async Task TestGetLastPost()
        {
            var client = _factory.CreateClient();
            var post = new Post() {
                UserId = Guid.NewGuid(),
                Title = Faker.Lorem.Sentence(),
                ImageUrl = Faker.Lorem.Sentence(),
            };

            user1.Name = Faker.Name.First();
            user1.Name = Faker.Internet.Email(user1.Name);

            var instance = new TokenGenerator();
            var token = instance.CreateToken(user1.Email!, false);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await client.PostAsJsonAsync("register", user1);

            await client.PostAsJsonAsync("posts", post);

            using HttpResponseMessage response = await client
                .GetAsync("posts/last");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Fact]
        public async Task TestGetLastPostByUserId()
        {
            var client = _factory.CreateClient();
            var post = new Post() {
                UserId = Guid.NewGuid(),
                Title = Faker.Lorem.Sentence(),
                ImageUrl = Faker.Lorem.Sentence(),
            };

            user1.Name = Faker.Name.First();
            user1.Name = Faker.Internet.Email(user1.Name);

            var instance = new TokenGenerator();
            var token = instance.CreateToken(user1.Email!, false);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await client.PostAsJsonAsync("register", user1);

            await client.PostAsJsonAsync("posts", post);

            using HttpResponseMessage response = await client
                .GetAsync("posts/last/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
