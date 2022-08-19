using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Assistant.Core
{
    public static class ClassicUOManager
    {
        private static List<string> ProfileProperties { get; set; } = new List<string>();

        /// <summary>
        /// Gets all properties.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllProperties()
        {
            if (!Client.IsOSI)
            {
                if (ProfileProperties.Count == 0)
                {
                    var currentProfileProperty = ClassicUOClient.Assembly?.GetType("ClassicUO.Configuration.ProfileManager")?.GetProperty("CurrentProfile", BindingFlags.Public | BindingFlags.Static);
                    if (currentProfileProperty != null)
                    {
                        var profile = currentProfileProperty.GetValue(null);
                        if (profile != null)
                        {
                            var profileClass = ClassicUOClient.Assembly?.GetType("ClassicUO.Configuration.Profile");

                            foreach (var propSearch in profileClass.GetProperties())
                            {
                                ProfileProperties.Add(propSearch.Name);
                            }

                            ProfileProperties.Sort();
                        }
                    }
                }

                return ProfileProperties;
            }

            return null;
        }

        /// <summary>
        /// Determines whether [is valid property] [the specified property].
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public static string IsValidProperty(string property)
        {
            foreach (string profileProperty in GetAllProperties())
            {
                if (profileProperty.Equals(property, StringComparison.OrdinalIgnoreCase))
                {
                    return profileProperty;
                }
            }

            return null;
        }

        /// <summary>
        /// Set a bool Config property in CUO by name
        /// </summary>
        public static void ProfilePropertySet(string propertyName, bool enable)
        {
            if (!Client.IsOSI)
            {
                var currentProfileProperty = ClassicUOClient.Assembly?.GetType("ClassicUO.Configuration.ProfileManager")?.GetProperty("CurrentProfile", BindingFlags.Public | BindingFlags.Static);
                if (currentProfileProperty != null)
                {
                    var profile = currentProfileProperty.GetValue(null);
                    if (profile != null)
                    {
                        var profileClass = ClassicUOClient.Assembly?.GetType("ClassicUO.Configuration.Profile");
                        PropertyInfo property = null;

                        foreach (var propSearch in profileClass.GetProperties())
                        {
                            if (propSearch.Name == propertyName)
                            {
                                property = propSearch;
                                break;
                            }
                        }

                        if (property != null)
                        {
                            property.SetValue(profile, enable);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Set a int Config property in CUO by name
        /// </summary>
        public static void ProfilePropertySet(string propertyName, int value)
        {
            if (!Client.IsOSI)
            {
                var currentProfileProperty = ClassicUOClient.Assembly?.GetType("ClassicUO.Configuration.ProfileManager")?.GetProperty("CurrentProfile", BindingFlags.Public | BindingFlags.Static);
                if (currentProfileProperty != null)
                {
                    var profile = currentProfileProperty.GetValue(null);
                    if (profile != null)
                    {
                        var profileClass = ClassicUOClient.Assembly?.GetType("ClassicUO.Configuration.Profile");
                        PropertyInfo property = null;

                        foreach (var propSearch in profileClass.GetProperties())
                        {
                            if (propSearch.Name == propertyName)
                            {
                                property = propSearch;
                                break;
                            }
                        }

                        if (property != null)
                        {
                            property.SetValue(profile, value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Set a string Config property in CUO by name
        /// </summary>
        public static void ProfilePropertySet(string propertyName, string value)
        {
            if (!Client.IsOSI)
            {
                var currentProfileProperty = ClassicUOClient.Assembly?.GetType("ClassicUO.Configuration.ProfileManager")?.GetProperty("CurrentProfile", BindingFlags.Public | BindingFlags.Static);
                if (currentProfileProperty != null)
                {
                    var profile = currentProfileProperty.GetValue(null);
                    if (profile != null)
                    {
                        var profileClass = ClassicUOClient.Assembly?.GetType("ClassicUO.Configuration.Profile");
                        System.Reflection.PropertyInfo property = null;
                        foreach (var propSearch in profileClass.GetProperties())
                        {
                            if (propSearch.Name == propertyName)
                            {
                                property = propSearch;
                                break;
                            }
                        }
                        if (property != null)
                        {
                            property.SetValue(profile, value);
                        }
                    }
                }
            }
        }
    }
}
