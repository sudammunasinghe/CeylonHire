namespace CeylonHire.Infrastructure.Persistence.Sql.Helpers
{
    public class SqlQueryLoader : ISqlQueryLoader
    {
        public string Load(string folder, string fileName)
        {
            var basePath = AppContext.BaseDirectory;
            var path = Path.Combine(basePath, "Persistence", "Sql", folder, fileName);
            return File.ReadAllText(path);
        }
    }
}
