using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace OnlineBrief24.Models
{
	public class OnlineBrief24Context : DbContext
	{
		public OnlineBrief24Context(DbContextOptions<OnlineBrief24Context> options)
	   : base(options) { }


		public DbSet<Dispatches> Dispatches { get; set; }
		public DbSet<File> Files { get; set; }
		public DbSet<Parameters> Parameters { get; set; }


	}
}
