using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace TdsCleaner
{
    public class TdsProject
    {
        public static class Namespaces
        {
            public static string MsBuild { get { return "{http://schemas.microsoft.com/developer/msbuild/2003}"; } }
        }

        public string BaseDirectory { get; set; }

        public string BaseItemDirectory { get; set; }

        public List<string> DirectoriesToBeDeleted { get; private set; }

        public XDocument Document { get; set; }

        public List<string> FilesToBeDeleted { get; private set; }

        public IList<TdsItem> Items
        {
            get
            {
                if (_items == null)
                {
                    _items = ItemRoot.Elements(Namespaces.MsBuild + "SitecoreItem").Select(xml => new TdsItem(this, xml)).ToList();
                }

                return _items;
            }

            set
            {
                _items = value;
            }
        }

        public XElement ItemRoot
        {
            get
            {
                if (Document == null)
                {
                    throw new InvalidOperationException("Project has not been loaded.");
                }

                return Document
                    .Descendants(Namespaces.MsBuild + "ItemGroup")
                    .Single(e => e.Elements(Namespaces.MsBuild + "SitecoreItem").Any());
            }
        }

        private IList<TdsItem> _items;

        public TdsProject()
        {
            FilesToBeDeleted = new List<string>();
            DirectoriesToBeDeleted = new List<string>();
        }

        public static string Decode(string value)
        {
            var decodedValue = Regex.Replace(value, "%([0-9a-fA-F]{2})", match => ((char)int.Parse(match.Groups[1].Value, NumberStyles.AllowHexSpecifier)).ToString());
            if (Regex.IsMatch(decodedValue, "%[0-9a-fA-F]{2}"))
            {
                decodedValue = Decode(decodedValue);
            }

            return decodedValue;
        }
    }
}