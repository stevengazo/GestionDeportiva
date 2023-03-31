using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GestionDeportiva.Models;

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
                return View();
            }catch(Exception f)
            {
                ViewBag.ErrorMessage = f.Message;
				return View();
			}
            
        }
    }
}
