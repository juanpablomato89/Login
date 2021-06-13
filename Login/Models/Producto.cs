using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Login.Models
{
    public class Producto
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre del producto no puede exceder los 50 caracteres")]
        [Display(Name = "Nombre Producto")]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage ="La descripcion del producto no puede exceder los 100 caracteres")]
        [Display(Name = "Descripcion Producto")]
        public string Descripcion { get; set; }

        [Required]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }

        [Required]
        [Display(Name = "Slug")]
        public int Slug { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Precio { get; set; }

        public List<OrdenCompra> OrdenCompras { get; set; }
    }
}
