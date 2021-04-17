﻿using ESSD_CA.Db;
using ESSD_CA.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ESSD_CA.Controllers
{
    public class ProductController : Controller
    {
        private readonly DbESSDCA db;
        public ProductController(DbESSDCA db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            List<Product> products = db.Products.ToList();  //retrieving products from database and putting into a list

            ViewData["Is_ProductMgmt"] = "bold_menu";
            ViewData["products"] = products;    //sending data view ViewData
            return View();
        }
    }
}
