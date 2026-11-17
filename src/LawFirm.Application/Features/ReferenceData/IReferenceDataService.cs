using System;
using LawFirm.Application.Features.ReferenceData.Dtos;
using LawFirm.Domain.Enums;
using LawFirm.Shared.Results;

namespace LawFirm.Application.Features.ReferenceData;

public interface IReferenceDataService
{
    //增：创建新字典项
    Task<Result<ReferenceDataDto>>CreateAsync(CreateReferenceDataCommand request);
    
    //禁/启：停用或恢复（替代“删”）
    Task<Result> DeactiveAsync(int id);
    Task<Result> ActivateAsync(int id);
    //改：更新字典项
    Task<Result<ReferenceDataDto>> UpdateAsync(int id, UpdateReferenceDataCommand request);

    //查：获取列表 用户在填表单时，前端需要把某个分类（比如“实践领域”）的所有选项一次性拉取下来渲染成下拉菜单。
    Task<List<ReferenceDataDto>> ListByTypeAsync(ReferenceDataType type);

    //查：根据 ID 获取单个字典详情
    Task<Result<ReferenceDataDto>> GetByIdAsync(int id);
    

    

    
}


  