using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.AndroidRoot.Models
{
    public class TwoColumnsElement<TEntity>
    {
        public bool FirstExist { get; set; } = false;
        public TEntity First { get; set; }

        public bool SecondExist { get; set; } = false;
        public TEntity Second { get; set; }
    }
}
