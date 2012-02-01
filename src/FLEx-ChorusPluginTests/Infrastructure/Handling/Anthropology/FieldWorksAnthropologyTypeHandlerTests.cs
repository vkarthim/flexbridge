﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Chorus.FileTypeHanders;
using Chorus.FileTypeHanders.xml;
using Chorus.merge.xml.generic;
using FLEx_ChorusPlugin.Infrastructure;
using FLEx_ChorusPluginTests.BorrowedCode;
using NUnit.Framework;
using Palaso.IO;
using Palaso.Progress.LogBox;

namespace FLEx_ChorusPluginTests.Infrastructure.Handling.Anthropology
{
	[TestFixture]
	public class FieldWorksAnthropologyTypeHandlerTests : BaseFieldWorksTypeHandlerTests
	{
		private ListenerForUnitTests _eventListener;
		private TempFile _ourFile;
		private TempFile _theirFile;
		private TempFile _commonFile;

		[SetUp]
		public void TestSetup()
		{
			_eventListener = new ListenerForUnitTests();
			FieldWorksTestServices.SetupTempFilesWithName(SharedConstants.DataNotebookFilename, out _ourFile, out _commonFile, out _theirFile);
		}

		[TearDown]
		public void TestTearDown()
		{
			_eventListener = null;
			FieldWorksTestServices.RemoveTempFilesAndParentDir(ref _ourFile, ref _commonFile, ref _theirFile);
		}

		[Test]
		public void DescribeInitialContentsShouldHaveAddedForLabel()
		{
			var initialContents = FileHandler.DescribeInitialContents(null, null);
			Assert.AreEqual(1, initialContents.Count());
			var onlyOne = initialContents.First();
			Assert.AreEqual("Added", onlyOne.ActionLabel);
		}

		[Test]
		public void ExtensionOfKnownFileTypesShouldBeReversal()
		{
			var extensions = FileHandler.GetExtensionsOfKnownTextFileTypes().ToArray();
			Assert.AreEqual(FieldWorksTestServices.ExpectedExtensionCount, extensions.Count(), "Wrong number of extensions.");
			Assert.IsTrue(extensions.Contains(SharedConstants.Ntbk));
		}

		[Test]
		public void ShouldNotBeAbleToValidateIncorrectFormatFile()
		{
			using (var tempModelVersionFile = new TempFile("<classdata />"))
			{
				var newpath = Path.ChangeExtension(tempModelVersionFile.Path, "ntbk");
				File.Copy(tempModelVersionFile.Path, newpath, true);
				Assert.IsFalse(FileHandler.CanValidateFile(newpath));
				File.Delete(newpath);
			}
		}

		[Test]
		public void ShouldBeAbleToValidateInProperlyFormattedFile()
		{
			const string data = @"<Anthropology>
</Anthropology>";
			File.WriteAllText(_ourFile.Path, data);
			Assert.IsTrue(FileHandler.CanValidateFile(_ourFile.Path));
		}
		/*
			const string data =
@"<?xml version='1.0' encoding='utf-8'?>
<TranslatedScripture>
<Scripture guid='06425922-3258-4094-a9ec-3c2fe5b52b39' />
</TranslatedScripture>";

			File.WriteAllText(_ourFile.Path, data);
			Assert.IsTrue(FileHandler.CanValidateFile(_ourFile.Path));
			Assert.IsTrue(FileHandler.CanDiffFile(_ourFile.Path));
			Assert.IsTrue(FileHandler.CanMergeFile(_ourFile.Path));
			Assert.IsTrue(FileHandler.CanPresentFile(_ourFile.Path));
		*/

		[Test]
		public void ShouldBeAbleToDoAllCanOperations()
		{
			const string data = @"<Anthropology>
</Anthropology>";
			File.WriteAllText(_ourFile.Path, data);
			Assert.IsTrue(FileHandler.CanValidateFile(_ourFile.Path));
			Assert.IsTrue(FileHandler.CanDiffFile(_ourFile.Path));
			Assert.IsTrue(FileHandler.CanMergeFile(_ourFile.Path));
			Assert.IsTrue(FileHandler.CanPresentFile(_ourFile.Path));
		}

		[Test]
		public void ShouldNotBeAbleToValidateFile()
		{
			const string data = @"<classdata>
</classdata>";
			File.WriteAllText(_ourFile.Path, data);
			Assert.IsNotNull(FileHandler.ValidateFile(_ourFile.Path, new NullProgress()));
		}

