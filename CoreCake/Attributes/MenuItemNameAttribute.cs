using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCake.Attributes
{
    public class MenuItemNameAttribute : Attribute
    {
        public string Name { get; set; }

        public MenuItemNameAttribute(string _name)
        {
            Name = _name;
        }
    }
}
