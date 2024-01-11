// <copyright file="DisposableOpacity.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Mitutoyo.MiCAT.ReportModule.PDFRenderer.Utilities
{
    public class DisposableOpacity : IDisposableOpacity
   {
        private FixedDocumentRenderContext context;
        private double opacity;

        public DisposableOpacity(FixedDocumentRenderContext context)
        {
            this.context = context;
            this.opacity = context.Opacity;
        }

        public void Dispose()
        {
            if (this.context != null)
            {
                this.context.Opacity = this.opacity;
                this.context = null;
            }
        }
    }
}
