//-----------------------------------------------------------------------------
// <copyright file="PdfDrawer.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс для отображения чертежа на листе PDF.
// </summary>
//-----------------------------------------------------------------------------

using System.Text;

using DwtDrawer.Components;
using DwtDrawer.Data;

using DwtReader.Data;
using DwtReader.Objects;
using DwtReader.Objects.Componets;

using iTextSharp.text;
using iTextSharp.text.pdf;

using static iTextSharp.text.Utilities;

namespace DwtDrawer
{
    /// <summary>
    /// Прорисовщик чертежа на листах PDF.
    /// </summary>
    public class PdfDrawer
    {
        private Document?  _document = null;
        private PdfWriter? _writer   = null;
        private BaseFont?  _font     = null;

        private string _fileName = string.Empty;

        /// <summary>
        /// Создает экземпляр класса.
        /// </summary>
        /// <param name="name">Имя создаваемого PDF файла.</param>
        public PdfDrawer(string name)
        {
            _document = new Document();
            _fileName = name;

            TextSetting();
        }

        /// <summary>
        /// Русует объекты в PDF документе.
        /// </summary>
        /// <param name="objects">Список объектов для прорисовки</param>
        /// <returns>True - если прорисовка прошла успешно, иначе - false</returns>
        public bool Draw(DwtFile file, List<PdfSheet> sheets)
        {
            // Создать PDF документ.
            if (CreateFile() == false)
                return false;

            // Проверить существование объектов _document и _writer.
            if (_document is null || _writer is null)
                return false;

            // Открыть документ и создать объект прорисовки данных.
            _document.Open();
            var content = _writer.DirectContent;

            // Берется лист из списка настроек листов.
            for (var i = 0; i < sheets.Count; ++i) {
                // Настраивается размер листа.
                _document.SetPageSize(ComputePageSize(sheets[i]));
                _document.NewPage();

                // Прорисовываются объекты на лоисте.
                DrawingObjects(file, sheets[i], content);
            }
            // Закрывается PDF документ.
            _document.Close();
            _writer.Close();

            return true;
        }

        /// <summary>
        /// Создание файла.
        /// </summary>
        /// <returns>True - если файл создан, иначе - false</returns>
        private bool CreateFile()
        {
            if (_fileName is null || _fileName == string.Empty)
                return false;

            try {
                _writer = PdfWriter.GetInstance(_document, new FileStream(_fileName, FileMode.Create));
            }
            catch (Exception) {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Настройка текста.
        /// </summary>
        private void TextSetting()
        {
            EncodingProvider encodingProvider = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(encodingProvider);

            SetTextFont("ARIAL.TTF");
        }

        /// <summary>
        /// Установка шрифта.
        /// </summary>
        /// <param name="fontName">Имя шрифта</param>
        private void SetTextFont(string fontName)
        {
            string ttf = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), fontName);
            _font = BaseFont.CreateFont(ttf, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
        }

        #region Calculations
        /// <summary>
        /// Определение размера листа.
        /// </summary>
        /// <param name="sheet">Лист</param>
        /// <returns>Размеры листа</returns>
        private Rectangle ComputePageSize(PdfSheet sheet)
            => new RectangleReadOnly(MillimetersToPoints(sheet.Setting.Width), MillimetersToPoints(sheet.Setting.Height));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="radian"></param>
        /// <returns></returns>
        private float ToGradus(float radian)
            => (radian * 180) / (float)Math.PI;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private float ComputeOffsetX(PdfSheet sheet, float value)
            => MillimetersToPoints(value - sheet.Setting.OffsetX);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private float ComputeOffsetY(PdfSheet sheet, float value)
            => MillimetersToPoints(value - sheet.Setting.OffsetY);
        #endregion

        /// <summary>
        /// Прорисовка объектов.
        /// </summary>
        /// <param name="file">Файл с данными</param>
        /// <param name="sheet">Страница</param>
        /// <param name="content">Прорисовщик данных</param>
        private void DrawingObjects(DwtFile file, PdfSheet sheet, PdfContentByte content)
        {
            foreach (var entity in file.Objects) {
                if (entity.type == ObjectType.LwPolyLine) {
                    var polyline = entity.entity as LwPolyLine;

                    if (polyline is null || polyline.Owner.reference != sheet.ID || polyline.Invisible == 1)
                        continue;

                    content.SetLineWidth(polyline.Thickness);

                    for (int p = 0; p < polyline.Points.Count - 1; ++p) {
                        content.MoveTo(ComputeOffsetX(sheet, (float)polyline.Points[p].x),
                                       ComputeOffsetY(sheet, (float)polyline.Points[p].y));
                        content.LineTo(ComputeOffsetX(sheet, (float)polyline.Points[p + 1].x),
                                       ComputeOffsetY(sheet, (float)polyline.Points[p + 1].y));
                        content.Stroke();
                    }
                }
                if (entity.type == ObjectType.Line) {
                    var line = entity.entity as Line;
                    if (line is null || line.Owner.reference != sheet.ID || line.Invisible == 1)
                        continue;
                    content.SetLineWidth(line.Thickness);
                    content.MoveTo(ComputeOffsetX(sheet, (float)line.StartX),
                                   ComputeOffsetY(sheet, (float)line.StartY));
                    content.LineTo(ComputeOffsetX(sheet, (float)line.EndX),
                                   ComputeOffsetY(sheet, (float)line.EndY));
                    content.Stroke();
                }
                if (entity.type == ObjectType.Circle) {
                    var circle = entity.entity as Circle;
                    if (circle is null || circle.Owner.reference != sheet.ID || circle.Invisible == 1)
                        continue;
                    content.SetLineWidth(circle.Thickness);
                    content.Circle(ComputeOffsetX(sheet, (float)circle.CenterX),
                                   ComputeOffsetY(sheet, (float)circle.CenterY),
                                   MillimetersToPoints(  (float)circle.Radius));

                    content.Stroke();
                }

                if (entity.type == ObjectType.Text) {
                    var text = entity.entity as Text;

                    if (text is null || text.Owner.reference != sheet.ID || text.Invisible == 1)
                        continue;

                    TextAlignment alignment = new TextAlignment();
                    (double x, double y) = alignment.GetInsertionCoordinates(text);

                    content.SetFontAndSize(_font, MillimetersToPoints((float)text.Height));
                    content.BeginText();
                    content.ShowTextAligned(1,
                        text.Value,
                        ComputeOffsetX(sheet, (float)x),
                        ComputeOffsetY(sheet, (float)y),
                        ToGradus((float)text.RotationAngle));

                    content.EndText();
                }
            }
        }
    }
}
