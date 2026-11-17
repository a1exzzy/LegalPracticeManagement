using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Azure.Core;
using LawFirm.Application.Features.ReferenceData.Dtos;
using LawFirm.Infrastructure.Persistence;
using LawFirm.Shared.Results;
using Microsoft.EntityFrameworkCore;
using LawFirm.Domain.Entities;
using LawFirm.Domain.Enums; 


namespace LawFirm.Application.Features.ReferenceData;

public class ReferenceDataService : IReferenceDataService
{// 直接用 DbContext 查询，按显示顺序排序，只查激活的
    private readonly LawFirmDbContext _db;//不知道为什么要加readonly但是client写了
    public ReferenceDataService(LawFirmDbContext db)
    {
        _db = db;
    }

    public async Task<Result<ReferenceDataDto>>CreateAsync(CreateReferenceDataCommand request)
    {
        //1. 业务规则校验.admin权限校验通常不在 Service 里面写
        // - Name is required
        // - Name should be unique within the same reference data category
        // - DisplayOrder must be numeric if provided
        //  Admin can create a new Matter Type and it becomes available in matter forms.
        // - Inactive values are hidden from create forms by default.
        if (ValidateCreate(request) is { } error)
            return error;

        // - Name should be unique within the same reference data category/type
        //查db中的referenceData表 有哪条符合？AnyAsync() 返回true就是找到了 false就是没有 高效不会把整张表的数据下载到内存里
        //用lambda 先找同request的type，然后名字trim首尾空格并且 tolower以防止意义一样 
        //查找是否nameExist
        //因为anyasync是一个异步方法所以要加 await
        var nameExist =await _db.ReferenceDataItems.AnyAsync(x => x.Type == x.Type && x.Name.ToLower() == request.Name.Trim().ToLower());

        //如果存在名字 就报错Error.Conflict（业务冲突错误 ➔ 对应 HTTP 409)
        if (nameExist)return Error.Conflict($"Name: {request.Name} has already exist");

        //create(string name, string code, int displayOrder, bool isActive, ReferenceDataType type)
        var item = ReferenceDataItem.Create(
            request.Name,
            request.Code,
            request.DisplayOrder,
            request.IsActive,
            request.Type
        );

        _db.ReferenceDataItems.Add(item);
        await _db.SaveChangesAsync();

        return item.ToDto();

    }

    //辅助方法to validate create在企业级开发中，最和谐的写法是把它们拆开保持 static：只负责基础的、不需要查数据库的格式校验（比如 Name 是否为空、Enum 是否合法）。
    private static Error? ValidateCreate(CreateReferenceDataCommand request)
    {
        // - Name is required
        //Error.Validation（参数验证错误 ➔ 对应 HTTP 400）
        if (string.IsNullOrEmpty(request.Name))
            return Error.Validation("Name is required");
        

        // DisplayOrder must be numeric if provided
        //request的order 判断是否int 如果不是 就valiadation错 100
        //但是我们之前已经 如果没提供（null），就默认给 0了

        //检查type
        if(!Enum.IsDefined(request.Type))
            return Error.Validation("Reference Data Type is required ");
          
        return null;
    }





//禁/启：停用或恢复（替代“删”）
    public async Task<Result> DeactiveAsync(int id)
    {
        var item = await _db.ReferenceDataItems.FirstOrDefaultAsync(x => x.Id == id);
        if(item is null)
        {
            return Result.Failure(Error.NotFound($"ReferenceDataItem",id));
        }

        item.Deactivate();
        await _db.SaveChangesAsync();
        return Result.Success();

    }

    public async Task<Result> ActivateAsync(int id)
    {
        var item = await _db.ReferenceDataItems.FirstOrDefaultAsync(x => x.Id == id);
        if(item is null)
        {
            return Result.Failure(Error.NotFound($"ReferenceDataItem",id));
        }

        item.Activate();
        await _db.SaveChangesAsync();
        return Result.Success();
    }



    //改：更新字典项
    public async Task<Result<ReferenceDataDto>> UpdateAsync(int id,  UpdateReferenceDataCommand request)
    {
        //1.先找有米有
        var item = await _db.ReferenceDataItems.FindAsync(id);
        if(item == null)
        {
            return Error.NotFound($"ReferenceData: {id} was not found",id);
        }

        //2.查name空否 type
        if (string.IsNullOrEmpty(request.Name))
            return Error.Validation("Name is required");
            
        if(!Enum.IsDefined(request.Type))
            return Error.Validation("Reference Data Type is required ");
        //3.查name重复

        var nameExist =await _db.ReferenceDataItems.AnyAsync(x => x.Type == x.Type && x.Name.ToLower() == request.Name.Trim().ToLower());

        //如果存在名字 就报错Error.Conflict（业务冲突错误 ➔ 对应 HTTP 409)
        if (nameExist)return Error.Conflict($"Name: {request.Name} has already exist");

        // 4.update
        item.Name = request.Name;
        item.Code = request.Code;
        item.DisplayOrder = request.DisplayOrder;
        item.IsActive = request.IsActive;
        item.Type = request.Type;

        await _db.SaveChangesAsync();
        return item.ToDto();

    }


//查：获取单个
    public async Task<Result<ReferenceDataDto>> GetByIdAsync(int id)
    {
        var item = await _db.ReferenceDataItems.FindAsync(id);
        if(item == null)
        {
            return Error.NotFound("ReferenceDataItem", id);
        }

        return item.ToDto();
    }

    //查获取列表
    public async Task<List<ReferenceDataDto>> ListByTypeAsync(ReferenceDataType type)
    {
        //从数据库查出来的 list，里面的元素类型是 ReferenceData
        var list = await _db.ReferenceDataItems
        .Where(x => x.Type == type && x.IsActive)
        .OrderBy(x =>x.DisplayOrder)
        //.ThenBy(x => x.Name) // 如果排序值一样，按名字字母顺序兜底
        .ToListAsync();

        //方法的返回值要求是 List<ReferenceDataDto>（数据传输对象列表）
        //必须通过 .Select(x => x.ToDto())
        return list.Select(x => x.ToDto()).ToList();
    }



/*
    //增：创建新字典项
    Task CreateAsync(CreateReferenceDataCommand request);
    //禁/启：停用或恢复（替代“删”）
    Task<Result>DeactiveAsync(int id);
    Task<Result> ActivateAsync(int id);
    //改：更新字典项
    Task<Result<ReferenceDataDto>> UpdateAsync(int id, UpdateReferenceDataCommand request);

    //查：获取列表 用户在填表单时，前端需要把某个分类（比如“实践领域”）的所有选项一次性拉取下来渲染成下拉菜单。
    Task<List<ReferenceDataDto>> ListByTypeAsync(ReferenceDataType type);

    //查：根据 ID 获取单个字典详情
    Task<Result<ReferenceDataDto>> GetByIdAsync(int id);
    */

}

