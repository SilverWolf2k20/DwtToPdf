//-----------------------------------------------------------------------------
// <copyright file="MainWindow.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс главного окна.
// </summary>
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using DwtDrawer;
using DwtDrawer.Components;
using DwtDrawer.Data;

using DwtReader;
using DwtReader.Data;
using DwtReader.Objects;
using DwtReader.Objects.Componets;

using Microsoft.Win32;

using Usl;

using Shapes = System.Windows.Shapes;

namespace GUIConverter
{
    /// <summary>
    /// Логика взаимодействия основного окна.
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _sourcePath = string.Empty;
        private string _exportPath = string.Empty;

        private bool _isSelect = false;
        DwtFile?     _dwtData  = null;

        private List<PdfSheet>? _pdfSheets = new();

        private double _factor = 1;

        #region WindowEvent
        public MainWindow()
        {
            InitializeComponent();
            _cmd.Content = "Выберите файл.";

            Logger.Level = LogLevel.Trace;
            Logger.WriteToFile("Converter.log");

        }

        /// <summary>
        /// Событие закрытия окна.
        /// </summary>
        /// <param name="sender">Объект</param>
        /// <param name="e">Событие</param>
        private void WindowClosed(object sender, EventArgs e)
        {
            Logger.Trace("Сохранение логов.");
            Logger.SaveFile();
        }
        #endregion

        #region ObjectEvent
        /// <summary>
        /// Обработчик нажатия кнопки конвертации.
        /// </summary>
        /// <param name="sender">Объект</param>
        /// <param name="e">Событие</param>
        private void ButtonConvertClick(object sender, RoutedEventArgs e)
        {
            if (_isSelect == false)
                return;

            if (ExportToPdfFile() == false) {
                _cmd.Content = "Преобразование прошло с ошибкой!";
                return;
            }

            _cmd.Content = "Преобразование прошло успешно!";
        }

        /// <summary>
        /// Обработчик нажатия кнопки выбора файла.
        /// </summary>
        /// <param name="sender">Объект</param>
        /// <param name="e">Событие</param>
        private void ButtonSelectClick(object sender, RoutedEventArgs e)
        {
            // Создание диалогового окна.
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CAD файлы (*.dwt)|*.dwt|Все файлы (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;

            bool? result = openFileDialog.ShowDialog();

            if (result is null)
                return;

            // Загрузка файла.
            _sourcePath  = openFileDialog.FileName;
            _dwtData     = ImportDwtFile(_sourcePath);
            _cmd.Content = "Чтение файла завершено.";

            if (_dwtData is null || result == false) {
                _cmd.Content = "Чтение файла завершено c ошибкой.";
                return;
            }

            // Определение имени файлов для импорта/экспорта.
            _dwtFile.Content = Path.GetFileName(_sourcePath);
            _exportPath      = Path.ChangeExtension(_sourcePath, ".pdf");
            _pdfFile.Content = Path.GetFileName(_exportPath);

            // Определение настроек для каждого листа.
            var customer = new SheetCustomizer();
            _pdfSheets   = customer.GetSheets(_dwtData);

            // Загрузка списка листов.
            AddSheetsOnList();

            // Отображение предпросмотра.
            UpdateInfo();
            ShowPreview();
            _isSelect = true;
        }

        /// <summary>
        /// Выбор другого листа дял просмотра.
        /// </summary>
        /// <param name="sender">Объект</param>
        /// <param name="e">Событие</param>
        private void SheetsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_sheets.SelectedItem is null)
                return;

