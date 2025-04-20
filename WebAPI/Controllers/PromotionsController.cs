using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

[ApiController]
[Produces("application/json")]
[Route("promotions")]
[Authorize(Roles = "Manager")]
public class PromotionsController : ControllerBase
{
    private readonly PromotionService _promotionService;

    public PromotionsController(PromotionService promotionService)
    {
        _promotionService = promotionService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllPromotion([FromQuery] bool includeInactive = false)
    {
        var promotions = await _promotionService.GetAllPromotionsAsync(includeInactive);
        return Ok(promotions);
    }

    [HttpGet("{id:Guid}")]
    public async Task<ActionResult> GetPromotionById(Guid id)
    {
        var promotion = await _promotionService.GetPromotionByIdAsync(id);
        return Ok(promotion);
    }

    [HttpPost]
    public async Task<ActionResult> CreatePromotion([FromForm][Required] CreatePromotionDTO dto)
    {
        var promotion = await _promotionService.CreatePromotionAsync(dto);
        return Ok(promotion);
    }

    [HttpPut("{id:Guid}")]
    public async Task<ActionResult> UpdatePromotion(Guid id, [FromForm][Required] UpdatePromotionDTO dto)
    {
        var promotion = await _promotionService.UpdatePromotionAsync(id, dto);
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