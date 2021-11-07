using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Enums { }
    public enum ItemType
    {
        Book,
        Journal
    }

    public enum Item_Search_By
    {
        Name,
        Date,
        Genre,
        ISBN
    }

    public enum GenreType
    {
        Fiction,
        Nonfiction
    }

    public enum Level
    {
        Manager,
        Employee,
        Guest
    }

    public enum Genre_Search_By
    {
        Fiction_Partial_Name,
        Nonfiction_Partial_Name
    }

    public enum Custome_Search_By
    {
        Partial_Name,
        Partial_ID
    }

    public enum Invoice_Search_By
    {
        Customer_Partial_Name,
        Customer_Partial_ID,
        Partial_Invoice_ID,
        Date
    }

}