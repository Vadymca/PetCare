namespace PetCare.Infrastructure.Services.Email;

using System.Threading.Tasks;
using PetCare.Application.Interfaces;
using RazorLight;

/// <summary>
/// Renders Razor templates to string using embedded resources.
/// </summary>
public sealed class EmailTemplateRenderer : IEmailTemplateRenderer
{
    private readonly RazorLightEngine engine;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailTemplateRenderer"/> class.
    /// Configures the RazorLight engine to use embedded resources from the assembly
    /// where <see cref="Application.EmailTemplates.ConfirmEmailTemplate"/> is defined.
    /// </summary>
    public EmailTemplateRenderer()
    {
        var assembly = typeof(Application.EmailTemplates.ConfirmEmailTemplate).Assembly;

        this.engine = new RazorLightEngineBuilder()
            .UseEmbeddedResourcesProject(assembly)
            .UseMemoryCachingProvider()
            .EnableDebugMode()
            .Build();
    }

    /// <summary>
    /// Renders the specified embedded Razor template with the provided model.
    /// </summary>
    /// <param name="templateName">
    /// The full name of the embedded resource template,
    /// e.g. "PetCare.Application.EmailTemplates.ConfirmEmailTemplate.cshtml".
    /// </param>
    /// <param name="model">The model to pass into the template.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, containing the rendered HTML string.</returns>
    public async Task<string> RenderAsync(string templateName, object model)
    {
        return await this.engine.CompileRenderAsync(templateName, model);
    }
}
