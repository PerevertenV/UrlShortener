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
	public class Domen
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int UserId { get; set; }
		[ForeignKey(nameof(UserId))]
		public User User { get; set; }

		[Required]
		[DisplayName("Домен")]
		public string UserDomen { get; set; }

		[Required]
		[DisplayName("Дата генерації")]
		public DateOnly CreatedDate { get; set; }

	}
}
