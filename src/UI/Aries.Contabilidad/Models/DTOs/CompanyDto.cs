using System;
using System.Collections.Generic;
using Aries.Contabilidad.Models.Enums;

namespace Aries.Contabilidad.Models.DTOs
{
    /// <summary>
    /// Data Transfer Object for Company information in the UI layer
    /// </summary>
    public class CompanyDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool Active { get; set; }
        public bool IsActive { get; set; } = true;
        public int UserId { get; set; }
        public string CopyFrom { get; set; }
        public string Code { get; set; }
        public string CompanyName { get; set; }
        public CompanyType CompanyType { get; set; }
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
        public CurrencyType MoneyType { get; set; }

        public override string ToString()
        {
            return $"{CompanyName?.ToUpper()}-{Code}";
        }
    }
} 