		[Test]
		public void ShouldBeAbleToValidateFile()
		{
			const string data =
@"<?xml version='1.0' encoding='utf-8'?>
<Anthropology>
</Anthropology>";
			File.WriteAllText(_ourFile.Path, data);
			Assert.IsNull(FileHandler.ValidateFile(_ourFile.Path, new NullProgress()));
		}

		[Test]
		public void NewEntryInChildReported()
		{
			const string parent =
@"<?xml version='1.0' encoding='utf-8'?>
<Anthropology>
<header>
<RnResearchNbk guid='c1ed6db2-e382-11de-8a39-0800200c9a66'>
</RnResearchNbk>
</header>
<RnGenericRec guid='c1ed6db3-e382-11de-8a39-0800200c9a66'>
</RnGenericRec>
</Anthropology>";

			const string child =
@"<?xml version='1.0' encoding='utf-8'?>
<Anthropology>
<header>
<RnResearchNbk guid='c1ed6db2-e382-11de-8a39-0800200c9a66'>
</RnResearchNbk>
</header>
<RnGenericRec guid='c1ed6db3-e382-11de-8a39-0800200c9a66'>
</RnGenericRec>
<RnGenericRec guid='c1ed6db4-e382-11de-8a39-0800200c9a66'>
</RnGenericRec>
</Anthropology>";

			File.WriteAllText(_commonFile.Path, parent);
			File.WriteAllText(_ourFile.Path, child);

			var differ = Xml2WayDiffer.CreateFromFiles(_commonFile.Path, _ourFile.Path, _eventListener,
				SharedConstants.Header,
				"RnGenericRec",
				SharedConstants.GuidStr);
			differ.ReportDifferencesToListener();
			_eventListener.AssertExpectedChangesCount(1);
			_eventListener.AssertFirstChangeType<XmlAdditionChangeReport>();
		}

		[Test]
		public void WinnerAndLoserEachAddedNewElement()
		{
			const string commonAncestor =
@"<?xml version='1.0' encoding='utf-8'?>
<Anthropology>
<header>
<RnResearchNbk guid='c1ed6db2-e382-11de-8a39-0800200c9a66'>
</RnResearchNbk>
</header>
<RnGenericRec guid='oldie'>
</RnGenericRec>
</Anthropology>";
			var ourContent = commonAncestor.Replace("</Anthropology>", "<RnGenericRec guid='newbieOurs'/></Anthropology>");
			var theirContent = commonAncestor.Replace("</Anthropology>", "<RnGenericRec guid='newbieTheirs'/></Anthropology>");

			FieldWorksTestServices.DoMerge(
				FileHandler,
				_ourFile, ourContent,
				_commonFile, commonAncestor,
				_theirFile, theirContent,
				new List<string> { @"Anthropology/RnGenericRec[@guid=""oldie""]", @"Anthropology/RnGenericRec[@guid=""newbieOurs""]", @"Anthropology/RnGenericRec[@guid=""newbieTheirs""]" }, null,
				0, new List<Type>(),
				2, new List<Type> { typeof(XmlAdditionChangeReport), typeof(XmlAdditionChangeReport) });
		}

