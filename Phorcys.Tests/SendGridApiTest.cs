using Xunit;
using Microsoft.Extensions.Configuration;
using Azure.Identity;
using System;

public class KeyVaultTests
{
	private readonly IConfiguration _configuration;

	public KeyVaultTests()
	{
		var keyVaultUri = new Uri("https://PhorcysKeyVault.vault.azure.net/");

		var builder = new ConfigurationBuilder()
			.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential());

		_configuration = builder.Build();
	}

	[Fact]
	public void ShouldRetrieveSendGridApiKeyFromKeyVault()
	{
		// Act
		var apiKey = _configuration["SendGrid:ApiKey"];

		// Assert
		Assert.False(string.IsNullOrEmpty(apiKey), "SendGrid:ApiKey was not retrieved from Key Vault");
		Assert.StartsWith("SG.", apiKey); // Optional: check for typical prefix
	}
}