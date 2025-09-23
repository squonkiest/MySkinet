﻿using Microsoft.AspNetCore.Mvc;
using Skinet.Core.Entities;
using Skinet.Core.Interfaces;

namespace Skinet.API.Controllers
{
    public class CartController(ICartService cartService) : BaseApiController
    {
        private readonly ICartService _cartService = cartService;

        [HttpGet]
        public async Task<ActionResult<ShoppingCart>> GetCartById(string id)
        {
            ShoppingCart? cart = await _cartService.GetCartAsync(id);

            return Ok(cart ?? new ShoppingCart { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateCart(ShoppingCart cart)
        {
            ShoppingCart? updatedCart = await _cartService.SetCartAsync(cart);

            if (updatedCart == null)
            {
                return BadRequest("Failed to update Shopping Cart.");
            }

            return Ok(updatedCart);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteCart(string id)
        {
            bool result = await _cartService.DeleteCartAsync(id);

            if (!result)
            {
                return BadRequest("Failed to delete Shopping Cart.");
            }

            return Ok();
        }
    }
}
