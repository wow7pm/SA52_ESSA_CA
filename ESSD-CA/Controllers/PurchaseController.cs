﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ESSD_CA.Models;
using ESSD_CA.Db;

namespace ESSD_CA.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly DbESSDCA db;

        public PurchaseController(DbESSDCA db)
        {
            this.db = db;
        }

        public IActionResult History()
        {
            // Check logged in?
            string sessionId = Request.Cookies["sessionId"];
            if (String.IsNullOrEmpty(sessionId))
                return RedirectToAction("Index", "Login");
            
            // retrieve all purchased items by user id from db
            User user = db.Users.FirstOrDefault(x => x.SessionId == sessionId);
            
            List<PurchaseOrder> userOrders = null;
            if (user != null)
                userOrders  = db.PurchaseOrders.Where(x => x.UserId == user.UserId).ToList();

            IEnumerable<HistoryViewModel> iter = null;
            if (userOrders != null && userOrders.Count > 0)
            {
                ViewData["hasHistory"] = true;

                List<PurchaseOrderDetails> poDetails = userOrders[0].PODetails.ToList();
                
                for (int i =0; i<userOrders.Count-1; i++)
                { 
                    poDetails.AddRange(userOrders[i+1].PODetails.ToList());
                }
            

                iter = 
                    from pod in poDetails
                    group pod by new 
                    { pod.OrderId,
                      pod.ProductId
                    }
                    into g
                    select new HistoryViewModel
                    {
                        Order = g.Select(x => x.Order).First(),
                        Product = g.Select(x => x.Product).First(),
                        ActivationCdList = g.Select(x => x.ActivationCode).ToList(),
                    };
            }
            else
            {
                ViewData["hasHistory"] = false;
            }

            ViewData["sessionId"] = sessionId;

            return View(iter);
        }

        public void AddPODetail(string orderId, string productId)
        {
            db.PODetails.Add(new PurchaseOrderDetails
            {
                ActivationCode = GenerateActivationCode(),
                ProductId = productId,
                OrderId = orderId
            });

            db.SaveChanges();

        }


        private string GenerateActivationCode()
        {
            return (Guid.NewGuid().ToString());
        }
    }
}
