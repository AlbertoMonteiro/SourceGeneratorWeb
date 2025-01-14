using DependencyInjectionGenerator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static DependenceyInjectionGeneratorTests.TestHelpers;

namespace DependenceyInjectionGeneratorTests;

public class TopLevelStatementsTests
{
    private const string generatedFileNameSufix = @"DependencyInjectionGenerator\DependencyInjectionGenerator.Generator\";
    private const string generatedFileName = $@"{generatedFileNameSufix}GeneratedServicesExtension.Generated.cs";

    [Fact]
    public async Task GeneratedCodeWithoutServicesWork()
    {
        var source = @"
using Microsoft.AspNetCore.Builder;
var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);
builder.Services.AddServicesToDI();
";

        const string expectedAttributeCode = @"// <auto-generated />
[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
internal class AddServiceAttribute : System.Attribute
{
}
";
        const string expectedExtensionCode = @"// <auto-generated />
using Microsoft.Extensions.DependencyInjection;

public static class GeneratedServicesExtension
{
    public static void AddServicesToDI(this IServiceCollection services)
    {
    }
}
";

        await GetTestRunner(source, OutputKind.ConsoleApplication, ($@"{generatedFileNameSufix}AddService.Generated.cs", SourceText.From(expectedAttributeCode, Encoding.UTF8)), (generatedFileName, SourceText.From(expectedExtensionCode, Encoding.UTF8)))
            .RunAsync();
    }
}
