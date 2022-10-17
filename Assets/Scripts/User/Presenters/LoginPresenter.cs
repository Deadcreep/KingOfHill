using Network;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace User.Presenters
{
	public class LoginPresenter : PresenterBehaviour<IUserManager>
	{
		[SerializeField] private GameObject _loginPanel;
		[SerializeField] private Button _openPanelButton;
		[SerializeField] private Button _logoutButton;
		[SerializeField] private TextMeshProUGUI _nameField;
		[SerializeField] private TextMeshProUGUI _failMessageField;
		[SerializeField] private TMP_InputField _loginNameField;
		[SerializeField] private TMP_InputField _passwordField;
		[SerializeField] private Button _loginButton;
		[SerializeField] private Button _registerButton;

		private readonly string _wrongPasswordMessage = "Wrong password";
		private readonly string _wrongUserMessage = "User not found";
		private readonly string _userExistsMessage = "User already exists";

		private void Awake()
		{
			_loginButton.onClick.AddListener(Login);
			_registerButton.onClick.AddListener(Register);
			_logoutButton.onClick.AddListener(Logout);
			_openPanelButton.onClick.AddListener(SwitchLoginPanelVisability);
		}

		private void OnDestroy()
		{
			_loginButton.onClick.RemoveListener(Login);
			_registerButton.onClick.RemoveListener(Register);
			_logoutButton.onClick.RemoveListener(Logout);
			_openPanelButton.onClick.RemoveListener(SwitchLoginPanelVisability);
		}

		protected override void OnInject()
		{
			if (Model.Status.Value == LoginStatus.Login)
			{
				_openPanelButton.gameObject.SetActive(false);
				_nameField.text = "Hello, " + Model.User.Name;
			}
			Model.Status.Subscribe(OnStatusChanged).AddTo(this);
			Model.RegisterCompleted += OnRegisterCompleted;
			Model.Login("John", "qwerty");
		}

		protected override void OnRemove()
		{
			Model.RegisterCompleted -= OnRegisterCompleted;
		}

		private void OnRegisterCompleted(RegisterStatus status)
		{
			if (status == RegisterStatus.UserExists)
			{
				_failMessageField.text = _userExistsMessage;
			}
		}

		private void OnStatusChanged(LoginStatus status)
		{
			switch (status)
			{
				case LoginStatus.Login:
					_loginPanel.SetActive(false);
					_nameField.text = "Hello, " + Model.User.Name;
					_openPanelButton.gameObject.SetActive(false);
					_logoutButton.gameObject.SetActive(true);
					break;

				case LoginStatus.UserNotFound:
					_failMessageField.text = _wrongUserMessage;
					break;

				case LoginStatus.WrongPassword:
					_failMessageField.text = _wrongPasswordMessage;
					break;

				case LoginStatus.NotLogined:
					_nameField.text = string.Empty;
					_openPanelButton.gameObject.SetActive(true);
					_logoutButton.gameObject.SetActive(false);
					break;

				default:
					break;
			}
			_loginButton.interactable = true;
			_registerButton.interactable = true;
		}

		private void Login()
		{
			_loginButton.interactable = false;
			_registerButton.interactable = false;
			Model.Login(_loginNameField.text, _passwordField.text);
		}

		private void Logout()
		{
			Model.Logout();
		}

		private void Register()
		{
			_loginButton.interactable = false;
			_registerButton.interactable = false;
			Model.Register(_loginNameField.text, _passwordField.text);
		}

		private void SwitchLoginPanelVisability()
		{
			_loginPanel.SetActive(!_loginPanel.activeSelf);
			_openPanelButton.gameObject.SetActive(!_loginPanel.activeSelf);
		}
	}
}