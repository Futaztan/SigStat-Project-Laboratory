using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace onlab;

public class ExcelManager
{
     public void PrintToExcelDecideAndFeature(List<DecideResult> results)
        {

            ExcelPackage.License.SetNonCommercialPersonal("onlab");
            using (var excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("AER");
                var uniqueDecides = results.Select(r => r.DecideName).Distinct().OrderBy(n => n).ToList();

                var uniqueFeatureSets = results
                    .Select(r => string.Join(", ", r.FeatureName))
                    .Distinct()
                    .ToList();
                using (var range = workSheet.Cells[1, 1, 1, uniqueDecides.Count + 1])
                {
                    range.Style.Font.Bold = true;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                }
                using (var range = workSheet.Cells[1, 1, uniqueFeatureSets.Count + 1, 1])
                {
                    range.Style.Font.Bold = true;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                }

                workSheet.Cells[1, 1].Value = "Decide\n/\nFeature";
                for (int i = 0; i < uniqueFeatureSets.Count; i++)
                {
                    var cell = workSheet.Cells[i + 2, 1];
                    cell.Value = uniqueFeatureSets[i];
                    cell.Style.WrapText = true;
                }

                for (int i = 0; i < uniqueDecides.Count; i++)
                {
                    workSheet.Cells[1, i + 2].Value = uniqueDecides[i];
                    workSheet.Cells[1, 1].Style.Font.Bold = true;
                }


                foreach (var res in results)
                {
                    int col = uniqueDecides.IndexOf(res.DecideName) + 2;
                    string currentFS = string.Join(", ", res.FeatureName);
                    int row = uniqueFeatureSets.IndexOf(currentFS) + 2;

                    workSheet.Cells[row, col].Value = res.AER;

                    workSheet.Cells[row, col].Style.Numberformat.Format = "0.00%";

                }
                
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                // Add TwoColorScale conditional formatting to visualize temperatures
                // ColorTranslator is a utility from System.Drawing.Primitives.
                var cfRule = workSheet.ConditionalFormatting.AddThreeColorScale(workSheet.Cells[2, 2, 2 + uniqueFeatureSets.Count, 2 + uniqueDecides.Count]);
                cfRule.LowValue.Color = ColorTranslator.FromHtml("#FF63BE7B");
                cfRule.MiddleValue.Color = ColorTranslator.FromHtml("#FFFFEB84");
                //cfRule.MiddleValue.Type = eExcelConditionalFormattingValueObjectType.Percentile;
                // cfRule.MiddleValue.Value = 50;
                cfRule.HighValue.Color = ColorTranslator.FromHtml("#FFF8696B");


                string path = @"C:\Users\David\Downloads\bme_decide_feature.xlsx";
                if (File.Exists(path)) File.Delete(path);
                File.WriteAllBytes(path, excel.GetAsByteArray());
            }

        }
     
     
        public void PrintDecideToExcel(List<DecideResult> results)
        {
            results = results.OrderBy(r => r.DecideName).ToList();
            ExcelPackage.License.SetNonCommercialPersonal("onlab");
            var excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Összesítés");

            workSheet.Row(1).Height = 20;
            workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(1).Style.Font.Bold = true;
            workSheet.Cells[1, 1].Value = "Decide Name";
            workSheet.Cells[1, 2].Value = "AER";
            workSheet.Cells[1, 3].Value = "FAR";
            workSheet.Cells[1, 4].Value = "FRR";
            int row = 2;
            foreach (var result in results)
            {
                for (int i = 1; i <= 4; i++)
                {
                    workSheet.Cells[row, i].Style.Numberformat.Format = "0.00%";
                }
                workSheet.Cells[row, 1].Value = result.DecideName;
                workSheet.Cells[row, 2].Value = result.AER;
                workSheet.Cells[row, 3].Value = result.FAR;
                workSheet.Cells[row, 4].Value = result.FRR;
                row++;

            }

            string path = @"C:\Users\David\Downloads\bme_decide.xlsx";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            FileStream objFileStrm = File.Create(path);
            objFileStrm.Close();


            File.WriteAllBytes(path, excel.GetAsByteArray());

            excel.Dispose();
        }
        
        
        public void PrintTestandTrainToExcel(List<Result> results)
        {
            ExcelPackage.License.SetNonCommercialPersonal("onlab");


            var excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Összesítés");


            workSheet.Row(1).Height = 20;
            workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(1).Style.Font.Bold = true;

            workSheet.Cells[1, 1].Value = "Train Name";
            workSheet.Cells[1, 2].Value = "Test Name";
            workSheet.Cells[1, 3].Value = "AER";
            workSheet.Cells[1, 4].Value = "FAR";
            workSheet.Cells[1, 5].Value = "FRR";


            results = results.OrderBy(o => o.TrainName).ThenBy(o => o.TestName).ToList();
            int row = 2;

            foreach (var result in results)
            {
                for (int i = 1; i <= 5; i++)
                {
                    workSheet.Cells[row, i].Style.Numberformat.Format = "0.00%";
                }
                workSheet.Cells[row, 1].Value = result.TrainName;
                workSheet.Cells[row, 2].Value = result.TestName;
                workSheet.Cells[row, 3].Value = result.AER;
                workSheet.Cells[row, 4].Value = result.FAR;
                workSheet.Cells[row, 5].Value = result.FRR;
                row++;

            }

            workSheet = excel.Workbook.Worksheets.Add("AER");

            workSheet.Row(1).Height = 20;
            workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(1).Style.Font.Bold = true;
            string prev = "";
            row = 1;
            int col = 2;
            foreach (var result in results)
            {
                if (!result.TrainName.Equals(prev))
                {
                    row++;
                    col = 2;
                    workSheet.Cells[row, 1].Value = result.TrainName;
                    prev = result.TrainName;
                }

                workSheet.Cells[1, col].Value = result.TestName;
                workSheet.Cells[row, col].Value = result.AER;
                workSheet.Cells[row, col].Style.Numberformat.Format = "0.00%";
                col++;
            }

            workSheet = excel.Workbook.Worksheets.Add("FAR");
            workSheet.Row(1).Height = 20;
            workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(1).Style.Font.Bold = true;
            prev = "";
            row = 1;
            col = 2;
            foreach (var result in results)
            {
                if (!result.TrainName.Equals(prev))
                {
                    row++;
                    col = 2;
                    workSheet.Cells[row, 1].Value = result.TrainName;
                    prev = result.TrainName;
                }

                workSheet.Cells[1, col].Value = result.TestName;
                workSheet.Cells[row, col].Value = result.FAR;
                workSheet.Cells[row, col].Style.Numberformat.Format = "0.00%";
                col++;
            }


            workSheet = excel.Workbook.Worksheets.Add("FRR");

            workSheet.Row(1).Height = 20;
            workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(1).Style.Font.Bold = true;
            prev = "";
            row = 1;
            col = 2;
            foreach (var result in results)
            {
                if (!result.TrainName.Equals(prev))
                {
                    row++;
                    col = 2;
                    workSheet.Cells[row, 1].Value = result.TrainName;
                    prev = result.TrainName;
                }

                workSheet.Cells[1, col].Value = result.TestName;
                workSheet.Cells[row, col].Value = result.FRR;
                workSheet.Cells[row, col].Style.Numberformat.Format = "0.00%";
                col++;
            }




            string path = @"C:\Users\David\Downloads\bme.xlsx";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            FileStream objFileStrm = File.Create(path);
            objFileStrm.Close();


            File.WriteAllBytes(path, excel.GetAsByteArray());

            excel.Dispose();
        }
}