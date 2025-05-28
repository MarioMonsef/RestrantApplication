using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestrantApplication.Core.Models.Order;
using RestrantApplication.Core.Services;

namespace RestrantApplication.MVC.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IUserContextService _userContextService;

        private string CurrentUserId => _userContextService.GetCurrentUserId();
        private string CurrentUserRole => _userContextService.GetRoleCurrentUser();

        public OrderController(IOrderService orderService, IUserContextService userContextService)
        {
            _orderService = orderService;
            _userContextService = userContextService;
        }

        // Get all orders for the logged-in user
        public async Task<IActionResult> GetAllUserOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync(CurrentUserId);
            return View(orders);
        }

        // Confirm an order for current user
        public async Task<IActionResult> ConfirmOrder(OrderType orderType)
        {
            bool isCreated = await _orderService.CreateOrderAsync(CurrentUserId, orderType);
            if (!isCreated)
            {
                TempData["ErrorMessage"] = "Failed to create the order.";
                return RedirectToAction("GetAllUserOrders");
            }

            TempData["SuccessMessage"] = "Order created successfully.";
            return RedirectToAction("GetAllUserOrders");
        }

        // Change the state of an order, role-based redirect
        public async Task<IActionResult> ChangeOrderState(int orderId, OrderState orderState)
        {
            bool isChanged = await _orderService.ChangeOrderState(orderId, CurrentUserId, orderState, CurrentUserRole);

            if (!isChanged)
            {
                TempData["ErrorMessage"] = "Failed to change order state.";
            }
            else
            {
                TempData["SuccessMessage"] = "Order state changed successfully.";
            }

            if (CurrentUserRole == "Chef" || string.IsNullOrEmpty(CurrentUserRole))
                return RedirectToAction("GetAllOrderDisplayForChef");
            if (CurrentUserRole == "Delivery Boy" || string.IsNullOrEmpty(CurrentUserRole))
                return RedirectToAction("GetAllOrdersDisplayForDelivery");
            if (CurrentUserRole == "Manger" || string.IsNullOrEmpty(CurrentUserRole))
                return RedirectToAction("GetAllOrdersThroughout24HoursForEmployees");

            return RedirectToAction("GetAllUserOrders");
        }

        // Get detailed order info
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {
            var order = await _orderService.GetOrderDetailsAsync(orderId);
            if (order == null)
            {
                return Unauthorized();
            }
            return View(order);
        }

        // Get orders assigned to chefs
        public async Task<IActionResult> GetAllOrderDisplayForChef()
        {
            var orders = await _orderService.GetAllOrdersAsync(null, isChef: true);
            return View(orders);
        }

        // Get orders assigned to delivery staff
        public async Task<IActionResult> GetAllOrdersDisplayForDelivery()
        {
            var orders = await _orderService.GetAllOrdersAsync(null, isDelivary: true);
            return View(orders);
        }

        // Get all orders within last 24h for employees
        public async Task<IActionResult> GetAllOrdersThroughout24HoursForEmployees()
        {
            var orders = await _orderService.GetAllOrdersAsync(null);
            return View(orders);
        }
    }
}