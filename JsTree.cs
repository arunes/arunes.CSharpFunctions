using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace arunes.JsTree
{
    public class Node
    {
        public Node()
        {
        }

        Attributes _attr;
        public Attributes attr
        {
            get
            {
                return _attr;
            }
            set
            {
                _attr = value;
            }
        }
        Data _data;
        public Data data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }
        string _state;
        public string state
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }
        List<Node> _children;
        public List<Node> children
        {
            get
            {
                return _children;
            }
            set
            {
                _children = value;
            }
        }

    }

    public class Attributes
    {
        string _id;

        public string id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        string _rel;

        public string rel
        {
            get
            {
                return _rel;
            }
            set
            {
                _rel = value;
            }
        }
        string _mdata;

        public string mdata
        {
            get
            {
                return _mdata;
            }
            set
            {
                _mdata = value;
            }
        }
    }

    public class Data
    {
        string _title;

        public string title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }

        string _icon;

        public string icon
        {
            get
            {
                return _icon;
            }
            set
            {
                _icon = value;
            }
        }
    }
}