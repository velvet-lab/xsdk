---
description: 'Refactor code to improve quality, maintainability, and performance'
---

# Code Refactoring Guide

You are helping to refactor code in the xSDK project. The goal is to improve code quality, maintainability, and performance while preserving existing functionality.

## Step 1: Understand the Context

Before refactoring:

1. **What code needs refactoring?** Identify the specific files/methods
2. **Why does it need refactoring?** What are the problems?
   - Code duplication?
   - Poor naming?
   - Complex logic?
   - Performance issues?
   - Tight coupling?
   - Poor testability?
3. **Are there tests?** Check if existing tests cover the code
4. **What are the dependencies?** Identify what depends on this code

## Step 2: Define Refactoring Goals

Be specific about what you want to achieve:

- [ ] Improve readability
- [ ] Reduce complexity
- [ ] Eliminate duplication
- [ ] Improve performance
- [ ] Enhance testability
- [ ] Better separation of concerns
- [ ] Improve error handling
- [ ] Update to modern C# patterns

## Step 3: Plan the Refactoring

Create a plan before making changes:

1. **Ensure tests exist**: If no tests, write them first
2. **Make incremental changes**: Small, focused refactorings
3. **Run tests frequently**: After each change
4. **Commit often**: Each refactoring step is a separate commit

## Common Refactoring Patterns

### 1. Extract Method

**Problem:** Long, complex methods that do multiple things

**Before:**
```csharp
public async Task<Order> ProcessOrderAsync(Order order)
{
    // Validate order
    if (order is null)
        throw new ArgumentNullException(nameof(order));
    
    if (string.IsNullOrEmpty(order.CustomerId))
        throw new ArgumentException("Customer ID is required");
    
    if (order.Items.Count == 0)
        throw new ArgumentException("Order must have items");
    
    // Calculate totals
    decimal subtotal = 0;
    foreach (var item in order.Items)
    {
        subtotal += item.Quantity * item.Price;
    }
    
    decimal tax = subtotal * 0.08m;
    decimal total = subtotal + tax;
    order.Subtotal = subtotal;
    order.Tax = tax;
    order.Total = total;
    
    // Apply discount
    if (order.Customer.IsPremium && total > 100)
    {
        decimal discount = total * 0.10m;
        order.Discount = discount;
        order.Total -= discount;
    }
    
    // Save order
    await _repository.AddAsync(order);
    
    return order;
}
```

**After:**
```csharp
public async Task<Order> ProcessOrderAsync(Order order)
{
    ValidateOrder(order);
    CalculateOrderTotals(order);
    ApplyDiscounts(order);
    
    await _repository.AddAsync(order);
    
    return order;
}

private void ValidateOrder(Order order)
{
    ArgumentNullException.ThrowIfNull(order);
    
    if (string.IsNullOrEmpty(order.CustomerId))
        throw new ArgumentException("Customer ID is required", nameof(order));
    
    if (order.Items.Count == 0)
        throw new ArgumentException("Order must have items", nameof(order));
}

private void CalculateOrderTotals(Order order)
{
    order.Subtotal = order.Items.Sum(item => item.Quantity * item.Price);
    order.Tax = order.Subtotal * TaxRate;
    order.Total = order.Subtotal + order.Tax;
}

private void ApplyDiscounts(Order order)
{
    if (order.Customer.IsPremium && order.Total > PremiumDiscountThreshold)
    {
        order.Discount = order.Total * PremiumDiscountRate;
        order.Total -= order.Discount;
    }
}
```

### 2. Replace Magic Numbers with Constants

**Before:**
```csharp
public bool IsEligible(User user)
{
    return user.Age >= 18 && user.AccountBalance > 1000;
}

public decimal CalculateDiscount(decimal amount)
{
    if (amount > 100)
        return amount * 0.15m;
    return amount * 0.10m;
}
```

