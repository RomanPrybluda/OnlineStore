using Domain;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebAPI.Filters;

[ApiController]
[Produces("application/json")]
[Route("promotions")]

public class PromotionsController : ControllerBase
{
    private readonly PromotionService _promotionService;

    public PromotionsController(PromotionService promotionService)
    {
        _promotionService = promotionService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllPromotion([FromQuery] PromotionStatusFilter statusFilter = PromotionStatusFilter.All)
    {
        var promotions = await _promotionService.GetAllPromotionsAsync(statusFilter);
        return Ok(promotions);
    }

    [HttpGet("{id:Guid}")]
    public async Task<ActionResult> GetPromotionById(Guid id)
    {
        var promotion = await _promotionService.GetPromotionByIdAsync(id);
        return Ok(promotion);
    }

    [HttpPost]
    public async Task<ActionResult> CreatePromotion([FromForm] CreatePromotionDTO dto)
    {
        var promotion = await _promotionService.CreatePromotionAsync(dto);
        return Ok(promotion);
    }

    [HttpPut("{id:Guid}")]
    public async Task<ActionResult> UpdatePromotion(Guid id, [FromForm] UpdatePromotionDTO request)
    {
        var promotion = await _promotionService.UpdatePromotionAsync(id, request);
        return Ok(promotion);
    }

    [HttpPatch("{id:Guid}/status")]
    public async Task<ActionResult> UpdatePromotionStatus(Guid id, [FromForm][FromBody] bool isActive)
    {
        await _promotionService.UpdatePromotionStatusAsync(id, isActive);
        return NoContent();
    }

    [HttpDelete("{id:Guid}")]
    public async Task<ActionResult> DeletePromotion(Guid id)
    {
        await _promotionService.DeletePromotionAsync(id);
        return NoContent();
    }
}