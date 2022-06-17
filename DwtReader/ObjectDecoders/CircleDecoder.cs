//-----------------------------------------------------------------------------
// <copyright file="CircleDecoder.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс декодировщика окружности.
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
    /// Декодировщик окружности.
    /// </summary>
    internal class CircleDecoder : IEntityDecoder
    {
        public Entity Decode(DwtStream stream, int bitSize, Entity entity)
        {
            Logger.Info($"-------------- Circle --------------");

            EntityDecoder entityDecoder = new EntityDecoder();
            var circle = (Circle)entityDecoder.Decode(stream, bitSize, new Circle());

            circle.Type = ObjectType.Circle;

            // Common:
            circle.CenterX = stream.GetBitDouble();
            circle.CenterY = stream.GetBitDouble();
            circle.CenterZ = stream.GetBitDouble();
            circle.Radius  = stream.GetBitDouble();

            circle.Thickness = stream.GetBitThickness(true);
            circle.Extrusion = stream.GetBitExtrusion(true);

            Logger.Trace($"X:              {circle.CenterX:f2}");
            Logger.Trace($"Y:              {circle.CenterY:f2}");
            Logger.Trace($"Z:              {circle.CenterZ:f2}");
            Logger.Trace($"Radius:         {circle.Radius:f2}");

            Logger.Debug($"Thickness:      {circle.Thickness}");
            Logger.Debug($"Extrusion:      {circle.Extrusion}");
            Logger.Trace($"CRC:            {stream.GetRawShort()}");

            return circle;
        }
    }
}
