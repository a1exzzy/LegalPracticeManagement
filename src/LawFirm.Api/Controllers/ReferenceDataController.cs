using LawFirm.Application.Features.ReferenceData;
using LawFirm.Application.Features.ReferenceData.Dtos;
using LawFirm.Domain.Entities;
using LawFirm.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LawFirm.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReferenceDataController : ControllerBase
    {
        // 1. 通过构造函数注入 Service
        private IReferenceDataService _referenceDataService;

        public ReferenceDataController( IReferenceDataService referenceDataService)
        {
            _referenceDataService = referenceDataService;
        }

        // 2. 查：根据类型获取列表 (GET: api/referencedata/type/MatterType)Task<List<ReferenceDataDto>> ListByTypeAsync(ReferenceDataType type);
        //返回的是一个list of referencedata?不对 是referencedataDTO即前端显示的
        [HttpGet("type/{type}")]
        public async Task<ActionResult<List<ReferenceDataDto>>> ListByType(ReferenceDataType type)
        {
            var result = await _referenceDataService.ListByTypeAsync(type);
            return Ok(result);
        }

        //Task<Result<ReferenceDataDto>> GetByIdAsync(int id);
        [HttpGet]
        public async Task<ActionResult<ReferenceDataDto>> GetById(int id)
        {
            var result = await _referenceDataService.GetByIdAsync(id);
            return Ok(result);
        }

        ////增：创建新字典项
        //Task<Result<ReferenceDataDto>>CreateAsync(CreateReferenceDataCommand request);


    }
}
