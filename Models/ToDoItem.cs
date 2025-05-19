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

        public DateTime DataUtworzenia { get; set; } = DateTime.Now;

        public DateTime? DataZrobienia { get; set; }

        [Required]
        public StatusZadania Status { get; set; }

        [Required]
        public KategoriaZadania Kategoria { get; set; }
    }

    public enum StatusZadania
    {
        Nowe,
        WTrakcie,
        Zakonczone,
        Zawieszone
    }

    public enum KategoriaZadania
    {
        Praca,
        Nauka,
        Dom,
        Inne
    }
}
