using System;
using System.Collections.Generic;
using System.Windows;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Architecture;
using System.Text;
using System.IO;

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

                // File to hold text content
                StringBuilder sb = new StringBuilder();
                FilterWalls(document, sb);

                using (StreamWriter outfile = new StreamWriter(@"C:\Revit\wall.txt", false))
                {
                    try
                    {
                        outfile.Write(sb.ToString());
                        outfile.Close();
                    }
                    catch (Exception ex)
                    {
                        TaskDialog.Show("Revit","Não conseguiu criar o arquivo texto. Motivo: " + ex.Message);
                        return Result.Failed;
                    }

                }
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

        public static void FilterWalls(Document document, StringBuilder sb)
        {
            sb.AppendLine("== Tratando Paredes ==");
            StringBuilder temp = new StringBuilder();
            // Find all Wall instances in the document by using category filter
            ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Walls);
            // Apply the filter to the elements in the active document
            // Use shortcut WhereElementIsNotElementType() to find wall instances only
            FilteredElementCollector collector = new FilteredElementCollector(document);
            IList<Element> walls =
            collector.WherePasses(filter).WhereElementIsNotElementType().ToElements();
            sb.AppendLine(string.Format("Paredes encontradas: {0}", walls.Count));
            String prompt = string.Format("Total Walls: {0}. The walls in the current document are:\n", walls.Count);
            Wall wall;
            double total = 0d;
            Parameter parameter;
            int areaPerHour = 3;
            double totalTime = 0;
            int index = 0;
            double areaSquareMeters;

            double timePerWall;

            int bricksPerRootSquare = 30;
            int bricksPerWall = 0;
            int totalBricks = 0;

            decimal cost100Bricks = 100;
            decimal costBricksPerWall = 0;
            decimal totalBricksCost = 0;

            foreach (Element e in walls)
            {
                wall = e as Wall;
                
                // Convert 
                areaSquareMeters = UnitUtils.ConvertFromInternalUnits(wall.GetOrderedParameters()[20].AsDouble(), DisplayUnitType.DUT_SQUARE_METERS);
                

                prompt += string.Format("Parede:{0}, Área: {1}. Tempo Necessário: {2} horas.\n", ++index, wall.GetOrderedParameters()[20].AsValueString(), totalTime);

                // Timer
                timePerWall = areaSquareMeters / areaPerHour;

                // Material
                // Total bricks equal to total area per number of bricker per wall square meters
                bricksPerWall = (int) Math.Ceiling(areaSquareMeters * bricksPerRootSquare);

                // Values
                costBricksPerWall = bricksPerWall * cost100Bricks / 100;

                // Update Totals
                totalTime += timePerWall;
                totalBricks += bricksPerWall;
                totalBricksCost += costBricksPerWall;

                // Log
                temp.AppendLine(string.Format("Parede {0}: {1}", index, wall.Name));
                temp.AppendLine(string.Format("Tempo de Construção: {0} horas", timePerWall));
                temp.AppendLine(string.Format("Área: {0}", wall.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED).AsValueString()));
                temp.AppendLine(string.Format("Volume: {0}", wall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsValueString()));
                temp.AppendLine(string.Format("Comprimento: {0} m", wall.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsValueString()));
                temp.AppendLine(string.Format("Altura: {0} m", wall.get_Parameter(BuiltInParameter.WALL_USER_HEIGHT_PARAM).AsValueString()));
                temp.AppendLine(string.Format("Tijolos Necessários: {0} unidades", bricksPerWall));
                temp.AppendLine(string.Format("Custo do Tijolo: {0:C2}", costBricksPerWall));
                // Calculates Materials

                temp.AppendLine();
            }
            
            prompt = "Tempo Total de Construção das paredes: " + totalTime+"\n"+prompt;
            TaskDialog.Show("Revit", prompt);
            sb.AppendLine(string.Format("Tempo Total de Construção das Paredes: {0} horas", totalTime));
            sb.AppendLine(string.Format("Total de tijolos necessários: {0} unidades", totalBricks));
            sb.AppendLine(string.Format("Custo Total de tijolos: {0:C2}", totalBricksCost));
            sb.AppendLine("--").AppendLine().Append(temp.ToString());
            sb.AppendLine("== Fim Tratando Paredes ==");
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
