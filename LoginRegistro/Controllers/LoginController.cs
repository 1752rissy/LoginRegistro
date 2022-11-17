using LoginRegistro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LoginRegistro.Controllers
{
    public class LoginController : Controller
    {
        private ColegioEntities _db = new ColegioEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult LogearUsuario()
        {
            return View("LogearUsuario");
        }

        [HttpPost]
        public ActionResult LogearUsuario(Usuario _usuario)
        {
            var h_password = GetMd5(_usuario.Password);
            var data = _db.Usuario.Where(u => u.Email.Equals(_usuario.Email) && u.Password.Equals(h_password)).ToList();
            if (data.Count() > 0)
            {
                //añadir a la sesion del usuario
                Session["NombreApellido"] = data.FirstOrDefault().Nombre + " " + data.FirstOrDefault().Apellido;
                Session["Email"] = data.FirstOrDefault().Email;
                Session["idUsuario"] = data.FirstOrDefault().idUsuario;
                return RedirectToAction("index");
            }
            else
            {
                ViewBag.error = data.Count;
                return View();
            }
        }

        public static string GetMd5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] TargeData = md5.ComputeHash(fromData);

            string byte25 = null;

            for (int i = 0; i < TargeData.Length; i++)
            {
                byte25 += TargeData[i].ToString("x2");
            }
            return byte25;
        }
    }
}