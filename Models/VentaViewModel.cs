﻿using GestorBiblioteca.Models.Entidades;
using System.ComponentModel.DataAnnotations;

namespace GestorBiblioteca.Models
{
    public class VentaViewModel
    {
        public string Titulo { get; set; }
        public Libro Libro { get; set; }
        public int LibroId { get; set; }
        public decimal Precio { get; set; }

        public string URLImagen { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Cantidad { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;
        public Usuario Usuario { get; set; }
    }
}