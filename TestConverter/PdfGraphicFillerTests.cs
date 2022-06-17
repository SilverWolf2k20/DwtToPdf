//-----------------------------------------------------------------------------
// <copyright file="PdfGraphicFillerTests.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс для тестирования модуля записи в файл.
// </summary>
//-----------------------------------------------------------------------------

using DwtDrawer;

using DwtReader.Data;

using NUnit.Framework;

namespace Converter.UnitTests
{
    /// <summary>
    /// Тестирование модуля записи в файл.
    /// </summary>
    public class PdfGraphicFillerTests
    {
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(@"DF:\", false)]
        public void FileAnalysis_VariousPaths_CheckThem(string file, bool expected)
        {
            var pdfDrawer = new PdfDrawer(file);
            var data = new DwtFile();
            Assert.AreEqual(pdfDrawer.Draw(data, null), expected);
        }
    }
}
