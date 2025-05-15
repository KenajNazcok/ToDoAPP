using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }

        [Required]
        public string Tytul { get; set; } = string.Empty;

        public string Opis { get; set; } = string.Empty;

        public bool CzyZrobione { get; set; }

        public DateTime DataUtworzenia { get; set; } = DateTime.Now;

        public DateTime? DataZrobienia { get; set; }
    }
}
