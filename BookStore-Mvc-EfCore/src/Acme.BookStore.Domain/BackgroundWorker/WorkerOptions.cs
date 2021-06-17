//  ************************************************************************************
//  项目名称：Acme.BookStore.Domain
//  文 件  名：WorkerOptions.cs
//  创建时间：2021-06-16
//  作    者：
//  说    明：
//  修改时间：2021-06-16
//  修 改 人：
//  Copyright © 2020 广州市晨旭自动化设备有限公司. 版权所有
//  *************************************************************************************

using Volo.Abp.Collections;

namespace Acme.BookStore.BackgroundWorker
{
    public class WorkerOptions
    {
        public WorkerOptions()
        {
            BackgroundWorkers = new TypeList<IWorker>();
        }

        /// <summary>
        /// 使用 ITypeList 逆变型集合存储 BackgroundWorker
        /// </summary>
        public ITypeList<IWorker> BackgroundWorkers { get; }
    }
}