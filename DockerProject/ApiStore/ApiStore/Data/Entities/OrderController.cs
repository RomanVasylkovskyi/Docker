using ApiStore.Data;
using ApiStore.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ApiStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ApiStoreDbContext _context;

        public OrderController(ApiStoreDbContext context)
        {
            _context = context;
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder(int userId)
        {
            var basket = await _context.Baskets
                .Include(b => b.BasketItems)
                .ThenInclude(bi => bi.Product)
                .FirstOrDefaultAsync(b => b.UserId == userId);

            if (basket == null || !basket.BasketItems.Any())
            {
                return BadRequest("Basket is empty or not found.");
            }

            var order = new OrderEntity
            {
                UserId = userId,
                OrderItems = basket.BasketItems.Select(bi => new OrderItemEntity
                {
                    ProductId = bi.ProductId,
                    Quantity = bi.Quantity,
                    Price = bi.Product.Price
                }).ToList(),
                TotalAmount = basket.BasketItems.Sum(bi => bi.Quantity * bi.Product.Price)
            };

            _context.Orders.Add(order);

            // Clear the user's basket
            _context.Baskets.Remove(basket);
            await _context.SaveChangesAsync();

            return Ok(new { OrderId = order.OrderId, TotalAmount = order.TotalAmount });
        }

        [HttpGet("get-orders")]
        public async Task<IActionResult> GetOrders(int userId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();

            if (!orders.Any())
            {
                return NotFound("No orders found.");
            }

            return Ok(orders.Select(o => new
            {
                o.OrderId,
                o.OrderDate,
                o.TotalAmount,
                Items = o.OrderItems.Select(oi => new
                {
                    oi.ProductId,
                    oi.Product.Name,
                    oi.Quantity,
                    oi.Price
                })
            }));
        }
    }
}