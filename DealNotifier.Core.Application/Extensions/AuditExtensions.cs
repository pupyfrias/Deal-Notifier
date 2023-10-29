using Catalog.Application.ViewModels.Common;
using Catalog.Domain.Entities;
using Newtonsoft.Json;

namespace Catalog.Application.Extensions
{
    public static class AuditExtensions
    {
        public static Audit ToAudit(this AuditEntry entry)
        {
            return new Audit
            {
                UserName = entry.UserName,
                Action = entry.Action.ToString(),
                TableName = entry.TableName,
                DateTime = DateTime.Now,
                PrimaryKey = JsonConvert.SerializeObject(entry.KeyValues),
                OldValues = entry.OldValues.Count == 0 ? null : JsonConvert.SerializeObject(entry.OldValues),
                NewValues = entry.NewValues.Count == 0 ? null : JsonConvert.SerializeObject(entry.NewValues),
                AffectedColumns = entry.ChangedColumns.Count == 0 ? null : JsonConvert.SerializeObject(entry.ChangedColumns)
            };
        }
    }
}