using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class Genre
    {
        public string Name { get; private set; }
        public GenreType Type { get; private set; }

        public Genre(string name, GenreType genre_type)
        {
            Name = name;
            Type = genre_type;
        }

        public override string ToString()
        => $"name: {Name} type: {Type}";

        public override bool Equals(object obj)
        {
            Genre other = obj as Genre;
            return Name == other.Name;
        }
    }
}
