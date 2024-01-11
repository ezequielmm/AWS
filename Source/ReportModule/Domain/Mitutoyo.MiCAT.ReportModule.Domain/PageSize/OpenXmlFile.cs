// <copyright file="OpenXmlFile.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Utilities
{
   public class OpenXmlFile
   {
      public XDocument Load(string xmlFilePath)
      {
         Assembly myAssembly = Assembly.GetExecutingAssembly();
         using (StreamReader sr = new StreamReader(myAssembly.GetManifestResourceStream(xmlFilePath), Encoding.UTF8))
         {
            return XDocument.Load(sr);
         }
      }
   }
}
