using CodeGenHero.Inflector;
using CodeGenHero.Template.Helpers;
using CodeGenHero.Template.Models;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.Template.Blazor5.Generators
{
    public class ApiStatusControllerGenerator : BaseBlazorGenerator
    {
        public ApiStatusControllerGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public string Generate(
            List<NamespaceItem> usings,
            string classNamespace,
            string namespacePostfix,
            string className,
            string baseAPIControllerClassName,
            string repositoryInterfaceClassName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GenerateHeader(usings, classNamespace));

            sb.AppendLine("\t[ApiController]");
            sb.AppendLine($"\tpublic partial class {className} : {baseAPIControllerClassName}");
            sb.AppendLine("\t{");

            sb.Append(GenerateConstructor(className, repositoryInterfaceClassName));
            sb.Append(GenerateGet());

            sb.Append(GenerateFooter());
            return sb.ToString();
        }

        private string GenerateConstructor(string className, string repositoryInterfaceName)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public {className}(ILogger<{className}> logger,");
            sb.AppendLine("\tIServiceProvider serviceProvider,");
            sb.AppendLine("\tIHttpContextAccessor httpContextAccessor,");
            sb.AppendLine("\tLinkGenerator linkGenerator,");
            sb.AppendLine($"\t{repositoryInterfaceName} repository)");
            sb.AppendLine("\t: base(logger, serviceProvider, httpContextAccessor, linkGenerator, repository)");
            sb.AppendLine("{");
            sb.AppendLine("}");

            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        private string GenerateGet()
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            string getLiteral = 
            @"[HttpGet]
		    [VersionedActionConstraint(allowedVersion: 1, order: 100)]
		    public async Task<IActionResult> Get()
		    {
			    try
			    {
				    var version = this.GetType().Assembly.GetName().Version;
				    return Ok(version);
			    }
			    catch (Exception ex)
			    {
				    Log.LogError(exception: ex, message: ex.Message);

				    var retVal = StatusCode(StatusCodes.Status500InternalServerError,
					    value: System.Diagnostics.Debugger.IsAttached ? ex : null);
				    return retVal;
			    }
		    }";

            sb.AppendLine(getLiteral);
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }
    }
}
