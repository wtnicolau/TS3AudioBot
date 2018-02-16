// TS3AudioBot - An advanced Musicbot for Teamspeak 3
// Copyright (C) 2017  TS3AudioBot contributors
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the Open Software License v. 3.0
//
// You should have received a copy of the Open Software License along with this
// program. If not, see <https://opensource.org/licenses/OSL-3.0>.

namespace TS3AudioBot
{
	using CommandSystem;
	using System;

	internal class TargetScript
	{
		private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
		private const string DefaultVoiceScript = "!whisper off";
		private const string DefaultWhisperScript = "!xecute (!whisper subscription) (!unsubscribe temporary) (!subscribe channeltemp (!getuser channel))";

		public Core Core { get; set; }
		public Bot Bot { get; set; }
		public CommandManager CommandManager { get; set; }

		public void BeforeResourceStarted(object sender, PlayInfoEventArgs e)
		{
			var mode = AudioValues.audioFrameworkData.AudioMode;
			string script;
			if (mode.StartsWith("!", StringComparison.Ordinal))
				script = mode;
			else if (mode.Equals("voice", StringComparison.OrdinalIgnoreCase))
				script = DefaultVoiceScript;
			else if (mode.Equals("whisper", StringComparison.OrdinalIgnoreCase))
				script = DefaultWhisperScript;
			else
			{
				Log.Error("Invalid voice mode");
				return;
			}
			CallScript(script, e.Invoker);
		}

		private void CallScript(string script, InvokerData invoker)
		{
			try
			{
				var info = new ExecutionInformation(Core, Bot, invoker, null) { SkipRightsChecks = true };
				CommandManager.CommandSystem.Execute(info, script);
			}
			catch (CommandException) { }
		}
	}
}
