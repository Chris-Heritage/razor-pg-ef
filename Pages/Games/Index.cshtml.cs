using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using razor_pg_ef.Models;

namespace razor_pg_ef.Pages.Games
{
    public class IndexModel : PageModel
    {
        private readonly StoreGameContext _context;

        [BindProperty(SupportsGet = true)]
        public string TitleFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool Before { get; set; } = false;

        [BindProperty(SupportsGet = true)]
        public DateTime ReleaseDate { get; set; }


        private readonly ILogger<IndexModel> _logger;

        public IndexModel(StoreGameContext context, ILogger<IndexModel> logger)
        {

            _context = context;
            _logger = logger;
        }

        public IList<Game> Game { get; set; }

        public async Task OnGetAsync()
        {
            _logger.Log(LogLevel.Information, TitleFilter);

            var games = from g in _context.Game select g;

            if (!string.IsNullOrEmpty(TitleFilter))
            {
                games = games.Where(g => g.Title.ToLower().Contains(TitleFilter.ToLower()));
            }
            games = games.Where(g =>
             Before ? g.ReleaseDate <= ReleaseDate :
             g.ReleaseDate >= ReleaseDate);

            Game = await games.ToListAsync();
        }
    }
}
