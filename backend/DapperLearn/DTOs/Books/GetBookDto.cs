﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperLearn.DTOs.Books
{
    public class GetBookDto
    {
        public int bookId { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string publishedYear { get; set; }
        public bool isAvailable { get; set; }
        public string genres { get; set; }
    }
}
