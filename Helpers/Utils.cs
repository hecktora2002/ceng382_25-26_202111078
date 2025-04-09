using System.Text.Json;

namespace MyApp.Helpers
{
    public sealed class Utils
    {
        private static readonly Lazy<Utils> lazy = new Lazy<Utils>(() => new Utils());

        public static Utils Instance { get { return lazy.Value; } }

        private Utils() { }

        public string ExportToJson<T>(IEnumerable<T> data, List<string>? selectedProperties = null)
        {
            if (selectedProperties == null || !selectedProperties.Any())
            {
                return JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            }

            var filteredData = data.Select(item =>
            {
                var dict = new Dictionary<string, object?>();
                var props = typeof(T).GetProperties();
                foreach (var prop in props)
                {
                    if (selectedProperties.Contains(prop.Name))
                    {
                        dict[prop.Name] = prop.GetValue(item);
                    }
                }
                return dict;
            });

            return JsonSerializer.Serialize(filteredData, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
