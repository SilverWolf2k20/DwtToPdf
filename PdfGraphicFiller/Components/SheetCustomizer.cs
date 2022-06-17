//-----------------------------------------------------------------------------
// <copyright file="SheetCustomizer.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс для определения настрок для листа.
// </summary>
//-----------------------------------------------------------------------------

using DwtDrawer.Data;

using DwtReader.Data;
using DwtReader.Objects;
using DwtReader.Objects.Componets;

namespace DwtDrawer.Components
{
    /// <summary>
    /// Класс для определения настрок для листов.
    /// </summary>
    public class SheetCustomizer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public List<PdfSheet>? GetSheets(DwtFile? file)
        {
            if (file is null)
                return null;

            var sheets = new List<PdfSheet>();
            var objects = new List<(ObjectType type, Entity entity)>();

            sheets.Add(new PdfSheet() { Name = "Без блока", ID = 0 });
            foreach (var entity in file.Objects) {
                // Получить список блоков.
                if (entity.type == ObjectType.BlockHeader) {
                    var block = entity.entity as BlockHeader;

                    if(block is null)
                        continue;

                    // Добавление листа.
                    sheets.Add(new PdfSheet() { 
                        Name = block.Name, 
                        ID = block.Handle.reference 
                    });
                }

                // Загрузка объектов для определения сдвига.
                if(entity.type == ObjectType.LwPolyLine) {
                    var lwPolyLine = entity.entity as LwPolyLine;

                    if (lwPolyLine is not null && lwPolyLine.Invisible != 1)
                        objects.Add((lwPolyLine.Type, lwPolyLine));
                }
                if (entity.type == ObjectType.Line) {
                    var line = entity.entity as Line;

                    if (line is not null && line.Invisible != 1)
                        objects.Add((line.Type, line));
                }
            }

            // Определить сдвиг и удалить пустые блоки.
            var adjuster = new PositoinAdjuster();
            for(int i = 0; i < sheets.Count; ++i) {
                adjuster.Calibrate(objects, sheets[i].ID);

                sheets[i].Setting = new PdfSetting(adjuster.Setting);

                if (adjuster.NotFound) {
                    sheets.Remove(sheets[i]);
                    --i;
                }
            }
            return sheets;
        }
    }
}