		[Test]
		public void We_JasonDeletedRecordThey_JohnAddedDescription()
		{
			// Part 1 of 2 of the DN merge failure: https://www.pivotaltracker.com/story/show/23829153
			const string ancestor = @"<?xml version='1.0' encoding='utf-8'?>
<Anthropology>
<header>
<RnResearchNbk guid='c1ed6db2-e382-11de-8a39-0800200c9a66'>
</RnResearchNbk>
</header>
	<RnGenericRec
		guid='db4bc870-40b5-4e7d-b55a-ddb33f0ddd52'>
		<DateCreated
			val='2002-3-14 6:0:0.0' />
		<DateModified
			val='2003-5-13 12:32:21.380' />
		<DateOfEvent
			val='200203141' />
		<Description>
			<StText
				guid='5eec4b34-320c-436f-8a10-25cf16345917'>
				<DateModified
					val='2011-2-2 19:24:11.11' />
				<Paragraphs>
					<StTxtPara
						guid='4e6bd967-355b-4918-9c78-4e3f9b38f43c'>
					</StTxtPara>
				</Paragraphs>
			</StText>
		</Description>
	</RnGenericRec>
	<RnGenericRec
		guid='dbad582e-1e2d-4bc6-ac9a-e31e03d6903d'>
	</RnGenericRec>
</Anthropology>";

			const string johnThey =
@"<?xml version='1.0' encoding='utf-8'?>
<Anthropology>
<header>
<RnResearchNbk guid='c1ed6db2-e382-11de-8a39-0800200c9a66'>
</RnResearchNbk>
</header>
	<RnGenericRec
		guid='db4bc870-40b5-4e7d-b55a-ddb33f0ddd52'>
		<DateCreated
			val='2002-3-14 6:0:0.0' />
		<DateModified
			val='2003-5-13 12:32:21.380' />
		<DateOfEvent
			val='200203141' />
		<Description>
			<StText
				guid='5eec4b34-320c-436f-8a10-25cf16345917'>
				<DateModified
					val='2011-2-2 19:24:11.11' />
				<Paragraphs>
					<StTxtPara
						guid='4e6bd967-355b-4918-9c78-4e3f9b38f43c'>
						<Contents>
							<Str>
								<Run
									ws='en'>New stuff.</Run>
							</Str>
						</Contents>
					</StTxtPara>
				</Paragraphs>
			</StText>
		</Description>
	</RnGenericRec>
	<RnGenericRec
		guid='dbad582e-1e2d-4bc6-ac9a-e31e03d6903d'>
	</RnGenericRec>
</Anthropology>";

			const string jasonWe = @"<?xml version='1.0' encoding='utf-8'?>
<Anthropology>
<header>
<RnResearchNbk guid='c1ed6db2-e382-11de-8a39-0800200c9a66'>
</RnResearchNbk>
</header>
	<RnGenericRec
		guid='dbad582e-1e2d-4bc6-ac9a-e31e03d6903d'>
	</RnGenericRec>
</Anthropology>";

			var result = FieldWorksTestServices.DoMerge(
				FileHandler,
				_ourFile, jasonWe,
				_commonFile, ancestor,
				_theirFile, johnThey,
				new List<string> { @"Anthropology/RnGenericRec[@guid=""db4bc870-40b5-4e7d-b55a-ddb33f0ddd52""]" }, null,
				1, new List<Type> { typeof(RemovedVsEditedElementConflict) },
				0, new List<Type>());
			Assert.IsTrue(result.Contains("New stuff."));
		}

		[Test]
		public void They_JasonDeletedRecordWe_JohnAddedDescription()
		{
			// Part 1 of 2 of the DN merge failure: https://www.pivotaltracker.com/story/show/23829153
			const string ancestor = @"<?xml version='1.0' encoding='utf-8'?>
<Anthropology>
<header>
<RnResearchNbk guid='c1ed6db2-e382-11de-8a39-0800200c9a66'>
</RnResearchNbk>
</header>
	<RnGenericRec
		guid='db4bc870-40b5-4e7d-b55a-ddb33f0ddd52'>
		<DateCreated
			val='2002-3-14 6:0:0.0' />
		<DateModified
			val='2003-5-13 12:32:21.380' />
		<DateOfEvent
			val='200203141' />
		<Description>
			<StText
				guid='5eec4b34-320c-436f-8a10-25cf16345917'>
				<DateModified
					val='2011-2-2 19:24:11.11' />
				<Paragraphs>
					<StTxtPara
						guid='4e6bd967-355b-4918-9c78-4e3f9b38f43c'>
					</StTxtPara>
				</Paragraphs>
			</StText>
		</Description>
	</RnGenericRec>
	<RnGenericRec
		guid='dbad582e-1e2d-4bc6-ac9a-e31e03d6903d'>
	</RnGenericRec>
</Anthropology>";

			const string johnWe =
@"<?xml version='1.0' encoding='utf-8'?>
<Anthropology>
<header>
<RnResearchNbk guid='c1ed6db2-e382-11de-8a39-0800200c9a66'>
</RnResearchNbk>
</header>
	<RnGenericRec
		guid='db4bc870-40b5-4e7d-b55a-ddb33f0ddd52'>
		<DateCreated
			val='2002-3-14 6:0:0.0' />
		<DateModified
			val='2003-5-13 12:32:21.380' />
		<DateOfEvent
			val='200203141' />
		<Description>
			<StText
				guid='5eec4b34-320c-436f-8a10-25cf16345917'>
				<DateModified
					val='2011-2-2 19:24:11.11' />
				<Paragraphs>
					<StTxtPara
						guid='4e6bd967-355b-4918-9c78-4e3f9b38f43c'>
						<Contents>
							<Str>
								<Run
									ws='en'>New stuff.</Run>
							</Str>
						</Contents>
					</StTxtPara>
				</Paragraphs>
			</StText>
		</Description>
	</RnGenericRec>
	<RnGenericRec
		guid='dbad582e-1e2d-4bc6-ac9a-e31e03d6903d'>
	</RnGenericRec>
</Anthropology>";

			const string jasonThey = @"<?xml version='1.0' encoding='utf-8'?>
<Anthropology>
<header>
<RnResearchNbk guid='c1ed6db2-e382-11de-8a39-0800200c9a66'>
</RnResearchNbk>
</header>
	<RnGenericRec
		guid='dbad582e-1e2d-4bc6-ac9a-e31e03d6903d'>
	</RnGenericRec>
</Anthropology>";

			var result = FieldWorksTestServices.DoMerge(
				FileHandler,
				_ourFile, johnWe,
				_commonFile, ancestor,
				_theirFile, jasonThey,
				new List<string> { @"Anthropology/RnGenericRec[@guid=""db4bc870-40b5-4e7d-b55a-ddb33f0ddd52""]" }, null,
				1, new List<Type> { typeof(EditedVsRemovedElementConflict) },
				0, new List<Type>());
			Assert.IsTrue(result.Contains("New stuff."));
		}

