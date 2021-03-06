// TS3AudioBot - An advanced Musicbot for Teamspeak 3
// Copyright (C) 2017  TS3AudioBot contributors
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the Open Software License v. 3.0
//
// You should have received a copy of the Open Software License along with this
// program. If not, see <https://opensource.org/licenses/OSL-3.0>.

namespace TS3AudioBot.Config
{
	using Nett;
	using System;

	public partial class ConfRoot : ConfigTable
	{
		public ConfBot Bot { get; } = Create<ConfBot>("bot",
			"! IMPORTANT !\n" +
			"All config tables here starting with 'bot.*' will only be used as default values for each bot.\n" +
			"To make bot-instance specific changes go to the 'Bots' folder (configs.bots_path) and set your configuration values in the desired bot config.");
		public ConfBots Bots { get; } = Create<ConfBots>("bots",
			"You can create new subtables matching the bot config name to configure meta-settings for each bot.\n" +
			"Current layout: { run:bool }");
		public ConfConfigs Configs { get; } = Create<ConfConfigs>("configs");
		public ConfDb Db { get; } = Create<ConfDb>("db");
		public ConfFactories Factories { get; } = Create<ConfFactories>("factories");
		public ConfTools Tools { get; } = Create<ConfTools>("tools");
		public ConfRights Rights { get; } = Create<ConfRights>("rights");
		public ConfPlugins Plugins { get; } = Create<ConfPlugins>("plugins");
		public ConfWeb Web { get; } = Create<ConfWeb>("web");

		//public ConfigValue<bool> ActiveDocumentation { get; } = new ConfigValue<bool>("_active_doc", true);
	}

	public class ConfBots : ConfigDynamicTable<BotTemplate> { }

	public class BotTemplate : ConfigTable
	{
		protected override TomlTable.TableTypes TableType => TomlTable.TableTypes.Inline;

		public ConfigValue<bool> Run { get; } = new ConfigValue<bool>("run", false);
	}

	public class ConfConfigs : ConfigTable
	{
		//public ConfigValue<string> RootPath { get; } = new ConfigValue<string>("root_path", "."); // TODO enable when done
		public ConfigValue<string> BotsPath { get; } = new ConfigValue<string>("bots_path", "Bots",
			"Path to a folder where the configuration files for each bot template will be stored.");
	}

	public class ConfDb : ConfigTable
	{
		public ConfigValue<string> Path { get; } = new ConfigValue<string>("path", "ts3audiobot.db",
			"The path to the database file for persistent data.");
	}

	public class ConfFactories : ConfigTable
	{
		public ConfPath Media { get; } = Create<ConfPath>("media",
			"The default path to look for local resources.");
	}

	public class ConfTools : ConfigTable
	{
		// youtube-dl can be empty by default as we make some thorough lookups.
		public ConfPath YoutubeDl { get; } = Create<ConfPath>("youtube-dl",
			"Path to the youtube-dl binary or local git repository.");
		public ConfToolsFfmpeg Ffmpeg { get; } = Create<ConfToolsFfmpeg>("ffmpeg",
			"The path to ffmpeg.");
		//public ConfPath Ffprobe { get; } = Create<ConfPath>("ffprobe");
	}

	public class ConfToolsFfmpeg : ConfigTable
	{
		public ConfigValue<string> Path { get; } = new ConfigValue<string>("path", "ffmpeg");
	}

	public class ConfRights : ConfigTable
	{
		public ConfigValue<string> Path { get; } = new ConfigValue<string>("path", "rights.toml",
			"Path to the permission file. The file will be generated if it doesn't exist.");
	}

	public class ConfPlugins : ConfigTable
	{
		public ConfigValue<string> Path { get; } = new ConfigValue<string>("path", "Plugins",
			"The path to the plugins folder.");
		public ConfigValue<bool> WriteStatusFiles { get; } = new ConfigValue<bool>("write_status_files", false,
			"Write to .status files to store a plugin enable status persistently and restart them on launch."); // TODO deprecate

		public ConfPluginsLoad Load { get; } = Create<ConfPluginsLoad>("load");
	}

	public class ConfPluginsLoad : ConfigTable
	{
		// TODO: dynamic table
	}

	public class ConfWeb : ConfigTable
	{
		public ConfigArray<string> Hosts { get; } = new ConfigArray<string>("hosts", new[] { "localhost", "127.0.0.1" },
			"An array of all urls the web api should be possible to be accessed with.");
		public ConfigValue<ushort> Port { get; } = new ConfigValue<ushort>("port", 8180,
			"The port for the web server.");

		public ConfWebApi Api { get; } = Create<ConfWebApi>("api");
		public ConfWebInterface Interface { get; } = Create<ConfWebInterface>("interface");
	}

