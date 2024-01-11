// <copyright file="FeatureValues.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Mitutoyo.MiCAT.ReportModule.Domain.CADElements.Callouts2D.FeatureCallouts
{
   public class FeatureValues
   {
      public double Nominal { get; }
      public double Actual { get; }
      public double Dev { get; }
      public double UpperTolerance { get; }
      public double LowerTolerance { get; }
      public double OutTolerance { get; }

      public FeatureValues(double nominal, double actual, double dev, double upTol, double loTol, double outTol)
      {
         Nominal = nominal;
         Actual = actual;
         Dev = dev;
         UpperTolerance = upTol;
         LowerTolerance = loTol;
         OutTolerance = outTol;
      }
   }
}
