namespace asp.netcore7mongodb.Entities.IOptions
{
    public class MoviesDatabaseSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string MoviesCollectionName { get; set; } = string.Empty;
    }

}
