using AutoMapper;
using DealNotifier.Core.Application.Enums;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.ViewModels.V1;
using DealNotifier.Core.Application.ViewModels.V1.PhoneUnlockTool;
using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhonePhoneUnlockTool;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Services
{
    /// <summary>
    /// This class represents the many-to-many relationship between <see cref="UnlockabledPhone"/> and <see cref="PhoneUnlockTool"/>.
    /// It serves as a "join table" to associate UnlockabledPhones with supported PhoneUnlockTools.
    /// </summary>
    public class UnlockabledPhonePhoneUnlockToolService : IUnlockabledPhonePhoneUnlockToolService

    {
        #region Private Variables

        private readonly IUnlockabledPhonePhoneUnlockToolRepository _repository;
        private readonly IMapper _mapper;

        #endregion Private Variables

        public UnlockabledPhonePhoneUnlockToolService(IUnlockabledPhonePhoneUnlockToolRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task CreateAsync(int unlockedPhoneId, int phoneUnlockToolId)
        {
            var unlockabledPhonePhoneUnlockTool = new UnlockabledPhonePhoneUnlockTool
            {
                UnlockabledPhoneId = unlockedPhoneId,
                PhoneUnlockToolId = phoneUnlockToolId
            };

            await _repository.CreateAsync(unlockabledPhonePhoneUnlockTool);
        }

        public async Task<bool> ExistsAsync(UnlockabledPhonePhoneUnlockToolDto entity)
        {
            var mappedEntity = _mapper.Map<UnlockabledPhonePhoneUnlockTool>(entity);
            return await _repository.ExistsAsync(mappedEntity);
        }


        public async Task CreateIfNotExists(int unlockabledPhoneId, int phoneUnlockToolId)
        {
            var unlockabledPhonePhoneUnlockToolDto = new UnlockabledPhonePhoneUnlockToolDto
            {
                UnlockabledPhoneId = unlockabledPhoneId,
                PhoneUnlockToolId = phoneUnlockToolId
            };

            var existsUnlockabledPhonePhoneUnlockTool = await ExistsAsync(unlockabledPhonePhoneUnlockToolDto);

            if (!existsUnlockabledPhonePhoneUnlockTool)
            {
                await CreateAsync(unlockabledPhoneId, phoneUnlockToolId);
            }
        }
    }
}