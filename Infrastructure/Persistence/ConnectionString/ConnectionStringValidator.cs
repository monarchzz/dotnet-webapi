using Application.Common.Persistence;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Infrastructure.Persistence.ConnectionString;

internal class ConnectionStringValidator : IConnectionStringValidator
{
    private readonly DatabaseSettings _dbSettings;
    private readonly ILogger<ConnectionStringValidator> _logger;

    public ConnectionStringValidator(IOptions<DatabaseSettings> dbSettings, ILogger<ConnectionStringValidator> logger)
    {
        _dbSettings = dbSettings.Value;
        _logger = logger;
    }

    public bool TryValidate(string connectionString, string? dbProvider = null)
    {
        try
        {
            var postgresqlcs = new NpgsqlConnectionStringBuilder(connectionString);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Connection String Validation Exception : {ExMessage}", ex.Message);
            return false;
        }
    }
}