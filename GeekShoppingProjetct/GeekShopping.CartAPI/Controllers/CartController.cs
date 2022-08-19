using GeekShopping.CartAPI.Data.DTOs;
using GeekShopping.CartAPI.Messages;
using GeekShopping.CartAPI.RabbitMQSender;
using GeekShopping.CartAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CartAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]

   public class CartController : ControllerBase
    {
        private ICartRepository _cartRepository;
        private ICouponRepository _couponRepository;
        private IRabbitMQMessageSender _rabbitMQMessageSender;

        public CartController(ICartRepository cartRepository, ICouponRepository couponRepository, IRabbitMQMessageSender rabbitMQMessageSender)
        {
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
            _couponRepository = couponRepository ?? throw new ArgumentNullException(nameof(couponRepository));
            _rabbitMQMessageSender = rabbitMQMessageSender ?? throw new ArgumentNullException(nameof(rabbitMQMessageSender));
        }

        [HttpGet("find-cart/{id}")]
        public async Task<ActionResult<CartDto>> FindbyId(string id)
        {
            var cart = await _cartRepository.FindCartByUserId(id);
            if(cart == null)
            {
                return NotFound();
            }
            return Ok(cart);
        }

        [HttpPost("add-cart")]
        public async Task<ActionResult<CartDto>> AddCart(CartDto dto)
        {
            var cart = await _cartRepository.SaveOrUpdateCart(dto);
            if (cart == null)
            {
                return NotFound();
            }
            return Ok(cart);
        }

        [HttpPut("update-cart")]
        public async Task<ActionResult<CartDto>> UpdateCart(CartDto dto)
        {
            var cart = await _cartRepository.SaveOrUpdateCart(dto);
            if (cart == null)
            {
                return NotFound();
            }
            return Ok(cart);
        }

        [HttpDelete("remove-cart/{id}")]
        public async Task<ActionResult<CartDto>> RemoveCart(int id)
        {
            var status = await _cartRepository.RemoveFromCart(id);
            if (!status)
            {
                return BadRequest();
            }
            return Ok(status);
        }

        [HttpPost("apply-coupon")]
        public async Task<ActionResult<CartDto>> ApplyCoupon(CartDto dto)
        {
            var status = await _cartRepository.ApplyCoupon(dto.CartHeader.UserId, dto.CartHeader.CouponCode);
            if (!status) return NotFound();
            return Ok(status);
        }

        [HttpDelete("remove-coupon/{userId}")]
        public async Task<ActionResult<CartDto>> ApplyCoupon(string userId)
        {
            var status = await _cartRepository.RemoveCoupon(userId);
            if (!status) return NotFound();
            return Ok(status);
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<CheckoutHeaderDTO>> Checkout(CheckoutHeaderDTO dto)
        {
            string token = Request.Headers["Authorization"];
            if (dto?.UserId == null) return BadRequest();

            var cart = await _cartRepository.FindCartByUserId(dto.UserId);
            if (cart == null) return NotFound();

            if(!string.IsNullOrEmpty(dto.CouponCode))
            {
                CouponDTO coupon = await _couponRepository.GetCoupon(dto.CouponCode, token);
                if(dto.DiscountAmount != coupon.DiscountAmount)
                {
                    return StatusCode(412);
                }
            }

            dto.CartDetails = cart.CartDetails;
            dto.DateTime = DateTime.Now;

            // rabbitMQ();
            _rabbitMQMessageSender.SendMessage(dto, "checkoutqueue");

            await _cartRepository.ClearCart(dto.UserId);

            return Ok(dto);
        }
    }
}