		[Test]
		public void WeRemovedStuffAndTheyEditedItInAListInHeader()
		{
			// Part 2 of 2 of the DN merge failure: https://www.pivotaltracker.com/story/show/23829153
			const string commonAncestor =
@"<?xml version='1.0' encoding='utf-8'?>
<Anthropology>
<header>
<RnResearchNbk guid='c1ed6db2-e382-11de-8a39-0800200c9a66'>
</RnResearchNbk>
		<AnthroList>
			<CmPossibilityList
				guid='d87ba7c6-ea5e-11de-9dd2-0013722f8dec'>
				<Possibilities>
					<CmAnthroItem
						guid='27df36c2-3a78-436b-b1ff-fd632039cad6'>
						<Name>
							<AUni
								ws='en'>Project Variables</AUni>
						</Name>
					</CmAnthroItem>
					<CmAnthroItem
						guid='27df36c2-3a78-436b-b1ff-fd632039cad7'>
						<Name>
							<AUni
								ws='en'>Project Stable Things</AUni>
						</Name>
					</CmAnthroItem>
				</Possibilities>
			</CmPossibilityList>
		</AnthroList>
</header>
<RnGenericRec guid='oldie'>
</RnGenericRec>
</Anthropology>";
			const string ourContent = @"<?xml version='1.0' encoding='utf-8'?>
<Anthropology>
<header>
<RnResearchNbk guid='c1ed6db2-e382-11de-8a39-0800200c9a66'>
</RnResearchNbk>
		<AnthroList>
			<CmPossibilityList
				guid='d87ba7c6-ea5e-11de-9dd2-0013722f8dec'>
				<Possibilities>
					<CmAnthroItem
						guid='27df36c2-3a78-436b-b1ff-fd632039cad7'>
						<Name>
							<AUni
								ws='en'>Project Stable Things</AUni>
						</Name>
					</CmAnthroItem>
				</Possibilities>
			</CmPossibilityList>
		</AnthroList>
</header>
<RnGenericRec guid='oldie'>
</RnGenericRec>
</Anthropology>";
			const string theirContent = @"<?xml version='1.0' encoding='utf-8'?>
<Anthropology>
<header>
<RnResearchNbk guid='c1ed6db2-e382-11de-8a39-0800200c9a66'>
</RnResearchNbk>
		<AnthroList>
			<CmPossibilityList
				guid='d87ba7c6-ea5e-11de-9dd2-0013722f8dec'>
				<Possibilities>
					<CmAnthroItem
						guid='27df36c2-3a78-436b-b1ff-fd632039cad6'>
						<Name>
							<AUni
								ws='en'>Their Deletion Prevention Change</AUni>
						</Name>
					</CmAnthroItem>
					<CmAnthroItem
						guid='27df36c2-3a78-436b-b1ff-fd632039cad7'>
						<Name>
							<AUni
								ws='en'>Project Stable Things</AUni>
						</Name>
					</CmAnthroItem>
				</Possibilities>
			</CmPossibilityList>
		</AnthroList>
</header>
<RnGenericRec guid='oldie'>
</RnGenericRec>
</Anthropology>";

			var results = FieldWorksTestServices.DoMerge(
				FileHandler,
				_ourFile, ourContent,
				_commonFile, commonAncestor,
				_theirFile, theirContent,
				null, null,
				1, new List<Type> { typeof(RemovedVsEditedElementConflict) },
				0, new List<Type>());
			Assert.IsTrue(results.Contains("Their Deletion Prevention Change"));
			Assert.IsTrue(results.Contains("Project Stable Things"));
		}

