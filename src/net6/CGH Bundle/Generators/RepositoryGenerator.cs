using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Inflector;
using CodeGenHero.Template.Helpers;
using CodeGenHero.Template.Models;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CodeGenHero.Template.Blazor6.Generators
{
    public class RepositoryGenerator : BaseBlazorGenerator
    {
        public RepositoryGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public string Generate(
            List<NamespaceItem> usings,
            string classNamespace,
            string namespacePostfix,
            IList<IEntityType> entities,
            string className,
            string repositoryInterfaceClassName,
            string dbContextName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GenerateHeader(usings, classNamespace));

            sb.AppendLine($"\tpublic partial class {className} : {repositoryInterfaceClassName}");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tprivate {dbContextName} _ctx;");
            sb.Append(GenerateConstructor(className, dbContextName));

            sb.AppendLine($"\t\tpublic {dbContextName} {dbContextName} {{ get {{ return _ctx; }} }}");
            sb.AppendLine(string.Empty);

            sb.Append(GenerateGenericOperations());

            foreach (var entity in entities)
            {
                string entityName = entity.ClrType.Name;
                string tableNamePlural = Inflector.Pluralize(entity.ClrType.Name);
                var primaryKeys = GetPrimaryKeys(entity);

                StringBuilder whereClauseSB = new StringBuilder();
                StringBuilder whereClauseWithObjectSB = new StringBuilder();
                var clauseStarter = "x => ";
                var clauseDivider = " && ";
                whereClauseSB.Append(clauseStarter);
                whereClauseWithObjectSB.Append(clauseStarter);
                var pkCount = primaryKeys.Count();
                foreach (var pk in primaryKeys)
                {
                    var cpk = Inflector.Camelize(pk);
                    whereClauseSB.Append($"x.{pk} == {cpk}");
                    whereClauseWithObjectSB.Append($"x.{pk} == item.{pk}");
                    pkCount--;
                    if (pkCount > 0)
                    {
                        whereClauseSB.Append(clauseDivider);
                        whereClauseWithObjectSB.Append(clauseDivider);
                    }
                }
                string whereClause = whereClauseSB.ToString();
                string whereClauseWithObjectInstancePrefix = whereClauseWithObjectSB.ToString();

                string methodParameterSignature = GetMethodParameterSignature(entity);
                string methodParameterSignatureWithoutFieldTypes = GetMethodParametersWithoutTypes(entity);

                sb.AppendLine($"\t\t#region {entityName}");
                sb.AppendLine(string.Empty);

                sb.Append(GenerateInsertOperation(entityName));
                sb.Append(GenerateGetQueryable(entityName));
                sb.Append(GenerateGetPageData(entityName));
                sb.Append(GenerateGetFirstOrDefault(entityName, methodParameterSignature, methodParameterSignatureWithoutFieldTypes, whereClause, whereClauseWithObjectInstancePrefix));
                sb.Append(GenerateUpdateOperation(entityName, tableNamePlural, whereClauseWithObjectInstancePrefix));
                sb.Append(GenerateDeleteOperations(entityName, tableNamePlural, whereClause, whereClauseWithObjectInstancePrefix, methodParameterSignature));
                sb.Append(GeneratePartialMethodSignatures(entityName, methodParameterSignature));

                sb.AppendLine("\t\t#endregion");
                sb.AppendLine(string.Empty);
            }

            sb.Append(GenerateFooter());
            return sb.ToString();
        }

        private string GenerateConstructor(string className, string dbContextName)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public {className}({dbContextName} ctx)");
            sb.AppendLine("{");

            sb.AppendLine("\t_ctx = ctx;");
            sb.AppendLine("\tctx.ChangeTracker.LazyLoadingEnabled = false;");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        private string GenerateDeleteOperations(string entityName, string tableNamePlural,
            string whereClause, string whereClauseWithObjectInstancePrefix, string methodParameterSignature)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.Append(GenerateDeleteByPrimaryKey(entityName, tableNamePlural, whereClause, methodParameterSignature));
            sb.Append(GenerateDeleteByObject(entityName, tableNamePlural, whereClauseWithObjectInstancePrefix));

            return sb.ToString();
        }

        private string GenerateGenericOperations()
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);
            sb.AppendLine("#region Generic Operations");

            sb.Append(GenerateGenericDelete());
            sb.Append(GenerateGenericGet());
            sb.Append(GenerateGenericInsert());
            sb.Append(GenerateGenericUpdate());
            sb.Append(GenerateGenericPartialMethods());

            sb.AppendLine("#endregion");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        #region Generic Operations Generators

        private string GenerateGenericDelete()
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            // Using a Verbatim String as this is a large block of static code.
            string verbatim = @"
                private async Task<IRepositoryActionResult<TEntity>> DeleteAsync<TEntity>(TEntity item) where TEntity : class
		        {
			        IRepositoryActionResult<TEntity> retVal = null;

			        try
			        {
				        if (item == null)
				        {
					        retVal = new RepositoryActionResult<TEntity>(null, cghEnums.RepositoryActionStatus.NotFound);
				        }
				        else
				        {
					        DbSet<TEntity> itemSet = _ctx.Set<TEntity>();
					        itemSet.Remove(item);
					        await _ctx.SaveChangesAsync();
					        retVal = new RepositoryActionResult<TEntity>(null, cghEnums.RepositoryActionStatus.Deleted);
				        }
			        }
			        catch (Exception ex)
			        {
				        retVal = new RepositoryActionResult<TEntity>(null, cghEnums.RepositoryActionStatus.Error, ex);
			        }

			        return retVal;
		        }";

            sb.Append(verbatim);
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateGenericGet()
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("public IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class");
            sb.AppendLine("{");

            sb.AppendLine("\treturn _ctx.Set<TEntity>();");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        private string GenerateGenericInsert()
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            string verbatim = @"
                public async Task<IRepositoryActionResult<TEntity>> InsertAsync<TEntity>(TEntity item)
			        where TEntity : class
		        {
			        IRepositoryActionResult<TEntity> retVal = null;

			        try
			        {
				        DbSet<TEntity> itemSet = _ctx.Set<TEntity>();
				        itemSet.Add(item);
				        var result = await _ctx.SaveChangesAsync();
				        RunCustomLogicAfterEveryInsert<TEntity>(item, result);

				        if (result > 0)
				        {
					        retVal = new RepositoryActionResult<TEntity>(item, cghEnums.RepositoryActionStatus.Created);
				        }
				        else
				        {
					        retVal = new RepositoryActionResult<TEntity>(item, cghEnums.RepositoryActionStatus.NothingModified, null);
				        }
			        }
			        catch (Exception ex)
			        {
				        retVal = new RepositoryActionResult<TEntity>(null, cghEnums.RepositoryActionStatus.Error, ex);
			        }

			        return retVal;
		        }";

            sb.Append(verbatim);
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateGenericPartialMethods()
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("partial void RunCustomLogicAfterEveryInsert<T>(T item, int numObjectsWritten) where T : class;");
            sb.AppendLine(string.Empty);

            sb.AppendLine("partial void RunCustomLogicAfterEveryUpdate<T>(T newItem, T oldItem, int numObjectsWritten) where T : class;");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateGenericUpdate()
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            string verbatim = @"
                private async Task<IRepositoryActionResult<TEntity>> UpdateAsync<TEntity>(TEntity item, TEntity existingItem) where TEntity : class
		        {
			        IRepositoryActionResult<TEntity> retVal = null;

			        try
			        { // only update when a record already exists for this id
				        if (existingItem == null)
				        {
					        retVal = new RepositoryActionResult<TEntity>(item, cghEnums.RepositoryActionStatus.NotFound);
				        }

				        // change the original entity status to detached; otherwise, we get an error on attach as the entity is already in the dbSet
				        // set original entity state to detached
				        _ctx.Entry(existingItem).State = EntityState.Detached;
				        DbSet<TEntity> itemSet = _ctx.Set<TEntity>();
				        itemSet.Attach(item); // attach & save
				        _ctx.Entry(item).State = EntityState.Modified; // set the updated entity state to modified, so it gets updated.

				        var result = await _ctx.SaveChangesAsync();
				        RunCustomLogicAfterEveryUpdate<TEntity>(newItem: item, oldItem: existingItem, numObjectsWritten: result);

				        if (result > 0)
				        {
					        retVal = new RepositoryActionResult<TEntity>(item, cghEnums.RepositoryActionStatus.Updated);
				        }
				        else
				        {
					        retVal = new RepositoryActionResult<TEntity>(item, cghEnums.RepositoryActionStatus.NothingModified, null);
				        }
			        }
			        catch (Exception ex)
			        {
				        retVal = new RepositoryActionResult<TEntity>(null, cghEnums.RepositoryActionStatus.Error, ex);
			        }

			        return retVal;
		        }";

            sb.Append(verbatim);
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        #endregion Generic Operations Generators

        private string GenerateGetFirstOrDefault(string tableName,
            string methodParameterSignature,
            string methodParameterSignatureWithoutFieldTypes,
            string whereClause,
            string whereClauseWithObjectInstancePrefix)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.Append(GenerateGetFirstOrDefaultByPrimaryKey(tableName, methodParameterSignature, methodParameterSignatureWithoutFieldTypes, whereClause));
            sb.Append(GenerateGetFirstOrDefaultByObject(tableName, whereClauseWithObjectInstancePrefix));

            return sb.ToString();
        }

        private string GenerateGetPageData(string entityName)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public async Task<RepositoryPageDataResponse<IList<{entityName}>>> GetPageData_{entityName}Async(RepositoryPageDataRequest request)");
            sb.AppendLine("{");

            sb.AppendLine($"\tvar qry = GetQueryable_{entityName}(request.RelatedEntitiesType).AsNoTracking();");
            sb.AppendLine($"\tvar retVal = new RepositoryPageDataResponse<IList<{entityName}>>(request);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\tIList<string> filterList = new List<string>(request.FilterList);");
            sb.AppendLine($"\tRunCustomLogicAfterGetQueryableList_{entityName}(ref qry, ref filterList);");
            sb.AppendLine("\tqry = qry.ApplyFilter(filterList);");
            sb.AppendLine($"\tqry = qry.ApplySort(request.Sort ?? (typeof({entityName}).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))");
            sb.AppendLine("\t\t.First().Name);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\tretVal.TotalCount = qry.Count();");
            sb.AppendLine("\tretVal.Data = await qry.Skip(request.PageSize * (request.Page - 1))");
            sb.AppendLine("\t\t.Take(request.PageSize).ToListAsync();");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\treturn retVal;");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateGetQueryable(string entityName)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public IQueryable<{entityName}> GetQueryable_{entityName}(Enums.RelatedEntitiesType relatedEntitiesType)");
            sb.AppendLine("{");
            sb.AppendLine($"\tvar retVal = GetQueryable<{entityName}>();");
            sb.AppendLine($"ApplyRelatedEntitiesType(ref retVal, relatedEntitiesType);");
            sb.AppendLine(string.Empty);
            sb.AppendLine("return retVal;");
            sb.AppendLine("}");

            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        private string GenerateInsertOperation(string entityName)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public async Task<IRepositoryActionResult<{entityName}>> InsertAsync({entityName} item)");
            sb.AppendLine("{");
            sb.AppendLine($"\tvar result = await InsertAsync<{entityName}>(item);");
            sb.AppendLine($"\tRunCustomLogicAfterInsert_{entityName}(item, result);");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"\treturn result;");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        #region Get First or Default Generators

        private string GenerateGetFirstOrDefaultByObject(string tableName, string whereClauseWithObjectInstancePrefix)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public async Task<{tableName}> GetFirstOrDefaultAsync(");
            sb.AppendLine($"\t{tableName} item, Enums.RelatedEntitiesType relatedEntitiesType)");
            sb.AppendLine("{");

            sb.AppendLine($"\tvar qry = GetQueryable_{tableName}(relatedEntitiesType)");
            sb.AppendLine($"\t\t.Where({whereClauseWithObjectInstancePrefix});");
            sb.AppendLine("\tvar retVal = await qry.FirstOrDefaultAsync();");
            sb.AppendLine(string.Empty);

            sb.AppendLine("return retVal;");

            //sb.AppendLine($"return await _ctx.{tableNamePlural}.Where({whereClauseWithObjectInstancePrefix}).FirstOrDefaultAsync();");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        private string GenerateGetFirstOrDefaultByPrimaryKey(string tableName, string methodParameterSignature, string methodParameterSignatureWithoutFieldTypes, string whereClause)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public async Task<{tableName}> Get_{tableName}Async({methodParameterSignature}, Enums.RelatedEntitiesType relatedEntitiesType)");
            sb.AppendLine("{");

            sb.AppendLine($"\tvar qry = GetQueryable_{tableName}(relatedEntitiesType);");
            sb.AppendLine($"\tRunCustomLogicOnGetQueryableByPK_{tableName}(ref qry, {methodParameterSignatureWithoutFieldTypes}, relatedEntitiesType);");
            sb.AppendLine("\tqry = qry.AsNoTracking();");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\tvar dbItem = await qry");
            sb.AppendLine($"\t\t.Where({whereClause})");
            sb.AppendLine($"\t\t.FirstOrDefaultAsync();");
            sb.AppendLine("\tif (!(dbItem is null))");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tRunCustomLogicOnGetEntityByPK_{tableName}(ref dbItem, {methodParameterSignatureWithoutFieldTypes}, relatedEntitiesType);");
            sb.AppendLine("\t}");
            sb.AppendLine(string.Empty);

            sb.AppendLine("return dbItem;");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        #endregion Get First or Default Generators

        private string GeneratePartialMethodSignatures(string tableName, string methodParameterSignature)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"partial void RunCustomLogicAfterGetQueryableList_{tableName}(");
            sb.AppendLine($"\tref IQueryable<{tableName}> dbItems,");
            sb.AppendLine("\tref IList<string> filterList);");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"partial void RunCustomLogicAfterInsert_{tableName}({tableName} item, IRepositoryActionResult<{tableName}> result);");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"partial void RunCustomLogicAfterUpdate_{tableName}({tableName} newItem, {tableName} oldItem, IRepositoryActionResult<{tableName}> result);");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"partial void RunCustomLogicOnGetQueryableByPK_{tableName}(ref IQueryable<{tableName}> qryItem, {methodParameterSignature}, Enums.RelatedEntitiesType relatedEntitiesType);");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"partial void RunCustomLogicOnGetEntityByPK_{tableName}(ref {tableName} dbItem, {methodParameterSignature}, Enums.RelatedEntitiesType relatedEntitiesType);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("partial void ApplyRelatedEntitiesType(");
            sb.AppendLine($"\tref IQueryable<{tableName}> qry, Enums.RelatedEntitiesType relatedEntitiesType);");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateUpdateOperation(string entityName, string tableNamePlural, string whereClauseWithObjectInstancePrefix)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public async Task<IRepositoryActionResult<{entityName}>> UpdateAsync({entityName} item)");
            sb.AppendLine("{");

            sb.AppendLine($"var oldItem = await _ctx.{tableNamePlural}");
            sb.AppendLine($"\t.FirstOrDefaultAsync({whereClauseWithObjectInstancePrefix});");
            sb.AppendLine($"var result = await UpdateAsync<{entityName}>(item, oldItem);");
            sb.AppendLine($"RunCustomLogicAfterUpdate_{entityName}(newItem: item, oldItem: oldItem, result: result);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("return result;");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        #region Delete Operation Generators

        private string GenerateDeleteByObject(string entityName, string tableNamePlural, string whereClauseWithObjectInstancePrefix)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public async Task<IRepositoryActionResult<{entityName}>> DeleteAsync({entityName} item)");
            sb.AppendLine("{");

            sb.AppendLine($"\treturn await DeleteAsync<{entityName}>(_ctx.{tableNamePlural}");
            sb.AppendLine($"\t\t.Where({whereClauseWithObjectInstancePrefix})");
            sb.AppendLine($"\t\t.FirstOrDefault());");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        private string GenerateDeleteByPrimaryKey(string entityName, string tableNamePlural, string whereClause, string methodParameterSignature)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public async Task<IRepositoryActionResult<{entityName}>> Delete_{entityName}Async({methodParameterSignature})");
            sb.AppendLine("{");

            sb.AppendLine($"\treturn await DeleteAsync<{entityName}>(_ctx.{tableNamePlural}");
            sb.AppendLine($"\t\t.Where({whereClause})");
            sb.AppendLine($"\t\t.FirstOrDefault());");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        #endregion Delete Operation Generators
    }
}