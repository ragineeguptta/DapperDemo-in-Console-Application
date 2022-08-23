using System;
using System.Collections.Generic;
using System.Text;

namespace DapperDemo
{
    class Author
    {
        public int Id { get; set; }
        public string FName { get; set; }
        public string LastName { get; set; }
        public List<Book> Books { get; set; }
    }
}
