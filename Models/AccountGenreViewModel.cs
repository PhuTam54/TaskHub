using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskHub.Models
{
    public class AccountGenreViewModel
    {
        public List<Account>? Accounts { get; set; }
        public SelectList? Genres { get; set; }
        public string? AccountGenre { get; set; }
        public string? SearchString { get; set; }
    }
}