	public class ConfWebApi : ConfigTable
	{
		public ConfigValue<bool> Enabled { get; } = new ConfigValue<bool>("enabled", true,
			"If you want to enable the web api.");
	}

	public class ConfWebInterface : ConfigTable
	{
		public ConfigValue<bool> Enabled { get; } = new ConfigValue<bool>("enabled", true,
			"If you want to enable the webinterface.");
		public ConfigValue<string> Path { get; } = new ConfigValue<string>("path", "",
			"The webinterface folder to host. Leave empty to let the bot look for default locations.");
	}

	public partial class ConfBot : ConfigTable
	{
		public ConfigValue<ulong> BotGroupId { get; } = new ConfigValue<ulong>("bot_group_id", 0,
			"This field will be automatically set when you call '!bot setup'.\n" +
			"The bot will use the specified group to set/update the required permissions and add himself into it.\n" +
			"You can set this field manually if you already have a preexisting group the bot should add himself to.");
		public ConfigValue<bool> GenerateStatusAvatar { get; } = new ConfigValue<bool>("generate_status_avatar", true,
			"Tries to fetch a cover image when playing.");
		public ConfigValue<string> Language { get; } = new ConfigValue<string>("language", "en",
			"The language the bot should use to respond to users. (Make sure you have added the required language packs)");
		public ConfigValue<string> CommandMatcher { get; } = new ConfigValue<string>("command_matcher", "ic3",
			"Defines how the bot tries to match your !commands. Possible types:\n" +
			" - exact : Only when the command matches exactly.\n" +
			" - substring : The shortest command starting with the given prefix.\n" +
			" - ic3 : 'interleaved continuous character chain' A fuzzy algorithm similar to hamming distance but preferring characters at the start."
			/* "hamming : " */);

		public ConfConnect Connect { get; } = Create<ConfConnect>("connect");
		public ConfAudio Audio { get; } = Create<ConfAudio>("audio");
		public ConfPlaylists Playlists { get; } = Create<ConfPlaylists>("playlists");
		public ConfHistory History { get; } = Create<ConfHistory>("history");
		public ConfEvents Events { get; } = Create<ConfEvents>("events");
	}

	public class ConfConnect : ConfigTable
	{
		public ConfigValue<string> Address { get; } = new ConfigValue<string>("address", "",
			"The address, ip or nickname (and port; default: 9987) of the TeamSpeak3 server");
		public ConfigValue<string> Channel { get; } = new ConfigValue<string>("channel", "",
			"Default channel when connecting. Use a channel path or '/<id>'.\n" +
			"Examples: 'Home/Lobby', '/5', 'Home/Afk \\/ Not Here'.");
		public ConfigValue<string> Badges { get; } = new ConfigValue<string>("badges", "",
			"The client badges. You can set a comma seperated string with max three GUID's. Here is a list: http://yat.qa/ressourcen/abzeichen-badges/");
		public ConfigValue<string> Name { get; } = new ConfigValue<string>("name",
			"TS3AudioBot", "Client nickname when connecting.");

		public ConfPassword ServerPassword { get; } = Create<ConfPassword>("server_password",
			"The server password. Leave empty for none.");
		public ConfPassword ChannelPassword { get; } = Create<ConfPassword>("channel_password",
			"The default channel password. Leave empty for none.");
		public ConfTsVersion ClientVersion { get; } = Create<ConfTsVersion>("client_version",
			"Overrides the displayed version for the ts3 client. Leave empty for default.");
		public ConfIdentity Identity { get; } = Create<ConfIdentity>("identity");
	}

	public class ConfIdentity : ConfigTable
	{
		new public ConfigValue<string> Key { get; } = new ConfigValue<string>("key", "",
			"||| DO NOT MAKE THIS KEY PUBLIC ||| The client identity. You can import a teamspeak3 identity here too.");
		public ConfigValue<ulong> Offset { get; } = new ConfigValue<ulong>("offset", 0,
			"The client identity offset determining the security level.");
		public ConfigValue<int> Level { get; } = new ConfigValue<int>("level", -1,
			"The client identity security level which should be calculated before connecting\n" +
			"or -1 to generate on demand when connecting.");
	}

