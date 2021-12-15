using CodeGenHero.Inflector;
using CodeGenHero.Template.Models;
using CodeGenHero.Core.Metadata.Interfaces;
using System.Text;
using System.Linq;

namespace CodeGenHero.Template.Blazor5
{
    public abstract class BaseBlazorGenerator : BaseGenerator
    {
        public BaseBlazorGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        protected string GetMethodParameterSignature(IEntityType entity, string seperator = ", ")
        {
            var primaryKeyNames = GetPrimaryKeys(entity);
            var properties = entity.GetProperties();

            StringBuilder signatureSB = new StringBuilder();
            var pkCount = primaryKeyNames.Count();
            foreach (var primaryKey in primaryKeyNames)
            {
                var property = properties.Where(x => x.Name == primaryKey).FirstOrDefault();
                if (property == null)
                {
                    continue;
                }
                var cType = GetCType(property);
                var simpleType = ConvertToSimpleType(cType);
                var camelPK = Inflector.Camelize(primaryKey);
                signatureSB.Append($"{simpleType} {camelPK}");
                pkCount--;
                if (pkCount > 0)
                {
                    signatureSB.Append(seperator);
                }
            }

            return signatureSB.ToString();
        }

        protected string GetMethodParametersWithoutTypes(IEntityType entity, string seperator = ", ", bool camelize = true)
        {
            var primaryKeys = GetPrimaryKeys(entity);
            StringBuilder pksb = new StringBuilder();
            var pkCount = primaryKeys.Count();
            foreach (var pk in primaryKeys)
            {
                var cpk = camelize ? Inflector.Camelize(pk) : pk;
                pksb.Append(cpk);
                pkCount--;
                if (pkCount > 0)
                {
                    pksb.Append(seperator);
                }
            }
            return pksb.ToString();
        }
    }
}