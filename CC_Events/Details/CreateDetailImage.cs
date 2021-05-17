﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.IO;
using CC_Plugin.Parameters;
using CC_Library;
using CC_Library.Parameters;
using Autodesk.Revit.Attributes;

/// <summary>
/// The details classes will have to do all of the following
/// 1) A class that stores XML Files containing information about all the objects in the detail (They should not include lines. PERIOD
/// Everything in this will be information about the family objects and their parameters including
/// XYZ location
/// Rotation / Orientation
/// Name
/// Type / Parameter information
/// The detail itself will need the following information as well.
/// JPG File Name for referencing (so it can be viewed in the "Open" menu.
/// Tag style detail information for searching such as "Door", "HM", Etc.
/// The detail itself will need a GUID for referencing as well.
/// This GUID will be added to the view as a parameter, so that updating is smooth and easy.
/// 
/// Once detail information is created it cannot be updated. Only deleted.
/// Only modifications to the XML file through revit will include adding tags, so you get more information to search and use.
/// 
/// If I have time eventually and figure it out, it would be nice to save these details to the google cloud using the google api. that way they  can be referenced from anywhere.
/// </summary>

namespace CC_Plugin.Details
{
    public static class DetailImage
    {
        public static void CreateDetailImage(this Document doc, string vid)
        {
            View v = doc.ActiveView;
            if (v.ViewType == ViewType.DraftingView || v.ViewType == ViewType.Detail)
            {
                string dir = "Details".GetMyDocs();
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                ImageExportOptions options = new ImageExportOptions();

                options.FilePath = dir + "\\" + vid;
                options.ExportRange = ExportRange.CurrentView;
                options.ZoomType = ZoomFitType.Zoom;
                options.Zoom = 100;
                options.ImageResolution = ImageResolution.DPI_72;

                doc.ExportImage(options);
            }
        }
    }
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class DetailImageButton : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            if (!doc.IsFamilyDocument)
            {
                using (TransactionGroup tg = new TransactionGroup(doc, "Export Detail"))
                {
                    tg.Start();
                    using (Transaction t = new Transaction(doc, "Add Param"))
                    {
                        t.Start();
                        doc.AddParam(Params.ViewID);
                        t.Commit();
                    }
                    string vid = null;

                    using (Transaction t = new Transaction(doc, "Set Param"))
                    {
                        t.Start();
                        View v = doc.ActiveView;
                        if (v.GetElementParam(Params.ViewID) != null)
                            vid = v.GetElementParam(Params.ViewID); 
                        else
                        {
                            vid = Guid.NewGuid().ToString();
                            v.SetElementParam(Params.ViewID, vid);
                        }
                        t.Commit();
                    }
                    using (Transaction trans = new Transaction(doc, "Export Image"))
                    {
                        trans.Start();
                        doc.CreateDetailImage(vid);
                        trans.Commit();
                    }
                    tg.Commit();
                }
            }
            return Result.Succeeded;
        }
    }
}
