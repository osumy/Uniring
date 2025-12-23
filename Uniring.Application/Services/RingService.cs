using Uniring.Application.Interfaces;
using Uniring.Application.Interfaces.Repositories;
using Uniring.Contracts.ApiResult;
using Uniring.Contracts.Ring;
using Uniring.Domain.Entities;

namespace Uniring.Application.Services
{
    public class RingService : IRingService
    {
        private readonly IRingRepository _ringRepository;
        private readonly IMediaRepository _mediaRepository;

        public RingService(IRingRepository ringRepository, IMediaRepository mediaRepository)
        {
            _ringRepository = ringRepository;
            _mediaRepository = mediaRepository;
        }

        public async Task<Result<RingResponse>> CreateRingAsync(RingRegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return Result<RingResponse>.Error("Name is required.");

            // Serial Generation
            if (await _ringRepository.ExistsBySerialAsync(request.Serial))
                return Result<RingResponse>.Error("A ring with this serial already exists.");

            // Uid Generation
            string uid = GenerateUniqueUid();

            var ring = new Ring
            {
                Id = Guid.NewGuid(),
                Uid = uid,
                Name = request.Name.Trim(),
                Serial = request.Serial.Trim(),
                Description = request.Description?.Trim()
            };

            await _ringRepository.AddAsync(ring);
            await _ringRepository.UnitOfWork.SaveChangesAsync();

            if (request.MediaIds?.Count > 0)
            {
                var validMedia = await _mediaRepository.GetUnassignedMediaByIdsAsync(request.MediaIds);
                if (validMedia.Count != request.MediaIds.Count)
                {
                    return Result<RingResponse>.Error("Some media items are invalid or already assigned.");
                }

                // اختصاص RingId به آن‌ها
                foreach (var media in validMedia)
                {
                    media.RingId = ring.Id;
                }

                await _mediaRepository.UnitOfWork.SaveChangesAsync();
            }

            return Result<RingResponse>.Success(ring);
        }

        public async Task<Result<RingResponse>> UpdateRingAsync(RingRegisterRequest request)
        {
            var ring = await _ringRepository.GetByIdAsync(request.Id);
            if (ring == null)
                return Result.Error("Ring not found.");

            // ✅ چک تکراری بودن Serial برای انگشترهای دیگر
            var existingRing = await _ringRepository.GetRingBySerialAsync(request.Serial);
            if (existingRing != null && existingRing.Id != request.Id)
                return Result.Error("Another ring with this serial already exists.");

            // 🔄 به‌روزرسانی اطلاعات
            ring.Name = request.Name?.Trim() ?? ring.Name;
            ring.Serial = request.Serial?.Trim() ?? ring.Serial;
            ring.Description = request.Description?.Trim();

            // 🔁 به‌روزرسانی مدیاها
            if (request.MediaIds != null)
            {
                // اول: پاک کردن ارتباط مدیاهای قدیمی
                var currentMedia = await _mediaRepository.GetMediaByRingIdAsync(ring.Id);
                foreach (var media in currentMedia)
                {
                    media.RingId = null; // قطع ارتباط
                }

                // دوم: اتصال مدیاهای جدید (اگر وجود داشت)
                if (request.MediaIds.Count > 0)
                {
                    var validMedia = await _mediaRepository.GetUnassignedMediaByIdsAsync(request.MediaIds);
                    if (validMedia.Count != request.MediaIds.Count)
                    {
                        return Result.Error("Some media items are invalid or already assigned.");
                    }

                    foreach (var media in validMedia)
                    {
                        media.RingId = ring.Id;
                    }
                }

                await _mediaRepository.UnitOfWork.SaveChangesAsync();
            }

            await _ringRepository.UnitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<RingResponse>> DeleteRingAsync(Guid id)
        {
            var ring = await _ringRepository.GetByIdAsync(id);
            if (ring == null)
                return Result.Error("Ring not found.");

            // 🔗 اختیاری: قطع ارتباط مدیاها (یا حذف — بسته به نیاز)
            var medias = await _mediaRepository.GetMediaByRingIdAsync(id);
            foreach (var media in medias)
            {
                media.RingId = null; // یا _mediaRepository.Delete(media) اگر می‌خواهی حذف شوند
            }
            await _mediaRepository.UnitOfWork.SaveChangesAsync();

            // 🧹 حذف خود انگشتر
            _ringRepository.Delete(ring);
            await _ringRepository.UnitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        private static string GenerateUid()
        {
            // مثال: RNG-20251223-ABC123
            return $"RNG-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";
        }

        public async Task<RingResponse?> GetRingBySerialAsync(string serial)
        {
            var result = await _ringRepository.GetRingBySerialAsync(serial);

            if (result == null) 
            {
                return null;
            }

            RingResponse response = new RingResponse
            {
                Uid = result.Uid,
                Serial = result.Serial,
                Name = result.Name,
                Id = result.Id,
                Description = result.Description
            };

            return response;
        }

        public async Task<RingResponse?> GetRingByUidAsync(string uid)
        {
            var result = await _ringRepository.GetRingByUidAsync(uid);

            if (result == null)
            {
                return null;
            }

            RingResponse response = new RingResponse 
            { 
                Uid = result.Uid,
                Serial = result.Serial,
                Name = result.Name,
                Id = result.Id,
                Description = result.Description
            };

            return response;
        }

        public Task<Result<RingResponse>> UpdateRingAsync(RingRegisterRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
