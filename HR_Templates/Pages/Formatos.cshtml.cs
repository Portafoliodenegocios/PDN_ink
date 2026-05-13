using Azure.Core;
using DevExpress.Charts.Native;
using HR_Templates.Helpers;
using HR_Templates.Models;
using HR_Templates.Proxys;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static DevExpress.Xpo.Helpers.AssociatedCollectionCriteriaHelper;


namespace HR_Templates.Pages
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FormatosModel : PageModel
    {
        [BindProperty]
        public string Document_Option_Id { get; set; }

        [BindProperty]
        public bool Is_SSRS { get; set; }

        [BindProperty]
        public string _token { get; set; }

        public List<FormatoVM> Formatos { get; set; }

        private readonly IConfiguration _configuration;

        private readonly IWebHostEnvironment _env;

        public string  stoken { get; set; }
        public FormatosModel(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _configuration = configuration;
        }

        public async Task OnGet()
        {          

            ViewData["_Token"] = HttpContext.Request.Query["access_token"].ToString();
            _token = HttpContext.Request.Query["access_token"].ToString();

            var result = await  PDNInk_vw_Document_Type_OptionCBR
                               .Instance
                               .GetBy("Is_Active = True AND Document_Classification.Contains(\"Capital humano\")");

            Formatos = result
                             .Select(r => new FormatoVM
                                {
                                    Id = r.Document_Option_Id,
                                    Name = r.Document_Option ,
                                    Is_SSRS = r.Report_Type_Id == 1 ? true : false
                                })
                              .OrderBy(x => x.Name)
                              .ToList();
                            
        }

     

        public async Task<IActionResult> OnPost()
        {
            if (string.IsNullOrEmpty(Document_Option_Id))
            {
                ModelState.AddModelError("", "Selecciona un formato");
                return Page();
            }
          
              
            if (Is_SSRS)
               return RedirectToPage("VisorReport", new { access_token = _token, id = Document_Option_Id });
            else 
               return RedirectToPage("Spreadsheet", new { id = Document_Option_Id, Extension = "xlsx" });

       
               
            
        }

       

        private async Task<IActionResult> GetDocumentsClassification()
        {
            var result = await PDNInk_cat_Document_TypeCBR.Instance.GetBy("Is_Active = True AND Document_Classification_Id = 2");

            var data = result 
                .Select(r => new
                {
                    id = r.Document_Type_Id, 
                    name = r.Document_Type_Description   
                })
                .OrderBy(x => x.name);

            return  new JsonResult(data);
        }



    }
}
