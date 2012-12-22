using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace arunes
{
    public class ListItem
    {
        private string _display;
        private object _value;
        public ListItem(string display, object value)
        {
            _display = display;
            _value = value;
        }

        public string Display
        {
            get { return _display; }
        }

        public object Value
        {
            get { return _value; }
        }

        public override string ToString()
        {
            return _display;
        }
    }
}
