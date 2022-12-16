using Project_for_Aceleration_Csharp_Tryitter.Models;
using FluentAssertions;

namespace Project_for_Aceleration_Csharp_Tryitter.Test;

public class TestUserValitation
{
    [Fact]
    public void TestUserValidationSuccess()
    {
        Validate.Validate.ValidateUser(new User()).Should().BeTrue();
    }

    [Theory(DisplayName = "Test formt email is valid")]
    [InlineData("test@mail.com")]
    [InlineData("test@mail.co")]
    [InlineData("test@provider.gg")]
    public void TestTokenGeneratorSuccess(string email)
    {
        Validate.Validate.ValidateEmail(email).Should().BeTrue();
    }   
    
    [Theory(DisplayName = "Test formt email is invalid")]
    [InlineData("testmail.com")]
    [InlineData("test@mail")]
    [InlineData("@provider.gg")]
    public void TestTokenGeneratorFail(string email)
    {
        Validate.Validate.ValidateEmail(email).Should().BeFalse();
    }
}
