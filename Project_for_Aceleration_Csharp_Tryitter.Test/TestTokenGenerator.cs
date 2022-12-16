using Project_for_Aceleration_Csharp_Tryitter.Models;
using Project_for_Aceleration_Csharp_Tryitter.Utils;
using FluentAssertions;

namespace Project_for_Aceleration_Csharp_Tryitter.Test;

public class TestTokenGenerator
{
    [Fact]
    public void TestTokenGeneratorSuccess()
    {
        var user = new User
        {
            Email = "test@mail.com",
            Password = "test123",
        };
        var instance = new TokenGenerator();
        var token = instance.CreateToken(user.Email, user.Admin);

        token.Should().NotBeNullOrEmpty();
        token.Split(".").Should().HaveCount(3);
    }
}
