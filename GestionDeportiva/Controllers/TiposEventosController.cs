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
    public class TiposEventosController : Controller
    {
        private GestionEventosEntities db = new GestionEventosEntities();

        // GET: TiposEventos
        public async Task<ActionResult> Index()
        {
            return View(await db.TiposEventos.ToListAsync());
        }

        // GET: TiposEventos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposEventos tiposEventos = await db.TiposEventos.FindAsync(id);
            
            if (tiposEventos == null)
            {
                return HttpNotFound();
            }
            var data = (from evento in db.Eventos where evento.TipoEventoId == tiposEventos.TipoEventoId select evento).ToList();
            ViewBag.Eventos = data;
			return View(tiposEventos);
        }

        // GET: TiposEventos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TiposEventos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TipoEventoId,Nombre")] TiposEventos tiposEventos)
        {
            if (ModelState.IsValid)
            {
                db.TiposEventos.Add(tiposEventos);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tiposEventos);
        }

        // GET: TiposEventos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposEventos tiposEventos = await db.TiposEventos.FindAsync(id);
            if (tiposEventos == null)
            {
                return HttpNotFound();
            }
            return View(tiposEventos);
        }

        // POST: TiposEventos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TipoEventoId,Nombre")] TiposEventos tiposEventos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tiposEventos).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tiposEventos);
        }

        // GET: TiposEventos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposEventos tiposEventos = await db.TiposEventos.FindAsync(id);
            if (tiposEventos == null)
            {
                return HttpNotFound();
            }
            return View(tiposEventos);
        }

        // POST: TiposEventos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TiposEventos tiposEventos = await db.TiposEventos.FindAsync(id);
            db.TiposEventos.Remove(tiposEventos);
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
    }
}