            UpdateInfo();
            ShowPreview();
        }

        /// <summary>
        /// Обработка нажатия на ссылку.
        /// </summary>
        /// <param name="sender">Объект</param>
        /// <param name="e">Событие</param>
        private void HyperlinkClick(object sender, RoutedEventArgs e)
        {
            var url = "https://vk.link/okolo_it_govnokoding";
            url = url.Replace("&", "^&");

            Process.Start(new ProcessStartInfo(url) {
                UseShellExecute = true
            });
        }
        #endregion

        #region Converter
        /// <summary>
        /// Чтение DWT файла.
        /// </summary>
        /// <param name="dwtPath">Путь до файла</param>
        /// <returns>Данные файла</returns>
        private DwtFile? ImportDwtFile(string dwtPath)
        {
            DwtFileReader fileReader = new DwtFileReader(dwtPath);

            var fileIsGood = fileReader.FileAnalysis();
            ReadStateAnalise(fileReader.State);

            if (fileIsGood == false)
                return null;

            _cmd.Content = "Чтение файла.";
            return fileReader.Read();
        }

        /// <summary>
        /// Запись в PDF файл.
        /// </summary>
        /// <returns>True - если запись прошла успешно, иначе - false</returns>
        private bool ExportToPdfFile()
        {
            if (_dwtData is null || _pdfSheets is null)
                return false;

            var drawer = new PdfDrawer(_exportPath);
            return drawer.Draw(_dwtData, _pdfSheets);
        }

        /// <summary>
        /// Вывод сообщения в зависимости от статуса загрузки.
        /// </summary>
        /// <param name="state">Статус</param>
        private void ReadStateAnalise(ReaderState state)
        {
            switch (state) {
                case ReaderState.Success:
                    _cmd.Content = "Чтение файла прошло успешно.";
                    break;
                case ReaderState.FileIsBig:
                    _cmd.Content = "Внимание! Файл имеет большой размер.";
                    break;
                case ReaderState.VersionNotSupported:
                    _cmd.Content = "Версия файла не поддерживается.";
                    break;
                case ReaderState.FatalError:
                    _cmd.Content = "Произошла критическая ошибка.";
                    break;
                case ReaderState.FileIsDwg:
                    _cmd.Content = "Внимание! Файл имеет формат DWG.";
                    break;
                case ReaderState.InvalidFileName:
                    _cmd.Content = "Указанно неверное имя файла.";
                    break;
                case ReaderState.PathIsNull:
                    _cmd.Content = "Ошибка пустого пути.";
                    break;
            }
        }

        /// <summary>
        /// Обновить информации о печати.
        /// </summary>
        private void UpdateInfo()
        {
            if (_pdfSheets is null || _pdfSheets.Count == 0)
                return;

            _format.Content = _pdfSheets[_sheets.SelectedIndex].Setting.Format.ToString();
            _orientation.Content = OrientationToString(_pdfSheets[_sheets.SelectedIndex].Setting.Orientation);
            _size.Content = _pdfSheets[_sheets.SelectedIndex].Setting.Width.ToString() + "x" + _pdfSheets[_sheets.SelectedIndex].Setting.Height.ToString();
        }

        /// <summary>
        /// Перевести ориентацию в текст.
        /// </summary>
        /// <param name="orientation">Ориентация</param>
        /// <returns>Ориентация</returns>
        private string OrientationToString(SheetOrientation orientation)
        {
            if (orientation == SheetOrientation.Bookstore)
                return "Книжная";
            return "Альбомная";
        }
        #endregion

        #region Preview
        /// <summary>
        /// Показать чертеж в окне предпросмотра.
        /// </summary>
        private void ShowPreview()
        {
            if (_dwtData is null || _pdfSheets is null || _pdfSheets.Count == 0)
                return;

            _content.Children.Clear();
            _factor = ComputeMagnificationFactor(
                _pdfSheets[_sheets.SelectedIndex].Setting.Height,
                _pdfSheets[_sheets.SelectedIndex].Setting.Width
            );

            var sheets = new Shapes.Rectangle();
            sheets.Fill     = Brushes.White;
            sheets.Height   = _factor * _pdfSheets[_sheets.SelectedIndex].Setting.Height;
            sheets.Width    = _factor * _pdfSheets[_sheets.SelectedIndex].Setting.Width;

            _content.Children.Add(sheets);
            Canvas.SetLeft(sheets, GetCenterOnX());
            Canvas.SetBottom(sheets, GetCenterOnY());

            foreach (var entity in _dwtData.Objects) {
                if (entity.type == ObjectType.LwPolyLine) {
                    var polyline = entity.entity as LwPolyLine;
                    if (polyline is null)
                        continue;
                    if (polyline.Owner.reference != _pdfSheets[_sheets.SelectedIndex].ID)
                        continue;
                    if (polyline.Invisible == 1)
                        continue;
                    for (int i = 0; i < polyline.Points.Count - 1; i++) {
                        var point1 = polyline.Points[i];
                        var point2 = polyline.Points[i + 1];

                        var canvasline = new Shapes.Line();
                        canvasline.Stroke = Brushes.Black;
                        canvasline.X1 = Norm(point1.x * _factor);
                        canvasline.X2 = Norm(point2.x * _factor);
                        canvasline.Y1 = Reflect(point1.y * _factor);
                        canvasline.Y2 = Reflect(point2.y * _factor);
                        canvasline.StrokeThickness = 1;
                        _content.Children.Add(canvasline);
                    }
                }

                if (entity.type == ObjectType.Line) {
                    var line = entity.entity as Line;
                    if (line is null)
                        continue;
                    if (line.Owner.reference != _pdfSheets[_sheets.SelectedIndex].ID)
                        continue;
                    if (line.Invisible == 1)
                        continue;
                    var canvasline = new Shapes.Line();
                    canvasline.Stroke = Brushes.Black;
                    canvasline.X1 = Norm(line.StartX * _factor);
                    canvasline.X2 = Norm(line.EndX * _factor);
                    canvasline.Y1 = Reflect(line.StartY * _factor);
                    canvasline.Y2 = Reflect(line.EndY * _factor);
                    canvasline.StrokeThickness = 1;
                    _content.Children.Add(canvasline);
                }
            }
        }

