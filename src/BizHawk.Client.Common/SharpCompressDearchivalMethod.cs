#nullable enable

using System.IO;
using System.Linq;
using BizHawk.Common;
using SharpCompress.Archives;

namespace BizHawk.Client.Common
{
	/// <summary>A <see cref="IFileDearchivalMethod{T}">dearchival method</see> for <see cref="HawkFile"/> implemented using <c>SharpCompress</c> from NuGet.</summary>
	public class SharpCompressDearchivalMethod : IFileDearchivalMethod<SharpCompressArchiveFile>
	{
		private SharpCompressDearchivalMethod() {}

		public bool CheckSignature(string fileName, out int offset, out bool isExecutable)
		{
			offset = 0;
			isExecutable = false;

			try
			{
				using var arcTest = ArchiveFactory.Open(fileName);
			}
			catch
			{
				return false;
			}
			return true; // no exception? good enough
		}

		public SharpCompressArchiveFile Construct(string path) => new SharpCompressArchiveFile(path);

		public static readonly SharpCompressDearchivalMethod Instance = new SharpCompressDearchivalMethod();
	}
}