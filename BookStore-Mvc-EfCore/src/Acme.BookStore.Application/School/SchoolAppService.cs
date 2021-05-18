//  ************************************************************************************
//  项目名称：Acme.BookStore.Application
//  文 件  名：SchoolService.cs
//  创建时间：2021-05-18
//  作    者：
//  说    明：
//  修改时间：2021-05-18
//  修 改 人：
//  Copyright © 2020 广州市晨旭自动化设备有限公司. 版权所有
//  *************************************************************************************

using System.Collections.Generic;
using System.Threading.Tasks;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore.School
{
    public class SchoolAppService : CrudAppService<Classes, ClassesDto, int, PagedAndSortedResultRequestDto, ClassesDto>, ISchoolService
    {
        private readonly IRepository<Classes, int> _repository;


        public SchoolAppService(IRepository<Classes, int> repository) : base(repository)
        {
            _repository = repository;
        }

        public override async Task<PagedResultDto<ClassesDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            //await Ins();
            var a = await base.GetListAsync(input);
            return a;
        }

        public async Task<List<ClassesDto>> GetAllClasses()
        {
            var a = await _repository.GetListAsync();

            var b = ObjectMapper.Map<List<Classes>, List<ClassesDto>>(a);
            return b;
        }
    }
}