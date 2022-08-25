#region license
// Razor: An Ultima Online Assistant
// Copyright (c) 2022 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
#endregion

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
