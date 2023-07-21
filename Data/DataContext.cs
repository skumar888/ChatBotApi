namespace HackChatbotApi.Data;

using Dapper;
using Npgsql;
using System.Data;

public interface IDataContext
{
    NpgsqlConnection CreateConnection();
    Task Init();
    public Task<string> GetSchema();
    public Task<string> RunQuery(string Query);
}

public class DataContext : IDataContext
{
    private readonly IConfiguration _config;

    public DataContext(IConfiguration config)
    {
        _config = config;
        Init();

    }

    public NpgsqlConnection CreateConnection()
    {
        var connectionString = _config["ConnectionString"];
        return new NpgsqlConnection(connectionString);
    }

    public async Task Init()
    {
        await _initDatabase();
    }

    private async Task _initDatabase()
    {
        // create database if it doesn't exist
        var connectionString = _config["ConnectionString"];
        using var connection = new NpgsqlConnection(connectionString);
    }

    public async Task<string> GetSchema()
    {
        string result;
        // create tables if they don't exist
        var connection = CreateConnection();
        connection.Open();
        string schema_query = "select\r\n    t.table_name,\r\n    STRING_AGG(c.column_name, ',') as columns\r\nfrom\r\n    information_schema.tables t\r\ninner join information_schema.columns c on\r\n    t.table_name = c.table_name\r\nwhere\r\n    t.table_schema = 'public'\r\n    and t.table_type= 'BASE TABLE'\r\n    and c.table_schema = 'public'\r\ngroup by t.table_name;";
        DataTable dt = new DataTable();
        var dbreader = await connection.ExecuteReaderAsync(schema_query);
        dt.Load(dbreader);
        result = string.Join(Environment.NewLine,
            dt.Rows.OfType<DataRow>().Select(x => string.Join(" ; ", x.ItemArray)));
        return result;
    }

    public async Task<string> RunQuery(string Query)
    {
        string result;
        var connection = CreateConnection();
        var dbreader = await connection.ExecuteReaderAsync(Query);
        DataTable dt = new DataTable();
        dt.Load(dbreader);
        result = string.Join(Environment.NewLine,
            dt.Rows.OfType<DataRow>().Select(x => string.Join(" ; ", x.ItemArray)));
        return result;
    }
}
