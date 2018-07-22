using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mehrsan.Dal.DB;

namespace Mehrsan.Core.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly WordEntities _context;

        public AccountController(WordEntities context)
        {
            _context = context;
        }

        // GET: Account
        public async Task<IActionResult> Index()
        {
            return View(await _context.AspNetUserClaims.ToListAsync());
        }

        // GET: Account/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUserClaim = await _context.AspNetUserClaims
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspNetUserClaim == null)
            {
                return NotFound();
            }

            return View(aspNetUserClaim);
        }

        // GET: Account/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Account/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,ClaimType,ClaimValue")] AspNetUserClaim aspNetUserClaim)
        {
            if (ModelState.IsValid)
            {

                _context.Add(aspNetUserClaim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aspNetUserClaim);
        }

        // GET: Account/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUserClaim = await _context.AspNetUserClaims.FindAsync(id);
            if (aspNetUserClaim == null)
            {
                return NotFound();
            }
            return View(aspNetUserClaim);
        }

        // POST: Account/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,ClaimType,ClaimValue")] AspNetUserClaim aspNetUserClaim)
        {
            if (id != aspNetUserClaim.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aspNetUserClaim);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AspNetUserClaimExists(aspNetUserClaim.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(aspNetUserClaim);
        }

        public List<AspNetUserClaim> GetUserClaims()
        {
            var result = Use
        }
            // GET: Account/Delete/5
            public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUserClaim = await _context.AspNetUserClaims
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspNetUserClaim == null)
            {
                return NotFound();
            }

            return View(aspNetUserClaim);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aspNetUserClaim = await _context.AspNetUserClaims.FindAsync(id);
            _context.AspNetUserClaims.Remove(aspNetUserClaim);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AspNetUserClaimExists(int id)
        {
            return _context.AspNetUserClaims.Any(e => e.Id == id);
        }
    }
}
