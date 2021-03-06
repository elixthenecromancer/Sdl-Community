﻿using System.Windows.Forms;
using Sdl.Community.projectAnonymizer.Helpers;
using Sdl.Community.projectAnonymizer.Models;
using Sdl.Community.projectAnonymizer.Process_Xliff;
using Sdl.Community.projectAnonymizer.Ui;
using Sdl.Core.Settings;
using Sdl.Desktop.IntegrationApi;

namespace Sdl.Community.projectAnonymizer.Batch_Task
{
	public class AnonymizerSettingsPage : DefaultSettingsPage<AnonymizerSettingsControl, AnonymizerSettings>
	{
		private AnonymizerSettings _settings;
		private AnonymizerSettingsControl _control;
		
		public override object GetControl()
		{		
			_settings = ((ISettingsBundle)DataSource).GetSettingsGroup<AnonymizerSettings>();
			_control = base.GetControl() as AnonymizerSettingsControl;
			_control.Settings = _settings;
			return _control;
		}

		public override void Save()
		{
			_settings.EncryptionKey = _control.EncryptionKey;

			if (!_settings.IsEncrypted)
				EncryptPatterns();

			_settings.RegexPatterns = _control.RegexPatterns;
			_settings.SelectAll = _control.SelectAll;
		}

		public override bool ValidateInput()
		{
			return AgreementMethods.UserAgreed();
		}

		private void EncryptPatterns()
		{
			foreach (var regexPattern in _control.RegexPatterns)
			{
				regexPattern.Pattern = AnonymizeData.EncryptData(regexPattern.Pattern, _settings.EncryptionKey);
			}
			_settings.IsEncrypted = true;
		}

		//public override void OnActivate()
		//{
		//	_control.EncryptionKey = _settings.EncryptionKey;
		//	_control.RegexPatterns = _settings.RegexPatterns;
		//}
	}
}
