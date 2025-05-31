using System;
using System.Text.Json.Serialization;

namespace Aries.Contabilidad.Models.Accounts
{
    public abstract class BaseAccount : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Memo { get; set; } = string.Empty;
        public bool Editable { get; set; }
        public int EditableMySql
        {
            get { return Convert.ToInt32(this.Editable); }
            set { this.Editable = Convert.ToBoolean(value); }
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AccountTag AccountTag { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AccountType AccountType { get; set; }

        public string CompanyId { get; set; } = string.Empty;
        public string PathDirection { get; set; } = string.Empty;
    }
} 