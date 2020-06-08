﻿using System;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CC_Library.Parameters;
using LoadFamilies;

using CC_RevitBasics;

namespace CC_Plugin
{
    internal class DocSynching
    {

        public static void synch(object sender, DocumentSynchronizingWithCentralEventArgs args)
        {
            Document doc = args.Document;
            CCMethod.Run("DocSynching", "CC_DocSynching.DocSynching", "synch", doc);
            /*
            using (TransactionGroup tg = new TransactionGroup(doc, "Doc Synching"))
            {
                TaskDialog.Show("Test", "Doc Synching is Running");
                tg.Start();
                foreach (CCParameter p in Enum.GetValues(typeof(CCParameter)))
                {
                    using (Transaction t = new Transaction(doc, "ADD Parameters"))
                    {
                        t.Start();
                        doc.AddParam(p);
                        t.Commit();
                    }
                }
                using (Transaction t = new Transaction(doc, "Set ID"))
                {
                    t.Start();
                    doc.SetID(doc.CheckID());
                    t.Commit();
                }
                using (Transaction t = new Transaction(doc, "Add Families"))
                {
                    t.Start();
                    doc.LoadSymbols();
                    t.Commit();
                }
                tg.Commit();
            } */
        }
        private static string directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private static string dir = directory + "\\CC_PrjData";

        public static Result OnStartup(UIControlledApplication app)
        {
            app.ControlledApplication.DocumentSynchronizingWithCentral += new EventHandler<DocumentSynchronizingWithCentralEventArgs>(synch);
            return Result.Succeeded;
        }
        public static Result OnShutdown(UIControlledApplication app)
        {
            app.ControlledApplication.DocumentSynchronizingWithCentral -= new EventHandler<DocumentSynchronizingWithCentralEventArgs>(synch);
            return Result.Succeeded;
        }
    }
}