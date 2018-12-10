using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Models.Models
{
    //public class IconBundle
    //{
    //    public IconBundle() { }

    //    public IconBundle(string replacedIcon = null) => Icon = replacedIcon;
        

    //    public string CollapsedIcon { get; set; }
    //    public string ExpandedIcon { get; set; }
    //    public string Icon { get; set; }
    //}

    public class Node<T>
    {
        public T Data { get; set; }
        public List<Node<T>> Children { get; set; }
        
        //public string CollapsedIcon { get; set; }
        //public string ExpandedIcon { get; set; }

        public string Icon { get; set; }

        public Node()
        {
            Children = new List<Node<T>>();
        }

        public Node(T value)
        {
            Data = value;

            Children = new List<Node<T>>();
        }

        public Node(T value, string icon)
        {
            Data = value;

            Icon = icon;

            Children = new List<Node<T>>();
        }

        //public Node(T value, IconBundle iconBundle)
        //{
        //    Data = value;

        //    // For folder
        //    CollapsedIcon = iconBundle.CollapsedIcon;
        //    ExpandedIcon = iconBundle.ExpandedIcon;
            
        //    // For file
        //    Icon = iconBundle.Icon;

        //    Children = new List<Node<T>>();
        //}
    }
}
