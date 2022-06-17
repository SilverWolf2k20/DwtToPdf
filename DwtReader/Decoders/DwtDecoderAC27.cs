//-----------------------------------------------------------------------------
// <copyright file="DwtDecoderAC27.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс для декодирования DWT файла 2013 года.
// </summary>
//-----------------------------------------------------------------------------

using DwtReader.Components.Sections;
using DwtReader.Core;

using Usl;

namespace DwtReader.Decoders
{
    /// <summary>
    /// Декодировщик для версии формата DWT файла 2013 года.
    /// </summary>
    public partial class DwtDecoderAC27 : DwtDecoderAC18
    {
        #region Constructor
        public DwtDecoderAC27() : base()
        {
        }
        #endregion

        #region Sections
        protected override bool DecodeSectionHeader(DwtStream stream)
        {
            Logger.Info("=========== Decode Section Header ===========");
            DataSection dataSection;

            try {
                dataSection = _dwtFile.DataSections[TypeSection.Header];
            }
            catch (Exception) {
                Logger.Error("Отсутствует секция заголовка.");
                return false;
            }

            var buffer = new DwtStream(ParseDataPage(stream, dataSection));
            var bufferHandle = (DwtStream)buffer.Clone();

            for (var i = 0; i < 16; ++i)
                Console.Write("0x{0:X2} ", buffer.GetRawChar());
            Console.WriteLine();

            // Расшифровка данных заголовка
            buffer.SetPosition(16);
            uint size = buffer.GetRawLong();
            uint endBitPosition = 160;
            Logger.Debug($"Размер данных: {size}");

            if (_dwtFile.MaintenanceVersion > 3) {
                var tmpSize = buffer.GetRawLong();
                endBitPosition += 32;
                Logger.Debug($"2010+ высота 32b:  {tmpSize}");
            }

            // Создание потока дескрипторов
            var bitSize = buffer.GetRawLong();
            Logger.Debug($"Размер битов:      {bitSize}");
            endBitPosition += bitSize;
            bufferHandle.SetPosition(endBitPosition >> 3);
            bufferHandle.SetBitPosition((byte)(endBitPosition & 7));

            // R2013+:
            Logger.Debug($"REQUIREDVERSIONS:  {buffer.GetBitLongLong()}");
            // Common:
            Logger.Debug("Unknown: {0:0.0#}", buffer.GetBitDouble()); // Default 412148564080.0.
            Logger.Debug("Unknown: {0:0.0#}", buffer.GetBitDouble()); // Default 1.0.
            Logger.Debug("Unknown: {0:0.0#}", buffer.GetBitDouble()); // Default 1.0.
            Logger.Debug("Unknown: {0:0.0#}", buffer.GetBitDouble()); // Default 1.0.
            Logger.Debug($"Unknown long: {buffer.GetBitLong()}"); // Default 24L.
            Logger.Debug($"Unknown long: {buffer.GetBitLong()}"); // Default 0L.
            // Common:
            Logger.Debug($"DIMASO:         {buffer.GetBit()}");
            Logger.Debug($"DIMSHO:         {buffer.GetBit()}");
            // Common:
            Logger.Debug($"PLINEGEN:       {buffer.GetBit()}");
            Logger.Debug($"ORTHOMODE:      {buffer.GetBit()}");
            Logger.Debug($"REGENMODE:      {buffer.GetBit()}");
            Logger.Debug($"FILLMODE:       {buffer.GetBit()}");
            Logger.Debug($"QTEXTMODE:      {buffer.GetBit()}");
            Logger.Debug($"PSLTSCALE:      {buffer.GetBit()}");
            Logger.Debug($"LIMCHECK:       {buffer.GetBit()}");
            // R2004+:
            Logger.Debug($"Undocumented:   {buffer.GetBit()}"); // User timer on/off.
            // Common:
            Logger.Debug($"USRTIMER        {buffer.GetBit()}");
            Logger.Debug($"SKPOLY          {buffer.GetBit()}");
            Logger.Debug($"ANGDIR          {buffer.GetBit()}");
            Logger.Debug($"SPLFRAME        {buffer.GetBit()}");
            // Common:
            Logger.Debug($"MIRRTEXT        {buffer.GetBit()}");
            Logger.Debug($"WORLDVIEW       {buffer.GetBit()}");
            // Common:
            Logger.Debug($"TILEMODE        {buffer.GetBit()}");
            Logger.Debug($"PLIMCHECK       {buffer.GetBit()}");
            Logger.Debug($"VISRETAIN       {buffer.GetBit()}");
            // Common:
            Logger.Debug($"DISPSILH        {buffer.GetBit()}");
            Logger.Debug($"PELLIPSE        {buffer.GetBit()}"); // Not present in DXF.
            Logger.Debug($"PROXIGRAPHICS   {buffer.GetBitShort()}");
            // Common:
            Logger.Debug($"TREEDEPTH       {buffer.GetBitShort()}");
            Logger.Debug($"LUNITS          {buffer.GetBitShort()}");
            Logger.Debug($"LUPREC          {buffer.GetBitShort()}");
            Logger.Debug($"AUNITS          {buffer.GetBitShort()}");
            Logger.Debug($"AUPREC          {buffer.GetBitShort()}");
            // Common:
            Logger.Debug($"ATTMODE         {buffer.GetBitShort()}");
            // Common:
            Logger.Debug($"PDMODE          {buffer.GetBitShort()}");
            // R2004+:
            Logger.Debug($"Unknown: {buffer.GetBitLong()}");
            Logger.Debug($"Unknown: {buffer.GetBitLong()}");
            Logger.Debug($"Unknown: {buffer.GetBitLong()}");
            // Common:
            Logger.Debug($"USERI1          {buffer.GetBitShort()}");
            Logger.Debug($"USERI2          {buffer.GetBitShort()}");
            Logger.Debug($"USERI3          {buffer.GetBitShort()}");
            Logger.Debug($"USERI4          {buffer.GetBitShort()}");
            Logger.Debug($"USERI5          {buffer.GetBitShort()}");
            Logger.Debug($"SPLINESEGS      {buffer.GetBitShort()}");
            Logger.Debug($"SURFU           {buffer.GetBitShort()}");
            Logger.Debug($"SURFV           {buffer.GetBitShort()}");
            Logger.Debug($"SURFTYPE        {buffer.GetBitShort()}");
            Logger.Debug($"SURFTAB1        {buffer.GetBitShort()}");
            Logger.Debug($"SURFTAB2        {buffer.GetBitShort()}");
            Logger.Debug($"SPLINETYPE      {buffer.GetBitShort()}");
            Logger.Debug($"SHADEDGE        {buffer.GetBitShort()}");
            Logger.Debug($"SHADEDIF        {buffer.GetBitShort()}");
            Logger.Debug($"UNITMODE        {buffer.GetBitShort()}");
            Logger.Debug($"MAXACTVP        {buffer.GetBitShort()}");
            Logger.Debug($"ISOLINES        {buffer.GetBitShort()}");
            Logger.Debug($"CMLJUST         {buffer.GetBitShort()}");
            Logger.Debug($"TEXTQLTY        {buffer.GetBitShort()}");
            Logger.Debug($"LTSCALE         {buffer.GetBitDouble()}");
            Logger.Debug($"TEXTSIZE        {buffer.GetBitDouble()}");
            Logger.Debug($"TRACEWID        {buffer.GetBitDouble()}");
            Logger.Debug($"SKETCHINC       {buffer.GetBitDouble()}");
            Logger.Debug($"FILLETRAD       {buffer.GetBitDouble()}");
            Logger.Debug($"THICKNESS       {buffer.GetBitDouble()}");
            Logger.Debug($"ANGBASE         {buffer.GetBitDouble()}");
            Logger.Debug($"PDSIZE          {buffer.GetBitDouble()}");
            Logger.Debug($"PLINEWID        {buffer.GetBitDouble()}");
            Logger.Debug($"USERR1          {buffer.GetBitDouble()}");
            Logger.Debug($"USERR2          {buffer.GetBitDouble()}");
            Logger.Debug($"USERR3          {buffer.GetBitDouble()}");
            Logger.Debug($"USERR4          {buffer.GetBitDouble()}");
            Logger.Debug($"USERR5          {buffer.GetBitDouble()}");
            Logger.Debug($"CHAMFERA        {buffer.GetBitDouble()}");
            Logger.Debug($"CHAMFERB        {buffer.GetBitDouble()}");
            Logger.Debug($"CHAMFERC        {buffer.GetBitDouble()}");
            Logger.Debug($"CHAMFERD        {buffer.GetBitDouble()}");
            Logger.Debug($"FACETRES        {buffer.GetBitDouble()}");
            Logger.Debug($"CMLSCALE        {buffer.GetBitDouble()}");
            Logger.Debug($"CELTSCALE       {buffer.GetBitDouble()}");
            // Common:
            Logger.Debug($"TDCREATE:       {buffer.GetBitLong()}"); // Julian day.
            Logger.Debug($"TDCREATE:       {buffer.GetBitLong()}"); // Milliseconds into the day.
            Logger.Debug($"TDUPDATE:       {buffer.GetBitLong()}"); // Julian day.
            Logger.Debug($"TDUPDATE:       {buffer.GetBitLong()}"); // Milliseconds into the day.
            // R2004+:
            Logger.Debug($"Unknown: {buffer.GetBitLong()}");
            Logger.Debug($"Unknown: {buffer.GetBitLong()}");
            Logger.Debug($"Unknown: {buffer.GetBitLong()}");
            // Common:
            Logger.Debug($"TDINDWG:        {buffer.GetBitLong()}"); // Days.
            Logger.Debug($"TDINDWG:        {buffer.GetBitLong()}"); // Milliseconds into the day.
            Logger.Debug($"TDUSRTIMER:     {buffer.GetBitLong()}"); // Days.
            Logger.Debug($"TDUSRTIMER:     {buffer.GetBitLong()}"); // Milliseconds into the day.
            Logger.Debug($"CECOLOR:        {buffer.GetCmColor(_dwtFile.Version)}");
            Logger.Debug($"HANDSEED:       {buffer.GetHandle()}");
            Logger.Debug($"CLAYER:         {bufferHandle.GetHandle()}");
            Logger.Debug($"TEXTSTYLE:      {bufferHandle.GetHandle()}");
            Logger.Debug($"CELTYPE:        {bufferHandle.GetHandle()}");
            // R2007+:
            Logger.Debug($"CMATERIAL:      {bufferHandle.GetHandle()}");
            // Common:
            Logger.Debug($"DIMSTYLE:       {bufferHandle.GetHandle()}");
            Logger.Debug($"CMLSTYLE:       {bufferHandle.GetHandle()}");
            // R2000+:
            Logger.Debug($"PSVPSCALE       {buffer.GetBitDouble()}");
            // Common:
            Logger.Debug($"INSBASE         {buffer.Get3BitDouble()}");   // PSPACE
            Logger.Debug($"EXTMIN          {buffer.Get3BitDouble()}");   // PSPACE
            Logger.Debug($"EXTMAX          {buffer.Get3BitDouble()}");   // PSPACE
            Logger.Debug($"LIMMIN          {buffer.Get2RawDouble()}");   // PSPACE
            Logger.Debug($"LIMMAX          {buffer.Get2RawDouble()}");   // PSPACE
            Logger.Debug($"ELEVATION       {buffer.GetBitDouble()}");    // PSPACE
            Logger.Debug($"UCSORG          {buffer.Get3BitDouble()}");   // PSPACE
            Logger.Debug($"UCSXDIR         {buffer.Get3BitDouble()}");   // PSPACE
            Logger.Debug($"UCSYDIR         {buffer.Get3BitDouble()}");   // PSPACE
            Logger.Debug($"UCSNAME:        {bufferHandle.GetHandle()}"); // PSPACE
            // R2000+:
            Logger.Debug($"PUCSORTHOREF:   {bufferHandle.GetHandle()}");
            Logger.Debug($"PUCSORTHOVIEW   {buffer.GetBitShort()}");
            Logger.Debug($"PUCSBASE:       {bufferHandle.GetHandle()}");
            Logger.Debug($"PUCSORGTOP      {buffer.Get3BitDouble()}");
            Logger.Debug($"PUCSORGBOTTOM   {buffer.Get3BitDouble()}");
            Logger.Debug($"PUCSORGLEFT     {buffer.Get3BitDouble()}");
            Logger.Debug($"PUCSORGRIGHT    {buffer.Get3BitDouble()}");
            Logger.Debug($"PUCSORGFRONT    {buffer.Get3BitDouble()}");
            Logger.Debug($"PUCSORGBACK     {buffer.Get3BitDouble()}");
            // Common:
            Logger.Debug($"INSBASE         {buffer.Get3BitDouble()}");   // MSPACE
            Logger.Debug($"EXTMIN          {buffer.Get3BitDouble()}");   // MSPACE
            Logger.Debug($"EXTMAX          {buffer.Get3BitDouble()}");   // MSPACE
            Logger.Debug($"LIMMIN          {buffer.Get2RawDouble()}");   // MSPACE
            Logger.Debug($"LIMMAX          {buffer.Get2RawDouble()}");   // MSPACE
            Logger.Debug($"ELEVATION       {buffer.GetBitDouble()}");    // MSPACE
            Logger.Debug($"UCSORG          {buffer.Get3BitDouble()}");   // MSPACE
            Logger.Debug($"UCSXDIR         {buffer.Get3BitDouble()}");   // MSPACE
            Logger.Debug($"UCSYDIR         {buffer.Get3BitDouble()}");   // MSPACE
            Logger.Debug($"UCSNAME:        {bufferHandle.GetHandle()}"); // MSPACE
            // R2000+:
            Logger.Debug($"UCSORTHOREF:    {bufferHandle.GetHandle()}");
            Logger.Debug($"UCSORTHOVIEW    {buffer.GetBitShort()}");
            Logger.Debug($"UCSBASE:        {bufferHandle.GetHandle()}");
            Logger.Debug($"UCSORGTOP       {buffer.Get3BitDouble()}");
            Logger.Debug($"UCSORGBOTTOM    {buffer.Get3BitDouble()}");
            Logger.Debug($"UCSORGLEFT      {buffer.Get3BitDouble()}");
            Logger.Debug($"UCSORGRIGHT     {buffer.Get3BitDouble()}");
            Logger.Debug($"UCSORGFRONT     {buffer.Get3BitDouble()}");
            Logger.Debug($"UCSORGBACK      {buffer.Get3BitDouble()}");
            // Common:
            Logger.Debug($"DIMSCALE        {buffer.GetBitDouble()}");
            Logger.Debug($"DIMASZ          {buffer.GetBitDouble()}");
            Logger.Debug($"DIMEXO          {buffer.GetBitDouble()}");
            Logger.Debug($"DIMDLI          {buffer.GetBitDouble()}");
            Logger.Debug($"DIMEXE          {buffer.GetBitDouble()}");
            Logger.Debug($"DIMRND          {buffer.GetBitDouble()}");
            Logger.Debug($"DIMDLE          {buffer.GetBitDouble()}");
            Logger.Debug($"DIMTP           {buffer.GetBitDouble()}");
            Logger.Debug($"DIMTM           {buffer.GetBitDouble()}");
            // R2007+
            Logger.Debug($"DIMFXL          {buffer.GetBitDouble()}");
            Logger.Debug($"DIMJOGANG       {buffer.GetBitDouble()}");
            Logger.Debug($"DIMTFILL        {buffer.GetBitShort()}");
            Logger.Debug($"DIMTFILLCLR     {buffer.GetCmColor(_dwtFile.Version)}");
            // R2000+
            Logger.Debug($"DIMTOL          {buffer.GetBit()}");
            Logger.Debug($"DIMLIM          {buffer.GetBit()}");
            Logger.Debug($"DIMTIH          {buffer.GetBit()}");
            Logger.Debug($"DIMTOH          {buffer.GetBit()}");
            Logger.Debug($"DIMSE1          {buffer.GetBit()}");
            Logger.Debug($"DIMSE2          {buffer.GetBit()}");
            Logger.Debug($"DIMTAD          {buffer.GetBitShort()}");
            Logger.Debug($"DIMZIN          {buffer.GetBitShort()}");
            Logger.Debug($"DIMAZIN         {buffer.GetBitShort()}");
            // R2007+
            Logger.Debug($"DIMARCSYM       {buffer.GetBitShort()}");
            // Common:
            Logger.Debug($"DIMTXT          {buffer.GetBitDouble()}");
            Logger.Debug($"DIMCEN          {buffer.GetBitDouble()}");
            Logger.Debug($"DIMTSZ          {buffer.GetBitDouble()}");
            Logger.Debug($"DIMALTF         {buffer.GetBitDouble()}");
            Logger.Debug($"DIMLFAC         {buffer.GetBitDouble()}");
            Logger.Debug($"DIMTVP          {buffer.GetBitDouble()}");
            Logger.Debug($"DIMTFAC         {buffer.GetBitDouble()}");
            Logger.Debug($"DIMGAP          {buffer.GetBitDouble()}");
            // R2000+:
            Logger.Debug($"DIMALTRND       {buffer.GetBitDouble()}");
            Logger.Debug($"DIMALT          {buffer.GetBit()}");
            Logger.Debug($"DIMALTD         {buffer.GetBitShort()}");
            Logger.Debug($"DIMTOFL         {buffer.GetBit()}");
            Logger.Debug($"DIMSAH          {buffer.GetBit()}");
            Logger.Debug($"DIMTIX          {buffer.GetBit()}");
            Logger.Debug($"DIMSOXD         {buffer.GetBit()}");
            // Common:
            Logger.Debug($"DIMCLRD         {buffer.GetCmColor(_dwtFile.Version)}");
            Logger.Debug($"DIMCLRE         {buffer.GetCmColor(_dwtFile.Version)}");
            Logger.Debug($"DIMCLRT         {buffer.GetCmColor(_dwtFile.Version)}");
            // R2000+:
            Logger.Debug($"DIAMDEC         {buffer.GetBitShort()}");
            Logger.Debug($"DIMDEC          {buffer.GetBitShort()}");
            Logger.Debug($"DIMTDEC         {buffer.GetBitShort()}");
            Logger.Debug($"DIMALTU         {buffer.GetBitShort()}");
            Logger.Debug($"DIMALTTD        {buffer.GetBitShort()}");
            Logger.Debug($"DIMAUNIT        {buffer.GetBitShort()}");
            Logger.Debug($"DIMFAC          {buffer.GetBitShort()}");
            Logger.Debug($"DIMLUNIT        {buffer.GetBitShort()}");
            Logger.Debug($"DIMDSEP         {buffer.GetBitShort()}");
            Logger.Debug($"DIMTMOVE        {buffer.GetBitShort()}");
            Logger.Debug($"DIMJUST         {buffer.GetBitShort()}");
            Logger.Debug($"DIMSD1          {buffer.GetBit()}");
            Logger.Debug($"DIMSD2          {buffer.GetBit()}");
            Logger.Debug($"DIMTOLJ         {buffer.GetBitShort()}");
            Logger.Debug($"DIMTZIN         {buffer.GetBitShort()}");
            Logger.Debug($"DIMALTZ         {buffer.GetBitShort()}");
            Logger.Debug($"DIMALTTZ        {buffer.GetBitShort()}");
            Logger.Debug($"DIMUPT          {buffer.GetBit()}");
            Logger.Debug($"DIMATFIT        {buffer.GetBitShort()}");
            // R2007+:
            Logger.Debug($"DIMFXLON        {buffer.GetBit()}");
            // R2010+
            Logger.Debug($"DIMTXTDIRECTION {buffer.GetBit()}");
            Logger.Debug($"DIMALTMZF       {buffer.GetBitDouble()}");
            Logger.Debug($"DIMMZF          {buffer.GetBitDouble()}");
            // R2000+
            Logger.Debug($"DIMTXSTY        {bufferHandle.GetHandle()}");
            Logger.Debug($"DIMLDRBLK       {bufferHandle.GetHandle()}");
            Logger.Debug($"DIMBLK          {bufferHandle.GetHandle()}");
            Logger.Debug($"DIMBLK1         {bufferHandle.GetHandle()}");
            Logger.Debug($"DIMBLK2         {bufferHandle.GetHandle()}");
            // R2010+
            Logger.Debug($"DIMLTYPE        {bufferHandle.GetHandle()}");
            Logger.Debug($"DIMLTEX1        {bufferHandle.GetHandle()}");
            Logger.Debug($"DIMLTEX2        {bufferHandle.GetHandle()}");
            // R2000+
            Logger.Debug($"DIMLWD          {buffer.GetBitShort()}");
            Logger.Debug($"DIMLWE          {buffer.GetBitShort()}");
            // Common
            Logger.Debug($"BLOCK CONTROL OBJECT         {bufferHandle.GetHandle()}");
            Logger.Debug($"LAYER CONTROL OBJECT         {bufferHandle.GetHandle()}");
            Logger.Debug($"STYLE CONTROL OBJECT         {bufferHandle.GetHandle()}");
            Logger.Debug($"LINETYPE CONTROL OBJECT      {bufferHandle.GetHandle()}");
            Logger.Debug($"VIEW CONTROL OBJECT          {bufferHandle.GetHandle()}");
            Logger.Debug($"UCS CONTROL OBJECT           {bufferHandle.GetHandle()}");
            Logger.Debug($"VPORT CONTROL OBJECT         {bufferHandle.GetHandle()}");
            Logger.Debug($"APPID CONTROL OBJECT         {bufferHandle.GetHandle()}");
            Logger.Debug($"DIMSTYLE CONTROL OBJECT      {bufferHandle.GetHandle()}");
            Logger.Debug($"DICTIONARY (ACAD_GROUP)      {bufferHandle.GetHandle()}");
            Logger.Debug($"DICTIONARY (ACAD_MLINESTYLE) {bufferHandle.GetHandle()}");
            Logger.Debug($"DICTIONARY (NAMED OBJECTS)   {bufferHandle.GetHandle()}");
            // R2000+
            Logger.Debug($"TSTACKALIGN                  {buffer.GetBitShort()}"); // Default = 1  (not present in DXF)
            Logger.Debug($"TSTACKSIZE                   {buffer.GetBitShort()}"); // Default = 70 (not present in DXF)
            Logger.Debug($"DICTIONARY (LAYOUTS)         {bufferHandle.GetHandle()}");
            Logger.Debug($"DICTIONARY (PLOTSETTINGS)    {bufferHandle.GetHandle()}");
            Logger.Debug($"DICTIONARY (PLOTSTYLES)      {bufferHandle.GetHandle()}");
            // R2004+
            Logger.Debug($"DICTIONARY (MATERIALS)       {bufferHandle.GetHandle()}");
            Logger.Debug($"DICTIONARY (COLORS)          {bufferHandle.GetHandle()}");
            // R2007+
            Logger.Debug($"DICTIONARY (VISUALSTYLE)     {bufferHandle.GetHandle()}");
            // R2013+
            Logger.Debug($"UNKNOWN                      {bufferHandle.GetHandle()}");
            // R2000+
            Logger.Debug($"Flags               {buffer.GetBitLong()}");
            Logger.Debug($"INSUNITS            {buffer.GetBitShort()}");
            if (buffer.GetBitShort() == 3) // CEPSNTYPE == 3
                Logger.Debug($"CPSNID              {bufferHandle.GetHandle()}");
            // R2004+
            Logger.Debug($"SORTENTS            {buffer.GetRawChar()}");
            Logger.Debug($"INDEXCTL            {buffer.GetRawChar()}");
            Logger.Debug($"HIDETEXT            {buffer.GetRawChar()}");
            Logger.Debug($"XCLIPFRAME          {buffer.GetRawChar()}"); // Before R2010 the value can be 0 or 1 only.
            Logger.Debug($"DIMASSOC            {buffer.GetRawChar()}");
            Logger.Debug($"HALOGAP             {buffer.GetRawChar()}");
            Logger.Debug($"OBSCUREDCOLOR       {buffer.GetBitShort()}");
            Logger.Debug($"INTERSECTIONCOLOR   {buffer.GetBitShort()}");
            Logger.Debug($"OBSCUREDLTYPE       {buffer.GetRawChar()}");
            Logger.Debug($"INTERSECTIONDISPLAY {buffer.GetRawChar()}");
            // Common
            Logger.Debug($"BLOCK_RECORD (*PAPER_SPACE)  {bufferHandle.GetHandle()}");
            Logger.Debug($"BLOCK_RECORD (*MODEL_SPACE)  {bufferHandle.GetHandle()}");
            Logger.Debug($"LTYPE (BYLAYER)              {bufferHandle.GetHandle()}");
            Logger.Debug($"LTYPE (BYBLOCK)              {bufferHandle.GetHandle()}");
            Logger.Debug($"LTYPE (CONTINUOUS)           {bufferHandle.GetHandle()}");
            // R2007+
            Logger.Debug($"CAMERADISPLAY       {buffer.GetBit()}");
            Logger.Debug($"Unknown             {buffer.GetBitLong()}");
            Logger.Debug($"Unknown             {buffer.GetBitLong()}");
            Logger.Debug($"Unknown             {buffer.GetBitDouble()}");
            Logger.Debug($"STEPSPERSEC         {buffer.GetBitDouble()}");
            Logger.Debug($"STEPSIZE            {buffer.GetBitDouble()}");
            Logger.Debug($"3DDWFPREC           {buffer.GetBitDouble()}");
            Logger.Debug($"LENSLENGTH          {buffer.GetBitDouble()}");
            Logger.Debug($"CAMERAHEIGHT        {buffer.GetBitDouble()}");
            Logger.Debug($"SOLIDHIST           {buffer.GetRawChar()}");
            Logger.Debug($"SHOWHIST            {buffer.GetRawChar()}");
            Logger.Debug($"PSOLWIDTH           {buffer.GetBitDouble()}");
            Logger.Debug($"PSOLHEIGHT          {buffer.GetBitDouble()}");
            Logger.Debug($"LOFTANG1:           {buffer.GetBitDouble()}");
            Logger.Debug($"LOFTANG2:           {buffer.GetBitDouble()}");
            Logger.Debug($"LOFTMAG1:           {buffer.GetBitDouble()}");
            Logger.Debug($"LOFTMAG2:           {buffer.GetBitDouble()}");
            Logger.Debug($"LOFTPARAM           {buffer.GetBitShort()}");
            Logger.Debug($"LOFTNORMALS         {buffer.GetRawChar()}");
            Logger.Debug($"LATITUDE            {buffer.GetBitDouble()}");
            Logger.Debug($"LONGITUDE           {buffer.GetBitDouble()}");
            Logger.Debug($"NORTHDIRECTION      {buffer.GetBitDouble()}");
            Logger.Debug($"TIMEZONE            {buffer.GetBitLong()}");
            Logger.Debug($"LIGHTGLYPHDISPLAY   {buffer.GetRawChar()}");
            Logger.Debug($"TILEMODELIGHTSYNCH  {buffer.GetRawChar()}");
            Logger.Debug($"DWFFRAME            {buffer.GetRawChar()}");
            Logger.Debug($"DGNFRAME            {buffer.GetRawChar()}");
            Logger.Debug($"Unknown             {buffer.GetBit()}");
            Logger.Debug($"INTERFERECOLOR      {buffer.GetCmColor(_dwtFile.Version)}");
            Logger.Debug($"INTERFEREOBJVS      {bufferHandle.GetHandle()}");
            Logger.Debug($"INTERFEREVPVS       {bufferHandle.GetHandle()}");
            Logger.Debug($"DRAGVS              {bufferHandle.GetHandle()}");
            Logger.Debug($"CSHADOW             {buffer.GetRawChar()}");
            Logger.Debug($"Unknown             {buffer.GetBitDouble()}");
            // R14+
            Logger.Debug($"Unknown short:      {buffer.GetBitShort()}");
            Logger.Debug($"Unknown short:      {buffer.GetBitShort()}");
            Logger.Debug($"Unknown short:      {buffer.GetBitShort()}");
            Logger.Debug($"Unknown short:      {buffer.GetBitShort()}");
            // Common
            // Пропуск данных...
            buffer.SetPosition(buffer.Size() - 16 - 2);
            Logger.Debug($"Crc: {buffer.GetRawShort()}");

            for (var i = 0; i < 16; ++i)
                Console.Write("0x{0:X2} ", buffer.GetRawChar());
            Console.WriteLine();

            return true;
        }
        #endregion
    }
}
