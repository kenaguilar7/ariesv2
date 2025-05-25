using System;
using AriesContador.Core.Models.Utils;
using System.Collections;
using System.Collections.Generic;
using AriesContador.Core.Models.PostingPeriods;
using AriesContador.Core.Models.Accounts;
using DocumentFormat.OpenXml.Spreadsheet;

namespace AriesContador.Core.Models.Companies
{
    public class Company : BaseModel
    {
        public bool IsActive { get; set; } = true; 
        public int UserId { get; set; }
        public string CopyFrom { get; set; }
        public string Code { get; set; }

        public string CompanyName { get; set; }

        public IdType IdType { get; set; }

        public string NumberId { get; set; }

        public string Op1 { get; set; }

        public string Op2 { get; set; }

        public string Address { get; set; }

        public string Mail { get; set; }

        public string PhoneNumber1 { get; set; }

        public string PhoneNumber2 { get; set; }

        public string Notes { get; set; }

        public string WebSite { get; set; }

        public CurrencyTypeCompany MoneyType { get; set; }

        public IEnumerable<PostingPeriod> PostingPeriods { get; set; } = new List<PostingPeriod>();

        public IEnumerable<Account> Account { get; set; } = new List<Account>(); 

        public override string ToString()
        {
            return $"{ CompanyName.ToUpper()}-{Code}";
        }

        public Company() { }
    }
}
