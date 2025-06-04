using Aries.Contabilidad.Models.Utils;
using System.Text.Json.Serialization;

namespace Aries.Contabilidad.Models.Accounts
{
    public class Account : BaseAccount
    {
        public int Id { get; set; }
        public int? FatherAccount { get; set; }
        private DebOrCred _debOCred;  // Backing field

        public DebOrCred DebOCred
        {
            //get { return _debOCred; }
            get
            {
                switch (this.AccountTag)
                {
                    case AccountTag.Activo:
                    case AccountTag.CostoVenta:
                    case AccountTag.Egreso:
                        return DebOrCred.Debito;
                    case AccountTag.Ingreso:
                    case AccountTag.Pasivo:
                    case AccountTag.Patrimonio:
                        return DebOrCred.Credito;
                       
                    default:
                        return DebOrCred.Debito; // Default to Debito if no match found
                        
                }
            }
        }

        public decimal PriorBalance { get; set; }
        public decimal PriorBalanceForeign { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public decimal DebitBalance { get; set; }
        public decimal CreditBalance { get; set; }
        public decimal DebitBalanceForeign { get; set; }
        public decimal CreditBalanceForeign { get; set; }

        public decimal CurrentBalance => DebOCred == DebOrCred.Debito
            ? PriorBalance + DebitBalance - CreditBalance
            : PriorBalance - DebitBalance + CreditBalance;

        public decimal CurrentBalanceForeign => DebOCred == DebOrCred.Debito
            ? PriorBalanceForeign + DebitBalanceForeign - CreditBalanceForeign
            : PriorBalanceForeign - DebitBalanceForeign + CreditBalanceForeign;

        public decimal MontlyBalance => DebOCred == DebOrCred.Debito
            ? DebitBalance - CreditBalance
            : CreditBalance - DebitBalance;

        public decimal MontlyBalanceForeign => DebOCred == DebOrCred.Debito
            ? DebitBalanceForeign - CreditBalanceForeign
            : CreditBalanceForeign - DebitBalanceForeign;
    }
} 