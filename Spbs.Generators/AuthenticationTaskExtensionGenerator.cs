using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Spbs.Generators;

internal static class AttributeHelper
{
    public const string Attribute = @"
namespace Spbs.Generators.UserExtensions
{
    [global::System.AttributeUsage(global::System.AttributeTargets.Class)]
    public class AuthenticationTaskExtensionAttribute : global::System.Attribute
    {
        public bool IncludeUserId { get; set; }
        public bool IncludeClaimsPrincipal { get; set; }
        
        public AuthenticationTaskExtensionAttribute()
        {
            IncludeUserId = true;
            IncludeClaimsPrincipal = false;
        }

        public AuthenticationTaskExtensionAttribute(bool includeUserId)
        {
            IncludeUserId = includeUserId;
            IncludeClaimsPrincipal = false;
        }

        public AuthenticationTaskExtensionAttribute(bool includeUserId, bool includeClaimsPrincipal)
        {
            IncludeUserId = includeUserId;
            IncludeClaimsPrincipal = includeClaimsPrincipal;
        }
    }
}";
}

internal struct Info
{
    public bool IncludeUserId { get; set; }
    public bool IncludeClaimsPrincipal { get; set; }
}

[Generator]
public class AuthenticationTaskExtensionGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(c =>
            c.AddSource(
                "AuthenticationTaskExtensionAttribute.g.cs",
                SourceText.From(AttributeHelper.Attribute, Encoding.UTF8)
            ));

        IncrementalValuesProvider<ClassDeclarationSyntax> classes = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => node is ClassDeclarationSyntax { AttributeLists.Count: > 0 },
                static (context, _) => Unsafe.As<ClassDeclarationSyntax>(context.Node))
            .Where(c => c is not null);

        var compilation = context.CompilationProvider.Combine(classes.Collect());

        context.RegisterSourceOutput(compilation, (spc, source) =>
            Execute(source.Left, source.Right, spc));
    }

    private void Execute(Compilation item1, ImmutableArray<ClassDeclarationSyntax> item2,
        SourceProductionContext spc)
    {
        foreach (var @class in item2)
        {
            var model = item1.GetSemanticModel(@class.SyntaxTree);
            if (model.GetDeclaredSymbol(@class) is not INamedTypeSymbol namedSymbol)
            {
                continue;
            }

            var className = namedSymbol.Name;
            var classFQName = namedSymbol.ToDisplayString();
            var @namespace = namedSymbol.ContainingNamespace;

            Info info = new();
            var attributeList = namedSymbol.GetAttributes();
            foreach (var attributeData in attributeList)
            {
                if (attributeData.AttributeClass?.ToDisplayString() ==
                    "Spbs.Generators.UserExtensions.AuthenticationTaskExtensionAttribute")
                {
                    bool isValid = true;
                    ImmutableArray<TypedConstant> attributeArguments = attributeData.ConstructorArguments;
                    foreach (TypedConstant arg in attributeArguments)
                    {
                        if (arg is { Kind: TypedConstantKind.Error })
                        {
                            isValid = false;
                            break;
                        }
                    }

                    if (!isValid)
                    {
                        continue;
                    }

                    switch (attributeArguments.Length)
                    {
                        case 0:
                            info.IncludeUserId = true;
                            info.IncludeClaimsPrincipal = false;
                            break;
                        case 1:
                            info.IncludeUserId = (bool)attributeArguments[0].Value!;
                            info.IncludeClaimsPrincipal = false;
                            break;
                        case 2:
                            info.IncludeUserId = (bool)attributeArguments[0].Value!;
                            info.IncludeClaimsPrincipal = (bool)attributeArguments[1].Value!;
                            break;
                        default: break;
                    }

                    // TODO: Named parameters

                    var authStateTask = info.IncludeUserId || info.IncludeClaimsPrincipal
                        ? "[CascadingParameter] private Task<AuthenticationState> authenticationStateTask { get; set; }"
                        : string.Empty;
                    var userIdPropCode =
                        info.IncludeUserId ? $"protected Guid? _userId;" : string.Empty;
                    var claimsPrincipalPropCode = info.IncludeClaimsPrincipal
                        ? $"protected ClaimsPrincipal _claimsPrincipal;"
                        : string.Empty;
                    var code = @$"
using Spbs.Ui.Auth;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
namespace {@namespace} {{
    public partial class {className}
    {{
        {authStateTask}
        {userIdPropCode}
        {claimsPrincipalPropCode}

        protected async Task<Guid?> UserId()
        {{
            if (_userId is not null)
            {{
                return _userId;
            }}
        
            var authState = await authenticationStateTask;
            {(info.IncludeClaimsPrincipal ? string.Empty : "var")} _claimsPrincipal = authState.User;

            _userId = _claimsPrincipal.GetUserId();
            return _userId;
        }}
    }}
}}";

                    spc.AddSource(namedSymbol + ".g.cs", code);
                    break;
                }
            }
        }
    }
}
