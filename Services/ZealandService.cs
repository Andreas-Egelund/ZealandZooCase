using ZealandZooCase.Data;
using ZealandZooCase.Models;

namespace ZealandZooCase.Services
{
    public class ZealandService
    {
        private readonly ZealandDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor; // Add IHttpContextAccessor  // SKAL FINDE UD AF HVA FUCK DEN GØR - ANDREAS E

        public ZealandService(ZealandDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor; // Initialize IHttpContextAccessor
        }

        public User SetCurrentUser()
        {
            if(_httpContextAccessor.HttpContext?.Session.GetString("Username") != null)
            {
                string username = _httpContextAccessor.HttpContext?.Session.GetString("Username");

                return _context.Users.FirstOrDefault(u => u.UserName == username);

            }
            // Use _httpContextAccessor.HttpContext to access HttpContext

            return null; //Kan give problemer da username helst ikke skal være null. Måske default til "Guest"?
        }
    }
}
