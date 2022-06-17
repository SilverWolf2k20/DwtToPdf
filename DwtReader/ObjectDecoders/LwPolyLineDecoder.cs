//-----------------------------------------------------------------------------
// <copyright file="LwPolyLineDecoder.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс декодировщика оптимизированной мультилинии.
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
    /// Декодировщик оптимизированной мультилинии.
    /// </summary>
    public class LwPolyLineDecoder : IEntityDecoder
    {
        public Entity Decode(DwtStream stream, int bitSize, Entity entity)
        {
            Logger.Info($"------------ LwPolyLine ------------");

            EntityDecoder entityDecoder = new EntityDecoder();
            var polyLine = (LwPolyLine)entityDecoder.Decode(stream, bitSize, new LwPolyLine());

            polyLine.Type = ObjectType.LwPolyLine;
            polyLine.Points = new List<(double x, double y)>();
            polyLine.Bulges = new List<double>();
            polyLine.VertexId = new List<uint>();
            polyLine.Widths = new List<(double start, double end)>();

            // Common:
            ushort flag = stream.GetBitShort();

            // R2010+:
            if ((flag & 0x04) > 0)
                polyLine.Width = stream.GetBitDouble();

            if ((flag & 0x08) > 0)
                polyLine.Elevation = stream.GetBitDouble();

            if ((flag & 0x02) > 0)
                polyLine.Thickness = stream.GetBitDouble();

            if ((flag & 0x01) > 0)
                polyLine.Extrusion = stream.GetBitExtrusion(true);

            polyLine.NumberVertex = stream.GetBitLong();

            if ((flag & 0x10) > 0) // 16
                polyLine.NumberBulges = stream.GetBitLong();

            // R2000+:
            if ((flag & 0x400) > 0) // 1024
                polyLine.VertexIdCount = stream.GetBitLong();

            if ((flag & 0x20) > 0) // 32
                polyLine.NumberWidths = stream.GetBitLong();

            if (polyLine.NumberVertex > 0)
                polyLine.Points.Add((stream.GetRawDouble(), stream.GetRawDouble()));

            for (int i = 1; i < polyLine.NumberVertex; ++i)
                polyLine.Points.Add((stream.GetDefaultDouble(polyLine.Points[i - 1].x),
                                     stream.GetDefaultDouble(polyLine.Points[i - 1].y)));

            for (int i = 0; i < polyLine.NumberBulges; ++i)
                polyLine.Bulges.Add(stream.GetBitDouble());

            for (int i = 0; i < polyLine.VertexIdCount; ++i)
                polyLine.VertexId.Add(stream.GetBitLong());

            for (int i = 0; i < polyLine.NumberWidths; ++i)
                polyLine.Widths.Add((stream.GetBitDouble(), stream.GetBitDouble()));

            // Common:
            polyLine = (LwPolyLine)entityDecoder.DecodeOwner(stream, polyLine);

            Logger.Debug($"Width:          {polyLine.Width}");
            Logger.Debug($"Elevation:      {polyLine.Elevation}");
            Logger.Debug($"Thickness:      {polyLine.Thickness}");
            Logger.Debug($"Extrusion:      {polyLine.Extrusion}");
            Logger.Debug($"NumberVertex:   {polyLine.NumberVertex}");
            Logger.Debug($"NumberBulges:   {polyLine.NumberBulges}");
            Logger.Debug($"VertexIdCount:  {polyLine.VertexIdCount}");
            Logger.Debug($"NumberWidths:   {polyLine.NumberWidths}");

            for (int i = 0; i < polyLine.NumberVertex; ++i)
                Logger.Trace($"Point {i}:        x = {polyLine.Points[i].x:f2} y = {polyLine.Points[i].y:f2}");
            for (int i = 0; i < polyLine.NumberBulges; ++i)
                Logger.Debug($"Bulge {i}:        {polyLine.Bulges[i]:f2}");
            for (int i = 0; i < polyLine.VertexIdCount; ++i)
                Logger.Debug($"VertexId {i}:     {polyLine.VertexId[i]:f2}");
            for (int i = 0; i < polyLine.NumberWidths; ++i)
                Logger.Debug($"Width {i}:        start = {polyLine.Widths[i].start:f2} end = {polyLine.Widths[i].end:f2}");

            Logger.Trace($"CRC:            {stream.GetRawShort()}");

            return polyLine;
        }
    }
}
