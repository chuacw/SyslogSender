using System;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using System.Net;

namespace Utils.SysLogSender
{	
	public class Sender
	{
		public  enum PriorityType
		{
			Emergency,
			Alert,
			Critical,
			Error,
			Warning,
			Notice,
			Informational,
			Debug
		}
		private static UdpClient udp;
		private static ASCIIEncoding ascii= new ASCIIEncoding();
		private static string machine= System.Net.Dns.GetHostName() + " ";
		private static string sysLogIpAddress ;
		private  Sender()
		{
					Sender.sysLogIpAddress=Dns.Resolve(Dns.GetHostName()).AddressList[0].ToString();
		}
		public static string SysLogIpAddress
		{
			get
			{
				return sysLogIpAddress;
			}
			
			set
			{
				sysLogIpAddress=value;
			}
		}

		public static void Send(string ipAddress,  string body)
		{		
			if(ipAddress==null || (ipAddress.Length<5)) ipAddress=Dns.Resolve(Dns.GetHostName()).AddressList[0].ToString();
			 Send(ipAddress,Sender.PriorityType.Warning,DateTime.Now,body);
		}

		public static void Send(string ipAddress, PriorityType priority, DateTime time, string body)
		{	
			if(ipAddress==null || (ipAddress.Length<5)) ipAddress=Dns.Resolve(Dns.GetHostName()).AddressList[0].ToString();
			udp = new UdpClient(ipAddress, 514);
			byte[] rawMsg;
			string[] strParams = { priority.ToString()+": ", time.ToString("MMM dd HH:mm:ss "),
									 machine,
									 body };
			rawMsg = ascii.GetBytes(string.Concat(strParams));
			udp.Send(rawMsg, rawMsg.Length);
			udp.Close();
			udp=null;			 
		}
	}
}
