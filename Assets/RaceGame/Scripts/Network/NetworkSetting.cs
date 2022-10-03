using System;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace RaceGame.Network
{
	[Obsolete("破棄", true)]
	public class NetworkSetting : MonoBehaviour
	{
		public string Address;

		public float pos1;
		public float pos2;
		public float pos3;
		public float pos4;
		public float pos5;

		private NetworkManager networkManager;

		public Button HostButton;
		public Button ClientButton;
		public Button ChangeButton;

		private static GameObject _instance;

		public static GameObject Instance
		{
			get
			{
				return _instance;
			}
		}

		private void Awake()
		{
			if (_instance == null)
			{
				_instance = gameObject;
			}
			else
			{
				Destroy(gameObject);
			}
		}

		private void Start()
		{
			networkManager = GetComponent<NetworkManager>();
			HostButton.onClick.AddListener(() => Host());
			ClientButton.onClick.AddListener(() => Client());
			ChangeButton.onClick.AddListener(() => Change());
		}

		public void Host()
		{
			networkManager.StartHost();
		}

		public void Client()
		{
			networkManager.networkAddress = Address;
			networkManager.StartClient();
		}

		public void Change()
		{
			DollyCartPos.pos1 = pos1;
			DollyCartPos.pos2 = pos2;
			DollyCartPos.pos3 = pos3;
			DollyCartPos.pos4 = pos4;
			DollyCartPos.pos5 = pos5;
			
			Debug.Log($"{DollyCartPos.pos1}");
			Debug.Log($"{DollyCartPos.pos2}");
			Debug.Log($"{DollyCartPos.pos3}");
			Debug.Log($"{DollyCartPos.pos4}");
			Debug.Log($"{DollyCartPos.pos5}");
		}
	}
}