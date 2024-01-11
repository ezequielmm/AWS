// <copyright file="FixedDocumentRenderContext.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Telerik.Windows.Documents.Fixed.Model.Editing;

namespace Mitutoyo.MiCAT.ReportModule.PDFRenderer.Utilities
{
    public class FixedDocumentRenderContext
    {
        public FixedContentEditor DrawingSurface;
        public Renderers.IRenderer Facade;
        public double Opacity;

        public FixedDocumentRenderContext(FixedContentEditor drawingSurface, Renderers.IRenderer facade)
        {
            this.DrawingSurface = drawingSurface;
            this.Facade = facade;
            this.Opacity = 1;
        }
    }
}
