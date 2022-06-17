//-----------------------------------------------------------------------------
// <copyright file="ObjectType.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит перечисление типов сущностей.
// </summary>
//-----------------------------------------------------------------------------

namespace DwtReader.Objects.Componets
{
    /// <summary>
    /// Типы сущностей.
    /// </summary>
    public enum ObjectType : short
    {
        Unused                  = 0x00,  // Неизвестный.
        Text                    = 0x01,  // Однострочный текст.
        Attribute               = 0x02,  // Атрибуты.
        AttributeDefine         = 0x03,  // Определение атрибута для данных в блоке.
        Block                   = 0x04,  // Группа объектов.
        EndBlock                = 0x05,  // Конец группы объектов.
        Seqend                  = 0x06,  // Последовательность.
        Insert                  = 0x07,  // Палитра блоков.
        Minsert                 = 0x08,  // Массив блоков.
        Vertex2D                = 0x0A,  // 2D вершина.
        Vertex3D                = 0x0B,  // 3D вершина.
        VertexMesh              = 0x0C,  // Вершина сетки.
        VertexPFace             = 0x0D,  // Вершина грани.
        VertexPFeceFace         = 0x0E,  // Вершина трехмерной многогранной сетки.
        PolyLine2D              = 0x0F,  // 2D мультилиния.
        PolyLine3D              = 0x10,  // 3D мультилиния.
        Arc                     = 0x11,  // Дуга.
        Circle                  = 0x12,  // Круг.
        Line                    = 0x13,  // Линия.
        DimensionOrdinate       = 0x14,  // Размер ординаты.
        DimensionLinear         = 0x15,  // Линейный размер.
        DimensionAligned        = 0x16,  // Выравненный размер.
        Dimension3Pt            = 0x17,  // Угловой размер.
        Dimension2Line          = 0x18,  // Линейный размер.
        DimensionRadius         = 0x19,  // Размер радиуса.
        DimensionDiameter       = 0x1A,  // Размер диаметра.
        Point                   = 0x1B,  // Точка.
        Face3D                  = 0x1C,  // 3х-4х сторонняя поверхность.
        PolyLinePface           = 0x1D,  // 3D сетка.
        PolyLineMesh            = 0x1E,  // Сетка.
        Solid                   = 0x1F,  // Твердотельный примитив.
        Trace                   = 0x20,  // Трассировка.
        Shape                   = 0x21,  // Форма (фигура).
        Viewport                = 0x22,  // Видовой экран.
        Ellipse                 = 0x23,  // Элипс.
        SpLine                  = 0x24,  // Плавная линия.
        Region                  = 0x25,  // Объект 2D плоскости.
        Solid3D                 = 0x26,  // 3D примитив (твердотельный).
        Body                    = 0x27,  // Объект тела.
        Ray                     = 0x28,  // Луч.
        Xline                   = 0x29,  // Направляющая.
        Dictionary              = 0x2A,  // Словарь.
        OleFrame                = 0x2B,  // Отображение рамок для объектов.
        Mtext                   = 0x2C,  // Многострочный текст.
        Leader                  = 0x2D,  // Выноска.
        Tolerance               = 0x2E,  // Неточность для размеров.
        Mline                   = 0x2F,  // Мультилиния.

        BlockControl            = 0x30,  // Блоки.
        BlockHeader             = 0x31,  // 
        LayerControl            = 0x32,  // Слои.
        Layer                   = 0x33,  // 
        ShapeFileControl        = 0x34,  // Файл формы.
        ShapeFile               = 0x35,  // 
        LineTypeControl         = 0x38,  // Типы линий.
        LineType                = 0x39,  // 
        ViewControl             = 0x3C,  // Вид.
        View                    = 0x3D,  // 
        UcsControl              = 0x3E,  // Система координат.
        Ucs                     = 0x3F,  // 
        ViewPortControl         = 0x40,  // Видовой экран.
        ViewPort                = 0x41,  // 
        AppIdControl            = 0x42,  // Данные о приложении.
        AppId                   = 0x43,  // 
        DimensionStyleControl   = 0x44,  // Стиль размеров.
        DimensionStyle          = 0x45,  // 
        ViewPortEntityControl   = 0x46,  // Видовой экран.
        ViewPortEntity          = 0x47,  // 

        Group                   = 0x48,  // Набор объектов.
        MlineStyle              = 0x49,  // Стиль мультилиний.
        Ole2Frame               = 0x4A,  // 3D рамка.
        Dummy                   = 0x4B,  // Дурачок??
        LongTransaction         = 0x4C,  // Сслыка на блок.
        LwPolyLine              = 0x4D,  // Оптимизированная PolyLine.
        Hatch                   = 0x4E,  // Штриховка.
        XRecord                 = 0x4F,  // Хранилище объектов.
        AdDbPlaceholder         = 0x50,  // Заполнитель.
        VbaProject              = 0x51,  // VBA проект.
        Layout                  = 0x52,  // Макет.
        AcadProxyEntity         = 0x1F2, // Сторонний объект (сущность)
        AcadProxyObject         = 0x1F3  // Сторонний объект (объект)
    }
}
