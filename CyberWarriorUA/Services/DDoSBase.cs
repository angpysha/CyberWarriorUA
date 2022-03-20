using System;
using System.Threading.Tasks;
using CyberWarriorUA.Models;

namespace CyberWarriorUA.Services
{
    public abstract class DDoSBase : IDDosBase
    {
        protected AttackModel AttackModel { get; }

        protected DDoSBase(AttackModel attackModel)
        {
            AttackModel = attackModel;
        }

        public abstract Task Attack();
    }

    public interface IDDosBase
    {
        Task Attack();
    }

    public static class DDoSerCreator
    {
        public static DDoSBase? CreateDDoSer(this AttackModel attackModel)
        {
            var protocolType = (EProtocolType)attackModel.Protocol;
            if (protocolType == EProtocolType.HTTP)
            {
                var httpAttacker =  new HttpDDoSAtacker(attackModel);

                return httpAttacker;
            } else if (protocolType == EProtocolType.TCP)
            {
                return new TcpDDoSAttacker(attackModel);
            } else
            {
                return new UDPDDoSAttacker(attackModel);
            }

            return null;
        }
    }
}
