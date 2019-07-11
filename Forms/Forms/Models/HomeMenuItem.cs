﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Forms.Models
{
    public enum MenuItemType
    {
        Circle,
        Browse,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
