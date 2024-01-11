// <copyright file="AppStateInitialize.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Mitutoyo.MiCAT.ReportModule.Setup
{
   using ApplicationState;

   public static class AppStateInitialize
   {
      public static ISnapShot Initialize(ISnapShot snapShot)
      {
         return snapShot;
      }
   }
}
