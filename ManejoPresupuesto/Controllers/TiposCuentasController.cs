using Dapper;
using Microsoft.Data.SqlClient;
using ManejoPresupuesto.Models;
using Microsoft.AspNetCore.Mvc;
using ManejoPresupuesto.Servicios;

namespace ManejoPresupuesto.Controllers
{
	public class TiposCuentasController: Controller
	{
		private readonly IRepositoriosTiposCuentas repositoriosTiposCuentas;
		private readonly IServicioUsuarios servicioUsuarios;
	
		public TiposCuentasController(IRepositoriosTiposCuentas repositoriosTiposCuentas, IServicioUsuarios servicioUsuarios)
		{
			this.repositoriosTiposCuentas = repositoriosTiposCuentas;
			this.servicioUsuarios = servicioUsuarios;
		}

		public async Task<IActionResult> Index()
		{
			var usuarioId = servicioUsuarios.ObtenerUsuarioId();
			var tiposCuentas = await repositoriosTiposCuentas.Obtener(usuarioId);
			return View(tiposCuentas);
		}

		
		public IActionResult Crear()
		{


			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Crear(TipoCuenta tipoCuenta)
		{
			//ejecuta la accion del campo que tiene [required] del constructor TipoCuenta //
			//si el modelo no es valido -> ! es la negacion // 
			if (!ModelState.IsValid) 
			{
				return View(tipoCuenta); 
			}
			tipoCuenta.UsuarioId = servicioUsuarios.ObtenerUsuarioId();

			var yaExisteTipoCuenta =
				await repositoriosTiposCuentas.Existe(tipoCuenta.Nombre, tipoCuenta.UsuarioId);

			if (yaExisteTipoCuenta)
			{
				ModelState.AddModelError(nameof(tipoCuenta.Nombre),
					$"El nombre {tipoCuenta.Nombre} ya existe.");

				return View(tipoCuenta);
			}

			await repositoriosTiposCuentas.Crear(tipoCuenta);

			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<ActionResult> Editar(int id)
		{
			var usuarioId = servicioUsuarios.ObtenerUsuarioId();
			var tipoCuenta = await repositoriosTiposCuentas.ObtenerPorID(id, usuarioId);

			if (tipoCuenta is null)
			{
				return RedirectToAction("NoEncontrado", "Home");
			}
			return View(tipoCuenta);
		}

		[HttpPost]
		public async Task<ActionResult> Editar(TipoCuenta tipoCuenta)
		{
			var usuarioId = servicioUsuarios.ObtenerUsuarioId();
			var tipoCuentaExiste = await repositoriosTiposCuentas.ObtenerPorID(tipoCuenta.Id, usuarioId);

			if (tipoCuentaExiste is null)
			{
				return RedirectToAction("NoEncontrado", "Home");
			}

			await repositoriosTiposCuentas.Actualizar(tipoCuenta);
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Borrar(int id)
		{
			var usuarioId = servicioUsuarios.ObtenerUsuarioId();
			var tipoCuenta = await repositoriosTiposCuentas.ObtenerPorID(id, usuarioId);

			if (tipoCuenta is null)
			{
				return RedirectToAction("NoEncontrado", "Home");
			}
			return View(tipoCuenta);
		}

		[HttpPost]
		public async Task<IActionResult> BorrarTipoCuenta(int id)
		{
			var usuarioId = servicioUsuarios.ObtenerUsuarioId();
			var tipoCuenta = await repositoriosTiposCuentas.ObtenerPorID(id, usuarioId);

			if (tipoCuenta is null)
			{
				return RedirectToAction("NoEncontrado", "Home");
			}
			await repositoriosTiposCuentas.Borrar(id);
			return RedirectToAction("Index");
		}


		[HttpGet]

		public async Task<IActionResult> VerificarExisteTipoCuenta(string nombre)
		{
			var usuarioId = servicioUsuarios.ObtenerUsuarioId();
			var yaExisteTipoCuenta = await repositoriosTiposCuentas.Existe(nombre, usuarioId);

			if (yaExisteTipoCuenta)
			{
				return Json($"El nombre {nombre} ya esta registrado en la base de datos");
			}
			return Json(true);
		}


		[HttpPost]
		public async Task<IActionResult> Ordenar([FromBody] int[] ids)
		{
			var usuarioId = servicioUsuarios.ObtenerUsuarioId();
			var tiposCuentas = await repositoriosTiposCuentas.Obtener(usuarioId);
			var idsTiposCuentas = tiposCuentas.Select(x => x.Id);

			var idsTiposCuentasNoPerteneceAlUsuario = ids.Except(idsTiposCuentas).ToList();

			if (idsTiposCuentasNoPerteneceAlUsuario.Count > 0)
			{
				return Forbid();
			}

			var tiposCuentasOrdenados = ids.Select((valor, indice) => 
				new TipoCuenta() { Id = valor, Orden = indice +1 }).AsEnumerable();

			await repositoriosTiposCuentas.Ordenar(tiposCuentasOrdenados);

			return Ok();
		}
	}
}
