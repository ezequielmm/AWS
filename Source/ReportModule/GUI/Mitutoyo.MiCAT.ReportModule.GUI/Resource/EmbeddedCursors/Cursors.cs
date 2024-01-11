// <copyright file="Cursors.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows.Input;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Resource.EmbeddedCursors
{
   public static class Cursors
   {
      private const string CursorsFolderPath = "Resource.EmbeddedCursors";

      private const string MoveCursorFile = "MoveCursor.cur";

      private const string BlackResizeNWSEFile = "BlackResizeNWSE.cur";

      private const string BlackResizeNESWFile = "BlackResizeNESW.cur";

      private const string BlackResizeWEFile = "BlackResizeWE.cur";

      private const string BlackResizeNSFile = "BlackResizeNS.cur";

      public static Cursor MoveCursor { get; } = GetCursor(MoveCursorFile);
      public static Cursor BlackResizeNS { get; } = GetCursor(BlackResizeNSFile);
      public static Cursor BlackResizeNWSE { get; } = GetCursor(BlackResizeNWSEFile);
      public static Cursor BlackResizeNESW { get; } = GetCursor(BlackResizeNESWFile);
      public static Cursor BlackResizeWE { get; } = GetCursor(BlackResizeWEFile);
      public static Cursor HandCursor { get; } = System.Windows.Input.Cursors.Hand;

      private static Cursor GetCursor(string filePath)
      {
         var absolutePath = GetCursorAbsolutePath(filePath);
         using (var stream = ResourceHelper.GetEmbeddedResourceStream(absolutePath))
         {
            return new Cursor(stream);
         }
      }

      private static string GetCursorAbsolutePath(string filePath) => $"{CursorsFolderPath}.{filePath}";
   }
}
