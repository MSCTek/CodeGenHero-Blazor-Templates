using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Inflector;
using CodeGenHero.Template.Helpers;
using CodeGenHero.Template.Models;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.Template.Blazor5.Generators
{
    public class WebApiDataServiceInterfaceGenerator : BaseBlazorGenerator
    {
        public WebApiDataServiceInterfaceGenerator(ICodeGenHeroInflector inflector) : base(inflector)
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

            sb.AppendLine($"\tpublic partial interface {className} : IWebApiDataServiceBase");
            sb.AppendLine("\t{");
            sb.AppendLine(string.Empty);

            sb.Append(GenerateGetAllPages(entities));
            sb.Append(GenerateGetPageData(entities));
            sb.Append(GenerateGetByPK(entities));
            sb.Append(GenerateCreate(entities));
            sb.Append(GenerateUpdate(entities));
            sb.Append(GenerateDelete(entities));

            sb.Append(GenerateFooter());
            return sb.ToString();
        }

        private string GenerateGetAllPages(IList<IEntityType> entities)
        {
            string methodSecondLine = "\tEnums.RelatedEntitiesType relatedEntitiesType = Enums.RelatedEntitiesType.None);";
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("#region GetAllPages");
            sb.AppendLine(string.Empty);

            foreach(var entity in entities)
            {
                string entityName = entity.ClrType.Name;
                string pluralEntityName = Inflector.Pluralize(entityName);

                sb.AppendLine($"Task<IList<xDTO.{entityName}>> GetAllPages{pluralEntityName}Async(bool? isActive = null, string sort = null,");
                sb.AppendLine(methodSecondLine);
                sb.AppendLine(string.Empty);

                sb.AppendLine($"Task<IList<xDTO.{entityName}>> GetAllPages{pluralEntityName}Async(List<IFilterCriterion> filterCriteria, string sort = null,");
                sb.AppendLine(methodSecondLine);
                sb.AppendLine(string.Empty);
            }

            sb.AppendLine("#endregion");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateGetPageData(IList<IEntityType> entities)
        {
            string methodThirdLine = "\tEnums.RelatedEntitiesType relatedEntitiesType = Enums.RelatedEntitiesType.None);";
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("#region GetPageData");
            sb.AppendLine(string.Empty);

            foreach (var entity in entities)
            {
                string entityName = entity.ClrType.Name;
                string pluralEntityName = Inflector.Pluralize(entityName);
                string methodFirstLine = $"Task<IHttpCallResultCGHT<IPageDataT<IList<xDTO.{entityName}>>>> Get{pluralEntityName}Async(";

                sb.AppendLine($"{methodFirstLine}IPageDataRequest pageDataRequest);");
                sb.AppendLine(string.Empty);

                sb.AppendLine(methodFirstLine);
                sb.AppendLine($"\tList<IFilterCriterion> filterCriteria, string sort = null, int page = 1, int pageSize = 100,");
                sb.AppendLine(methodThirdLine);
                sb.AppendLine(string.Empty);

                sb.AppendLine(methodFirstLine);
                sb.AppendLine("\tbool? isActive = null, string sort = null, int page = 1, int pageSize = 100,");
                sb.AppendLine(methodThirdLine);
                sb.AppendLine(string.Empty);
            }

            sb.AppendLine("#endregion");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateGetByPK(IList<IEntityType> entities)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("#region Get By PK");
            sb.AppendLine(string.Empty);

            foreach (var entity in entities)
            {
                string entityName = entity.ClrType.Name;
                string signature = GetMethodParameterSignature(entity);

                sb.AppendLine($"Task<IHttpCallResultCGHT<xDTO.{entityName}>> Get{entityName}Async({signature},");
                sb.AppendLine("\tEnums.RelatedEntitiesType relatedEntitiesType = Enums.RelatedEntitiesType.None);");
                sb.AppendLine(string.Empty);
            }

            sb.AppendLine("#endregion");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateCreate(IList<IEntityType> entities)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("#region Create");
            sb.AppendLine(string.Empty);

            foreach (var entity in entities)
            {
                string entityName = entity.ClrType.Name;

                sb.AppendLine($"Task<IHttpCallResultCGHT<xDTO.{entityName}>> Create{entityName}Async(xDTO.{entityName} item);");
                sb.AppendLine(string.Empty);
            }

            sb.AppendLine("#endregion");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateUpdate(IList<IEntityType> entities)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("#region Update");
            sb.AppendLine(string.Empty);

            foreach (var entity in entities)
            {
                string entityName = entity.ClrType.Name;

                sb.AppendLine($"Task<IHttpCallResultCGHT<xDTO.{entityName}>> Update{entityName}Async(xDTO.{entityName} item);");
                sb.AppendLine(string.Empty);
            }

            sb.Append("#endregion");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateDelete(IList<IEntityType> entities)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("#region Delete");
            sb.AppendLine(string.Empty);

            foreach (var entity in entities)
            {
                string entityName = entity.ClrType.Name;
                string signature = GetMethodParameterSignature(entity);

                sb.AppendLine($"Task<IHttpCallResultCGHT<xDTO.{entityName}>> Delete{entityName}Async({signature});");
                sb.AppendLine(string.Empty);
            }

            sb.AppendLine("#endregion");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }
    }
}