**After:**
```csharp
private const int MinimumAge = 18;
private const decimal MinimumBalance = 1000m;
private const decimal PremiumThreshold = 100m;
private const decimal PremiumDiscountRate = 0.15m;
private const decimal StandardDiscountRate = 0.10m;

public bool IsEligible(User user)
{
    return user.Age >= MinimumAge && user.AccountBalance > MinimumBalance;
}

public decimal CalculateDiscount(decimal amount)
{
    var rate = amount > PremiumThreshold ? PremiumDiscountRate : StandardDiscountRate;
    return amount * rate;
}
```

### 3. Simplify Conditional Logic

**Before:**
```csharp
public async Task<bool> CanProcessAsync(Order order)
{
    if (order is not null)
    {
        if (order.Items.Count > 0)
        {
            if (order.Customer is not null)
            {
                if (order.Customer.IsActive)
                {
                    if (!order.Customer.IsSuspended)
                    {
                        return true;
                    }
                }
            }
        }
    }
    return false;
}
```

**After:**
```csharp
public async Task<bool> CanProcessAsync(Order order)
{
    if (order is null || order.Items.Count == 0)
        return false;
    
    if (order.Customer is null || !order.Customer.IsActive)
        return false;
    
    return !order.Customer.IsSuspended;
}

// Or even better with guard clauses:
public async Task<bool> CanProcessAsync(Order order)
{
    return order is not null
        && order.Items.Count > 0
        && order.Customer is { IsActive: true, IsSuspended: false };
}
```

### 4. Extract Class

**Problem:** Class doing too many things

**Before:**
```csharp
public class UserService
{
    public async Task<User> CreateUserAsync(string email, string password)
    {
        // Validation logic
        if (!IsValidEmail(email))
            throw new ArgumentException("Invalid email");
        
        // Password hashing logic
        var salt = GenerateSalt();
        var hash = HashPassword(password, salt);
        
        // Create user
        var user = new User
        {
            Email = email,
            PasswordHash = hash,
            PasswordSalt = salt
        };
        
        // Send email
        await SendWelcomeEmailAsync(user);
        
        return user;
    }
    
    // Many email-related methods
    // Many password-related methods
    // Many validation methods
}
```

**After:**
```csharp
public class UserService
{
    private readonly IEmailService _emailService;
    private readonly IPasswordService _passwordService;
    private readonly IValidationService _validationService;
    
    public async Task<User> CreateUserAsync(string email, string password)
    {
        _validationService.ValidateEmail(email);
        
        var (hash, salt) = _passwordService.HashPassword(password);
        
        var user = new User
        {
            Email = email,
            PasswordHash = hash,
            PasswordSalt = salt
        };
        
        await _emailService.SendWelcomeEmailAsync(user);
        
        return user;
    }
}

// Now you have focused services:
// EmailService, PasswordService, ValidationService
```

### 5. Use LINQ for Collection Operations

**Before:**
```csharp
public List<User> GetActiveUsers(List<User> users)
{
    var result = new List<User>();
    foreach (var user in users)
    {
        if (user.IsActive)
        {
            result.Add(user);
        }
    }
    return result;
}

public decimal GetTotalAmount(List<Order> orders)
{
    decimal total = 0;
    foreach (var order in orders)
    {
        total += order.Amount;
    }
    return total;
}
```

**After:**
```csharp
public List<User> GetActiveUsers(List<User> users)
{
    return users.Where(u => u.IsActive).ToList();
}

public decimal GetTotalAmount(List<Order> orders)
{
    return orders.Sum(o => o.Amount);
}
```

### 6. Use Pattern Matching

**Before:**
```csharp
public string GetUserType(User user)
{
    if (user is PremiumUser)
    {
        var premium = (PremiumUser)user;
        return $"Premium: {premium.Level}";
    }
    else if (user is AdminUser)
    {
        return "Admin";
    }
    else
    {
        return "Standard";
    }
}
```

