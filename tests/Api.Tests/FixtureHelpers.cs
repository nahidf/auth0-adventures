using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Api.Tests
{
    public static class FixtureHelpers
    {
        public static Task<string> GetFixtureContentsAsync(this string fileName)
        {
            var uri = new UriBuilder(Assembly.GetExecutingAssembly().Location);
            var sourcePath = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));

            var filePath = Path.Combine(sourcePath, "sample-data", fileName);
            return Task.Factory.StartNew(() => File.ReadAllText(filePath));
        }
    }
}
