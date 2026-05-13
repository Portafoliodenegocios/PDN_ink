using HR_Templates.Proxys;
using Microsoft.AspNetCore.Mvc;
using Pdnink.Controllers;
using Pdnink.Models;
using PDNInk;
using Pdnink_Coremvc.Helpers;
using Pdnink_Coremvc.Models;
using System.Diagnostics;

namespace Pdnink_Coremvc.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IConfiguration _config;

        public HomeController(IConfiguration config)
        {
            _config = config;
        }
        public IActionResult Check(string Username)
        {
            var decodedUserName = Username.FromBase64().ToLower();
            //Check if the username in Claims is the same of the requested login. 
            if (decodedUserName != (UserName ?? "").ToLower())
            {
                return RedirectToAction("Logout", "Account");
            }

            return RedirectToAction("Index");

        }

        public IActionResult Index()
        {

            string token_ = Request.Cookies["PdnInkToken"];

            ViewData["TokenRH"] = token_;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult originacion()
        {

            string token_ = Request.Cookies["PdnInkToken"];

            ViewData["TokenRH"] = token_;

            string filtro = "";
            var listaDocumentos = PDNInk_vw_Document_UserCBR.Instance.GetBy(filtro);

            var listaTest = new List<OriginacionModelTable>();
            for (int i = 0; i < 10; i++)
            {
                var Test = new OriginacionModelTable()
                {
                    Origin_System = "Brokers/Visitas",
                    solicitud_number = ((byte)(i + 1)),
                    credit_number = ((byte)(i + 1)),
                    Expedient_number = ((byte)(i + 1)),
                    ComercialName = $"Razon {(i + 1)}",
                    Document = "FV-SEG-FISCAL-CARRETERA A SAN ISIDRO MAZACANTEPEC NÚMERO 5215, INTERIOR A 3, COLONIA LOS TEPETATES, MUNICIPIO TLAJOMULCO DE ZÚÑIGA, J-24132.pdf",
                    Create_Date = DateTime.Now,
                    Create_User = "Marco Antonio Palacios Aguirre",
                    Date = DateTime.Now,
                    User = "usrcrtlderiesgo Ambiente de Pruebas",
                    Signatures = "usrtestcerrador Pruebas Brokers"


                };

                listaTest.Add(Test);
            }

            return View(listaTest);

        }

        public IActionResult generales()
        {

            string token_ = Request.Cookies["PdnInkToken"];

            ViewData["TokenRH"] = token_;
            string filtro = "";
            var listaDocumentos = PDNInk_vw_Document_UserCBR.Instance.GetBy(filtro);

            var listTest = new List<GeneralesModelTable>();
            for (int i = 0; i < 10; i++)
            {
                var Tes = new GeneralesModelTable()
                {
                    Document_Name = "Brokers/Visitas",
                    Id_Document = "DOC-2026-001",
                    Creator = "David",
                    Owner = "David",
                    Participants = "\t\r\nAnalista Gerente",
                    Create_Date = DateTime.Now,
                    Status = "Enviado"
                };
                listTest.Add(Tes);
            }
            return View(listTest);
        }

        public async Task<IActionResult> carpetas()
        {

            string token_ = Request.Cookies["PdnInkToken"];

            ViewData["TokenRH"] = token_;
            string filtro = "";
            var listaDocumentos = await PDNInk_vw_Document_User_FolderCBR.Instance.GetBy(filtro);
            return View("_ListaDocumentos", listaDocumentos);

        } 
    
        public IActionResult vistos()
        {
            string token_ = Request.Cookies["PdnInkToken"];

            ViewData["TokenRH"] = token_;
            return View();

        }

        public IActionResult dashboard()
        {
            string token_ = Request.Cookies["PdnInkToken"];

            ViewData["TokenRH"] = token_;
            return View();

        }




    }
}
