﻿using System;
using System.Collections.Generic;
using System.Xml;
using Chorus.FileTypeHanders;
using Chorus.FileTypeHanders.xml;
using Chorus.merge;
using Chorus.merge.xml.generic;
using Chorus.VcsDrivers.Mercurial;
using FLEx_ChorusPlugin.Infrastructure;
using Palaso.IO;
using Palaso.Progress.LogBox;

namespace FLEx_ChorusPlugin.Contexts.General
{
	/// <summary>
	/// File handler for a FieldWorks 7.0 xml class data file (not the main fwdata file).
	/// </summary>
	internal sealed class FieldWorksFileHandler : IChorusFileTypeHandler
	{
		private const string Extension = "ClassData";
		private readonly Dictionary<string, bool> _filesChecked = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
		private readonly MetadataCache _mdc = MetadataCache.MdCache;

		#region Implementation of IChorusFileTypeHandler

		public bool CanDiffFile(string pathToFile)
		{
			return CheckThatInputIsValidFieldWorksFile(pathToFile);
		}

		public bool CanMergeFile(string pathToFile)
		{
			return CheckThatInputIsValidFieldWorksFile(pathToFile);
		}

		public bool CanPresentFile(string pathToFile)
		{
			return CheckThatInputIsValidFieldWorksFile(pathToFile);
		}

		public bool CanValidateFile(string pathToFile)
		{
			if (!FileUtils.CheckValidPathname(pathToFile, Extension))
				return false;

			try
			{
				var settings = new XmlReaderSettings { ValidationType = ValidationType.None };
				using (var reader = XmlReader.Create(pathToFile, settings))
				{
					reader.MoveToContent();
					return reader.LocalName == "classdata";
				}
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Do a 3-file merge, placing the result over the "ours" file and returning an error status
		/// </summary>
		/// <remarks>Implementations can exit with an exception, which the caller will catch and deal with.
		/// The must not have any UI, no interaction with the user.</remarks>
		public void Do3WayMerge(MergeOrder mergeOrder)
		{
			if (mergeOrder == null) throw new ArgumentNullException("mergeOrder");

			// Add optional custom property information to MDC.
			FieldWorksMergingServices.AddCustomPropInfo(_mdc, mergeOrder, "DataFiles", 1);

			XmlMergeService.Do3WayMerge(mergeOrder,
				new FieldWorksMergingStrategy(mergeOrder.MergeSituation, _mdc),
				null,
				"rt", "guid", WritePreliminaryInformation);
		}

		public IEnumerable<IChangeReport> Find2WayDifferences(FileInRevision parent, FileInRevision child, HgRepository repository)
		{
			return Xml2WayDiffService.ReportDifferences(repository, parent, child,
				null,
				"rt", "guid");
		}

		public IChangePresenter GetChangePresenter(IChangeReport report, HgRepository repository)
		{
			if (report is IXmlChangeReport)
				return new FieldWorksChangePresenter((IXmlChangeReport)report);

			if (report is ErrorDeterminingChangeReport)
				return (IChangePresenter)report;

			return new DefaultChangePresenter(report, repository);
		}

		/// <summary>
		/// return null if valid, otherwise nice verbose description of what went wrong
		/// </summary>
		public string ValidateFile(string pathToFile, IProgress progress)
		{
			try
			{
				var settings = new XmlReaderSettings { ValidationType = ValidationType.None };
				using (var reader = XmlReader.Create(pathToFile, settings))
				{
					reader.MoveToContent();
					if (reader.LocalName == "classdata")
					{
						// It would be nice, if it could really validate it.
						while (reader.Read())
						{
						}
					}
					else
					{
						throw new InvalidOperationException("Not a FieldWorks file.");
					}
				}
			}
			catch (Exception error)
			{
				return error.Message;
			}
			return null;
		}

		/// <summary>
		/// This is like a diff, but for when the file is first checked in.  So, for example, a dictionary
		/// handler might list any the words that were already in the dictionary when it was first checked in.
		/// </summary>
		public IEnumerable<IChangeReport> DescribeInitialContents(FileInRevision fileInRevision, TempFile file)
		{
			return new IChangeReport[] { new DefaultChangeReport(fileInRevision, "Added") };
		}

		public IEnumerable<string> GetExtensionsOfKnownTextFileTypes()
		{
			yield return Extension;
		}

		/// <summary>
		/// Return the maximum file size that can be added to the repository.
		/// </summary>
		/// <remarks>
		/// Return UInt32.MaxValue for no limit.
		/// </remarks>
		public uint MaximumFileSize
		{
			get { return UInt32.MaxValue; }
		}

		#endregion

		private bool CheckThatInputIsValidFieldWorksFile(string pathToFile)
		{
			if (!FileUtils.CheckValidPathname(pathToFile, Extension))
				return false;

			bool seenBefore;
			if (_filesChecked.TryGetValue(pathToFile, out seenBefore))
				return seenBefore;
			var retval = ValidateFile(pathToFile, null) == null;
			_filesChecked.Add(pathToFile, retval);
			return retval;
		}

		private static void WritePreliminaryInformation(XmlReader reader, XmlWriter writer)
		{
			reader.MoveToContent();
			writer.WriteStartElement("classdata");
			reader.Read();
		}
	}
}