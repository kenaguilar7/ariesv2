using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        
        [Required(ErrorMessage = "El código es requerido")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "El código debe tener 4 caracteres")]
        public string Code { get; set; }

        [Required(ErrorMessage = "El nombre de la compañía es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "El tipo de compañía es requerido")]
        public CompanyType CompanyType { get; set; }

        [Required(ErrorMessage = "El tipo de identificación es requerido")]
        public IdType IdType { get; set; }

        [Required(ErrorMessage = "El número de identificación es requerido")]
        [RegularExpression(@"^\d{1,2}-\d{3,4}-\d{3,4}$", ErrorMessage = "Formato inválido. Use el formato: X-XXX-XXXX")]
        public string NumberId { get; set; }

        [StringLength(100, ErrorMessage = "El nombre del representante legal no puede exceder los 100 caracteres")]
        public string Op1 { get; set; }

        [RegularExpression(@"^\d{1,2}-\d{3,4}-\d{3,4}$", ErrorMessage = "Formato inválido. Use el formato: X-XXX-XXXX")]
        public string Op2 { get; set; }

        [Required(ErrorMessage = "La dirección es requerida")]
        [StringLength(200, ErrorMessage = "La dirección no puede exceder los 200 caracteres")]
        public string Address { get; set; }

        [EmailAddress(ErrorMessage = "El formato del correo electrónico es inválido")]
        public string Mail { get; set; }

        [Phone(ErrorMessage = "El formato del teléfono es inválido")]
        public string PhoneNumber1 { get; set; }

        [Phone(ErrorMessage = "El formato del teléfono es inválido")]
        public string PhoneNumber2 { get; set; }

        [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder los 500 caracteres")]
        public string Notes { get; set; }

        [Url(ErrorMessage = "El formato de la URL es inválido")]
        public string WebSite { get; set; }

        [Required(ErrorMessage = "El tipo de moneda es requerido")]
        public CurrencyType MoneyType { get; set; }

        public override string ToString()
        {
            return $"{CompanyName?.ToUpper()}-{Code}";
        }
    }
} 