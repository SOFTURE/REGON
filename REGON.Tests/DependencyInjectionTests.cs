using Microsoft.Extensions.DependencyInjection;
using REGON.Client;
using REGON.Client.Services;
using REGON.Client.Settings;
using Shouldly;

namespace REGON.Tests;

public class DependencyInjectionTests
{
    private sealed class TestSettings : IRegonSettings
    {
        public RegonSettings Regon { get; init; } = new() { ApiKey = "abcde12345abcde12345" };
    }

    [Test]
    public void AddRegonClient_Should_Register_Client_When_Settings_Are_Registered()
    {
        var services = new ServiceCollection();
        services.AddSingleton(new TestSettings());

        services.AddRegonClient<TestSettings>();

        services.BuildServiceProvider().GetService<IRegonClient>().ShouldBeOfType<RegonClient>();
    }

    [Test]
    public void AddRegonClient_Should_Throw_When_Settings_Are_Not_Registered()
    {
        var services = new ServiceCollection();

        Should.Throw<InvalidOperationException>(() => services.AddRegonClient<TestSettings>());
    }
}
