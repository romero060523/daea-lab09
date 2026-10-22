using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using LAB09.Models;

namespace LAB09.Controllers
{
    public class ProductoController : Controller
    {
        private const string ClaveSesion = "productos";

        // Recupera la lista de productos desde la sesión.
        // Solo la inicializa con datos de ejemplo si todavía no existe.
        private List<ProductoModel> ObtenerProductos()
        {
            if (Session[ClaveSesion] == null)
            {
                List<ProductoModel> productos = new List<ProductoModel>();
                productos.Add(new ProductoModel { Id = 1, Nombre = "Laptop Lenovo IdeaPad 3", Descripcion = "Laptop 15.6\", Intel Core i5, 8 GB RAM, 512 GB SSD", Precio = 2499.90m, Stock = 12 });
                productos.Add(new ProductoModel { Id = 2, Nombre = "Mouse Logitech M170", Descripcion = "Mouse inalámbrico óptico con receptor USB", Precio = 39.90m, Stock = 85 });
                productos.Add(new ProductoModel { Id = 3, Nombre = "Monitor Samsung 24\"", Descripcion = "Monitor LED Full HD de 24 pulgadas, 75 Hz", Precio = 549.00m, Stock = 20 });
                productos.Add(new ProductoModel { Id = 4, Nombre = "Teclado Redragon Kumara", Descripcion = "Teclado mecánico TKL con iluminación RGB", Precio = 159.90m, Stock = 34 });
                Session[ClaveSesion] = productos;
            }

            return (List<ProductoModel>)Session[ClaveSesion];
        }

        private ProductoModel BuscarProducto(int id)
        {
            return ObtenerProductos().FirstOrDefault(p => p.Id == id);
        }

        // GET: Producto
        public ActionResult Index()
        {
            return View(ObtenerProductos());
        }

        // GET: Producto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProductoModel producto = BuscarProducto(id.Value);
            if (producto == null)
            {
                return HttpNotFound();
            }

            return View(producto);
        }

        // GET: Producto/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Producto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nombre,Descripcion,Precio,Stock")] ProductoModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            List<ProductoModel> productos = ObtenerProductos();
            model.Id = productos.Any() ? productos.Max(p => p.Id) + 1 : 1;
            productos.Add(model);

            TempData["Mensaje"] = "Producto \"" + model.Nombre + "\" registrado correctamente.";
            return RedirectToAction("Index");
        }

        // GET: Producto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProductoModel producto = BuscarProducto(id.Value);
            if (producto == null)
            {
                return HttpNotFound();
            }

            return View(producto);
        }

        // POST: Producto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Descripcion,Precio,Stock")] ProductoModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ProductoModel producto = BuscarProducto(model.Id);
            if (producto == null)
            {
                return HttpNotFound();
            }

            producto.Nombre = model.Nombre;
            producto.Descripcion = model.Descripcion;
            producto.Precio = model.Precio;
            producto.Stock = model.Stock;

            TempData["Mensaje"] = "Producto \"" + producto.Nombre + "\" actualizado correctamente.";
            return RedirectToAction("Index");
        }

        // GET: Producto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProductoModel producto = BuscarProducto(id.Value);
            if (producto == null)
            {
                return HttpNotFound();
            }

            return View(producto);
        }

        // POST: Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            List<ProductoModel> productos = ObtenerProductos();
            ProductoModel producto = productos.FirstOrDefault(p => p.Id == id);
            if (producto == null)
            {
                return HttpNotFound();
            }

            productos.Remove(producto);

            TempData["Mensaje"] = "Producto \"" + producto.Nombre + "\" eliminado correctamente.";
            return RedirectToAction("Index");
        }
    }
}
