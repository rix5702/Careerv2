﻿using CareerProject.Common;
using CareerProject.Models.DTO;
using CareerProject.Models.Service;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CareerProject.Controllers
{
    public class AuthenController : Controller
    {
        // GET: Authen
        AuthenService authenService = new AuthenService();
        CareerDBContext db = new CareerDBContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        public ActionResult SignUp()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return View("Login");
        }

        [HttpPost]
        public ActionResult GoogleLogin(string name, string email)
        {

            var isExistAccount = db.tbl_User.Where(x => x.Email == email).ToList();
            tbl_User user = new tbl_User();

            if (isExistAccount != null && isExistAccount.Count() > 0)
            {
                var test = db.tbl_User.ToList();
                var data = db.tbl_User.Where(x => x.Email.ToLower().TrimEnd().Equals(email.ToLower().Trim())).ToList();
                user = data.First();

            }
            else
            {
                user = new tbl_User();
                user.Name = name;
                user.Email = email;
                user.Major = "None";
                user.status = "Active";
                db.tbl_User.Add(user);
                db.SaveChanges();
            }
            if (email == "tqninh.work@gmail.com")
            {
                var userSession = new UserLogin();
                userSession.Mail = email;
                userSession.UserID = 0;
                userSession.TenKhachHang = "Admin";
                userSession.Role = "Admin";
                Session.Add(CommonConstant.USER_SESSION, userSession);
                return RedirectToAction("Index", "Admin/Apply");
            }
            else
            {
                var userSession = new UserLogin();
                if (user.status.Trim() == "Inactive")
                {
                    ViewBag.signin = "Login failed, your account is inactive";
                    return RedirectToAction("Login", "Authen");
                }
                userSession.Mail = email;
                userSession.UserID = user.ID;
                userSession.TenKhachHang = user.Name;
                userSession.Role = "User";
                Session.Add(CommonConstant.USER_SESSION, userSession);
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult Login(string email, string password, string company)
        {
            if (email == "tqninh.work@gmail.com" && password == "123456")
            {
                var userSession = new UserLogin();
                userSession.Mail = email;
                userSession.UserID = 0;
                userSession.TenKhachHang = "Admin";
                userSession.Role = "Admin";
                Session.Add(CommonConstant.USER_SESSION, userSession);
                return RedirectToAction("Index", "Admin/Apply");
            }
            if (company != null && company == "on")
            {
                tbl_Company tblCompany = authenService.signInCompany(email, password);
                if (tblCompany != null)
                {
                    if (tblCompany.status.Trim() == "Inactive")
                    {
                        ViewBag.signin = "Login failed, your account is inactive";
                        return View();
                    }
                    var userSession = new UserLogin();
                    userSession.Mail = email;
                    userSession.UserID = tblCompany.ID;
                    userSession.TenKhachHang = tblCompany.Name;
                    userSession.Role = "Company";
                    Session.Add(CommonConstant.USER_SESSION, userSession);
                    return RedirectToAction("Index", "Admin/Apply");
                }
                else
                {
                    ViewBag.signin = "Login failed, account does not exists";
                    return View();

                }
            }
            else
            {
                tbl_User user = authenService.signInUser(email, password);
                if (user != null)
                {
                    if (user.status.Trim() == "Inactive")
                    {
                        ViewBag.signin = "Login failed, your account is inactive";
                        return View();
                    }
                    var userSession = new UserLogin();
                    userSession.Mail = email;
                    userSession.UserID = user.ID;
                    userSession.TenKhachHang = user.Name;
                    userSession.Role = "User";
                    Session.Add(CommonConstant.USER_SESSION, userSession);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.signin = "Login failed, account does not exists";
                    return View();

                }
            }
        }

        [HttpPost]
        public ActionResult SignUp(
            string name,
            string dob,
            string major,
            string jobCity,
            string profileUser,
            string skill,
            string expected,
            string experiences,
            string position,
            string email,
            string password
            )
        {

            if (authenService.signUpUser(
                name,
                Convert.ToDateTime(dob),
                major,
                jobCity,
                profileUser,
                skill,
               float.Parse(expected),
               Convert.ToInt32(experiences),
               position,
               email,
               password
                ))
            {
                return RedirectToAction("Login", "Authen");

            }
            else
            {
                ViewBag.signup = "Sign up failed, Your email already existed";
                return View();

            }

        }
        [HttpPost]
        public ActionResult SignUpForCompany(
            string companyName,
            string description,
            HttpPostedFileBase avatar,
            string phoneNumber,
            string email,
            string password,
            string location
)
        {
            // Validate and process the uploaded avatar file
            string picPhim = System.IO.Path.GetFileName(avatar.FileName);
            string pathPhim = System.IO.Path.Combine(Server.MapPath("~/Img"), picPhim);
            avatar.SaveAs(pathPhim);

            using (MemoryStream ms = new MemoryStream())
            {
                avatar.InputStream.CopyTo(ms);
                byte[] array = ms.GetBuffer();
            }
            // Perform the registration using the provided data
            if (authenService.signUpCompany(
                companyName,
                description,
                picPhim,
                phoneNumber,
                location,
                email,
                password
            ))
            {
                return RedirectToAction("Login", "Authen"); // Redirect to a list of registered companies
            }
            else
            {
                ViewBag.signup = "Sign up failed, Your email already existed";
                return View(); // Return to the view with an error message
            }
        }

        public ActionResult SignUpForCompany()
        {
            return View();
        }
    }
}