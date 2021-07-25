namespace GlossarySystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class GlossaryEntryEditModel
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Please enter a word or a short phrase"), StringLength(50, MinimumLength = 1, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string Term { get; set; }

        [Required(ErrorMessage = "Please enter the definition of the term"), StringLength(1000, MinimumLength = 10, ErrorMessage = "The {0} value must be between {2}-{1} characters. ")]
        public string Definition { get; set; }
    }
}
