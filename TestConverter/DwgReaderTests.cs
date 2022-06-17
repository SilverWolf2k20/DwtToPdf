//-----------------------------------------------------------------------------
// <copyright file="DwgReaderTests.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс для тестирования модуля чтения фала.
// </summary>
//-----------------------------------------------------------------------------

using DwtReader;
using DwtReader.Data;

using NUnit.Framework;

namespace Converter.UnitTests
{
    /// <summary>
    /// Тестирование модуля чтения файла.
    /// </summary>
    public class DwgReaderTests
    {
        /// <summary>
        /// Проверка различных файлов.
        /// </summary>
        /// <param name="file">Файл</param>
        /// <param name="expected">Ожидаемое возвращаемое значение</param>
        [TestCase(null,                                            ReaderState.PathIsNull)]
        [TestCase("",                                              ReaderState.InvalidFileName)]
        [TestCase(@"D:\FQW\Входные файлы\specification_2018.dwg",  ReaderState.FileIsDwg)]
        [TestCase(@"D:\FQW\Входные файлы\specification_2018.DWG",  ReaderState.FileIsDwg)]
        [TestCase(@"D:\FQW\Входные файлы\specification_2013.dwt",  ReaderState.Success)]
        [TestCase(@"D:\FQW\Входные файлы\specification_2013.DWT",  ReaderState.Success)]
        [TestCase(@"D:\FQW\Входные файлы\layoutIn_model_2013.dwg", ReaderState.FileIsBig)]
        [TestCase(@"D:\FQW\Входные файлы\does_not_exist.dwt",      ReaderState.FatalError)]
        public void FileAnalysis_VariousPaths_CheckThem(string file, ReaderState expected)
        {
            var reader = new DwtFileReader(file);
            reader.FileAnalysis();
            Assert.AreEqual(reader.State, expected);
        }

        /// <summary>
        /// Проверка различных версий файлов.
        /// </summary>
        /// <param name="file">Файл</param>
        /// <param name="expected">Ожидаемое возвращаемое значение</param>
        [TestCase(@"D:\FQW\Входные файлы\specification_2000.dwt", null)]
        [TestCase(@"D:\FQW\Входные файлы\specification_2004.dwt", null)]
        [TestCase(@"D:\FQW\Входные файлы\specification_2007.dwt", null)]
        [TestCase(@"D:\FQW\Входные файлы\specification_2018.dwt", null)]
        public void Read_VariousFileVersion_CheckThem(string file, DwtFile expected)
        {
            var reader = new DwtFileReader(file);
            reader.FileAnalysis();
            Assert.AreEqual(reader.Read(), expected);
        }
    }
}