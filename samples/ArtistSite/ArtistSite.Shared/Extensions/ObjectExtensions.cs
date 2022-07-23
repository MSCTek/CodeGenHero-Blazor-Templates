using System;
using System.ComponentModel;

namespace ArtistSite.Shared.Extensions
{
    public static class ObjectExtensions
    {
        // Based off of https://stackoverflow.com/questions/3502493/is-there-any-generic-parse-function-that-will-convert-a-string-to-any-type-usi
        public static T ConvertTo<T>(this object value)
        {
            if (value is T variable) return variable;

            try
            {
                // Handling Nullable types i.e, int?, double?, bool? .. etc
                if (Nullable.GetUnderlyingType(typeof(T)) != null || typeof(T).FullName.Contains("System.Guid"))
                {
                    return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(value);
                }

                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception ex)
            {
                throw ex; // "Does this mask an error when it shouldn't? Would it be best to let this fail?"
            }
        }
    }
}
