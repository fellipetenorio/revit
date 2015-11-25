using System;
using System.Collections.Generic;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Architecture;

namespace ManagementRevitPlugin
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class Plugin : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                // Select some elements in Revit before invoking this command
                //TaskDialog dialog = new TaskDialog("Info");
                //dialog.W
                // Get the handle of current document.
                UIDocument uidoc = commandData.Application.ActiveUIDocument;

                // Get the element selection of current document.
                Selection selection = uidoc.Selection;
                ICollection<ElementId> selectedIds = uidoc.Selection.GetElementIds();
                Document document = uidoc.Document;

                FilterWalls(document);
                
            }
            catch (Exception e)
            {
                message = e.Message;
                return Result.Failed;
            }

            return Result.Succeeded;
        }

        public static void FilterWalls2(Document document)
        {
            RoomFilter filter;

        }

        public static void FilterWalls(Document document)
        {
            // Find all Wall instances in the document by using category filter
            ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Walls);
            // Apply the filter to the elements in the active document
            // Use shortcut WhereElementIsNotElementType() to find wall instances only
            FilteredElementCollector collector = new FilteredElementCollector(document);
            IList<Element> walls =
            collector.WherePasses(filter).WhereElementIsNotElementType().ToElements();
            String prompt = string.Format("Total Walls: {0}. The walls in the current document are:\n", walls.Count);
            Wall wall;
            Double total = 0d;
            Parameter parameter;
            foreach (Element e in walls)
            {
                wall = e as Wall;
                parameter = wall.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED);
                prompt += string.Format("{0} ({1})\n", parameter.AsDouble(), parameter.DisplayUnitType);
                total += parameter.AsDouble();
                //prompt += wall.GetMaterialArea(e.Id, true) + "\n";
            }
            prompt = "Total: " + total+"\n"+prompt;
            TaskDialog.Show("Revit", prompt);
        }

        public static void FilterDoors(Document document)
        {
            // Create a Filter to get all the doors in the document

            ElementClassFilter familyInstanceFilter = new ElementClassFilter(typeof(FamilyInstance));
            ElementCategoryFilter doorsCategoryfilter = new ElementCategoryFilter(BuiltInCategory.OST_Walls);
            LogicalAndFilter doorInstancesFilter = new LogicalAndFilter(familyInstanceFilter, doorsCategoryfilter);
            FilteredElementCollector collector = new FilteredElementCollector(document);
            ICollection<ElementId> doors = collector.WherePasses(doorInstancesFilter).ToElementIds();

            String prompt = "The ids of the doors in the current document are: ";
            String temp = "";
            Element element;
            foreach (ElementId id in doors)
            {
                // Get the category instance from the Category property
                temp = "";
                element = document.GetElement(id);
                Category category = element.Category;
                BuiltInCategory enumCategory = (BuiltInCategory)category.Id.IntegerValue;
                //temp += Enum.GetName(typeof(BuiltInCategory), enumCategory);
                // Get the level object to which the element is assigned.
                if (element.LevelId.Equals(ElementId.InvalidElementId))
                {
                    TaskDialog.Show("Revit", "The element isn't based on a level.");
                }
                else
                {
                    Level level = element.Document.GetElement(element.LevelId) as Level;

                    // Format the prompt information(Name and elevation)
                    temp = "The element is based on a level.";
                    temp += "\nThe level name is:  " + level.Name;
                    temp += "\nThe level elevation is:  " + level.Elevation;

                    // Show the information to the user.
                    //TaskDialog.Show("Revit", prompt);
                }
                prompt += "\n\t" + id.IntegerValue + "(" + temp + ")";
            }

            // Give the user some information
            TaskDialog.Show("Revit", prompt);


            // Get settings of current document
            Settings documentSettings = document.Settings;

            // Get all categories of current document
            Categories groups = documentSettings.Categories;

            // Show the number of all the categories to the user
            prompt = "Number of all categories in current Revit document:" + groups.Size;

            // get Floor category according to OST_Floors and show its name
            Category floorCategory = groups.get_Item(BuiltInCategory.OST_Walls);
            prompt += floorCategory.Name;

            // Give the user some information
            TaskDialog.Show("Revit", prompt);

            //    foreach (var item in uidoc.Selection.GetElementIds())
            //    {
            //        if (Units.)
            //}

            //if (0 == selectedIds.Count)
            //{
            //    // If no elements selected.
            //    TaskDialog.Show("Revit", "You haven't selected any elements.");
            //}
            //else
            //{
            //    String info = "Ids of selected elements in the document are: ";
            //    foreach (ElementId id in selectedIds)
            //    {
            //        info += "\n\t" + id.IntegerValue;
            //    }

            //    TaskDialog.Show("Revit", info);
            //}
        }
    }
}
