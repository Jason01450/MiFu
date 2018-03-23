using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiFu.Core
{
    public class MiFuServiceBase<TMessage> : IMiFuService<TMessage>
        where TMessage : class
    {
        public virtual TMessage Activate(IMiFuActivationContext<TMessage> context)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TMessage> ActivateAsync(IMiFuActivationContext<TMessage> context)
        {
            throw new NotImplementedException();
        }
    }
}
