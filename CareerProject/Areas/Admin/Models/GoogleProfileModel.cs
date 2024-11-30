﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareerProject.Areas.Admin.Models
{
    public class GoogleProfileModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Email { get; set; }
        public string Verified_Email { get; set; }
        public bool IsDisable { get; set; }
    }
}