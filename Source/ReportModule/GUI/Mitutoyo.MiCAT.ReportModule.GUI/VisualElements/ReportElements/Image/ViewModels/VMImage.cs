// <copyright file="VMImage.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Input;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.GUI.Dialog;
using Mitutoyo.MiCAT.ReportModule.GUI.Properties;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Helpers;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using EmbeddedCursors = Mitutoyo.MiCAT.ReportModule.GUI.Resource.EmbeddedCursors.Cursors;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Image.ViewModels
{
   public class VMImage : VMReportComponent
   {
      private byte[] _image;
      private string _stretch;
      private bool _hasCustomImage;
      private Cursor _defaultCursor;
      private string _toolTip;
      private IActionCaller _actionCaller;

      public ICommand LoadImageCommand { get; private set; }
      public readonly IImageController _controller;
      public IDialogWithPreviewService DialogWithPreviewService;

      public VMImage(
         IAppStateHistory appStateHistory,
         Id<ReportImage> imageId,
         IVMReportComponentPlacement vmPlacement,
         IDeleteComponentController deleteComponentController,
         IImageController controller,
         IActionCaller actionCaller)
         : base(
            appStateHistory,
            imageId,
            vmPlacement,
            deleteComponentController)
      {
         _controller = controller;
         _actionCaller = actionCaller;

         VMPlacement.MinWidth = 20;
         VMPlacement.MinHeight = 20;

         LoadImageCommand = new RelayCommand<object>(OnMouseLeftClick);
         DialogWithPreviewService = new DialogWithPreviewService(new FileDialogManager());
      }

      new private Id<ReportImage> Id => base.Id as Id<ReportImage>;

      public byte[] Image
      {
         get => _image;
         set
         {
            if (_image == value) return;
            _image = value;
            RaisePropertyChanged();
         }
      }

      public string Stretch
      {
         get { return _stretch; }
         set
         {
            _stretch = value;
            RaisePropertyChanged();
         }
      }

      public bool HasCustomImage
      {
         get { return _hasCustomImage; }
         set
         {
            _hasCustomImage = value;
            RaisePropertyChanged();
         }
      }

      public Cursor DefaultCursor
      {
         get { return _defaultCursor; }
         set
         {
            _defaultCursor = value;
            RaisePropertyChanged();
         }
      }

      public string ToolTip
      {
         get { return _toolTip; }
         set
         {
            _toolTip = value;
            RaisePropertyChanged();
         }
      }

      protected override void UpdateFromBusinessEntity(IReportComponent reportComponentBefore, IReportComponent reportComponentAfter)
      {
         base.UpdateFromBusinessEntity(reportComponentBefore, reportComponentAfter);
         UpdateReportImageComponent(((ReportImage)reportComponentAfter).Image);
      }

      private void UpdateReportImageComponent(string content)
      {
         var image = string.IsNullOrEmpty(content) ? ImageHelper.GetBase64StringFromImage(GetDefaultAddImage())
            : content;
         Image = Convert.FromBase64String(image);
         HasCustomImage = !string.IsNullOrEmpty(content);
         Stretch = HasCustomImage ? "Fill" : "None";
         DefaultCursor = HasCustomImage ?
            EmbeddedCursors.MoveCursor :
            EmbeddedCursors.HandCursor;
         ToolTip = HasCustomImage ? String.Empty : Resources.ImageToolTipString;
      }

      private Bitmap GetDefaultAddImage()
      {
         Assembly guiAssembly = Assembly.GetExecutingAssembly();
         Stream myStream = guiAssembly.GetManifestResourceStream("Mitutoyo.MiCAT.ReportModule.GUI.Resource.DefaultAddImage.png");
         return new Bitmap(myStream);
      }

      private void OnMouseLeftClick(object obj)
      {
         if (!_hasCustomImage)
         {
            var selectedImage = DialogWithPreviewService.TryGetUserSelectedFile(Resources.DialogImagesTitle,
               Resources.DialogImagesExtensions +
               "(*.jpg,*.png,*.ico,*.tif)|*.jpg;*.png;*.ico;*.tif");
            if (string.IsNullOrEmpty(selectedImage)) return;

            _actionCaller.RunUIThreadAction(() =>
            {
               var imageContent = ImageHelper.GetBase64StringFromFile(selectedImage);

               _controller.UpdateImage(Id, imageContent.ImageString, Math.Max(imageContent.WidthData, VMPlacement.MinWidth), Math.Max(imageContent.HeightData, VMPlacement.MinHeight));
            });
         }
      }
   }
}
