using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HR_Templates.Pages
{
    public class IndexxModel : PageModel
    {
        private readonly ILogger<IndexxModel> _logger;

        public IndexxModel(ILogger<IndexxModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
