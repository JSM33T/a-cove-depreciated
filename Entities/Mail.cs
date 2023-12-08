using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Almondcove.Entities
{
    public class Mail
    {
        public int Id { get; set; }
        public string EMail { get; set; }
        public string Name { get; set; }
        public string Origin { get; set; }
        public DateTime DateAdded { get; set; }
    }
}