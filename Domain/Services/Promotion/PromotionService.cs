using DAL;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class PromotionService
    {
        private readonly OnlineStoreDbContext _context;
        private readonly ImageService _imageService;

        public PromotionService(OnlineStoreDbContext context, ImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public async Task<IEnumerable<PromotionDTO>> GetAllPromotionsAsync(PromotionStatusFilter statusFilter)
        {
            var promotionsQuery = _context.Promotions.AsQueryable();

            switch (statusFilter)
            {
                case PromotionStatusFilter.ActiveOnly:
                    promotionsQuery = promotionsQuery.Where(p => p.IsActive);
                    break;
                case PromotionStatusFilter.InactiveOnly:
                    promotionsQuery = promotionsQuery.Where(p => !p.IsActive);
                    break;
                case PromotionStatusFilter.All:
                default:
                    break;
            }

            var promotions = await promotionsQuery.ToListAsync();

            if (!promotions.Any())
                throw new CustomException(CustomExceptionType.NotFound, "Promotions not found");

            var promotionDTOs = new List<PromotionDTO>();

            foreach (var promotion in promotions)
            {
                var promotionDTO = PromotionDTO.FromPromotion(promotion);
                promotionDTOs.Add(promotionDTO);
            }

            return promotionDTOs;
        }

        public async Task<PromotionByIdDTO> GetPromotionByIdAsync(Guid id)
        {
            var promotionById = await _context.Promotions.FindAsync(id);

            if (promotionById == null)
                throw new CustomException(CustomExceptionType.NotFound,
                    $"Promotion not found with ID {id}");

            var promotionByIdDTO = PromotionByIdDTO.FromPromotion(promotionById);

            return promotionByIdDTO;
        }

        public async Task<PromotionDTO> CreatePromotionAsync(CreatePromotionDTO request)
        {
            var existingPromotion = await _context.Promotions
                .FirstOrDefaultAsync(c => c.Name == request.Name);

            if (existingPromotion != null)
                throw new CustomException(CustomExceptionType.IsAlreadyExists,
                    $"Category '{request.Name}' already exists.");

            string imageBaseName = string.Empty;
            if (request.Banner != null)
                imageBaseName = await _imageService.UploadImageAsync(request.Banner);

            var promotion = CreatePromotionDTO.ToPromotion(request, imageBaseName);

            _context.Promotions.Add(promotion);
            await _context.SaveChangesAsync();

            var createdPromotion = await _context.Promotions.FindAsync(promotion.Id);

            if (createdPromotion == null)
                throw new CustomException(CustomExceptionType.NotFound,
                    "The promotion could not be found after creation.");

            var promotionDTO = PromotionDTO.FromPromotion(createdPromotion);

            return promotionDTO;
        }

        public async Task<PromotionDTO> UpdatePromotionAsync(Guid id, UpdatePromotionDTO request)
        {
            var promotionById = await _context.Promotions.FindAsync(id);

            if (promotionById == null)
                throw new CustomException(CustomExceptionType.NotFound,
                    $"Promotion not found with ID {id}");

            var existingPromotion = await _context.Promotions
                .FirstOrDefaultAsync(c => c.Name == request.Name && c.Id != id);

            if (existingPromotion != null)
                throw new CustomException(CustomExceptionType.IsAlreadyExists,
                    $"Category '{request.Name}' already exists.");

            string imageBaseName = string.Empty;
            if (request.Banner != null)
                imageBaseName = await _imageService.UploadImageAsync(request.Banner);

            UpdatePromotionDTO.ToPromotion(request, imageBaseName, promotionById);
            
            await _context.SaveChangesAsync();

            var createdPromotion = await _context.Promotions.FindAsync(id);

            if (createdPromotion == null)
                throw new CustomException(CustomExceptionType.NotFound,
                    "The promotion could not be found after creation.");

            var promotionDTO = PromotionDTO.FromPromotion(createdPromotion);

            return promotionDTO;
        }

        public async Task<PromotionDTO> UpdatePromotionStatusAsync(Guid id, bool isActive)
        {

            var promotion = await _context.Promotions.FindAsync(id);

            if (promotion == null)
                throw new CustomException(CustomExceptionType.NotFound, "Promotion not found");

            promotion.IsActive = isActive;

            await _context.SaveChangesAsync();

            var promotionDTO = PromotionDTO.FromPromotion(promotion);
            return promotionDTO;
        }

        public async Task DeletePromotionAsync(Guid id)
        {
            var promotion = await _context.Promotions.FindAsync(id);

            if (promotion == null)
                throw new CustomException(CustomExceptionType.NotFound,
                    $"Promotion not found with ID {id}");

            _context.Promotions.Remove(promotion);
            await _context.SaveChangesAsync();
        }

    }
}
