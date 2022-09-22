using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Inflector;
using CodeGenHero.Template.Helpers;
using CodeGenHero.Template.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenHero.Template.Blazor6.Generators
{
    public sealed class APIControllerCustomGenerator : BaseBlazorGenerator
    {
        public APIControllerCustomGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public string Generate(
            List<NamespaceItem> usings,
            string classNamespace,
            string namespacePostfix,
            IEntityType entity,
            string className,
            string repositoryInterfaceClassName)
        {
            var entityName = $"{entity.ClrType.Name}";
            var entitySignature = GetMethodParameterSignature(entity);

            // Begin Generation.
            StringBuilder sb = new StringBuilder();

            sb.Append(GenerateHeader(usings, classNamespace));

            sb.AppendLine($"\tpublic partial class {className}");
            sb.AppendLine("\t{");

            sb.AppendLine($"\t\tprotected {repositoryInterfaceClassName} {entityName}Repository {{ get; set; }}");

            sb.Append(GenerateMethodSignatures(entityName, namespacePostfix, entitySignature, repositoryInterfaceClassName));

            sb.Append(GenerateFooter());

            return sb.ToString();
        }

        private string GenerateMethodSignatures(string entityName, string namespacePostfix, string entitySignature, string repositoryInterfaceClassName)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(GenerateRunCustomLogicAfterCtor(entityName, repositoryInterfaceClassName));
            sb.Append(GenerateRunCustomLogicAfterInsert(entityName, namespacePostfix));
            sb.Append(GenerateRunCustomLogicAfterUpdatePatch(entityName, namespacePostfix));
            sb.Append(GenerateRunCustomLogicAfterUpdatePut(entityName, namespacePostfix, entitySignature));
            sb.Append(GenerateRunCustomLogicOnGetEntityByPK(entityName, namespacePostfix, entitySignature));

            return sb.ToString();
        }

        private string GenerateRunCustomLogicAfterCtor(string entityName, string repositoryInterfaceClassName)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"protected override void RunCustomLogicAfterCtor()");
            sb.AppendLine("{");

            sb.AppendLine($"{entityName}Repository = ServiceProvider.GetService<{repositoryInterfaceClassName}>();");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateRunCustomLogicAfterInsert(string entityName, string namespacePostfix)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"partial void RunCustomLogicAfterInsert(ref ent{namespacePostfix}.{entityName} newDBItem, ref IRepositoryActionResult<ent{namespacePostfix}.{entityName}> result)");
            sb.AppendLine("{");
            sb.AppendLine(string.Empty);
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateRunCustomLogicAfterUpdatePatch(string entityName, string namespacePostfix)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"partial void RunCustomLogicAfterUpdatePatch(ref ent{namespacePostfix}.{entityName} updatedDBItem, ref IRepositoryActionResult<ent{namespacePostfix}.{entityName}> result)");
            sb.AppendLine("{");
            sb.AppendLine(string.Empty);
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateRunCustomLogicAfterUpdatePut(string entityName, string namespacePostfix, string entitySignature)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"partial void RunCustomLogicAfterUpdatePut(ref ent{namespacePostfix}.{entityName} updatedDBItem, ref IRepositoryActionResult<ent{namespacePostfix}.{entityName}> result)");
            sb.AppendLine("{");
            sb.AppendLine(string.Empty);
            sb.AppendLine("}");

            sb.AppendLine(string.Empty);

            sb.AppendLine($"partial void RunCustomLogicBeforeUpdatePut(ref ent{namespacePostfix}.{entityName} updatedDBItem, {entitySignature})");
            sb.AppendLine("{");
            sb.AppendLine(string.Empty);
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateRunCustomLogicOnGetEntityByPK(string entityName, string namespacePostfix, string entitySignature)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"partial void RunCustomLogicOnGetEntityByPK(ref ent{namespacePostfix}.{entityName} dbItem, {entitySignature}, Enums.RelatedEntitiesType relatedEntitiesType)");
            sb.AppendLine("{");
            sb.AppendLine(string.Empty);
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }
    }
}
