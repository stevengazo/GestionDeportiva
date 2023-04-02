﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GestionDeportiva.Models;
using System.IO;
using System.Drawing;

namespace GestionDeportiva.Controllers
{
    public class EventosController : Controller
    {
        private GestionEventosEntities db = new GestionEventosEntities();

        // GET: Eventos
        public async Task<ActionResult> Index()
        {
            var eventos = db.Eventos.Include(e => e.TiposEventos).OrderByDescending(e => e.Fecha);           
            return View(await eventos.ToListAsync());
        }

        // GET: Eventos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eventos eventos = await db.Eventos.FindAsync(id);
            if (eventos == null)
            {
                return HttpNotFound();
            }
            return View(eventos);
        }

        // GET: Eventos/Create
        public ActionResult Create()
        {
			var user = Session["UserProfile"];
            if (user != null)
            {
                ViewBag.TipoEventoId = new SelectList(db.TiposEventos, "TipoEventoId", "Nombre");
                return View();
            }
            else
            {
                return RedirectToAction(nameof(AdministradoresController.Login),"Administradores");
            }
        }

        // POST: Eventos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EventoId,Nombre,Descripcion,Fecha,Lugar,TipoEventoId")] Eventos eventos)
        {
			var user = Session["UserProfile"];
            if (user != null)
            {

                if (ModelState.IsValid)
                {
                    if (Request.FilePath[0] != null)
                    {
                        /*
                         recibe la imagen en Request.Files 
                         */
                        var file = Request.Files[0];
                        // Convierte el archivo recibido en un Byte[] para luego asignarlo al Objeto de tipo Evento (el cual se almacenará en la base de datos)
                        MemoryStream target = new MemoryStream();
                        file.InputStream.CopyTo(target);
                        eventos.Imagen = target.ToArray();
                    }
                    db.Eventos.Add(eventos);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                ViewBag.TipoEventoId = new SelectList(db.TiposEventos, "TipoEventoId", "Nombre", eventos.TipoEventoId);
                return View(eventos);
            }
            else
            {
				return RedirectToAction(nameof(AdministradoresController.Login), "Administradores");
			}
        }

        // GET: Eventos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eventos eventos = await db.Eventos.FindAsync(id);
            if (eventos == null)
            {
                return HttpNotFound();
            }
            ViewBag.TipoEventoId = new SelectList(db.TiposEventos, "TipoEventoId", "Nombre", eventos.TipoEventoId);
            return View(eventos);
        }

        // POST: Eventos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EventoId,Nombre,Descripcion,Fecha,Lugar,Imagen,TipoEventoId")] Eventos eventos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventos).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.TipoEventoId = new SelectList(db.TiposEventos, "TipoEventoId", "Nombre", eventos.TipoEventoId);
            return View(eventos);
        }

        // GET: Eventos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eventos eventos = await db.Eventos.FindAsync(id);
            if (eventos == null)
            {
                return HttpNotFound();
            }
            return View(eventos);
        }

        // POST: Eventos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Eventos eventos = await db.Eventos.FindAsync(id);
            db.Eventos.Remove(eventos);
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