	public class ConfAudio : ConfigTable
	{
		public ConfAudioVolume Volume { get; } = Create<ConfAudioVolume>("volume",
			"When a new song starts the volume will be trimmed to between min and max.\n" +
			"When the current volume already is between min and max nothing will happen.\n" +
			"To completely or partially disable this feature, set min to 0 and/or max to 100.");
		public ConfigValue<float> MaxUserVolume { get; } = new ConfigValue<float>("max_user_volume", 30,
			"The maximum volume a normal user can request. Only user with the 'ts3ab.admin.volume' permission can request higher volumes.");
		public ConfigValue<int> Bitrate { get; } = new ConfigValue<int>("bitrate", 48,
			"Specifies the bitrate (in kbps) for sending audio.\n" +
			"Values between 8 and 98 are supported, more or less can work but without guarantees.\n" +
			"Reference values: 16 - poor (~3KiB/s), 24 - okay (~4KiB/s), 32 - good (~5KiB/s), 48 - very good (~7KiB/s), 64 - not noticeably better than 48, stop wasting your bandwith, go back (~9KiB/s)");
		public ConfigValue<string> SendMode { get; } = new ConfigValue<string>("send_mode", "voice",
			"How the bot should play music. Options are:\n" +
			" - whisper : Whispers to the channel where the request came from. Other users can join with '!subscribe'.\n" +
			" - voice : Sends via normal voice to the current channel. '!subscribe' will not work in this mode.\n" +
			" - !... : A custom command. Use '!xecute (!a) (!b)' for example to execute multiple commands.");
	}

	public class ConfAudioVolume : ConfigTable
	{
		protected override TomlTable.TableTypes TableType => TomlTable.TableTypes.Default;  // TODO inline when Nett has fixed the inline bug.

		public ConfigValue<float> Default { get; } = new ConfigValue<float>("default", 10);
		public ConfigValue<float> Min { get; } = new ConfigValue<float>("min", 10);
		public ConfigValue<float> Max { get; } = new ConfigValue<float>("max", 50);
	}

	public class ConfPlaylists : ConfigTable
	{
		protected override TomlTable.TableTypes TableType => TomlTable.TableTypes.Default;  // TODO inline when Nett has fixed the inline bug.

		public ConfigValue<string> Path { get; } = new ConfigValue<string>("path", "Playlists",
			"Path to the folder where playlist files will be saved.");
	}

	public class ConfHistory : ConfigTable
	{
		public ConfigValue<bool> Enabled { get; } = new ConfigValue<bool>("enabled", true,
			"Enable or disable history features completely to save resources.");
		public ConfigValue<bool> FillDeletedIds { get; } = new ConfigValue<bool>("fill_deleted_ids", true,
			"Whether or not deleted history ids should be filled up with new songs.");
	}

	public class ConfEvents : ConfigTable
	{
		public ConfigValue<string> OnConnect { get; } = new ConfigValue<string>("onconnect", "",
			"Called when the bot is connected.");
		public ConfigValue<string> OnDisconnect { get; } = new ConfigValue<string>("ondisconnect", "",
			"Called when the bot gets disconnected.");
		public ConfigValue<string> OnIdle { get; } = new ConfigValue<string>("onidle", "",
			"Called when the bot does not play anything for a certain amount of time.");
		public ConfigValue<TimeSpan> IdleTime { get; } = new ConfigValue<TimeSpan>("idletime", TimeSpan.FromMinutes(5),
			"Specifies how long the bot has to be idle until the 'onidle' event gets fired.\n" +
			"You can specify the time in the ISO-8601 format with qutotation marks \"PT30S\" or like: 15s, 1h, 3m30s");
	}

	// Utility config structs

	public class ConfPath : ConfigTable
	{
		protected override TomlTable.TableTypes TableType => TomlTable.TableTypes.Default;  // TODO inline when Nett has fixed the inline bug.

		public ConfigValue<string> Path { get; } = new ConfigValue<string>("path", string.Empty);
	}

	public class ConfPassword : ConfigTable
	{
		protected override TomlTable.TableTypes TableType => TomlTable.TableTypes.Default;  // TODO inline when Nett has fixed the inline bug.

		public ConfigValue<string> Password { get; } = new ConfigValue<string>("pw", string.Empty);
		public ConfigValue<bool> Hashed { get; } = new ConfigValue<bool>("hashed", false);
		public ConfigValue<bool> AutoHash { get; } = new ConfigValue<bool>("autohash", false);

		public TS3Client.Password Get()
		{
			if (string.IsNullOrEmpty(Password))
				return TS3Client.Password.Empty;
			var pass = Hashed
				? TS3Client.Password.FromHash(Password)
				: TS3Client.Password.FromPlain(Password);
			if (AutoHash && !Hashed)
			{
				Password.Value = pass.HashedPassword;
				Hashed.Value = true;
			}
			return pass;
		}
	}

	public class ConfTsVersion : ConfigTable
	{
		protected override TomlTable.TableTypes TableType => TomlTable.TableTypes.Default;  // TODO inline when Nett has fixed the inline bug.

		public ConfigValue<string> Build { get; } = new ConfigValue<string>("build", string.Empty);
		public ConfigValue<string> Platform { get; } = new ConfigValue<string>("platform", string.Empty);
		public ConfigValue<string> Sign { get; } = new ConfigValue<string>("sign", string.Empty);
	}
}
