namespace Sqordia.Application.Common.Security;

// TODO: Authorization interface
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class AuthorizeAttribute : Attribute
{
    public string[] Roles { get; set; } = Array.Empty<string>();
    public string Policy { get; set; } = string.Empty;

    public AuthorizeAttribute() { }

    public AuthorizeAttribute(string roles)
    {
        Roles = roles.Split(',', StringSplitOptions.RemoveEmptyEntries)
                     .Select(r => r.Trim())
                     .ToArray();
    }

    public AuthorizeAttribute(params string[] roles)
    {
        Roles = roles;
    }
}
