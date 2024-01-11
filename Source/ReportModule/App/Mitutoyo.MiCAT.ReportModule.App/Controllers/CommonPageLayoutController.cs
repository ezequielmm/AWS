// <copyright file="CommonPageLayoutController.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;

namespace Mitutoyo.MiCAT.ReportModuleApp.Controllers
{
   public class CommonPageLayoutController : ICommonPageLayoutController
   {
      private readonly IPageSizeList _pageSizeList;
      private readonly IAppStateHistory _history;

      public CommonPageLayoutController(IAppStateHistory history, IPageSizeList pageSizeList)
      {
         _pageSizeList = pageSizeList;
         _history = history;
      }

      public CommonPageLayout GetCurrentCommonPageLayout()
      {
         return _history.CurrentSnapShot.GetItems<CommonPageLayout>().SingleOrDefault();
      }

      public IPageSizeList GetSupportedPageSizeList()
      {
         return _pageSizeList;
      }

      public void SetPageSize(PageSizeInfo pageSizeInfo)
      {
         var commonPageLayout = _history.CurrentSnapShot.GetItems<CommonPageLayout>().SingleOrDefault();
         if (!commonPageLayout.PageSize.Equals(pageSizeInfo))
            _history.RunUndoable((snapShot) => SetPageSize(snapShot, pageSizeInfo, commonPageLayout));
      }
      private ISnapShot SetPageSize(ISnapShot snapShot, PageSizeInfo pageSizeInfo, CommonPageLayout currentCommonPageLayout)
      {
         return snapShot.UpdateItem(currentCommonPageLayout, currentCommonPageLayout.With(pageSizeInfo));
      }

      public void SetMarginSize(Margin margin)
      {
         var commonPageLayout = _history.CurrentSnapShot.GetItems<CommonPageLayout>().SingleOrDefault();
         if (!commonPageLayout.CanvasMargin.Equals(margin))
            _history.RunUndoable(snapShot => SetMarginSize(snapShot, margin, commonPageLayout));
      }
      private ISnapShot SetMarginSize(ISnapShot snapShot, Margin margin, CommonPageLayout currentCommonPageLayout)
      {
         return snapShot.UpdateItem(currentCommonPageLayout, currentCommonPageLayout.With(margin));
      }
   }
}
