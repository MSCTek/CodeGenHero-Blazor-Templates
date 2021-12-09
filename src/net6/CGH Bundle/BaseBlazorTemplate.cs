using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Template.Models;

namespace CodeGenHero.Template.Blazor5
{
    public abstract class BaseBlazorTemplate : BaseTemplate
    {
        protected string TokenReplacements(string tokenString, IEntityType entity)
        {
            string retVal = tokenString
                .Replace("[tablename]", Inflector.Pascalize(entity.ClrType.Name))
                .Replace("[tablepluralname]", Inflector.Pluralize(entity.ClrType.Name));

            return retVal;
        }
    }
}