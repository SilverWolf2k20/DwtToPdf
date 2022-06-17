//-----------------------------------------------------------------------------
// <copyright file="LineDecoder.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс декодировщика линии.
// </summary>
//-----------------------------------------------------------------------------

using DwtReader.Core;
using DwtReader.ObjectDecoders.Core;
using DwtReader.Objects;
using DwtReader.Objects.Componets;

using Usl;

namespace DwtReader.ObjectDecoders
{
    /// <summary>
    /// Декодировщик линии.
    /// </summary>
    internal class LineDecoder : IEntityDecoder
    {
        public Entity Decode(DwtStream stream, int bitSize, Entity entity)
        {
            Logger.Info($"--------------- Line ---------------");

            EntityDecoder entityDecoder = new EntityDecoder();
            var line = (Line)entityDecoder.Decode(stream, bitSize, new Line());

            line.Type = ObjectType.Line;

            // R2000+:
            line.ZIsNull = Convert.ToBoolean(stream.GetBit());
            line.StartX = stream.GetRawDouble();
            line.EndX = stream.GetDefaultDouble(line.StartX);
            line.StartY = stream.GetRawDouble();
            line.EndY = stream.GetDefaultDouble(line.StartY);

            if (line.ZIsNull == false) {
                line.StartZ = stream.GetRawDouble();
                line.EndZ = stream.GetDefaultDouble(line.StartZ);
            }

            // Common:
            line.Thickness = stream.GetBitThickness(true);
            line.Extrusion = stream.GetBitExtrusion(true);
            line = (Line)entityDecoder.DecodeOwner(stream, line);

            Logger.Debug($"Z in null?:     {line.ZIsNull}");

            Logger.Trace($"X1:             {line.StartX:f2}");
            Logger.Trace($"Y1:             {line.StartY:f2}");
            Logger.Trace($"Z1:             {line.StartZ:f2}");

            Logger.Trace($"X2:             {line.EndX:f2}");
            Logger.Trace($"Y2:             {line.EndY:f2}");
            Logger.Trace($"Z2:             {line.EndZ:f2}");

            Logger.Debug($"Thickness:      {line.Thickness}");
            Logger.Debug($"Extrusion:      {line.Extrusion}");
            Logger.Trace($"CRC:            {stream.GetRawShort()}");

            return line;
        }
    }
}
