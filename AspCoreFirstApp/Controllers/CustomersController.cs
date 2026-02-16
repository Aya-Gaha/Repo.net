using AspCoreFirstApp.Models;
using AspCoreFirstApp.Repositories.Interfaces;
using AspCoreFirstApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspCoreFirstApp.Controllers;

public class CustomersController : Controller
{
    private readonly ICustomerService _customerService;
    private readonly IMembershipTypeRepository _membershipTypeRepository;

    public CustomersController(ICustomerService customerService, IMembershipTypeRepository membershipTypeRepository)
    {
        _customerService = customerService;
        _membershipTypeRepository = membershipTypeRepository;
    }

    public async Task<IActionResult> Index()
    {
        var customers = await _customerService.GetAllCustomersWithMembershipAsync();
        return View(customers);
    }

    public async Task<IActionResult> Details(int id)
    {
        var customer = await _customerService.GetCustomerByIdWithMembershipAsync(id);

        if (customer == null)
            return NotFound();

        return View(customer);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await PopulateMembershipTypesAsync();
        return View(new Customer());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Customer customer)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            await PopulateMembershipTypesAsync(customer.MembershipTypeId);
            return View(customer);
        }

        await _customerService.CreateCustomerAsync(customer);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null)
            return NotFound();

        await PopulateMembershipTypesAsync(customer.MembershipTypeId);
        return View(customer);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Customer customer)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            await PopulateMembershipTypesAsync(customer.MembershipTypeId);
            return View(customer);
        }

        if (!await _customerService.CustomerExistsAsync(customer.Id))
            return NotFound();

        await _customerService.UpdateCustomerAsync(customer);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var customer = await _customerService.GetCustomerByIdWithMembershipAsync(id);

        if (customer == null)
            return NotFound();

        return View(customer);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (!await _customerService.CustomerExistsAsync(id))
            return NotFound();

        await _customerService.DeleteCustomerAsync(id);
        return RedirectToAction(nameof(Index));
    }

    // LINQ Query Examples
    public async Task<IActionResult> HighDiscountCustomers()
    {
        var customers = await _customerService.GetCustomersWithHighDiscountAsync(20);
        return View("Index", customers);
    }

    public async Task<IActionResult> CustomerStats()
    {
        var stats = await _customerService.GetCustomerStatsByMembershipTypeAsync();
        return Json(stats);
    }

    // private async Task PopulateMembershipTypesAsync(int? selectedId = null)
    // {
    //     var membershipTypes = await _membershipTypeRepository
    //         .GetAllAsync()
    //         .OrderBy(mt => mt.Id)
    //         .ToList();
    //
    //     ViewBag.MembershipTypes = new SelectList(membershipTypes, "Id", "DiscountRate", selectedId);
    //
    //     ViewBag.MembershipTypes = new SelectList(membershipTypes, "Id", "DiscountRate", selectedId);
    // }
    private async Task PopulateMembershipTypesAsync(int? selectedId = null)
    {
        var membershipTypes = (await _membershipTypeRepository.GetAllAsync())
            .OrderBy(mt => mt.Id)
            .ToList();
    
        ViewBag.MembershipTypes = new SelectList(membershipTypes, "Id", "DiscountRate", selectedId);
    }

}
           
