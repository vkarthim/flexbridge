﻿using System.Text;

namespace FLEx_ChorusPlugin.Infrastructure
{
	internal static class SharedConstants
	{
		internal static readonly Encoding Utf8 = Encoding.UTF8;

		// General
		internal const string Collections = "Collections";
		internal const string MultiAlt = "MultiAlt";
		internal const string Owning = "Owning";
		internal const string Objsur = "objsur";
		internal const string GuidStr = "guid";
		internal const string Header = "header";
		internal const string OwnerGuid = "ownerguid";
		internal const string Class = "class";
		internal const string Name = "name";
		internal const string Ownseq = "ownseq";
		internal const string Refseq = "refseq";
		internal const string Custom = "Custom";

		// Old style
		internal const string RtTag = "rt";
		internal const string ClassData = "ClassData";

		// Model Version
		internal const string ModelVersion = "ModelVersion";

		// Custom Properties
		internal const string AdditionalFieldsTag = "AdditionalFields";
		internal const string CustomProperties = "CustomProperties";

		// Common
		internal const string List = "list";
		internal const string Styles = "Styles";
		internal const string Style = "style";
		internal const string StStyle = "StStyle";

		// Linguistics
		internal const string Reversal = "reversal";

		// Anthropology
		internal const string DataNotebook = "DataNotebook";
		internal const string Ntbk = "ntbk";
		internal const string DataNotebookFilename = DataNotebook + "." + Ntbk;

		// Scripture
		internal const string TranslatedScripture = "TranslatedScripture";
		internal const string ScriptureReferenceSystem = "ScriptureReferenceSystem";
		internal const string ArchivedDrafts = "ArchivedDrafts";
		internal const string ArchivedDraft = "ArchivedDraft";
		internal const string ImportSettingsFilename = "Settings." + ImportSetting;
		internal const string ImportSettings = "ImportSettings";
		internal const string ImportSetting = "ImportSetting";
		internal const string Trans = "trans";
		internal const string Scripture = "Scripture";
		internal const string Srs = "srs";
		internal const string ScriptureReferenceSystemFilename = ScriptureReferenceSystem + "." + Srs;
		internal const string ScriptureTranslation = "ScriptureTranslation";
		internal const string ScriptureTransFilename = ScriptureTranslation + "." + Trans;
		internal const string Linguistics = "Linguistics";
		internal const string Anthropology = "Anthropology";
	}
}