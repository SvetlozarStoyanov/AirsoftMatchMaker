using AirsoftMatchMaker.Core.Contracts;
using Ganss.Xss;
using System.Reflection;

namespace AirsoftMatchMaker.Core.Services
{
    public class HtmlSanitizingService : IHtmlSanitizingService
    {
        private readonly HtmlSanitizer sanitizer;
        public HtmlSanitizingService()
        {
            sanitizer = new HtmlSanitizer();
        }

        public T SanitizeObject<T>(T obj)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties().Where(p=> p.PropertyType.Name == "String").ToArray();
            foreach (PropertyInfo property in properties)
            {
                string pName = property.Name;
                object pValue = property.GetValue(obj, null);
                if (pValue == null) 
                    continue;
                property.SetValue(obj, sanitizer.Sanitize(pValue.ToString()), null);
            }
            return obj;
        }

        public string SanitizeStringProperty(string? stringProperty)
        {
            if (stringProperty == null)
                return null;
            return sanitizer.Sanitize(stringProperty);
        }
    }
}
