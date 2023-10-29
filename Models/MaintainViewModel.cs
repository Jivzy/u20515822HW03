﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace u20515822_HW03.Models
{
    public class MaintainViewModel
    {
        // declaring
        public List<types> Types { get; set; }
        public List<authors> Authors { get; set; }
        public List<borrows> Borrows { get; set; }

        public int AuthorsPageNumber { get; set; } // Current page for authors
        public int TypesPageNumber { get; set; } // Current page for types
        public int BorrowsPageNumber { get; set; } // Current page for borrows

        public int TotalAuthorsPages { get; set; } // Total pages for authors
        public int TotalTypesPages { get; set; } // Total pages for types
        public int TotalBorrowsPages { get; set; } // Total pages for borrows

        // Additional properties for author editing
        public authors Author { get; set; } // Updated author data
        public string OriginalAuthorName { get; set; } // Original author name

        // Additional properties for types editing
        public types Type { get; set; } // Updated type data
        public string OriginalTypeName { get; set; } // Original type name

        // Additional properties for borrow editing
        public borrows Borrow { get; set; }
        public DateTime OriginalBorrowDate { get; set; }
        public DateTime OriginalReturnDate { get; set; }

    }
}