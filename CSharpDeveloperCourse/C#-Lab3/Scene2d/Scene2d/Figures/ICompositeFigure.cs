﻿namespace Scene2d.Figures
{
    using System.Collections.Generic;

    public interface ICompositeFigure : IFigure
    {
        IList<IFigure> ChildFigures { get; }
    }
}