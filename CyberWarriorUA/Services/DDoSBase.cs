using System;
using System.Threading.Tasks;
using CyberWarriorUA.Models;
using CyberWarriorUA.Services.ConsoleLogService;
using Prism.Ioc;
using Xamarin.CommunityToolkit.Helpers;

namespace CyberWarriorUA.Services
{
    public abstract class DDoSBase : IDDosBase
    {
        protected AttackInfo AttackModel { get; }

        protected IConsoleLogService Logger => ContainerLocator.Container.Resolve<IConsoleLogService>();

        protected WeakEventManager<DDoSInfo> _ddosInfoManager = new WeakEventManager<DDoSInfo>();


        public event EventHandler<DDoSInfo> OnAttackFinished
        {
            add => _ddosInfoManager.AddEventHandler(value);
            remove => _ddosInfoManager.RemoveEventHandler(value);
        }

        protected DDoSBase(AttackInfo attackModel)
        {
            AttackModel = attackModel;
        }

        public abstract Task<DDoSInfo> Attack();
    }

    public interface IDDosBase
    {
        Task<DDoSInfo> Attack();
    }

    public static class DDoSerCreator
    {
        public static DDoSBase? CreateDDoSer(this AttackInfo attackModel)
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
