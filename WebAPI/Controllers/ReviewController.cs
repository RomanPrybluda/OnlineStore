using Domain;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAPI
{
    [ApiController]
    [Produces("application/json")]
    [Route("reviews")]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewService _reviewService;

        public ReviewController(ReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("products/{productId:Guid}")]
        public async Task<ActionResult> GetReviewsByProductAsync([Required] Guid productId)
        {
            var reviews = await _reviewService.GetReviewsByProductIdAsync(productId);
            return Ok(reviews);
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult> GetReviewByIdAsync([Required] Guid id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            return Ok(review);
        }

        [HttpPost]
        public async Task<ActionResult> CreateReviewAsync([FromForm][FromBody] CreateReviewDTO request)
        {
            var review = await _reviewService.CreateReviewAsync(request);
            return Ok(review);
        }

        [HttpPut("{id:Guid}")]
        public async Task<ActionResult> UpdateReviewAsync([Required] Guid id, [FromForm][FromBody] UpdateReviewDTO request)
        {
            var updatedReview = await _reviewService.UpdateReviewAsync(id, request);
            return Ok(updatedReview);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteReviewAsync([Required] Guid id)
        {
            await _reviewService.DeleteReviewAsync(id);
            return NoContent();
        }
    }
}
