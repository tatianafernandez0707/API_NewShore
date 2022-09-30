using Models.ReturnModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access.Interfaces
{
    public interface IServiceConsumption
    {
        Task<ReturnControl> PointOneServiceConsumption(int level);
    }
}
