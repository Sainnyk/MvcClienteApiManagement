using MvcClienteApiManagement.Models;
using System.Net.Http.Headers;
using System.Web;

namespace MvcClienteApiManagement.Services
{
    public class ServiceApiManagement
    {
        private string ApiUrlEmpleados;
        private string ApiUrlDepartamentos;
        private MediaTypeWithQualityHeaderValue header;

        public ServiceApiManagement(string urlemp, string urldept)
        {
            this.ApiUrlDepartamentos = urldept;
            this.ApiUrlEmpleados = urlemp;
            this.header = new MediaTypeWithQualityHeaderValue("application/json");
        }

        //ACCESO A LA API GRATUITA
        public async Task<List<Empleado>> GetEmpleadosAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                //Tenemos que modificar el Request. Debemos enviar al final una cadena vacia.
                var queryString = HttpUtility.ParseQueryString(string.Empty);
                string request = "/api/empleados?" + queryString;
     
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                //Debemos indicar que no utilizamos caché para las peticiones
                client.DefaultRequestHeaders.CacheControl = CacheControlHeaderValue.Parse("no-cache");
                //La peticion al api management se realiza con la url y el request a la vez, no separados (no se usa BaseAddress)
                HttpResponseMessage response = await client.GetAsync(this.ApiUrlDepartamentos + request);
                if(response.IsSuccessStatusCode)
                {
                    List<Empleado> empleados = await response.Content.ReadAsAsync<List<Empleado>>();
                    return empleados;
                }
                else
                {
                    return null;
                }
            }
        }

        //ACESO A LA API DE PAGO CON SUSCRIPCION
        public async Task<List<Departamento>> GetDepartamentosAsync(string suscripcion)
        {
            using (HttpClient client = new HttpClient())
            {

                var queryString = HttpUtility.ParseQueryString(string.Empty);
                string request = "/api/departamentos?" + queryString;

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);

                client.DefaultRequestHeaders.CacheControl = CacheControlHeaderValue.Parse("no-cache");
                //AÑADIMOS NUESTRA CLAVE DE SUSCRIPCION
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Ke", suscripcion); //Las dos comillas indican lo que vamos a enviar (Header-name de la key en la suscripcion en Azure)
       
                HttpResponseMessage response = await client.GetAsync(this.ApiUrlDepartamentos + request);
                if (response.IsSuccessStatusCode)
                {
                    List<Departamento> departamentos = await response.Content.ReadAsAsync<List<Departamento>>();
                    return departamentos;
                }
                else
                {
                    return null;
                }
            }
        }


    }
}
