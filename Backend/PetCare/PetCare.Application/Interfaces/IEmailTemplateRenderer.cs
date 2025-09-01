namespace PetCare.Application.Interfaces;

/// <summary>
/// Defines a service for rendering email templates with dynamic content.
/// </summary>
public interface IEmailTemplateRenderer
{
    /// <summary>
    /// Renders the specified email template using the provided model.
    /// </summary>
    /// <param name="templateName">The name or identifier of the template to render.</param>
    /// <param name="model">The data model used to populate the template.</param>
    /// <returns>
    /// A <see cref="Task{String}"/> representing the asynchronous operation,
    /// containing the rendered email body as a string.
    /// </returns>
    Task<string> RenderAsync(string templateName, object model);
}
