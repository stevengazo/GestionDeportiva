﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using GestionDeportiva.Models;
using System.Threading;

namespace GestionDeportiva.Controllers
{
    public class AdministradoresController : Controller
    {
        private GestionEventosEntities db = new GestionEventosEntities();

        // GET: Administradores
        public async Task<ActionResult> Index()
        {
            return View(await db.Administradores.ToListAsync());
        }

        // GET: Administradores/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administradores administradores = await db.Administradores.FindAsync(id);
            if (administradores == null)
            {
                return HttpNotFound();
            }
            return View(administradores);
        }

        // GET: Administradores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administradores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AdministradorId,NombreUsuario,Nombre,Apellidos,HashContrasena")] Administradores administradores)
        {
            if (ModelState.IsValid)
            {

                administradores.HashContrasena = GetMD5Hash(administradores.HashContrasena);
                db.Administradores.Add(administradores);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(administradores);
        }

        // GET: Administradores/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administradores administradores = await db.Administradores.FindAsync(id);
            if (administradores == null)
            {
                return HttpNotFound();
            }
            return View(administradores);
        }

        // POST: Administradores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AdministradorId,NombreUsuario,Nombre,Apellidos,HashContrasena")] Administradores administradores)
        {
            if (ModelState.IsValid)
            {
                db.Entry(administradores).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(administradores);
        }

        // GET: Administradores/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            var user = Session["UserProfile"];
            if (user != null)
            {
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				Administradores administradores = await db.Administradores.FindAsync(id);
				if (administradores == null)
				{
					return HttpNotFound();
				}
				return View(administradores);
			}
            else
            {
                return RedirectToAction(nameof(Login));
            }

        }

        // POST: Administradores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Administradores administradores = await db.Administradores.FindAsync(id);
            db.Administradores.Remove(administradores);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpGet]
		public async Task<ActionResult> Login()
		{
			return View();
		}
        [HttpPost]
		public async Task<ActionResult> Login([Bind(Include = "LoginUser,LoginPassword")] LoginModel login)
        {
            try
            {
                login.LoginPassword = GetMD5Hash(login.LoginPassword);
                // Logica de validación
                bool LoginSucess = validateLogin(login.LoginUser, login.LoginPassword, out int idUsuario);
                if (LoginSucess)
                {
                    Administradores UserActive = GetAdministradores(idUsuario);
                    Session["UserProfile"] = UserActive;
                    Session["UserName"] = UserActive.Nombre;
					return RedirectToAction(nameof(Index));
                }
                return View();
            }catch(Exception f)
            {
                ViewBag.ErrorMessage = f.Message;
				return View();
			}            
        }        

        public async Task<ActionResult> CloseSession()
        {
            Session["UserProfile"] = null;
            Session.Abandon();
			return RedirectToAction("Index", "Eventos");
        }
		private String GetMD5Hash(String input)
		{
			MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
			byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
			bs = x.ComputeHash(bs);
			System.Text.StringBuilder s = new System.Text.StringBuilder();
			foreach (byte b in bs)
			{
				s.Append(b.ToString("x2").ToLower());
			}
			String hash = s.ToString();
			return hash;
		}
        private bool validateLogin(string usuario, string passwordHash, out int idUsuario)
        {
            try
            {
                int LinqResult = (from admin in db.Administradores
                                  where admin.NombreUsuario.ToLower() == usuario.ToLower() && admin.HashContrasena == passwordHash
                                  select admin.AdministradorId).FirstOrDefault();
                if(LinqResult != 0)
                {
                    idUsuario = LinqResult;
                    return true;
                }
                else
                {
                    idUsuario = 0;
                    return false;
                }
                
            }catch(Exception f)
            {
				idUsuario = 0;
				return false;
            }
        }
        private Administradores GetAdministradores(int id)
        {
            try
            {
                var user = (from admin in db.Administradores
                            where admin.AdministradorId == id
                            select admin).FirstOrDefault();
                if (user != null)
                {
                    user.HashContrasena = string.Empty;
                }
                return user;
			}
			catch(Exception f)
            {
                Console.WriteLine(f.Message);
                return null;
            }
        }
	}
}
