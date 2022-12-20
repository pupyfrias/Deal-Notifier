using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace WebScraping.Core.Application.SetupOptions
{
    public static class SeriLog
    {
        [Obsolete]
        public static Action<HostBuilderContext, LoggerConfiguration> Options = (hostBuilderContext, loggerContiguration) =>
        {
            /*loggerContiguration.ReadFrom
                .Configuration(hostBuilderContext.Configuration)
                .Enrich.FromLogContext();*/

            loggerContiguration.AuditTo.MSSqlServer(
                    connectionString: hostBuilderContext.Configuration.GetConnectionString("DefaultConnection"),
                    tableName: "AudiLogs",
                    autoCreateSqlTable: true
                ).WriteTo.Console();
            /* var sinkOptions = new MSSqlServerSinkOptions()
             {
                 TableName = "Your table name",
                 AutoCreateSqlTable = true

             };


             var columnUserName = new SqlColumn
             {
                 ColumnName = "UserName",
                 DataType = System.Data.SqlDbType.NVarChar
             };

             var columnOptions = new ColumnOptions();

             columnOptions.Store.Remove(StandardColumn.MessageTemplate);
             //columnOptions.AdditionalColumns.Add(columnUserName);

             loggerContiguration
             .WriteTo.Console()
             .WriteTo.File(
                 path: "./logs/log-.txt",
                 rollingInterval: RollingInterval.Day
                 )
             .WriteTo.MSSqlServer(
                 connectionString: hostBuilderContext.Configuration.GetConnectionString("DefaultConnection"),
                  tableName: "Logs",
                  autoCreateSqlTable: true,
                  columnOptions: columnOptions

                 );*/




        };



    }
}
