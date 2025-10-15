namespace PetCare.Infrastructure.Services.Email;

using System;
using Microsoft.AspNetCore.Hosting;
using PetCare.Application.Interfaces;

/// <summary>
/// Provides access to email-related assets (such as images) from the file system.
/// Specifically, it loads the PetCare logo from the <c>wwwroot/images</c> folder
/// and converts it into a Base64 data URI string for embedding in email templates.
/// </summary>
public sealed class FileEmailAssetProvider : IEmailAssetProvider
{
    private readonly string logoPath;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileEmailAssetProvider"/> class.
    /// </summary>
    /// <param name="env">
    /// The hosting environment, used to determine the physical path to the <c>wwwroot</c> folder.
    /// </param>
    public FileEmailAssetProvider(IWebHostEnvironment env)
    {
        // Лого лежить у wwwroot/images/logo.png
        this.logoPath = Path.Combine(env.WebRootPath, "images", "logo.png");
    }

    /// <summary>
    /// Retrieves the PetCare logo as a Base64-encoded data URI string.
    /// </summary>
    /// <returns>
    /// A string in the format <c>data:image/png;base64,...</c> suitable for embedding directly
    /// in HTML <c>&lt;img&gt;</c> tags.
    /// </returns>
    /// <exception cref="FileNotFoundException">
    /// Thrown when the <c>logo.png</c> file is missing in the <c>wwwroot/images</c> folder.
    /// </exception>
    public string GetLogoBase64()
    {
        if (!File.Exists(this.logoPath))
        {
            throw new FileNotFoundException("Logo file not found", this.logoPath);
        }

        var bytes = File.ReadAllBytes(this.logoPath);
        return $"data:image/png;base64,{Convert.ToBase64String(bytes)}";
    }
}
