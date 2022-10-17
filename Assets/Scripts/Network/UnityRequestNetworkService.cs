using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using UnityEngine.Networking;
using User;
using Utils;

namespace Network
{
	public class UnityRequestNetworkService : INetworkService
	{
		public void Login(string name, string password, Action<LoginStatus> callback)
		{
			var request = UnityWebRequest.Get($"{URLs.BASE_URL}{URLs.LOGIN_URL}?name={name}&password={password}");
			var op = request.SendWebRequest();
			CoroutineRunner.RunCoroutine(LoginCoroutine(op, callback));
		}

		public void Register(string name, string password, Action<RegisterStatus> callback)
		{
			var request = UnityWebRequest.Get($"{URLs.BASE_URL}{URLs.REGISTER_URL}?name={name}&password={password}");
			var op = request.SendWebRequest();
			CoroutineRunner.RunCoroutine(RegisterCoroutine(op, callback));
		}

		public void SaveScore(string name, int score)
		{
			string body = $"{{\"score\": {score},\"owner_name\": \"{name}\"}}";
			var data = Encoding.UTF8.GetBytes(body);
			var request = UnityWebRequest.Post($"{URLs.BASE_URL}{URLs.SAVE_URL}", body);
			request.uploadHandler = new UploadHandlerRaw(data);
			request.uploadHandler.contentType = "application/json";
			var op = request.SendWebRequest();
			CoroutineRunner.RunCoroutine(SaveScoreCoroutine(op));
		}

		public void GetLeaders(int count, Action<Highscores> callback)
		{
			var request = UnityWebRequest.Get($"{URLs.BASE_URL}{URLs.LEADERS_URL}?count={count}");
			var op = request.SendWebRequest();
			CoroutineRunner.RunCoroutine(GetLeadersCoroutine(op, callback));
		}

		private IEnumerator GetLeadersCoroutine(UnityWebRequestAsyncOperation op, Action<Highscores> callback)
		{
			yield return op;
			if (op.isDone)
			{
				var data = op.webRequest.downloadHandler.text;
				var scores = JsonConvert.DeserializeObject<List<HighscoresRow>>(data);
				callback?.Invoke(new Highscores() { Rows = scores });
				op.webRequest.Dispose();
			}
		}

		private IEnumerator LoginCoroutine(UnityWebRequestAsyncOperation op, Action<LoginStatus> callback)
		{
			yield return op;
			if (op.isDone)
			{
				var request = op.webRequest;
				if (request.responseCode == 200)
				{
					callback?.Invoke(LoginStatus.Login);
				}
				else if (request.responseCode == 404)
				{
					callback?.Invoke(LoginStatus.UserNotFound);
				}
				else if (request.responseCode == 401)
				{
					callback?.Invoke(LoginStatus.WrongPassword);
				}
			}
			else
			{
				callback?.Invoke(LoginStatus.NotLogined);
			}
			op.webRequest.Dispose();
		}

		private IEnumerator RegisterCoroutine(UnityWebRequestAsyncOperation op, Action<RegisterStatus> callback)
		{
			yield return op;
			if (op.isDone)
			{
				var request = op.webRequest;
				if (request.responseCode == 200)
				{
					callback?.Invoke(RegisterStatus.Success);
				}
				else if (request.responseCode == 400)
				{
					callback?.Invoke(RegisterStatus.UserExists);
				}
			}
			else
			{
				callback?.Invoke(RegisterStatus.Fail);
			}
			op.webRequest.Dispose();
		}

		private IEnumerator SaveScoreCoroutine(UnityWebRequestAsyncOperation op)
		{
			yield return op;
			if (op.isDone)
			{
				if (op.webRequest.responseCode != 201)
				{
					throw new HttpRequestException();
				}
			}
			op.webRequest.Dispose();
		}
	}
}