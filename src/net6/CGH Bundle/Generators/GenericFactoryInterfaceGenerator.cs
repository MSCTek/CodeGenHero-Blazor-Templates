using CodeGenHero.Inflector;
using CodeGenHero.Template.Models;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.Template.Blazor6.Generators
{
    public sealed class GenericFactoryInterfaceGenerator : BaseBlazorGenerator
    {
        public GenericFactoryInterfaceGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public string Generate(
            List<NamespaceItem> usings,
            string classNamespace,
            string namespacePostfix,
            string className)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GenerateHeader(usings, classNamespace));

            sb.AppendLine($"public interface {className}<TEntity, TDto>");
            sb.AppendLine("{");

            sb.AppendLine("\tTEntity Create(TDto item);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\tTDto Create(TEntity item);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\tobject CreateDataShapedObject(object item, List<string> fieldList);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\tobject CreateDataShapedObject(TEntity item, List<string> lstOfFields);");
            sb.AppendLine(string.Empty);

            sb.Append(GenerateFooter());
            return sb.ToString();
        }
    }
}