using System;
using UnityEngine;
using UnityEngine.UI;

namespace RaceGame.Network
{
	
	[Obsolete("破棄", true)]
	public class NetworkSettingTester : MonoBehaviour
	{
		public Button HostButton;
		public Button ClientButton;
		public Button ChangeButton;

		//private NetworkSetting networkSetting;
/*
		private void Start()
		{
			networkSetting = NetworkSetting.Instance.GetComponent<NetworkSetting>();
			networkSetting.HostButton = HostButton;
			networkSetting.ClientButton = ClientButton;
			networkSetting.ChangeButton = ChangeButton;
		}*/
	}
}