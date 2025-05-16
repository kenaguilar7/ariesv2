using System;
using System.ComponentModel.DataAnnotations;

namespace Aries.Contabilidad.Core.Models
{
    public class Company
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El nombre de la empresa es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "La identificación fiscal es requerida")]
        [StringLength(20, ErrorMessage = "La identificación fiscal no puede exceder los 20 caracteres")]
        public string TaxId { get; set; }

        [StringLength(200, ErrorMessage = "La dirección no puede exceder los 200 caracteres")]
        public string Address { get; set; }

        [StringLength(50, ErrorMessage = "El teléfono no puede exceder los 50 caracteres")]
        public string Phone { get; set; }

        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
        [StringLength(100, ErrorMessage = "El correo electrónico no puede exceder los 100 caracteres")]
        public string Email { get; set; }

        [StringLength(200, ErrorMessage = "El sitio web no puede exceder los 200 caracteres")]
        public string Website { get; set; }

        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; }

        public Company()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }
    }
} 