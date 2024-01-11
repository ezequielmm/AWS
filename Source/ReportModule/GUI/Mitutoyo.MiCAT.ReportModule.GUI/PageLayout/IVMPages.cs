// <copyright file="IVMPages.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.ObjectModel;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.DisabledSpace;

namespace Mitutoyo.MiCAT.ReportModule.GUI.PageLayout
{
   public interface IVMPages
   {
      ObservableCollection<VMPage> Pages { get; }
      void ClearPages();
      VMPage NextPage(VMPage actualPage, CommonPageLayout actualPageSettings);
      VMPage NextPage(CommonPageLayout actualPageSettings);
      VMPage GetPageForPosition(int visualY);
      int GetFakeSpaceStartingPosition(int visualY);
      int GetFakeSpaceHeight(int visualY);
      int GetDomainOutsidePages(int visualY);
      void ResetDomainInfoAffectedByDisabledSpace(DisabledSpaceData disabledSpace);
   }
}
