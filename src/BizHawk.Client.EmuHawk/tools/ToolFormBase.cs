﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using BizHawk.Client.Common;
using BizHawk.Common;
using BizHawk.Emulation.Common;

namespace BizHawk.Client.EmuHawk
{
	public class ToolFormBase : FormBase, IToolForm, IDialogParent
	{
		public ToolManager Tools { protected get; set; }

		public DisplayManager DisplayManager { protected get; set; }

		public InputManager InputManager { protected get; set; }

		public IMainFormForTools MainForm { protected get; set; }

		public IMovieSession MovieSession { protected get; set; }

		public IGameInfo Game { protected get; set; }

		public IDialogController DialogController => MainForm;

		public virtual bool AskSaveChanges() => true;

		public virtual void Restart() {}

		public virtual void UpdateValues(ToolFormUpdateType type)
		{
			switch (type)
			{
				case ToolFormUpdateType.PreFrame:
					UpdateBefore();
					break;
				case ToolFormUpdateType.PostFrame:
					UpdateAfter();
					break;
				case ToolFormUpdateType.General:
					GeneralUpdate();
					break;
				case ToolFormUpdateType.FastPreFrame:
					FastUpdateBefore();
					break;
				case ToolFormUpdateType.FastPostFrame:
					FastUpdateAfter();
					break;
			}
		}

		protected virtual void UpdateBefore() { }
		protected virtual void UpdateAfter() { }
		protected virtual void GeneralUpdate() { }
		protected virtual void FastUpdateBefore() { }
		protected virtual void FastUpdateAfter() { }

		public FileInfo OpenFileDialog(string currentFile, string path, string fileType, string fileExt)
		{
			return OpenFileDialog(currentFile, path, new FilesystemFilterSet(new FilesystemFilter(fileType, new[] { fileExt })));
		}
		public FileInfo OpenFileDialog(string currentFile, string path, FilesystemFilterSet filterSet)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			using var ofd = new OpenFileDialog
			{
				FileName = !string.IsNullOrWhiteSpace(currentFile)
					? Path.GetFileName(currentFile)
					: $"{Game.FilesystemSafeName()}.{filterSet.Filters.FirstOrDefault()?.Extensions.FirstOrDefault()}",
				InitialDirectory = path,
				Filter = filterSet.ToString(),
				RestoreDirectory = true
			};

			var result = this.ShowDialogWithTempMute(ofd);
			return result.IsOk() ? new FileInfo(ofd.FileName) : null;
		}

		public static FileInfo SaveFileDialog(string currentFile, string path, string fileType, string fileExt, IDialogParent parent)
		{
			return SaveFileDialog(currentFile, path, new FilesystemFilterSet(new FilesystemFilter(fileType, new[] { fileExt })), parent);
		}
		public static FileInfo SaveFileDialog(string currentFile, string path, FilesystemFilterSet filterSet, IDialogParent parent)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			using var sfd = new SaveFileDialog
			{
				FileName = Path.GetFileName(currentFile),
				InitialDirectory = path,
				Filter = filterSet.ToString(),
				RestoreDirectory = true
			};

			var result = parent.ShowDialogWithTempMute(sfd);
			return result.IsOk() ? new FileInfo(sfd.FileName) : null;
		}

		public FileInfo GetWatchFileFromUser(string currentFile)
		{
			return OpenFileDialog(currentFile, Config.PathEntries.WatchAbsolutePath(), "Watch Files", "wch");
		}

		public FileInfo GetWatchSaveFileFromUser(string currentFile)
		{
			return SaveFileDialog(currentFile, Config.PathEntries.WatchAbsolutePath(), "Watch Files", "wch", this);
		}

		public void ViewInHexEditor(MemoryDomain domain, IEnumerable<long> addresses, WatchSize size)
		{
			Tools.Load<HexEditor>();
			Tools.HexEditor.SetToAddresses(addresses, domain, size);
		}

		protected void GenericDragEnter(object sender, DragEventArgs e)
		{
			e.Set(DragDropEffects.Copy);
		}

		protected override void OnLoad(EventArgs e)
		{
			if (MainMenuStrip != null)
			{
				MainMenuStrip.MenuActivate += (sender, args) => MainForm.MaybePauseFromMenuOpened();
				MainMenuStrip.MenuDeactivate += (sender, args) => MainForm.MaybeUnpauseFromMenuClosed();
			}
			base.OnLoad(e);
		}
	}
}
