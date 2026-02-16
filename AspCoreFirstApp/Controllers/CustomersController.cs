using AspCoreFirstApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AspCoreFirstApp.Controllers;

public class CustomersController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public CustomersController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult Index()
    {
        var customers = _dbContext.Customers
            .Include(c => c.MembershipType)
            .ToList();

        return View(customers);
    }

    public IActionResult Details(int id)
    {
        var customer = _dbContext.Customers
            .Include(c => c.MembershipType)
            .FirstOrDefault(c => c.Id == id);

        if (customer == null)
            return NotFound();

        return View(customer);
    }

    [HttpGet]
    public IActionResult Create()
    {
        PopulateMembershipTypes();
        return View(new Customer());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Customer customer)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            PopulateMembershipTypes(customer.MembershipTypeId);
            return View(customer);
        }

        _dbContext.Customers.Add(customer);
        _dbContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var customer = _dbContext.Customers.Find(id);
        if (customer == null)
            return NotFound();

        PopulateMembershipTypes(customer.MembershipTypeId);
        return View(customer);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Customer customer)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            PopulateMembershipTypes(customer.MembershipTypeId);
            return View(customer);
        }

        var customerInDb = _dbContext.Customers.FirstOrDefault(c => c.Id == customer.Id);
        if (customerInDb == null)
            return NotFound();

        customerInDb.Name = customer.Name;
        customerInDb.MembershipTypeId = customer.MembershipTypeId;

        _dbContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var customer = _dbContext.Customers
            .Include(c => c.MembershipType)
            .FirstOrDefault(c => c.Id == id);

        if (customer == null)
            return NotFound();

        return View(customer);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var customer = _dbContext.Customers.Find(id);
        if (customer == null)
            return NotFound();

        _dbContext.Customers.Remove(customer);
        _dbContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    private void PopulateMembershipTypes(int? selectedId = null)
    {
        var membershipTypes = _dbContext.MembershipTypes
            .OrderBy(mt => mt.Id)
            .ToList();

        ViewBag.MembershipTypes = new SelectList(membershipTypes, "Id", "DiscountRate", selectedId);
    }
}