		[Test]
		public void WeEditedStuffAndTheyRemovedItInAListInHeader()
		{
			// Part 2 of 2 of the DN merge failure: https://www.pivotaltracker.com/story/show/23829153
			const string commonAncestor =
@"<?xml version='1.0' encoding='utf-8'?>
<Anthropology>
<header>
<RnResearchNbk guid='c1ed6db2-e382-11de-8a39-0800200c9a66'>
</RnResearchNbk>
		<AnthroList>
			<CmPossibilityList
				guid='d87ba7c6-ea5e-11de-9dd2-0013722f8dec'>
				<Possibilities>
					<CmAnthroItem
						guid='27df36c2-3a78-436b-b1ff-fd632039cad6'>
						<Name>
							<AUni
								ws='en'>Project Variables</AUni>
						</Name>
					</CmAnthroItem>
					<CmAnthroItem
						guid='27df36c2-3a78-436b-b1ff-fd632039cad7'>
						<Name>
							<AUni
								ws='en'>Project Stable Things</AUni>
						</Name>
					</CmAnthroItem>
				</Possibilities>
			</CmPossibilityList>
		</AnthroList>
</header>
<RnGenericRec guid='oldie'>
</RnGenericRec>
</Anthropology>";
			const string ourContent = @"<?xml version='1.0' encoding='utf-8'?>
<Anthropology>
<header>
<RnResearchNbk guid='c1ed6db2-e382-11de-8a39-0800200c9a66'>
</RnResearchNbk>
		<AnthroList>
			<CmPossibilityList
				guid='d87ba7c6-ea5e-11de-9dd2-0013722f8dec'>
				<Possibilities>
					<CmAnthroItem
						guid='27df36c2-3a78-436b-b1ff-fd632039cad6'>
						<Name>
							<AUni
								ws='en'>Our Deletion Prevention Change</AUni>
						</Name>
					</CmAnthroItem>
					<CmAnthroItem
						guid='27df36c2-3a78-436b-b1ff-fd632039cad7'>
						<Name>
							<AUni
								ws='en'>Project Stable Things</AUni>
						</Name>
					</CmAnthroItem>
				</Possibilities>
			</CmPossibilityList>
		</AnthroList>
</header>
<RnGenericRec guid='oldie'>
</RnGenericRec>
</Anthropology>";
			const string theirContent = @"<?xml version='1.0' encoding='utf-8'?>
<Anthropology>
<header>
<RnResearchNbk guid='c1ed6db2-e382-11de-8a39-0800200c9a66'>
</RnResearchNbk>
		<AnthroList>
			<CmPossibilityList
				guid='d87ba7c6-ea5e-11de-9dd2-0013722f8dec'>
				<Possibilities>
					<CmAnthroItem
						guid='27df36c2-3a78-436b-b1ff-fd632039cad7'>
						<Name>
							<AUni
								ws='en'>Project Stable Things</AUni>
						</Name>
					</CmAnthroItem>
				</Possibilities>
			</CmPossibilityList>
		</AnthroList>
</header>
<RnGenericRec guid='oldie'>
</RnGenericRec>
</Anthropology>";

			var results = FieldWorksTestServices.DoMerge(
				FileHandler,
				_ourFile, ourContent,
				_commonFile, commonAncestor,
				_theirFile, theirContent,
				null, null,
				1, new List<Type> { typeof(EditedVsRemovedElementConflict) },
				0, new List<Type>());
			Assert.IsTrue(results.Contains("Our Deletion Prevention Change"));
			Assert.IsTrue(results.Contains("Project Stable Things"));
		}
	}
}