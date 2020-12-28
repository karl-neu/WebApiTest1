using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTest1.Models;

namespace WebApiTest1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderContext _context;

        public OrdersController(OrderContext context)
        {
            _context = context;
            var loc = new Location() { Latitude = 1.5d, Longitude = 1.5d };

            _context.OrderResponses.Add(
                new OrderResponse()
                {
                    Dimension = "Some dimension",
                    Pickup = loc,
                    DropOff = loc
                });
            _context.SaveChanges();
        }

        // GET: api/Orders
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrderResponses(int start, int quantity, string status)
        {
            var res = await _context.OrderResponses
                                    .Skip(start)
                                    .Take(quantity)
                                    .Include(or => or.Pickup)
                                    .Include(or => or.DropOff)
                                    .Where(s => s.Status == status)
                                    .ToListAsync();

            if (res == null || res.Count < 1)
            {
                return NoContent();
            }
            return Ok(res);
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<ActionResult<OrderResponse>> GetOrderResponse(string id)
        {
            var orderResponse = await _context.OrderResponses.FindAsync(id);

            if (orderResponse == null)
            {
                return NotFound();
            }

            return Ok(orderResponse);
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [Authorize]
        public async Task<ActionResult<OrderResponse>> PostOrderResponse(OrderRequest orderRequest)
        {
            OrderResponse resp = new OrderResponse();

            try
            {
                resp.Dimension = orderRequest.Dimension;
                resp.Pickup = orderRequest.Pickup;
                resp.DropOff = orderRequest.DropOff;

                _context.OrderResponses.Add(resp);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return UnprocessableEntity(new Error()
                {
                    Code = 1050,
                    Message = "Invalid data",
                    Details = "Invalid location field in incoming object"
                });
            }

            return CreatedAtAction(nameof(GetOrderResponse), new { id = resp.Id }, resp);
        }

        private bool OrderResponseExists(string id)
        {
            return _context.OrderResponses.Any(e => e.Id == int.Parse(id));
        }
    }
}