**After:**
```csharp
public string GetUserType(User user) => user switch
{
    PremiumUser premium => $"Premium: {premium.Level}",
    AdminUser => "Admin",
    _ => "Standard"
};
```

### 7. Async/Await Refactoring

**Before:**
```csharp
public User GetUser(string id)
{
    return _repository.GetByIdAsync(id).Result; // ❌ Deadlock risk!
}

public async Task<string> GetUserNameAsync(string id)
{
    return await Task.Run(() => 
    {
        return _repository.GetByIdAsync(id).Result.Name; // ❌ Bad
    });
}
```

**After:**
```csharp
public async Task<User> GetUserAsync(string id, CancellationToken cancellationToken = default)
{
    return await _repository.GetByIdAsync(id, cancellationToken);
}

public async Task<string> GetUserNameAsync(string id, CancellationToken cancellationToken = default)
{
    var user = await _repository.GetByIdAsync(id, cancellationToken);
    return user.Name;
}
```

### 8. Replace Conditionals with Polymorphism

**Before:**
```csharp
public decimal CalculateShipping(Order order)
{
    if (order.ShippingMethod == "Standard")
    {
        return order.Weight * 0.5m;
    }
    else if (order.ShippingMethod == "Express")
    {
        return order.Weight * 1.5m + 10m;
    }
    else if (order.ShippingMethod == "Overnight")
    {
        return order.Weight * 3m + 25m;
    }
    return 0;
}
```

**After:**
```csharp
public interface IShippingCalculator
{
    decimal Calculate(Order order);
}

public class StandardShipping : IShippingCalculator
{
    public decimal Calculate(Order order) => order.Weight * 0.5m;
}

public class ExpressShipping : IShippingCalculator
{
    public decimal Calculate(Order order) => order.Weight * 1.5m + 10m;
}

public class OvernightShipping : IShippingCalculator
{
    public decimal Calculate(Order order) => order.Weight * 3m + 25m;
}

// Usage
public decimal CalculateShipping(Order order, IShippingCalculator calculator)
{
    return calculator.Calculate(order);
}
```

## Step 4: Execute Refactoring

Follow this process:

1. **Run existing tests** to ensure they pass
2. **Make one refactoring change** at a time
3. **Run tests again** to ensure nothing broke
4. **Commit the change** with a descriptive message
5. **Repeat** for the next refactoring

## Step 5: Update Tests

After refactoring:

- [ ] Tests still pass
- [ ] Test names still make sense
- [ ] Add tests for new abstractions
- [ ] Remove obsolete tests
- [ ] Update test setup if needed

## Refactoring Checklist

Before considering refactoring complete:

- [ ] All tests pass
- [ ] No loss of functionality
- [ ] Code is more readable
- [ ] Complexity is reduced
- [ ] No code duplication
- [ ] Names are descriptive
- [ ] Methods are focused and small
- [ ] Classes have single responsibility
- [ ] Documentation updated
- [ ] Performance not degraded (measure if critical)

## Safety Guidelines

1. **Always have tests**: Don't refactor without test coverage
2. **Make small changes**: Incremental refactoring is safer
3. **Run tests frequently**: After each change
4. **Don't change behavior**: Refactoring should preserve functionality
5. **Commit often**: Small, focused commits
6. **Review carefully**: Double-check changes before committing
7. **Measure performance**: If refactoring for performance, profile before and after

## When NOT to Refactor

- When there are no tests (write tests first)
- When it's not clear what the code does (understand it first)
- When you're under tight deadline (defer to later)
- When the code works and will never be touched again
- Just for the sake of it (must have clear benefit)

## Documentation After Refactoring

Update documentation:

- [ ] XML documentation for changed public APIs
- [ ] README if public API changed
- [ ] Architecture diagrams if structure changed
- [ ] Comments explaining complex logic
- [ ] Migration guide if breaking changes

Remember: The goal of refactoring is to make code easier to understand and modify, not to make it clever or complex.
