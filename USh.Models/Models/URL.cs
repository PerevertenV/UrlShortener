using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USh.Models.Models
{
    public class URL
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserWhoCreatedUrlId { get; set; }
        [ForeignKey(nameof(UserWhoCreatedUrlId))]
        public User User {  get; set; }

        [Required]
        public int domenId { get; set; }

        [Required]
        [DisplayName("Ваше довге посилання")]
        public string LongUrl { get; set; }

        [Required]
        [DisplayName("Генероване коротке посилання")]
        public string ShortUrl { get; set; }

        [Required]
		[DisplayName("Унікальний код")]
		public int UniqueCode { get; set; }

        [Required]
        [DisplayName("Дата генерації")]
        public DateOnly CreatedDate { get; set; }

    }
}
