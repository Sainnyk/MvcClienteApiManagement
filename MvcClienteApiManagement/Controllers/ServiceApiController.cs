using Microsoft.AspNetCore.Mvc;
using MvcClienteApiManagement.Models;
using MvcClienteApiManagement.Services;

namespace MvcClienteApiManagement.Controllers
{
    public class ServiceApiController : Controller
    {
        private ServiceApiManagement service;
        public ServiceApiController(ServiceApiManagement service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Empleados()
        {
            List<Empleado> empleados = await this.service.GetEmpleadosAsync();
            return View(empleados);
        }

        public IActionResult Departamentos()
        {
 
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Departamentos(string suscripcion)
        {
            List<Departamento> departamentos = await this.service.GetDepartamentosAsync(suscripcion);
            return View(departamentos);
        }
    }
}
