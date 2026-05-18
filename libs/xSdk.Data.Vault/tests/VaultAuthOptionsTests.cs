/*
 * Copyright 2026 Roland Breitschaft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace xSdk.Data;

public class TokenAuthOptionsTests
{
    [Fact]
    public void TokenAuthOptions_DefaultToken_IsNull()
    {
        var options = new TokenAuthOptions();

        Assert.Null(options.Token);
    }

    [Fact]
    public void TokenAuthOptions_SetToken_StoresValue()
    {
        var options = new TokenAuthOptions();
        options.Token = "my-token";

        Assert.Equal("my-token", options.Token);
    }
}

public class TokenAuthOptionsValidatorTests
{
    private readonly TokenAuthOptionsValidator _validator = new();

    [Fact]
    public void Validate_WhenTokenPresent_IsValid()
    {
        var options = new TokenAuthOptions { Token = "some-token" };

        var result = _validator.Validate(options);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_WhenTokenEmpty_IsInvalid()
    {
        var options = new TokenAuthOptions { Token = string.Empty };

        var result = _validator.Validate(options);

        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_WhenTokenNull_IsInvalid()
    {
        var options = new TokenAuthOptions { Token = null };

        var result = _validator.Validate(options);

        Assert.False(result.IsValid);
    }
}

public class CertAuthOptionsTests
{
    [Fact]
    public void CertAuthOptions_DefaultCertificate_IsNull()
    {
        var options = new CertAuthOptions();

        Assert.Null(options.Certificate);
    }

    [Fact]
    public void CertAuthOptions_DefaultKey_IsNull()
    {
        var options = new CertAuthOptions();

        Assert.Null(options.Key);
    }

    [Fact]
    public void CertAuthOptions_SetCertificate_StoresValue()
    {
        var options = new CertAuthOptions();
        options.Certificate = "cert-data";

        Assert.Equal("cert-data", options.Certificate);
    }

    [Fact]
    public void CertAuthOptions_SetKey_StoresValue()
    {
        var options = new CertAuthOptions();
        options.Key = "key-data";

        Assert.Equal("key-data", options.Key);
    }

    [Fact]
    public void CertAuthOptions_CreateCertificate_WithInvalidData_ThrowsSdkException()
    {
        var options = new CertAuthOptions();
        options.Certificate = "not-valid-pem";
        options.Key = "not-valid-pem";

        Assert.Throws<SdkException>(() => options.CreateCertificate());
    }

    [Fact]
    public void CertAuthOptions_CreateCertificate_WithNullCertificate_ThrowsSdkException()
    {
        var options = new CertAuthOptions();

        Assert.Throws<SdkException>(() => options.CreateCertificate());
    }
}

public class CertAuthOptionsValidatorTests
{
    private readonly CertAuthOptionsValidator _validator = new();

    [Fact]
    public void Validate_WhenBothPresent_IsValid()
    {
        var options = new CertAuthOptions
        {
            Certificate = "cert",
            Key = "key"
        };

        var result = _validator.Validate(options);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_WhenCertificateEmpty_IsInvalid()
    {
        var options = new CertAuthOptions
        {
            Certificate = string.Empty,
            Key = "key"
        };

        var result = _validator.Validate(options);

        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_WhenKeyEmpty_IsInvalid()
    {
        var options = new CertAuthOptions
        {
            Certificate = "cert",
            Key = string.Empty
        };

        var result = _validator.Validate(options);

        Assert.False(result.IsValid);
    }
}

public class UsernamePasswordAuthTests
{
    [Fact]
    public void UsernamePasswordAuth_DefaultUsername_IsNull()
    {
        var options = new UsernamePasswordAuth();

        Assert.Null(options.Username);
    }

    [Fact]
    public void UsernamePasswordAuth_DefaultPassword_IsNull()
    {
        var options = new UsernamePasswordAuth();

        Assert.Null(options.Password);
    }

    [Fact]
    public void UsernamePasswordAuth_SetUsername_StoresValue()
    {
        var options = new UsernamePasswordAuth();
        options.Username = "john";

        Assert.Equal("john", options.Username);
    }

    [Fact]
    public void UsernamePasswordAuth_SetPassword_StoresValue()
    {
        var options = new UsernamePasswordAuth();
        options.Password = "secret";

        Assert.Equal("secret", options.Password);
    }
}
