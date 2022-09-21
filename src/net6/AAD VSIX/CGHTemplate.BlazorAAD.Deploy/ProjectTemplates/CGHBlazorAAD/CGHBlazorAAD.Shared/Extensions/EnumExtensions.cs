using System.ComponentModel;
using System.Reflection;

namespace $safeprojectname$.Extensions
{
    public static class EnumExtensions
    {

        // https://stackoverflow.com/questions/1415140/can-my-enums-have-friendly-names
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }

        public static IEnumerable<Enum> ConvertToEnumerable(this Enum value)
        {
            Type enumType = value.GetType();
            return (IEnumerable<Enum>)Enum.GetValues(enumType);
        }

    }
}
