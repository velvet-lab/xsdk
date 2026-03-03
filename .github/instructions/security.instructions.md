---
description: 'Security best practices and guidelines for xSDK project'
applyTo: '**/*.cs'
---

# Security Guidelines

## General Security Principles

1. **Defense in Depth**: Implement multiple layers of security
2. **Principle of Least Privilege**: Grant minimum necessary permissions
3. **Secure by Default**: Default configurations should be secure
4. **Fail Securely**: When errors occur, fail safely
5. **Don't Trust Input**: Validate and sanitize all external input

## Secrets Management

**NEVER commit secrets to source control:**

- API keys
- Connection strings
- Passwords
- Certificates
- Private keys
- Access tokens

### Proper Secret Handling

```csharp
// ❌ BAD: Hardcoded secret
var apiKey = "sk_live_abc123xyz789";

// ✅ GOOD: Load from configuration
var apiKey = configuration["ApiKey"];

// ✅ BETTER: Use environment variables or Azure Key Vault
var apiKey = Environment.GetEnvironmentVariable("API_KEY");
```

### Configuration Files

- Never commit `appsettings.Production.json` with secrets
- Use `appsettings.Development.json` for local development
- Use User Secrets for local development: `dotnet user-secrets set "ApiKey" "value"`
- Use Azure Key Vault or similar for production
- Add `**/appsettings.*.json` (except Development) to `.gitignore`

## Input Validation

Validate ALL input from external sources:

```csharp
/// <summary>
/// Creates a new user with the specified name.
/// </summary>
/// <exception cref="ArgumentException">Thrown when name is invalid.</exception>
public async Task<User> CreateUserAsync(string name, CancellationToken cancellationToken)
{
    // Validate input at entry point
    if (string.IsNullOrWhiteSpace(name))
    {
        throw new ArgumentException("Name cannot be empty.", nameof(name));
    }

    if (name.Length > 100)
    {
        throw new ArgumentException("Name cannot exceed 100 characters.", nameof(name));
    }

    // Additional validation...
    var user = new User { Name = name };
    return await _repository.AddAsync(user, cancellationToken);
}
```

Use FluentValidation for complex validation:

```csharp
public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100)
            .Matches("^[a-zA-Z\\s]+$").WithMessage("Name must contain only letters and spaces");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}
```

## SQL Injection Prevention

Entity Framework Core uses parameterized queries by default, which prevents SQL injection:

```csharp
// ✅ SAFE: EF Core uses parameterized query
var user = await context.Users
    .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

// ❌ DANGEROUS: Raw SQL with string concatenation
var query = $"SELECT * FROM Users WHERE Email = '{email}'";
var users = context.Users.FromSqlRaw(query).ToList();

// ✅ SAFE: Parameterized raw SQL
var users = await context.Users
    .FromSqlRaw("SELECT * FROM Users WHERE Email = {0}", email)
    .ToListAsync(cancellationToken);
```

## Authentication and Authorization

### API Key Authentication

```csharp
// Configure API key authentication
services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme)
    .AddApiKeyInHeaderOrQueryParams<ApiKeyProvider>(options =>
    {
        options.Realm = "xSDK API";
        options.KeyName = "X-API-Key";
    });

// Protect endpoints
[Authorize]
[ApiController]
public class UsersController : ControllerBase
{
    // Protected endpoint
}
```

### Role-Based Authorization

```csharp
[Authorize(Roles = "Admin")]
public async Task<IActionResult> DeleteUser(string id)
{
    // Only admins can delete users
}
```

### Policy-Based Authorization

```csharp
services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("RequireElevatedRights", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Admin") ||
            context.User.IsInRole("PowerUser")));
});
```

## Data Protection

### Encrypting Sensitive Data

```csharp
public class SecureDataService
{
    private readonly IDataProtector _protector;

    public SecureDataService(IDataProtectionProvider provider)
    {
        _protector = provider.CreateProtector("SecureDataService.Purpose");
    }

    public string Encrypt(string plainText)
    {
        return _protector.Protect(plainText);
    }

    public string Decrypt(string cipherText)
    {
        return _protector.Unprotect(cipherText);
    }
}
```

### Hashing Passwords

Never store passwords in plain text:

```csharp
// Use ASP.NET Core Identity's password hasher
public class PasswordService
{
    private readonly IPasswordHasher<User> _passwordHasher;

    public PasswordService(IPasswordHasher<User> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public string HashPassword(User user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }

    public bool VerifyPassword(User user, string hash, string password)
    {
        var result = _passwordHasher.VerifyHashedPassword(user, hash, password);
        return result == PasswordVerificationResult.Success;
    }
}
```