#nullable disable
        /// <summary>
        /// Выравнивание по оси X.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Исправленное значение</returns>
        private int Norm(double value)
            => (int)(value - _pdfSheets[_sheets.SelectedIndex].Setting.OffsetX * _factor + GetCenterOnX());

        /// <summary>
        /// Выравнивание и отражение по оси Y.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Исправленное значение</returns>
        private int Reflect(double value)
            => (int)((_content.ActualHeight - (value - _pdfSheets[_sheets.SelectedIndex].Setting.OffsetY * _factor + +GetCenterOnY())));

        /// <summary>
        /// Определение центра для оси X.
        /// </summary>
        /// <returns>Исправленное значение</returns>
        private int GetCenterOnX()
        {
            var width = _pdfSheets[_sheets.SelectedIndex].Setting.Width;
            return (int)((_content.ActualWidth + width * _factor) / 2 - width * _factor);
        }

        /// <summary>
        /// Определение центра для оси Y.
        /// </summary>
        /// <returns>Исправленное значение</returns>
        private int GetCenterOnY()
        {
            var height = _pdfSheets[_sheets.SelectedIndex].Setting.Height;
            return (int)((_content.ActualHeight + height * _factor) / 2 - height * _factor);
        }
#nullable enable

        /// <summary>
        /// Расчет коэффициента увеличения.
        /// </summary>
        /// <param name="height">Высота четежа</param>
        /// <param name="width">Ширина чертежа</param>
        /// <returns>Коэффициент увеличения</returns>
        private double ComputeMagnificationFactor(double height, double width)
        {
            var magnificationFactorX = _content.ActualHeight / height;
            var magnificationFactorY = _content.ActualWidth  / width;

            return Math.Min(magnificationFactorX, magnificationFactorY) * 0.98;
        }

        /// <summary>
        /// Добавить страницу в список страниц.
        /// </summary>
        private void AddSheetsOnList()
        {
            if (_pdfSheets is null)
                return;

            _sheets.Items.Clear();
            _sheets.SelectedIndex = 0;

            foreach (var sheet in _pdfSheets)
                _sheets.Items.Add(sheet.Name);
        }
        #endregion
    }
}
