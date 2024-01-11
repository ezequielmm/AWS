// <copyright file="PaletteContextMenu.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.LayoutCalculator;
using Mitutoyo.MiCAT.ReportModule.GUI.Properties;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;

namespace Mitutoyo.MiCAT.ReportModule.GUI.ContextMenu
{
   public class PaletteContextMenu : ContextMenuBase
   {
      private readonly ITextBoxController _textBoxController;
      private readonly IImageController _imageController;
      private readonly ITessellationViewController _tessellationViewController;
      private readonly IHeaderFormController _headerFormController;
      private readonly ITableViewController _tableViewController;
      private readonly IRenderedData _renderedData;

      public PaletteContextMenu(
         ITextBoxController textBoxController,
         IImageController imageController,
         ITessellationViewController tessellationViewController,
         IHeaderFormController headerFormController,
         ITableViewController tableViewController,
         IRenderedData renderedData)
      {
         _textBoxController = textBoxController;
         _imageController = imageController;
         _tessellationViewController = tessellationViewController;
         _headerFormController = headerFormController;
         _tableViewController = tableViewController;
         _renderedData = renderedData;

         _items = new ObservableCollection<ContextMenuItem>()
         {
            new ContextMenuItem()
            {
               Command = CreateCommand(()=>AddTableView()),
               Text = Resources.CharacteristicTableString,
               IconName = ContextMenuItemIcon.CharacteristicTable
            },
            new ContextMenuItem()
            {
               Command = CreateCommand(()=>AddHeaderForm()),
               Text = Resources.HeaderFormString,
               IconName = ContextMenuItemIcon.HeaderForm
            },
            new ContextMenuItem()
            {
               Command = CreateCommand(()=>AddImage()),
               Text = Resources.ImageString,
               IconName = ContextMenuItemIcon.Image
            },
            new ContextMenuItem()
            {
               Command = CreateCommand(()=>AddTextBox()),
               Text = Resources.TextboxString,
               IconName = ContextMenuItemIcon.Textbox
            },
         };
      }

      private ICommand CreateCommand(Action actionCommand)
      {
         return new RelayCommand<object>((obj) => { actionCommand(); });
      }

      private void AddTextBox()
      {
         if (OverFakeSpace())
            _textBoxController.AddTextboxOnFakeSpace(X, GetDomainY(), GetFakeSpaceStartPosition(), GetFakeSpaceHeight());
         else
            _textBoxController.AddTextboxToBody(X, GetDomainY());
      }

      private void AddImage()
      {
         if (OverFakeSpace())
            _imageController.AddImageOnFakeSpace(X, GetDomainY(), GetFakeSpaceStartPosition(), GetFakeSpaceHeight());
         else
            _imageController.AddImageToBody(X, GetDomainY());
      }

      private void AddHeaderForm()
      {
         if (OverFakeSpace())
            _headerFormController.AddHeaderFormOnFakeSpace(X, GetDomainY(), GetFakeSpaceStartPosition(), GetFakeSpaceHeight());
         else
            _headerFormController.AddHeaderFormToBody(X, GetDomainY());
      }

      private void AddTableView()
      {
         if (OverFakeSpace())
            _tableViewController.AddTableViewOnFakeSpace(X, GetDomainY(), GetFakeSpaceStartPosition(), GetFakeSpaceHeight());
         else
            _tableViewController.AddTableView(X, GetDomainY());
      }

      private void AddTessellationView()
      {
         if (OverFakeSpace())
            _tessellationViewController.AddComponentOnFakeSpace(GetDomainY(), GetFakeSpaceStartPosition(), GetFakeSpaceHeight());
         else
            _tessellationViewController.AddComponent(GetDomainY());
      }

      private bool OverFakeSpace()
      {
         return _renderedData.IsFakeSpace(VisualY);
      }

      private int GetFakeSpaceStartPosition()
      {
         return _renderedData.Pages.GetFakeSpaceStartingPosition(VisualY);
      }

      private int GetFakeSpaceHeight()
      {
         return _renderedData.Pages.GetFakeSpaceHeight(VisualY);
      }

      private int GetDomainY()
      {
         return _renderedData.ConvertToDomainY(VisualY);
      }
   }
}
