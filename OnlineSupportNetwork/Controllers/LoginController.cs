﻿using OnlineSupportNetwork.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;

namespace OnlineSupportNetwork.Controllers
{
    public class LoginController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Post()
        {
            XMLLoad xmlLoad = new XMLLoad();
            string xmlPath = AppDomain.CurrentDomain.BaseDirectory + "\\DataBase1.xml";
            string Result = string.Empty;
            int accounttype = -1;//Acount Type Not Set

            string username = HttpContext.Current.Request.Form["username"];
            string password = HttpContext.Current.Request.Form["password"];
            string check = string.Empty;
                                   
            if (File.Exists(xmlPath.ToString()) != true)
            {
                Result = "database not Exist";
            }
            else
            {
                //$.B Read User Name And Password From xml file \/
                xmlLoad.LoadFile(xmlPath.ToString(), "/User/" + username.ToString());
                //$.B Read User Name And Password From xml file /\
                check = xmlLoad.check.ToString();
                
                if (xmlLoad.accounttype == "technicalsupport")
                {
                    accounttype = 0;// accounttype == "technicalsupport"
                }
                else if (xmlLoad.accounttype == "fieldexpert")
                {
                    accounttype = 1;// accounttype == "fieldexpert"
                }
                else if (xmlLoad.accounttype == "Both")
                {
                    accounttype = 2;// accounttype == "Both" = (technicalsupport + fieldexpert)
                }
                else
                {
                    accounttype = -1;// Acount Type Not Set
                }
            }

            string oldText = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Login.txt");
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Login.txt", "username = " + username + " ___ password = " + password + " ___ accounttype = " + accounttype +  "/n" + oldText);

            if (xmlLoad.username.ToString().ToLower() == "" || xmlLoad.username.ToString().ToLower() == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    statusCode = 200,
                    statusMessage = "This User Name not find",
                    resultID = 4,
                    CheckID = check,

                });
            }
            else if (username.ToLower() == xmlLoad.username.ToString().ToLower() && password == xmlLoad.password.ToString().ToLower())
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    statusCode = 200,
                    statusMessage = "Username and Password is curect",
                    resultID = accounttype,
                    CheckID = check,

                });
            else if ((username.ToLower() == xmlLoad.username.ToString().ToLower() && password != xmlLoad.password.ToString().ToLower()) 
                || (username.ToLower() != xmlLoad.username.ToString().ToLower() && password == xmlLoad.password.ToString().ToLower())
                || (username.ToLower() != xmlLoad.username.ToString().ToLower() && password != xmlLoad.password.ToString().ToLower())
                )
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    statusCode = 200,
                    statusMessage = "Username or Password is incurect",
                    resultID = 3,
                    CheckID = check,

                });
            else if (username.ToLower() != xmlLoad.username.ToString().ToLower())
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    statusCode = 200,
                    statusMessage = "This User Name is not register",
                    resultID = 4,
                    CheckID = check,

                });
            else
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new
                {
                    statusCode = 401,
                    statusMessage = "Request Failed"+ "_" +Result,
                    resultID = 1,
                    CheckID = check,

                });

        }
    }
}
