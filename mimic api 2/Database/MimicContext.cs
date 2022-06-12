using Microsoft.EntityFrameworkCore;
using mimic_api_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mimic_api_2.Database
{
    public class MimicContext: DbContext
    {


        public MimicContext(DbContextOptions<MimicContext> options) : base(options)
        {

        }

        public DbSet<Palavra> Palavras { get; set; }
    }
}
