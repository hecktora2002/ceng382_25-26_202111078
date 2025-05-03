using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RAZOR_PAGE_KOPYASI.Pages
{
    public class TableModel : PageModel
    {
        public string? DisplayUsername { get; set; }
        public string? DisplayToken { get; set; }
        public string? DisplaySessionId { get; set; }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            var sessionUsername = HttpContext.Session.GetString("username");
            var sessionToken = HttpContext.Session.GetString("token");
            var sessionId = HttpContext.Session.GetString("session_id");

            var cookieUsername = HttpContext.Request.Cookies["username"];
            var cookieToken = HttpContext.Request.Cookies["token"];
            var cookieSessionId = HttpContext.Request.Cookies["session_id"];

            bool isValid =
                !string.IsNullOrEmpty(sessionUsername) &&
                !string.IsNullOrEmpty(cookieUsername) &&
                sessionUsername == cookieUsername &&
                !string.IsNullOrEmpty(sessionToken) &&
                !string.IsNullOrEmpty(cookieToken) &&
                sessionToken == cookieToken &&
                !string.IsNullOrEmpty(sessionId) &&
                !string.IsNullOrEmpty(cookieSessionId) &&
                sessionId == cookieSessionId;

            if (!isValid)
            {
                context.Result = new RedirectToPageResult("/Login");
                return; // güvenli çıkış
            }

            // Session bilgilerini sayfada göstermek için ViewModel'e aktarıyoruz
            DisplayUsername = sessionUsername;
            DisplayToken = sessionToken;
            DisplaySessionId = sessionId;

            base.OnPageHandlerExecuting(context);
        }

        public void OnGet()
        {
            // Tablo verileri buraya gelecek
        }
    }
}
