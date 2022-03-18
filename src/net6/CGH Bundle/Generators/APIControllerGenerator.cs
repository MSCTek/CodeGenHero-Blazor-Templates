using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Inflector;
using CodeGenHero.Template.Helpers;
using CodeGenHero.Template.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenHero.Template.Blazor6.Generators
{
    public class APIControllerGenerator : BaseBlazorGenerator
    {
        public APIControllerGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public string Generate(
            List<NamespaceItem> usings,
            string classNamespace,
            string namespacePostfix,
            IEntityType entity,
            List<NameValue> maxRequestPerPageOverrides,
            string className,
            string baseAPIControllerClassName,
            string repositoryInterfaceClassName,
            string genericFactoryInterfaceClassName)
        {
            var entityName = $"{entity.ClrType.Name}";

            // Set max page size.
            var maxPageSize = GetMaxPageSize(entity, maxRequestPerPageOverrides);

            // Begin Generation.
            StringBuilder sb = new StringBuilder();

            sb.Append(GenerateHeader(usings, classNamespace));

            sb.AppendLine($"\tpublic partial class {className} : {baseAPIControllerClassName}");
            sb.AppendLine("\t{");

            sb.AppendLine($"\t\tprivate const string GET_LIST_ROUTE_NAME = \"{entityName}{namespacePostfix}List\";");
            sb.AppendLine($"\t\tprivate {genericFactoryInterfaceClassName}<ent{namespacePostfix}.{entityName}, dto{namespacePostfix}.{entityName}> _factory;");
            sb.AppendLine($"\t\tprivate int MaxPageSize {{ get; set; }} = {maxPageSize};");
            sb.AppendLine(string.Empty);

            sb.Append(GenerateConstructor(className, namespacePostfix, entityName, repositoryInterfaceClassName, genericFactoryInterfaceClassName));

            sb.Append(GenerateDelete(entity, entityName));
            sb.Append(GenerateGet(entity, namespacePostfix, entityName));
            sb.Append(GeneratePatch(entity, namespacePostfix, entityName));
            sb.Append(GeneratePost(entity, namespacePostfix, entityName));
            sb.Append(GeneratePut(entity, namespacePostfix, entityName));

            sb.Append(GeneratePartialMethodSignatures(entity, namespacePostfix, entityName));

            sb.Append(GenerateFooter());

            return sb.ToString();
        }

        private string GenerateConstructor(string className, string namespacePostfix, string entityName, string repositoryInterfaceClassName, string genericFactoryInterfaceClassName)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public {className}(ILogger<{className}> logger,");
            sb.AppendLine("\tIServiceProvider serviceProvider,");
            sb.AppendLine("\tIHttpContextAccessor httpContextAccessor,");
            sb.AppendLine("\tLinkGenerator linkGenerator,");
            sb.AppendLine($"\t{repositoryInterfaceClassName} repository,");
            sb.AppendLine($"\t{genericFactoryInterfaceClassName}<ent{namespacePostfix}.{entityName}, dto{namespacePostfix}.{entityName}> factory)");
            sb.AppendLine($"\t: base(logger, serviceProvider, httpContextAccessor, linkGenerator, repository)");
            sb.AppendLine("{");
            sb.AppendLine("\t_factory = factory;");
            sb.AppendLine("\tRunCustomLogicAfterCtor();");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateDelete(IEntityType entity, string entityName)
        {
            var routeSignature = GetMethodParametersWithoutTypes(entity, "}/{");
            var methodSignature = GetMethodParametersWithoutTypes(entity);
            var methodSignatureWithType = GetMethodParameterSignature(entity);
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"[HttpDelete(\"{{{routeSignature}}}\")]");
            sb.AppendLine("[VersionedActionConstraint(allowedVersion: 1, order: 100)]");
            sb.AppendLine($"public async Task<IActionResult> Delete({methodSignatureWithType})");
            sb.AppendLine("{");

            sb.AppendLine("\ttry");
            sb.AppendLine("\t{");

            sb.AppendLine("\t\tif (!base.OnActionExecuting(out int httpStatusCode, out string message))");
            sb.AppendLine("\t\t\treturn StatusCode(httpStatusCode, message);");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"\t\tvar result = await Repo.Delete_{entityName}Async({methodSignature});");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\tif (result.Status == RepositoryActionStatus.Deleted)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\treturn NoContent();");
            sb.AppendLine("\t\t}");
            sb.AppendLine("\t\telse if (result.Status == RepositoryActionStatus.NotFound)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\treturn PrepareNotFoundResponse();");
            sb.AppendLine("\t\t}");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\treturn PrepareExpectationFailedResponse(result.Exception);");

            sb.AppendLine("\t}");
            sb.AppendLine("\tcatch (Exception ex)");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\treturn PrepareInternalServerErrorResponse(ex);");
            sb.AppendLine("\t}");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        private string GenerateGet(IEntityType entity, string namespacePostfix, string entityName)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.Append(GenerateGetPages(namespacePostfix, entityName));
            sb.Append(GenerateGetByPK(entity, namespacePostfix, entityName));

            return sb.ToString();
        }

        #region Get Method Generators

        private string GenerateGetByPK(IEntityType entity, string namespacePostfix, string entityName)
        {
            var routeSignature = GetMethodParametersWithoutTypes(entity, "}/{");
            var methodSignature = GetMethodParametersWithoutTypes(entity);
            var methodSignatureWithType = GetMethodParameterSignature(entity);

            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"[HttpGet(template: \"ById/{{{routeSignature}}}/{{relatedEntitiesType:relatedEntitiesType=None}}\")]");
            sb.AppendLine("[VersionedActionConstraint(allowedVersion: 1, order: 100)]");
            sb.AppendLine($"public async Task<IActionResult> Get({methodSignatureWithType}, Enums.RelatedEntitiesType relatedEntitiesType)");
            sb.AppendLine("{");

            sb.AppendLine("\ttry");
            sb.AppendLine("\t{");

            sb.AppendLine("\t\tif (!base.OnActionExecuting(out int httpStatusCode, out string message))");
            sb.AppendLine("\t\t\treturn StatusCode(httpStatusCode, message);");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"\t\tvar dbItem = await Repo.Get_{entityName}Async({methodSignature}, relatedEntitiesType);");
            sb.AppendLine($"\t\tif (dbItem == null)");
            sb.AppendLine("\t\t\treturn PrepareNotFoundResponse();");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"\t\tRunCustomLogicOnGetEntityByPK(ref dbItem, {methodSignature}, relatedEntitiesType);");
            sb.AppendLine("\t\treturn Ok(_factory.Create(dbItem));");

            sb.AppendLine("\t}");
            sb.AppendLine("\tcatch (Exception ex)");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\treturn PrepareInternalServerErrorResponse(ex);");
            sb.AppendLine("\t}");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        private string GenerateGetPages(string namespacePostfix, string entityName)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("[HttpGet(template: \"{relatedEntitiesType:relatedEntitiesType=None}\", Name = GET_LIST_ROUTE_NAME)]");
            sb.AppendLine("[VersionedActionConstraint(allowedVersion: 1, order: 100)]");
            sb.AppendLine("public async Task<IActionResult> Get(Enums.RelatedEntitiesType relatedEntitiesType,");
            sb.AppendLine("\tstring sort = null, string fields = null, string filter = null,");
            sb.AppendLine("\tint page = 1, int pageSize = 100)");
            sb.AppendLine("{");

            sb.AppendLine("\ttry");
            sb.AppendLine("\t{");

            sb.AppendLine("\t\tif (!base.OnActionExecuting(out int httpStatusCode, out string message))");
            sb.AppendLine("\t\t\treturn StatusCode(httpStatusCode, message);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\tvar fieldList = GetListByDelimiter(fields);");
            sb.AppendLine("\t\tvar filterList = GetListByDelimiter(filter);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\tif (pageSize > MaxPageSize)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tpageSize = MaxPageSize;");
            sb.AppendLine("\t\t}");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\tvar request = new RepositoryPageDataRequest(fieldList: fieldList,");
            sb.AppendLine("\t\t\tfilterList: filterList, page: page, pageSize: pageSize, sort: sort,");
            sb.AppendLine("\t\t\trelatedEntitiesType: relatedEntitiesType);");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"\t\tRepositoryPageDataResponse<IList<ent{namespacePostfix}.{entityName}>> response =");
            sb.AppendLine($"\t\t\tawait Repo.GetPageData_{entityName}Async(request);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\tPageData paginationHeader = BuildPaginationHeader(nameof(Get), page: page,");
            sb.AppendLine("\t\t\ttotalCount: response.TotalCount, pageSize: response.PageSize, sort: response.Sort);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\tHttpContextAccessor.HttpContext.Response.Headers.Add(\"Access-Control-Expose-Headers\", \"X-Pagination\");");
            sb.AppendLine("\t\tHttpContextAccessor.HttpContext.Response.Headers.Add(\"X-Pagination\", Newtonsoft.Json.JsonConvert.SerializeObject(paginationHeader));");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\tvar retVal = response.Data.Select(x => _factory.CreateDataShapedObject(x, fieldList));");
            sb.AppendLine("\t\treturn Ok(retVal);");

            sb.AppendLine("\t}");
            sb.AppendLine("\tcatch (Exception ex)");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\treturn PrepareInternalServerErrorResponse(ex);");
            sb.AppendLine("\t}");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        #endregion Get Method Generators

        private string GeneratePartialMethodSignatures(IEntityType entity, string namespacePostfix, string entityName)
        {
            var entitySignature = GetMethodParameterSignature(entity);

            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"partial void RunCustomLogicAfterInsert(ref ent{namespacePostfix}.{entityName} newDBItem, ref IRepositoryActionResult<ent{namespacePostfix}.{entityName}> result);");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"partial void RunCustomLogicAfterUpdatePatch(ref ent{namespacePostfix}.{entityName} updatedDBItem, ref IRepositoryActionResult<ent{namespacePostfix}.{entityName}> result);");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"partial void RunCustomLogicAfterUpdatePut(ref ent{namespacePostfix}.{entityName} updatedDBItem, ref IRepositoryActionResult<ent{namespacePostfix}.{entityName}> result);");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"partial void RunCustomLogicBeforeUpdatePut(ref ent{namespacePostfix}.{entityName} updatedDBItem, {entitySignature});");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"partial void RunCustomLogicOnGetEntityByPK(ref ent{namespacePostfix}.{entityName} dbItem, {entitySignature}, Enums.RelatedEntitiesType relatedEntitiesType);");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GeneratePatch(IEntityType entity, string namespacePostfix, string entityName)
        {
            var routeSignature = GetMethodParametersWithoutTypes(entity, "}/{");
            var methodSignature = GetMethodParametersWithoutTypes(entity);
            var methodSignatureWithType = GetMethodParameterSignature(entity);
            var primaryKeys = GetPrimaryKeys(entity);

            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"[HttpPatch(\"{{{routeSignature}}}\")]");
            sb.AppendLine("[VersionedActionConstraint(allowedVersion: 1, order: 100)]");
            sb.AppendLine($"public async Task<IActionResult> Patch({methodSignatureWithType}, [FromBody] JsonPatchDocument<dto{namespacePostfix}.{entityName}> patchDocument)");
            sb.AppendLine("{");

            sb.AppendLine("\ttry");
            sb.AppendLine("\t{");

            sb.AppendLine("\t\tif (!base.OnActionExecuting(out int httpStatusCode, out string message))");
            sb.AppendLine("\t\treturn StatusCode(httpStatusCode, message);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\tif (patchDocument == null)");
            sb.AppendLine("\t\t\treturn BadRequest();");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"\t\tvar dbItem = await Repo.Get_{entityName}Async({methodSignature});");
            sb.AppendLine("\t\tif (dbItem == null)");
            sb.AppendLine("\t\t\treturn NotFound();");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\tvar dtoItem = _factory.Create(dbItem); // Map the DB Entity to a DTO.");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\t// Apply changes to the DTO");
            sb.AppendLine("\t\tpatchDocument.ApplyTo(dtoItem);");
            foreach(var pk in primaryKeys)
            {
                var ppk = Inflector.Pascalize(pk);
                var cpk = Inflector.Camelize(pk);
                sb.AppendLine($"\t\tdtoItem.{ppk} = {cpk};");
            }
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\t// Map the DTO with applied changes back to the DB Entity and perform the update.");
            sb.AppendLine("\t\tvar updatedDBItem = _factory.Create(dtoItem); // Map the DTO to a DB Entity.");
            sb.AppendLine("\t\tvar result = await Repo.UpdateAsync(updatedDBItem);");
            sb.AppendLine("\t\tRunCustomLogicAfterUpdatePatch(ref updatedDBItem, ref result);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\tif (result.Status == RepositoryActionStatus.Updated)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tvar patchedDTOItem = _factory.Create(result.Entity); // Map the updated DB Entity to a DTO.");
            sb.AppendLine("\t\t\treturn Ok(patchedDTOItem);");
            sb.AppendLine("\t\t}");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\treturn PrepareExpectationFailedResponse(result.Exception);");

            sb.AppendLine("\t}");
            sb.AppendLine("\tcatch (Exception ex)");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\treturn PrepareInternalServerErrorResponse(ex);");
            sb.AppendLine("\t}");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        private string GeneratePost(IEntityType entity, string namespacePostfix, string entityName)
        {
            var primaryKeys = GetPrimaryKeys(entity);
            StringBuilder uriValueSB = new StringBuilder();
            var pkCount = primaryKeys.Count();
            foreach(var pk in primaryKeys)
            {
                uriValueSB.Append($"newDTOItem.{pk}");
                pkCount--;
                if (pkCount > 0)
                {
                    uriValueSB.Append(", ");
                }
            }
            string uriValue = uriValueSB.ToString();

            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("[HttpPost]");
            sb.AppendLine("[VersionedActionConstraint(allowedVersion: 1, order: 100)]");
            sb.AppendLine($"public async Task<IActionResult> Post([FromBody] dto{namespacePostfix}.{entityName} dtoItem)");
            sb.AppendLine("{");

            sb.AppendLine("\ttry");
            sb.AppendLine("\t{");

            sb.AppendLine("\t\tif (!base.OnActionExecuting(out int httpStatusCode, out string message))");
            sb.AppendLine("\t\t\treturn StatusCode(httpStatusCode, message);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\tif (dtoItem == null)");
            sb.AppendLine("\t\t\treturn BadRequest();");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\tvar newDBItem = _factory.Create(dtoItem); // Map incoming DTO to DB Entity");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\tvar result = await Repo.InsertAsync(newDBItem);");
            sb.AppendLine("\t\tRunCustomLogicAfterInsert(ref newDBItem, ref result);");

            sb.AppendLine("\t\tif (result.Status == RepositoryActionStatus.Created)");
            sb.AppendLine("\t\t{");

            sb.AppendLine("\t\t\tvar newDTOItem = _factory.Create(result.Entity); // Map created DB Entity to a DTO");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\t\tvar uriFormatted = LinkGenerator.GetUriByAction(");
            sb.AppendLine("\t\t\t\thttpContext: HttpContextAccessor.HttpContext,");
            sb.AppendLine("\t\t\t\taction: nameof(Get),");
            sb.AppendLine("\t\t\t\tcontroller: null, // Stay in this controller");
            sb.AppendLine($"\t\t\t\tvalues: new {{ {uriValue} }}");
            sb.AppendLine("\t\t\t\t);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\t\treturn Created(uriFormatted, newDTOItem);");

            sb.AppendLine("\t\t}");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\treturn PrepareExpectationFailedResponse(result.Exception);");

            sb.AppendLine("\t}");
            sb.AppendLine("\tcatch (Exception ex)");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\treturn PrepareInternalServerErrorResponse(ex);");
            sb.AppendLine("\t}");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        private string GeneratePut(IEntityType entity, string namespacePostfix, string entityName)
        {
            var routeSignature = GetMethodParametersWithoutTypes(entity, "}/{");
            var methodSignature = GetMethodParametersWithoutTypes(entity);
            var methodSignatureWithType = GetMethodParameterSignature(entity);
            var primaryKeys = GetPrimaryKeys(entity);

            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"[HttpPut(\"{{{routeSignature}}}\")]");
            sb.AppendLine($"[VersionedActionConstraint(allowedVersion: 1, order: 100)]");
            sb.AppendLine($"public async Task<IActionResult> Put({methodSignatureWithType}, [FromBody] dto{namespacePostfix}.{entityName} dtoItem)");
            sb.AppendLine("{");

            sb.AppendLine("\ttry");
            sb.AppendLine("\t{");

            sb.AppendLine("\t\tif (!base.OnActionExecuting(out int httpStatusCode, out string message))");
            sb.AppendLine("\t\t\treturn StatusCode(httpStatusCode, message);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\tif (dtoItem == null)");
            sb.AppendLine("\t\t\treturn BadRequest();");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\t// ensure we update the record matching the parameter");
            foreach(var pk in primaryKeys)
            {
                var ppk = Inflector.Pascalize(pk);
                var cpk = Inflector.Camelize(pk);
                sb.AppendLine($"\t\tdtoItem.{ppk} = {cpk};");
            }
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\tvar updatedDBItem = _factory.Create(dtoItem); // Map the incoming DTO to a DB entity.");
            sb.AppendLine($"\t\tRunCustomLogicBeforeUpdatePut(ref updatedDBItem, {methodSignature});");
            sb.AppendLine("\t\tvar result = await Repo.UpdateAsync(updatedDBItem);");
            sb.AppendLine("\t\tRunCustomLogicAfterUpdatePut(ref updatedDBItem, ref result);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\tif (result.Status == RepositoryActionStatus.Updated)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tvar updatedDTOItem = _factory.Create(result.Entity); // Map the updated DB Entity to a DTO");
            sb.AppendLine("\t\t\treturn Ok(updatedDTOItem);");
            sb.AppendLine("\t\t}");
            sb.AppendLine("\t\telse if (result.Status == RepositoryActionStatus.NotFound)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\treturn NotFound();");
            sb.AppendLine("\t\t}");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\treturn PrepareExpectationFailedResponse(result.Exception);");

            sb.AppendLine("\t}");
            sb.AppendLine("\tcatch (Exception ex)");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\treturn PrepareInternalServerErrorResponse(ex);");
            sb.AppendLine("\t}");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        private int GetMaxPageSize(IEntityType entity, List<NameValue> maxRequestPerPageOverrides)
        {
            int retVal = -1;
            var maxRequestPerPageOverRideString = maxRequestPerPageOverrides
                .FirstOrDefault(x => x.Name == entity.ClrType.Name)?.Value;
            if (!string.IsNullOrWhiteSpace(maxRequestPerPageOverRideString))
            {
                int.TryParse(maxRequestPerPageOverRideString, out retVal);
            }

            if (retVal <= 0)
            {
                retVal = 100;
            }

            return retVal;
        }
    }
}