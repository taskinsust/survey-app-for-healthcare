using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SurveyApp.Web.Areas.Identity.Data;
using SurveyApp.Web.Data;
using SurveyApp.Web.Models;
using SurveyApp.Web.Models.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SurveyApp.Web.Services
{
    public class SurveyService
    {
        static IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
        private static string CONNSTR = conf["ConnectionStrings:DefaultConnection"].ToString();

        private readonly ApplicationDbContext _context;
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter da;
        private DataTable ResultsData;
        private int rowsPerSheet = 100;

        public SurveyService(ApplicationDbContext context)
        {
            _context = context;
            ResultsData = new DataTable();
            ResultsData.Clear();
        }

        public async Task<Survey[]> GetAllSurveysAsync(ApplicationUser user)
        {
            return await _context.Surveys
                .Where(s => s.UserId == user.Id)
                .Include(s => s.Questions)
                .Include(s => s.FilledSurveys)
                .ToArrayAsync();
        }

        public async Task<Survey> GetSurveyOfUserByIdAsync(int id, ApplicationUser user)
        {
            return await _context.Surveys
                .Where(s => s.Id == id && s.UserId == user.Id)
                .Include(s => s.Questions)
                    .ThenInclude(q => q.Options)
                .Include(s => s.FilledSurveys)
                    .ThenInclude(f => f.FilledSurveyOptions)
                        .ThenInclude(o => o.Option)
                            .ThenInclude(o => o.Question)
                .FirstOrDefaultAsync();
        }

        public async Task<Survey> GetSurveyByIdAsync(int id)
        {
            var survey = await _context.Surveys
                        .Where(s => s.Id == id)
                        .Include(s => s.Questions)
                        .ThenInclude(q => q.Options)

                        .FirstOrDefaultAsync();

            // Sort the Options within each Question by Rank
            foreach (var question in survey.Questions)
            {
                question.Options = question.Options.OrderBy(o => o.Rank).ToList();
            }

            return survey;
        }

        public async Task<bool> CreateSurveyAsync(SurveyViewModel model, ApplicationUser user)
        {
            var survey = new Survey()
            {
                Title = model.Title,
                Questions = model.Questions,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                UserId = user.Id,
                Centre = _context.Center.Where(x => x.Id == model.CentreId).SingleOrDefault()
            };

            _context.Surveys.Add(survey);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult > 1;
        }

        public async Task<bool> CreateFilledSurveyAsync(FilledSurveyViewModel model)
        {
            var survey = await GetSurveyByIdAsync(model.SurveyId);
            var filledSurvey = new FilledSurvey
            {
                CreatedAt = DateTime.Now,
                Survey = survey,
                PatientId = model.PatientId,
                //Email = model.Email,
                SurveyId = model.SurveyId,
                Center = this.GetCentreById(model.CentreId),
                //FilledSurveyOptions = model.FilledSurveyOptions
            };

            foreach (var item in model.FilledSurveyOptions)
            {
                //Check if Option is null
                if (item.OptionId == 0 || item.OptionId == null)
                {
                    Question question = _context.Questions.Where(x => x.Id == item.QuestionNo).SingleOrDefault();

                    //Save this Option
                    Option option = new Option()
                    {
                        Question = question,
                        QuestionId = question.Id,
                        Title = item.Value != "" && item.Value != null ? item.Value : "NA",
                        IsTextValue = true
                    };

                    _context.Options.Add(option);
                    await _context.SaveChangesAsync();

                    item.OptionId = option.Id;
                }
            }

            filledSurvey.FilledSurveyOptions = model.FilledSurveyOptions;
            _context.FilledSurveys.Add(filledSurvey);
            var saveResult = _context.SaveChanges();
            return saveResult > 1;
        }

        private Center GetCentreById(int centreId)
        {
            return _context.Center.Where(x => x.Id == centreId).FirstOrDefault();
        }

        //public async Task<bool> isEmailAnsweredSurvey(string email, int surveyId)
        //{
        //    var result = await _context.FilledSurveys
        //        .FirstOrDefaultAsync(f => f.Email == email && f.SurveyId == surveyId);

        //    return !(result == null);
        //}

        public async Task<bool> DeleteSurveyAsync(Survey survey, ApplicationUser user)
        {
            _context.Surveys.Remove(survey);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult > 1;
        }

        #region Load Center

        public async Task<List<Center>> LoadCentersAsync()
        {
            return _context.Center.ToList();
        }

        internal List<Center> LoadCenters()
        {
            return _context.Center.ToList();
        }

        internal async Task<List<Survey>> GetAllSurveysAsync(ApplicationUser currentUser, int start, int length)
        {
            return await _context.Surveys.Where(x => x.UserId == currentUser.Id)
                .Include(s => s.Questions)
                .Include(s => s.FilledSurveys)
                .Skip(start).Take(length).ToListAsync();

        }

        internal async Task<List<Survey>> GetAllSurveysAsync(ApplicationUser currentUser, int page)
        {
            return await _context.Surveys//.Where(x => x.UserId == currentUser.Id)
                .Include(s => s.Questions)
                .Include(s => s.FilledSurveys)
                .Skip((page - 1) * 10).Take(10).ToListAsync();

        }

        internal async Task<int> CountSurveyAsync(ApplicationUser currentUser)
        {
            return await _context.Surveys.Where(x => x.UserId == currentUser.Id).CountAsync();
        }

        internal async Task<List<QuestionType>> LoadQuestionTypeAsync()
        {
            return await _context.QuestionTypes.ToListAsync();
        }

        internal List<Question> LoadQuestionByQuesTypeandSurveyId(int id, string[] selectedValues)
        {
            List<Question> resultset = new List<Question>();
            var questypeids = string.Join(",", selectedValues);

            var result_one = from s in _context.Surveys
                             join q in _context.Questions on s.Id equals q.SurveyId
                             join qt in _context.QuestionTypes on q.QuestionType.Id equals qt.Id
                             where s.Id == id &&
                             questypeids.Contains(Convert.ToString(qt.Id))
                             select q;

            resultset.AddRange(result_one.ToList<Question>());
            var result_two = from s in _context.Surveys
                             join q in _context.Questions on s.Id equals q.SurveyId
                             join qt in _context.QuestionTypes on q.QuestionType.Id equals qt.Id into qTypeGroup
                             from qt in qTypeGroup.DefaultIfEmpty()
                             where s.Id == id && qt == null
                             select q;
            resultset.AddRange(result_two.ToList<Question>());

            return resultset;


        }
        #endregion

        internal async Task<FileStreamResult> DownloadReport(int id)
        {
            con = new SqlConnection(CONNSTR);
            string sql = @"SELECT s.Title, C.Name 'Centre', fs.CreatedAt Surveydate, fs.PatientId, Q.ShortTitle 'Question', o.Title 'Answer'
               FROM dbo.FilledSurveys fs
               INNER JOIN dbo.FilledSurveyOption fso ON fs.Id = fso.FilledSurveyId
               INNER JOIN dbo.Options o ON fso.OptionId = o.Id
               INNER JOIN Questions q on q.Id = o.QuestionId
               INNER JOIN Center c ON c.Id = fS.CenterId
               INNER JOIN Surveys s on s.Id = fs.SurveyId
               where s.Id = " + id;

            var command = new SqlCommand(sql, con);
            con.Open();
            command.CommandTimeout = 250; //seconds
            SqlDataReader reader = command.ExecuteReader();
            int counter = 0;
            bool firstTime = true;

            DataTable dtSchema = reader.GetSchemaTable();
            var listCols = new List<DataColumn>();
            if (dtSchema != null)
            {
                foreach (DataRow drow in dtSchema.Rows)
                {
                    string columnName = Convert.ToString(drow["ColumnName"]);
                    var column = new DataColumn(columnName, (Type)(drow["DataType"]));
                    column.Unique = (bool)drow["IsUnique"];
                    column.AllowDBNull = (bool)drow["AllowDBNull"];
                    column.AutoIncrement = (bool)drow["IsAutoIncrement"];
                    listCols.Add(column);
                    ResultsData.Columns.Add(column);
                }
            }

            while (reader.Read())
            {
                DataRow dataRow = ResultsData.NewRow();
                for (int i = 0; i < listCols.Count; i++)
                {
                    dataRow[(listCols[i])] = reader[i];
                }
                ResultsData.Rows.Add(dataRow);
                counter++;
            }

            if (counter == rowsPerSheet)
            {
                counter = 0;
                return await ExportToOxml(firstTime, "survey_report", ResultsData);
                ResultsData.Clear();
                firstTime = false;
            }

            reader.Close();

            return await ExportToOxml(firstTime, "survey_report", ResultsData);

        }

        public async Task<FileStreamResult> ExportToOxml(bool firstTime, string fileName, DataTable resultSet = null)
        {
            ResultsData = resultSet;
            //fileName = string.Format(fileName + "-{0:d}.xlsx", DateTime.Now.Date).Replace("/", "-");
            fileName = fileName + ".xlsx";
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "ExcelReport\\" + fileName;
            //Delete the file if it exists. 
            if (firstTime && File.Exists(filePath))
                File.Delete(fileName);

            uint sheetId = 1; //Start at the first sheet in the Excel workbook.

            if (firstTime)
            {
                //This is the first time of creating the excel file and the first sheet.
                // Create a spreadsheet document by supplying the filepath.
                // By default, AutoSave = true, Editable = true, and Type = xlsx.
                SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook);

                // Add a WorkbookPart to the document.
                WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();

                // Add a WorksheetPart to the WorkbookPart.
                var worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                //sheetData.Append(CreateHeader(1));

                worksheetPart.Worksheet = new Worksheet(sheetData);

                // Add Sheets to the Workbook.
                Sheets sheets;
                sheets = spreadsheetDocument.WorkbookPart.Workbook.
                    AppendChild<Sheets>(new Sheets());

                // Append a new worksheet and associate it with the workbook.
                var sheet = new Sheet()
                {
                    Id = spreadsheetDocument.WorkbookPart.
                        GetIdOfPart(worksheetPart),
                    SheetId = sheetId,
                    Name = "Sheet" + sheetId
                };
                sheets.Append(sheet);

                //Add Header Row.
                var headerRow = new Row();

                foreach (DataColumn column in ResultsData.Columns)
                {
                    var cell = new Cell
                    {
                        DataType = CellValues.String,
                        CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName)
                        //StyleIndex = 5
                    };
                    headerRow.AppendChild(cell);
                }
                sheetData.AppendChild(headerRow);

                try
                {
                    foreach (DataRow row in ResultsData.Rows)
                    {
                        var newRow = new Row();

                        foreach (DataColumn col in ResultsData.Columns)
                        {
                            try
                            {
                                var cell = new Cell
                                {
                                    DataType = CellValues.String,
                                    CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(Regex.Replace(row[col].ToString(), @"[\u0000-\u0008\u000A-\u001F\u0100-\uFFFF]", ""))
                                };
                                newRow.AppendChild(cell);
                            }
                            catch (Exception e)
                            {
                                throw e;
                            }
                        }
                        sheetData.AppendChild(newRow);
                    }
                    workbookpart.Workbook.Save();
                    spreadsheetDocument.Dispose();
                }
                catch (Exception e)
                {

                }
            }
            else
            {
                // Open the Excel file that we created before, and start to add sheets to it.
                var spreadsheetDocument = SpreadsheetDocument.Open(fileName, true);

                var workbookpart = spreadsheetDocument.WorkbookPart;
                if (workbookpart.Workbook == null)
                    workbookpart.Workbook = new Workbook();

                var worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);
                var sheets = spreadsheetDocument.WorkbookPart.Workbook.Sheets;

                if (sheets.Elements<Sheet>().Any())
                {
                    //Set the new sheet id
                    sheetId = sheets.Elements<Sheet>().Max(s => s.SheetId.Value) + 1;
                }
                else
                {
                    sheetId = 1;
                }

                // Append a new worksheet and associate it with the workbook.
                var sheet = new Sheet()
                {
                    Id = spreadsheetDocument.WorkbookPart.
                        GetIdOfPart(worksheetPart),
                    SheetId = sheetId,
                    Name = "Sheet" + sheetId
                };
                sheets.Append(sheet);

                //Add the header row here.
                var headerRow = new Row();
                headerRow.Height = 500;
                headerRow.Collapsed = false;
                foreach (DataColumn column in ResultsData.Columns)
                {
                    var cell = new Cell { DataType = CellValues.String, CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName) };
                    headerRow.AppendChild(cell);
                }
                sheetData.AppendChild(headerRow);

                foreach (DataRow row in ResultsData.Rows)
                {
                    var newRow = new Row();
                    newRow.Height = 500;
                    newRow.Collapsed = false;

                    foreach (DataColumn col in ResultsData.Columns)
                    {
                        var cell = new Cell
                        {
                            DataType = CellValues.String,
                            CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(row[col].ToString())
                        };
                        newRow.AppendChild(cell);
                    }
                    sheetData.AppendChild(newRow);
                }

                workbookpart.Workbook.Save();

                // Close the document.
                //spreadsheetDocument.Close();
                spreadsheetDocument.Dispose();
            }
            return await DownloadFileAsync(fileName, filePath);
        }

        public async Task<FileStreamResult> DownloadFileAsync(string fileName, string filePath)
        {
            // Set the content type to plain text (you should change this to the appropriate MIME type for your file)
            var contentType = "text/plain";

            // Add the header to indicate a file attachment with the provided filename
            var contentDisposition = "attachment; filename=" + fileName;

            // Create a file stream result
            var fileStreamResult = new FileStreamResult(new FileStream(filePath, FileMode.Open), contentType)
            {
                FileDownloadName = fileName
            };

            return fileStreamResult;
        }

        public List<T> ConvertToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>()
                    .Select(c => c.ColumnName)
                    .ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {

                    try
                    {
                        PropertyInfo pI = objT.GetType().GetProperty(pro.Name);
                        pro.SetValue(objT, row[pro.Name] == DBNull.Value ? null : Convert.ChangeType(row[pro.Name], pI.PropertyType));

                    }
                    catch (Exception e)
                    {

                    }
                }
                return objT;
            }).ToList();
        }

        public T ConvertToModel<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>()
                    .Select(c => c.ColumnName)
                    .ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    try
                    {
                        PropertyInfo pI = objT.GetType().GetProperty(pro.Name);
                        pro.SetValue(objT, row[pro.Name] == DBNull.Value ? null : Convert.ChangeType(row[pro.Name], pI.PropertyType));


                        //if (columnNames.Contains(pro.Name))
                        //{
                        //    if (pro.Name == "SiteInclusionDate" || pro.Name == "ProcessCompletionDate" || pro.Name == "CreationDate" || pro.Name == "ModificationDate")
                        //    {
                        //        PropertyInfo pI = objT.GetType().GetProperty(pro.Name);
                        //        if (row[pro.Name] == DBNull.Value)
                        //        {
                        //            pro.SetValue(objT, null);
                        //        }
                        //        else
                        //        {
                        //            pro.SetValue(objT, Convert.ChangeType(row[pro.Name], Date.GetTypeCode()));
                        //        }

                        //    }
                        //    else
                        //    {
                        //        PropertyInfo pI = objT.GetType().GetProperty(pro.Name);
                        //        pro.SetValue(objT, row[pro.Name] == DBNull.Value ? null : Convert.ChangeType(row[pro.Name], pI.PropertyType));
                        //    }

                        //}
                    }
                    catch (Exception e)
                    {

                    }
                }
                return objT;
            }).SingleOrDefault();
        }

        internal int CountTotalFeedback()
        {
            //string sql = @" SELECT count (fso.Id) from [FilledSurveyOption] fso inner join [dbo].[FilledSurveys] fs on fs.id = fso.[FilledSurveyId]";
            string sql = @" select count(*) from [dbo].[FilledSurveys]";

            using (con = new SqlConnection(CONNSTR))
            {
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.CommandTimeout = 250; //seconds
                int resultCount = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
                return resultCount;
            }
        }

        /// <summary>
        /// Refactor this function later. This is just for the prototype purposes 
        /// </summary>
        /// <returns></returns>
        internal int CountFromKazipara()
        {
            //            string sql = @" SELECT count(*) from [FilledSurveyOption] fso inner join [dbo].[FilledSurveys] fs on fs.id = fso.[FilledSurveyId]
            //inner join Surveys s on s.id = fs.SurveyId
            //inner join Center c on c.id = fs.CenterId
            //where c.Id =1";
            string sql = @" SELECT count(*) from [dbo].[FilledSurveys] fs 
                            inner join Surveys s on s.id = fs.SurveyId
                            inner join Center c on c.id = fs.CenterId
                            where c.Id =1";

            using (con = new SqlConnection(CONNSTR))
            {
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.CommandTimeout = 250; //seconds
                int resultCount = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
                return resultCount;
            }
        }

        internal int CountFromUttara()
        {
            //            string sql = @" SELECT count(*) from [FilledSurveyOption] fso inner join [dbo].[FilledSurveys] fs on fs.id = fso.[FilledSurveyId]
            //inner join Surveys s on s.id = fs.SurveyId
            //inner join Center c on c.id = fs.CenterId
            //where c.Id =2";
            string sql = @" SELECT count(*) from [dbo].[FilledSurveys] fs 
                            inner join Surveys s on s.id = fs.SurveyId
                            inner join Center c on c.id = fs.CenterId
                            where c.Id =2";

            using (con = new SqlConnection(CONNSTR))
            {
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.CommandTimeout = 250; //seconds
                int resultCount = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
                return resultCount;
            }
        }

        internal int CountUniqueFeedback()
        {
            string sql = @"SELECT count (distinct fs.PatientId) from [FilledSurveyOption] fso inner join [dbo].[FilledSurveys] fs on fs.id = fso.[FilledSurveyId]";

            using (con = new SqlConnection(CONNSTR))
            {
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.CommandTimeout = 250; //seconds
                int resultCount = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
                return resultCount;
            }
        }

        internal List<FeedbackViewModel> LoadLastFiveFeedback()
        {
            string sql = @"select Top(5) fs.CreatedAt, fs.PatientId, c.Name CentreName, s.Title Survey from FilledSurveys fs inner join Center c on c.Id = fs.CenterId inner join Surveys s on s.id = fs.SurveyId order by fs.id desc";

            using (con = new SqlConnection(CONNSTR))
            {
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.CommandTimeout = 250; //seconds
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                List<FeedbackViewModel> list = ConvertToList<FeedbackViewModel>(dt);
                return list;
            }
        }

        internal dynamic CountTodaysFeedback()
        {
            string sql = @"SELECT count (distinct fs.PatientId) from [FilledSurveyOption] fso inner join [dbo].[FilledSurveys] fs on fs.id = fso.[FilledSurveyId] where CAST(fs.CreatedAt as date) = CAST(getdate() as date)";

            using (con = new SqlConnection(CONNSTR))
            {
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.CommandTimeout = 250; //seconds
                int resultCount = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
                return resultCount;
            }
        }

        internal List<FeedbackViewModel> LoadFeedback()
        {
            string sql = @"select fs.CreatedAt, fs.PatientId, c.Name CentreName, s.Title Survey, s.Id SurveyId from FilledSurveys fs inner join Center c on c.Id = fs.CenterId inner join Surveys s on s.id = fs.SurveyId order by fs.id desc";

            using (con = new SqlConnection(CONNSTR))
            {
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.CommandTimeout = 250; //seconds
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                List<FeedbackViewModel> list = ConvertToList<FeedbackViewModel>(dt);
                return list;
            }
        }

        internal List<CentreSurveyViewModel> LoadCentreWiseSurveyCount()
        {
            string sql = @" SELECT c.Name, count(c.Name) SurveyCount, [dbo].[getCentreButtonbyName](c.id) [ButtonClass] , [dbo].[getCentreIconbyName](c.Id) [IconClass], [dbo].[getCentreTextbyName](c.Id) [TextClass] from [dbo].[FilledSurveys] fs 
                            inner join Surveys s on s.id = fs.SurveyId
                            inner join Center c on c.id = fs.CenterId
							group by c.Id, c.Name";

            using (con = new SqlConnection(CONNSTR))
            {
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.CommandTimeout = 250; //seconds
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                List<CentreSurveyViewModel> list = ConvertToList<CentreSurveyViewModel>(dt);
                return list;
            }
        }

        internal List<SurveyResultViewModel> LoadFeedbackDetails(int surveyId, string patientId, string surveycollectionDate)
        {
            string sql = @" SELECT s.Title, C.Name 'Centre', fs.CreatedAt Surveydate, fs.PatientId, Q.ShortTitle 'Question', o.Title 'Answer'
               FROM dbo.FilledSurveys fs
               INNER JOIN dbo.FilledSurveyOption fso ON fs.Id = fso.FilledSurveyId
               INNER JOIN dbo.Options o ON fso.OptionId = o.Id
               INNER JOIN Questions q on q.Id = o.QuestionId
               INNER JOIN Center c ON c.Id = fS.CenterId
               INNER JOIN Surveys s on s.Id = fs.SurveyId
               Where s.Id =" + surveyId + " and fs.PatientId = '" + patientId + "' and CAST(fs.CreatedAt as date) = CAST('" + surveycollectionDate + "' as date)";

            using (con = new SqlConnection(CONNSTR))
            {
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.CommandTimeout = 250; //seconds
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                List<SurveyResultViewModel> list = ConvertToList<SurveyResultViewModel>(dt);
                return list;
            }
        }
    }
}
