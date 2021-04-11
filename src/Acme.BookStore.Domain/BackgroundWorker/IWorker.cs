using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Volo.Abp.DependencyInjection;

namespace Acme.BookStore.BackgroundWorker
{
   public interface IWorker: ISingletonDependency
    {
        public void Start();
    }
}
