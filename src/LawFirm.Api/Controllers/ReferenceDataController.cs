using LawFirm.Application.Features.ReferenceData;
using LawFirm.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LawFirm.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReferenceDataController : ControllerBase
    {
        // 1. 通过构造函数注入 Service
        private ReferenceDataService _referenceDataService;

        public ReferenceDataController( ReferenceDataService referenceDataService)
        {
            _referenceDataService = referenceDataService;
        }

        //新增
        //[HttpPost]
        //public ActionResult CreateReferenceData([FromBody] ReferenceData referenceData)
        //{
            //ReferenceData newReferenceData = _referenceDataService.CreateReferenceData(referenceData);
            //return Created($"GetReferenceDataById",newReferenceData);
        //}
    }
}
