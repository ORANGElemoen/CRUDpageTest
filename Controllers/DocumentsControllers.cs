using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUDpageTest.Data;
using CRUDpageTest.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using System.Linq;

namespace CRUDpageTest.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DocumentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Index action method with filter parameter
        public async Task<IActionResult> Index(string filter)
        {
            // Fetch all documents initially
            var documents = from d in _context.Documents select d;

            // Apply filter if the selected value is not "All" or empty
            if (!string.IsNullOrEmpty(filter) && filter != "All")
            {
                documents = documents.Where(d => d.Author == filter);
            }

            // Fetch distinct authors for the dropdown menu
            var authors = await _context.Documents.Select(d => d.Author).Distinct().ToListAsync();
            ViewBag.Authors = new SelectList(authors);

            // Return the view with the filtered or unfiltered list of documents
            return View(await documents.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Author,CreatedDate,UploadedFile")] Document document)
        {
            if (ModelState.IsValid)
            {
                if (document.UploadedFile != null)
                {
                    using var memoryStream = new MemoryStream();
                    await document.UploadedFile.CopyToAsync(memoryStream);
                    document.FileData = memoryStream.ToArray();
                    document.FileName = document.UploadedFile.FileName;
                }

                _context.Add(document);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(document);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }
            return View(document);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,CreatedDate,UploadedFile")] Document document)
        {
            if (id != document.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (document.UploadedFile != null)
                    {
                        using var memoryStream = new MemoryStream();
                        await document.UploadedFile.CopyToAsync(memoryStream);
                        document.FileData = memoryStream.ToArray();
                        document.FileName = document.UploadedFile.FileName;
                    }

                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.Id))
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
            return View(document);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document != null)
            {
                _context.Documents.Remove(document);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DownloadFile(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (document == null || document.FileData == null || document.FileName == null)
            {
                return NotFound();
            }

            return File(document.FileData, "application/octet-stream", document.FileName);
        }


        private bool DocumentExists(int id)
        {
            return _context.Documents.Any(e => e.Id == id);
        }
    }
}