## Logging Security

### What NOT to Log

Never log sensitive information:

- Passwords or password hashes
- API keys or tokens
- Credit card numbers
- Personal Identifiable Information (PII)
- Session identifiers
- Security questions and answers

### Secure Logging

```csharp
// ❌ BAD: Logging sensitive data
_logger.LogInformation("User {Email} logged in with password {Password}", 
    email, password);

// ✅ GOOD: Log without sensitive data
_logger.LogInformation("User {UserId} logged in successfully", userId);

// ✅ GOOD: Redact sensitive data
_logger.LogInformation("Processing card ending in {LastFour}", 
    creditCard.Substring(creditCard.Length - 4));
```

### Security Event Logging

Log security-relevant events:

```csharp
// Log authentication failures
_logger.LogWarning("Failed login attempt for user {Email} from IP {IpAddress}", 
    email, ipAddress);

// Log authorization failures
_logger.LogWarning("User {UserId} attempted unauthorized access to {Resource}", 
    userId, resourceName);

// Log suspicious activity
_logger.LogWarning("Rate limit exceeded for IP {IpAddress}", ipAddress);
```

## Cross-Site Scripting (XSS) Prevention

ASP.NET Core automatically encodes output in Razor views. For APIs:

- Return JSON data, don't construct HTML
- Use proper content types
- Validate and sanitize input
- Use Content Security Policy headers

```csharp
// Add security headers
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'");
    await next();
});
```

## Cross-Site Request Forgery (CSRF) Protection

For web applications (not typically needed for APIs using bearer tokens):

```csharp
services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
});
```

## Dependency Security

### Keep Dependencies Updated

- Regularly update NuGet packages
- Monitor security advisories
- Use `dotnet list package --vulnerable` to check for vulnerabilities

### Package Validation

```powershell
# Check for vulnerable packages
dotnet list package --vulnerable

# Update packages
dotnet add package PackageName

# Use central package management (already configured)
# Update versions in Directory.Packages.props
```

## Error Handling Security

Don't expose sensitive information in error messages:

```csharp
// ❌ BAD: Exposes internal details
catch (Exception ex)
{
    return BadRequest($"Database error: {ex.Message} - Connection: {connectionString}");
}

// ✅ GOOD: Generic error message for client, detailed log for admins
catch (Exception ex)
{
    _logger.LogError(ex, "Failed to create user");
    return StatusCode(500, "An error occurred while processing your request.");
}
```

Use Problem Details (RFC 9457) for consistent error responses:

```csharp
services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        // Don't include exception details in production
        if (!context.HttpContext.RequestServices
            .GetRequiredService<IHostEnvironment>().IsDevelopment())
        {
            context.ProblemDetails.Extensions.Remove("exception");
            context.ProblemDetails.Extensions.Remove("trace");
        }
    };
});
```

## Rate Limiting

Implement rate limiting to prevent abuse:

```csharp
services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.User.Identity?.Name ?? context.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,
                QueueLimit = 0,
                Window = TimeSpan.FromMinutes(1)
            }));
});
```

## HTTPS and TLS

Always use HTTPS in production:

```csharp
// Enforce HTTPS
app.UseHttpsRedirection();

// Configure Kestrel with TLS
builder.WebHost.ConfigureKestrel(options =>
{
    options.ConfigureHttpsDefaults(httpsOptions =>
    {
        httpsOptions.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13;
    });
});
```

## Security Checklist

Before releasing code:

- [ ] No secrets in source code or configuration files
- [ ] All inputs are validated
- [ ] All SQL uses parameterized queries
- [ ] Sensitive data is encrypted at rest and in transit
- [ ] Passwords are hashed, never stored in plain text
- [ ] No sensitive information in logs
- [ ] Security events are logged appropriately
- [ ] Error messages don't expose internal details
- [ ] HTTPS is enforced
- [ ] Dependencies are up to date
- [ ] Authentication and authorization are properly implemented
- [ ] Rate limiting is in place
- [ ] Security headers are configured

## OWASP Top 10 Awareness

Be aware of OWASP Top 10 vulnerabilities:

1. Broken Access Control
2. Cryptographic Failures
3. Injection
4. Insecure Design
5. Security Misconfiguration
6. Vulnerable and Outdated Components
7. Identification and Authentication Failures
8. Software and Data Integrity Failures
9. Security Logging and Monitoring Failures
10. Server-Side Request Forgery (SSRF)

Reference: https://owasp.org/www-project-top-ten/
