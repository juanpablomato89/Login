using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Login.viewModels;

namespace Login.Models
{
    public class OrdenCompra
    {
        public int ID { get; set; }
        
        [Required]
        [Display(Name = "Nombre del Producto")]
        public int ProductoID { get; set; }

        [Required]
        [Display(Name = "Nombre del Usuario")]
        public string UsuarioID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha Orden")]
        public DateTime Fecha { get; set; }

        [Required]
        [DisplayFormat(NullDisplayText = "Sin Estado")]
        public Estado Estado { get; set; }

        [Required]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }

        public Usuario Usuario { get; set; }

        public Producto Producto { get; set; }

    }

    public enum Estado
    {
        Created=1,
        Confirmed=2,
        Canceled=3
    }
}
