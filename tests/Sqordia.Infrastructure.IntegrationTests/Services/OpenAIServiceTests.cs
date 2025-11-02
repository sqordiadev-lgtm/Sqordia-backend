using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Sqordia.Infrastructure.Services;
using Xunit;

namespace Sqordia.Infrastructure.IntegrationTests.Services;

public class OpenAIServiceTests
{
    private readonly Mock<ILogger<OpenAIService>> _loggerMock;

    public OpenAIServiceTests()
    {
        _loggerMock = new Mock<ILogger<OpenAIService>>();
    }

    [Fact]
    public void Constructor_WithValidSettings_ShouldInitializeSuccessfully()
    {
        // Arrange
        var settings = new OpenAISettings
        {
            ApiKey = "sk-test-key",
            Model = "gpt-4",
            UseAzure = false
        };
        var options = Options.Create(settings);

        // Act
        var service = new OpenAIService(options, _loggerMock.Object);

        // Assert
        service.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithEmptyApiKey_ShouldLogWarning()
    {
        // Arrange
        var settings = new OpenAISettings
        {
            ApiKey = "",
            Model = "gpt-4",
            UseAzure = false
        };
        var options = Options.Create(settings);

        // Act
        var service = new OpenAIService(options, _loggerMock.Object);

        // Assert
        service.Should().NotBeNull();
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("OpenAI API key not configured")),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }

    [Fact]
    public void Constructor_WithAzureSettings_ShouldInitializeSuccessfully()
    {
        // Arrange
        var settings = new OpenAISettings
        {
            ApiKey = "test-azure-key",
            Model = "gpt-4",
            UseAzure = true,
            Endpoint = "https://test.openai.azure.com/"
        };
        var options = Options.Create(settings);

        // Act
        var service = new OpenAIService(options, _loggerMock.Object);

        // Assert
        service.Should().NotBeNull();
    }

    [Fact]
    public async Task GenerateContentAsync_WithoutApiKey_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var settings = new OpenAISettings
        {
            ApiKey = "",
            Model = "gpt-4",
            UseAzure = false
        };
        var options = Options.Create(settings);
        var service = new OpenAIService(options, _loggerMock.Object);

        // Act
        Func<Task> act = async () => await service.GenerateContentAsync(
            "System prompt",
            "User prompt",
            maxTokens: 100,
            temperature: 0.7f);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*not configured*");
    }

    [Theory]
    [InlineData("gpt-4")]
    [InlineData("gpt-3.5-turbo")]
    [InlineData("gpt-4-turbo")]
    public void Constructor_WithDifferentModels_ShouldAcceptValidModels(string model)
    {
        // Arrange
        var settings = new OpenAISettings
        {
            ApiKey = "sk-test-key",
            Model = model,
            UseAzure = false
        };
        var options = Options.Create(settings);

        // Act
        var service = new OpenAIService(options, _loggerMock.Object);

        // Assert
        service.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithInvalidAzureEndpoint_ShouldHandleGracefully()
    {
        // Arrange
        var settings = new OpenAISettings
        {
            ApiKey = "test-key",
            Model = "gpt-4",
            UseAzure = true,
            Endpoint = "not-a-valid-url"
        };
        var options = Options.Create(settings);

        // Act
        Action act = () => new OpenAIService(options, _loggerMock.Object);

        // Assert - Should not throw, but log error
        act.Should().NotThrow();
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Failed to initialize OpenAI client")),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }

    // Note: Integration tests with real OpenAI API should be run manually
    // or in a separate integration test suite with [Fact(Skip = "Requires OpenAI API key")]
    [Fact(Skip = "Requires valid OpenAI API key - Run manually with real key")]
    public async Task GenerateContentAsync_WithRealApiKey_ShouldGenerateContent()
    {
        // This test should be run manually with a real API key for integration testing
        // Arrange
        var settings = new OpenAISettings
        {
            ApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? "sk-your-key-here",
            Model = "gpt-4",
            UseAzure = false
        };
        var options = Options.Create(settings);
        var service = new OpenAIService(options, _loggerMock.Object);

        // Act
        var result = await service.GenerateContentAsync(
            "You are a helpful assistant.",
            "Say 'Hello, World!' and nothing else.",
            maxTokens: 50);

        // Assert
        result.Should().NotBeNullOrWhiteSpace();
        result.Should().Contain("Hello");
    }

    [Fact(Skip = "Requires valid Azure OpenAI credentials - Run manually with real credentials")]
    public async Task GenerateContentAsync_WithAzureOpenAI_ShouldGenerateContent()
    {
        // This test should be run manually with real Azure OpenAI credentials
        // Arrange
        var settings = new OpenAISettings
        {
            ApiKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY") ?? "your-azure-key",
            Model = "gpt-4",
            UseAzure = true,
            Endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT") ?? "https://your-resource.openai.azure.com/"
        };
        var options = Options.Create(settings);
        var service = new OpenAIService(options, _loggerMock.Object);

        // Act
        var result = await service.GenerateContentAsync(
            "You are a helpful assistant.",
            "Say 'Hello from Azure!' and nothing else.",
            maxTokens: 50);

        // Assert
        result.Should().NotBeNullOrWhiteSpace();
        result.Should().Contain("Hello");
    }
}

