//-----------------------------------------------------------------------------
// <copyright file="TypeSection.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит перечисление всех типов разделов.
// </summary>
//-----------------------------------------------------------------------------

namespace DwtReader.Components.Sections
{
    /// <summary>Типы разделов.</summary>
    public enum TypeSection : byte
    {
        /// <summary>
        /// Неизвестный раздел.
        /// </summary>
        Unknowns,

        /// <summary>
        /// Содержит переменные заголовка чертежа.
        /// </summary>
        Header,

        /// <summary>
        /// Вспомогательная информация.
        /// </summary>
        AuxHeader,

        /// <summary>
        /// Раздел пользовательских классов.
        /// </summary>
        Classes,

        /// <summary>
        /// Список дескрипторов со смещениями в разделе AcDb:AcDbObjects.
        /// </summary>
        Handles,

        /// <summary>
        /// Шаблон (содержит только системную переменную MEASUREMENT).
        /// </summary>
        Template,

        /// <summary>
        /// Свободной пространство объектов.
        /// </summary>
        ObjFreeSpace,

        /// <summary>
        /// Объекты базы данных.
        /// </summary>
        Objects,

        /// <summary>
        /// Лист регистраций изменений.
        /// </summary>
        RevHistory,

        /// <summary>
        /// Содержит такие поля, как Название, Тема, Автор.
        /// </summary>
        SumaryInfo,

        /// <summary>
        /// Предварительный просмотр растрового изображения для этого рисунка.
        /// </summary>
        Preview,

        /// <summary>
        /// Содержит информацию о приложении, которое записало файл .dwg 
        /// (зашифровано = 2).
        /// </summary>
        AppInfo,

        /// <summary>
        /// 
        /// </summary>
        AppInfoHistory,

        /// <summary>
        /// Содержит зависимости файлов (например, файлы IMAGE или шрифты, 
        /// используемые STYLE).
        /// </summary>
        FileDepList,

        /// <summary>
        /// 
        /// </summary>
        SectionMap,

        /// <summary>
        /// 
        /// </summary>
        Extedata,

        /// <summary>
        /// Данные заголовка файла.
        /// </summary>
        FileHeader,

        /// <summary>
        /// Прототип.
        /// </summary>
        Prototype,

        /// <summary>
        /// Прокси графика?
        /// </summary>
        ProxyGraphics,

        /// <summary>
        /// Содержит информацию о пароле и шифровании данных.
        /// </summary>
        Security,

        /// <summary>
        /// Не написано ODA.
        /// </summary>
        Signature,

        /// <summary>
        /// Содержит данные проекта VBA для этого чертежа.
        /// </summary>
        VbaProject,
    }
}
