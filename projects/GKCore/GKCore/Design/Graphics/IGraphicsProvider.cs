﻿/*
 *  "GEDKeeper", the personal genealogical database editor.
 *  Copyright (C) 2009-2023 by Sergey V. Zhdanovskih.
 *
 *  This file is part of "GEDKeeper".
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.IO;
using BSLib;

namespace GKCore.Design.Graphics
{
    /// <summary>
    /// Interface for platform-independent graphics rendering providers.
    /// </summary>
    public interface IGraphicsProvider
    {
        /// <summary>
        /// Check photo orientation.
        /// </summary>
        /// <param name="inputStream"></param>
        /// <returns>Returns true if normal, false if transformations are needed.</returns>
        Stream CheckOrientation(Stream inputStream);

        IColor CreateColor(int argb);
        IColor CreateColor(int r, int g, int b);
        IColor CreateColor(int a, int r, int g, int b);

        IPen CreatePen(IColor color, float width);
        IBrush CreateSolidBrush(IColor color);

        IFont CreateFont(string fontName, float size, bool bold);

        IGfxPath CreatePath();

        IImage LoadImage(Stream stream, int thumbWidth, int thumbHeight, ExtRect cutoutArea);

        /// <summary>
        /// Loading an image from a file (already cached by other functions or service images/icons of application).
        /// That is, loading without any additional processing procedures.
        /// </summary>
        IImage LoadImage(string fileName);

        IImage LoadResourceImage(string resName);
        IImage LoadResourceImage(Type baseType, string resName);
        void SaveImage(IImage image, string fileName);

        ExtSizeF GetTextSize(string text, IFont font, object target);

        string GetDefaultFontName();

        IColor CreateColor(string signature);

        IGfxPath CreateCirclePath(float x, float y, float width, float height);
        IGfxPath CreateCircleSegmentPath(float inRad, float extRad, float wedgeAngle, float ang1, float ang2);
        IGfxPath CreateCircleSegmentPath(int ctX, int ctY, float inRad, float extRad, float wedgeAngle,
                                     float ang1, float ang2);

        IImage LoadResourceImage(string resName, bool makeTransp);
    }
}
