using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Inflector;
using CodeGenHero.Template.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenHero.Template.Blazor6.Generators
{
    public class RepositoryCrudInterfaceGenerator : BaseBlazorGenerator
    {
        public RepositoryCrudInterfaceGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public string Generate(
            List<NamespaceItem> usings,
            string classNamespace,
            string namespacePostfix,
            IList<IEntityType> entities,
            string className)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GenerateHeader(usings, classNamespace));

            sb.AppendLine($"\tpublic interface {className} : ");

            var i = entities.Count();
            string comma;

            foreach (var entity in entities)
            {
                var entityName = entity.ClrType.Name;
                comma = (--i == 0) ? "" : ",";
                string text = $"\t\tICRUDOperation<{entityName}>{comma}";
                sb.AppendLine(text);
            }

            sb.AppendLine("\t{");

            foreach (var entity in entities)
            {
                var entityName = entity.ClrType.Name;
                sb.AppendLine($"\t\tIQueryable<{entityName}> GetQueryable_{entityName}(");
                sb.AppendLine($"\t\t\tEnums.RelatedEntitiesType relatedEntitiesType = Enums.RelatedEntitiesType.None);");
            }

            sb.Append(GenerateFooter());
            return sb.ToString();
        }
    }
}