using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Inflector;
using CodeGenHero.Template.Helpers;
using CodeGenHero.Template.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenHero.Template.Blazor6.Generators
{
    public class AutoMapperProfileGenerator : BaseBlazorGenerator
    {
        private const string EXCLUDEPERNAVIGATIONPROPERTYCONFIGURATION = " -- Excluded navigation property per configuration.";

        public AutoMapperProfileGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public string Generate(
            List<NamespaceItem> usings,
            string classNamespace,
            string namespacePostfix,
            IList<IEntityType> entities,
            bool includeEntityNavigations,
            IList<IEntityNavigation> excludedEntityNavigations,
            string className)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GenerateHeader(usings, classNamespace));

            sb.AppendLine($"public partial class {className} : Profile");
            sb.AppendLine("{");

            sb.Append(GenerateConstructor(className));
            sb.Append(GenerateInitializers(entities, includeEntityNavigations, excludedEntityNavigations));

            sb.Append(GenerateFooter());
            return sb.ToString();
        }

        private string GenerateConstructor(string className)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public {className}()");
            sb.AppendLine("{");

            sb.AppendLine("InitializeProfile();");
            sb.AppendLine("InitializePartial();");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        private string GenerateInitializers(IList<IEntityType> entities, bool includeEntityNavigations, IList<IEntityNavigation> excludedEntityNavigations)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("partial void InitializePartial();");
            sb.AppendLine(string.Empty);

            sb.AppendLine("private void InitializeProfile()");
            sb.AppendLine("{");

            foreach (var entity in entities)
            {
                string tableName = Inflector.Pascalize(entity.ClrType.Name);

                sb.AppendLine($"\tCreateMap<xDTO.{tableName}, xENT.{tableName}>()");
                if (entity.ForeignKeys.Any() || entity.Navigations.Any())
                {
                    List<string> referenced = new List<string>();

                    foreach (var navigation in entity.Navigations)
                    {
                        var navigationName = navigation.Name;
                        string commentOut = EntityNavigationsContainsNavigationName(excludedEntityNavigations, entity, navigationName) ? "//" : string.Empty;

                        sb.AppendLine($"\t\t{commentOut}.ForMember(x => x.{navigationName}, opt => opt.Ignore())");
                        if (!string.IsNullOrEmpty(commentOut))
                        {
                            sb.Append(EXCLUDEPERNAVIGATIONPROPERTYCONFIGURATION);
                        }
                    }

                    if (includeEntityNavigations)
                    {
                        foreach (var foreignKey in entity.ForeignKeys)
                        {
                            string fkName = Inflector.Pascalize(foreignKey.DependentToPrincipal.ClrType.Name);
                            string commentOut = EntityNavigationsContainsNavigationName(excludedEntityNavigations, entity, fkName) ? "//" : string.Empty;

                            sb.AppendLine($"\t\t{commentOut}.ForMember(d => d.{fkName}, opt => opt.Ignore())");
                            if (!string.IsNullOrEmpty(commentOut))
                            {
                                sb.Append(EXCLUDEPERNAVIGATIONPROPERTYCONFIGURATION);
                            }
                        }
                    }
                }
                sb.AppendLine("\t\t.ReverseMap()");
                sb.AppendLine("\t\t.PreserveReferences();");
                sb.AppendLine(string.Empty);
            }

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }
    }
}