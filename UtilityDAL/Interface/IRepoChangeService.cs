using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityInterface;


namespace UtilityDAL
{

    public interface IRepoChangeService<T>: IService<IChangeSet<T, int>>
    {

        void CallBack(IObservable<KeyValuePair<UtilityEnum.Database.Operation, T>> ops);

    }

}
