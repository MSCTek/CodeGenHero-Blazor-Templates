using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Inflector;
using CodeGenHero.Template.Models;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.Template.Blazor6.Generators
{
    public sealed class RepositoryInterfaceGenerator : BaseBlazorGenerator
    {
        public RepositoryInterfaceGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public string Generate(
            List<NamespaceItem> usings,
            string classNamespace,
            string namespacePostfix,
            IList<IEntityType> entities,
            string className,
            string repositoryCrudInterfaceName,
            string dbContextName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GenerateHeader(usings, classNamespace));

            sb.AppendLine($"\tpublic partial interface {className} : {repositoryCrudInterfaceName}");
            sb.AppendLine("{");

            sb.AppendLine($"\t\tent{namespacePostfix}.{dbContextName} {dbContextName} {{ get; }}");
            sb.AppendLine(string.Empty);

            foreach (var entity in entities)
            {
                string entityName = entity.ClrType.Name;
                string methodParameterSignature = GetMethodParameterSignature(entity);

                sb.AppendLine($"// {entityName}");
                sb.AppendLine($"\t\tTask<IRepositoryActionResult<ent{namespacePostfix}.{entityName}>> Delete_{entityName}Async({methodParameterSignature});");
                sb.AppendLine(string.Empty);
                sb.AppendLine($"\t\tTask<ent{namespacePostfix}.{entityName}> Get_{entityName}Async({methodParameterSignature}, Enums.RelatedEntitiesType relatedEntitiesType = Enums.RelatedEntitiesType.None);");
                sb.AppendLine(string.Empty);
                sb.AppendLine($"\t\tTask<RepositoryPageDataResponse<IList<ent{namespacePostfix}.{entityName}>>> GetPageData_{entityName}Async(RepositoryPageDataRequest request);");
                sb.AppendLine(string.Empty);
            }

            sb.Append(GenerateFooter());
            return sb.ToString();
        }
    }
}