namespace HourglassServer.Data.Application.MetadataModel
{
    public class MetadataApplicationModel
    {
        public string TableName { get; set; }
        public string[] ColumnNames { get; set; }
        public string[] DataTypes { get; set; }
        public string GeoType { get; set; }
    }
}