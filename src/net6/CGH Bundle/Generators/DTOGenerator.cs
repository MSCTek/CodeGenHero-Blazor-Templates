using System.Collections.Generic;
using System.Text;
using CodeGenHero.Inflector;
using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Template.Models;
using CodeGenHero.Template.Helpers;
using System.Linq;

namespace CodeGenHero.Template.Blazor5.Generators
{
    public class DTOGenerator : BaseBlazorGenerator
    {
        public DTOGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {

        }

        public string Generate(
            List<NamespaceItem> usings,
            string classNamespace,
            string namespacePostfix,
            IEntityType entity,
            IList<IEntityNavigation> excludedEntityNavigations,
            bool dtoIncludeRelatedObjects,
            string className)
        {
            var primaryKeys = GetPrimaryKeys(entity);
            var entityProperties = entity.GetProperties();

            StringBuilder sb = new StringBuilder();
            sb.Append(GenerateHeader(usings, classNamespace));
            sb.AppendLine($"\tpublic partial class {className}");
            sb.AppendLine("{");
            sb.Append(GenerateConstructor(className));

            foreach(var property in entityProperties)
            {
                string cType = GetCType(property);
                var simpleType = ConvertToSimpleType(cType);
                string propertyName = property.Name;
                bool isPrimaryKey = primaryKeys.Any(x => x.Equals(propertyName));

                if (isPrimaryKey)
                {
                    sb.AppendLine($"\t\t// Primary Key");
                }
                sb.AppendLine($"\t\tpublic {simpleType} {Inflector.Pascalize(propertyName)} {{ get; set; }}");
                sb.AppendLine(string.Empty);
            }

            sb.Append(GenerateReverseNavigation(dtoIncludeRelatedObjects, entity, excludedEntityNavigations));

            sb.AppendLine("\t\tpartial void InitializePartial();");

            sb.Append(GenerateFooter());
            return sb.ToString();
        }

        private string GenerateConstructor(string className)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public {className}()");
            sb.AppendLine("{");
            sb.AppendLine("\tInitializePartial();");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateReverseNavigation(bool includeRelatedObjects,
            IEntityType entity,
            IList<IEntityNavigation> excludedNavigationProperties)
        {
            if (!includeRelatedObjects)
            {
                return string.Empty;
            }

            SortedSet<IForeignKey> foreignKeyList = entity.ForeignKeys;
            var excludedNavigations = excludedNavigationProperties.Select(x => x.Navigation).ToList();
            var excludedForeignKeys = GetForeignKeyNames(excludedNavigations);

            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            foreach (var reverseFK in entity.Navigations)
            {
                var keyName = GetForeignKeyName(reverseFK);
                bool preventCircularReference = string.IsNullOrWhiteSpace(keyName) ? false : excludedForeignKeys.Contains(keyName);

                if (preventCircularReference)
                {
                    sb.Append("// ");
                }

                if (!reverseFK.ClrType.Name.Equals("ICollection`1"))
                {
                    sb.AppendLine($"public virtual {reverseFK.ForeignKey.PrincipalEntityType.ClrType.Name} {Inflector.Pascalize(reverseFK.Name)} {{ get; set; }} // One to One mapping"); // Foreign Key
                }
                else
                {
                    sb.AppendLine($"public virtual ICollection<{reverseFK.ForeignKey.DeclaringEntityType.ClrType.Name}> {reverseFK.Name} {{ get; set; }} = new List<{reverseFK.ForeignKey.DeclaringEntityType.ClrType.Name}>(); // Many to many mapping"); // Foreign Key
                }
                sb.AppendLine(string.Empty);
            }

            return sb.ToString();
        }
    }
}
