using LawFirm.Application.Features.ReferenceData;
using LawFirm.Application.Features.ReferenceData.Dtos;
using LawFirm.Domain.Constants;
using LawFirm.Domain.Entities;
using LawFirm.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LawFirm.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = AppRoles.SystemAdmin)] //如果整个 Controller 里的接口都只给SystemAdmin 
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
        public async Task<IActionResult>ListByType(ReferenceDataType type)
        {
            var result = await _referenceDataService.ListByTypeAsync(type);
            return Ok(result);
        }

        //Task<Result<ReferenceDataDto>> GetByIdAsync(int id);
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _referenceDataService.GetByIdAsync(id);

            //如果 result.IsSuccess 为真（查到了/成功了），就返回 Ok(result.Value)；否则（没查到/失败了），就返回 NotFound(result.Error)。
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        ////增：创建新字典项
        //Task<Result<ReferenceDataDto>>CreateAsync(CreateReferenceDataCommand request);
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReferenceDataCommand request)
        {
            var result = await _referenceDataService.CreateAsync(request);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            if (result.Error?.Code == "conflict")
            {
                return Conflict(result.Error);
            }
            return BadRequest(result.Error);//其他业务校验失败 → BadRequest(result.Error)（400）
            

        }

        //禁/启：停用或恢复（替代“删”）
        //Task<Result> DeactiveAsync(int id);
        //Task<Result> ActivateAsync(int id);
        //成功返回204 No Content（无内容） 状态码响应体（Body）是空的，只告诉前端：“请求成功处理，一切正常”
        //失败 则可能是 没找到id对应的
        [HttpPost("{id:int}/deactive")]
        public async Task<IActionResult> Deactive (int id)
        {
            var result = await _referenceDataService.DeactiveAsync(id);
            return result.IsSuccess ? NoContent() : NotFound(result.Error);
        }
        [HttpPost("{id:int}/activate")]
        public async Task<IActionResult> Activate (int id)
        {
            var result = await _referenceDataService.ActivateAsync(id);
            return result.IsSuccess ? NoContent() : NotFound(result.Error);
        }


        //改：更新字典项
        //Task<Result<ReferenceDataDto>> UpdateAsync(int id, UpdateReferenceDataCommand request);
        //正常返回value
        //错误 可能是conflict或者没找到这个id或者badrequest
        [HttpPost("{id:int}")]
        public async Task<IActionResult> Update (int id, [FromBody]UpdateReferenceDataCommand request)
        {
            var result = await _referenceDataService.UpdateAsync(id,request);
            if(result.IsSuccess)
                return Ok(result.Value);
            if(result.Error?.Code == "conflict")
                return Conflict(result.Error);
            if(result.Error?.Code == "not_found")
                return NotFound(result.Error);
            return BadRequest(result.Error);
        }
    }
}
