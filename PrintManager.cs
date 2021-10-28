using System;
using System.ComponentModel;
using Regularity_Rally.Control;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Linq;


namespace Regularity_Rally
{
    class PrintManager
    {
        public List<Race.RaceSummary> RaceScore = new List<Race.RaceSummary>();
        public List<Tournament.ScoredRacerInTourByPos> TourScore = new List<Tournament.ScoredRacerInTourByPos>();
        public List<CarView> PrintCarsList = new List<CarView>();
        public List<CatView> PrintCategoryList = new List<CatView>();
        public List<BrandView> PrintMeasBrandsList = new List<BrandView>();
        public List<RacerView> PrintRacersList = new List<RacerView>();
        public List<RacesView> PrintRacesDBList = new List<RacesView>();
        public List<TeamView> PrintTeamsList = new List<TeamView>();
        public List<TournamentsView> PrintTournamentsDBList = new List<TournamentsView>();
        public List<UserView> PrintUsersList = new List<UserView>();
        List<MeasurerInRace> PrintTimekeepersInRace = new List<MeasurerInRace>();

        public enum PrintType
        {
            Races,
            Tournament,
            Cars,
            Category,
            Brands,
            Racers,
            RacesDB,
            Teams,
            TournamentsDB,
            Users
        }

        public PrintManager()
        {
            //empty
        }

        public void createPDF(ListView listview, bool horizontal = true, string headerText = " ", string subheaderText = " ", string headerFontLocation = null, string contentFontLocation = null, string[] infoText = null, float[] width = null, int[] headerIndexes = null, bool jurySign = false, string[] juries = null)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            // Nastavení filtrace souborů na soubory PDF a výběr všech souborů, nastavení počátečního adresáře pro ukládání na hlavní adresář programu
            saveFileDialog1.Filter = "soubory PDF (*.pdf)|*.pdf|Všechny soubory(*.*)|*.*";
            saveFileDialog1.InitialDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Print";
            saveFileDialog1.FilterIndex = 1;

            // Přiřazení lokací fontů z globálních dat, pokud lokace fontů nebyly přiřazeny ve volání metody
            headerFontLocation = headerFontLocation ?? @"C:\WINDOWS\Fonts\verdana.ttf";
            contentFontLocation = contentFontLocation ?? @"C:\WINDOWS\Fonts\arial.ttf";

            // Činnost dialogového okna pro uložení souboru
            string reportName = "";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                reportName = saveFileDialog1.FileName;

                FileStream fs = new FileStream(reportName, FileMode.Create, FileAccess.Write, FileShare.None);
                Document doc = new Document(PageSize.A4, 40f, 40f, 120f, 65f);

                try
                {
                    PdfWriter writer = PdfWriter.GetInstance(doc, fs);

                    // Nastavení vytváření záhlaví a zápatí pro každou novou stránku PDF souboru
                    ITextEvents newPageEvent = new ITextEvents();
                    newPageEvent.Header = headerText;
                    newPageEvent.SubHeader = subheaderText;
                    newPageEvent.Font = headerFontLocation;
                    IPdfPageEvent headerFooterAdd = (IPdfPageEvent)newPageEvent;
                    writer.PageEvent = headerFooterAdd;
                    doc.Open();
                    doc.AddCreator("iTextSharp 5.5.11");

                    // Vytvoření fontů pro PDF soubor
                    BaseFont bf = BaseFont.CreateFont(contentFontLocation, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    iTextSharp.text.Font contentFont = new iTextSharp.text.Font(bf, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                    iTextSharp.text.Font contentFontBold = new iTextSharp.text.Font(bf, 12f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                    iTextSharp.text.Font categoryNameFont = new iTextSharp.text.Font(bf, 16f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

                    Phrase content = null;
                    if (listview.Items.Count > 0)
                    {
                        // Vytvoření horizontálně orientované tabulky (informace jsou ve sloupcích, každý záznam zabírá 1 řádek)
                        if (horizontal)
                        {
                            // Nastavení hodnot informačních popisků a šířek sloupců v případě, že nebyly zadány při volání metody
                            infoText = infoText ?? Enumerable.Repeat<string>(" ", listview.Items[0].SubItems.Count).ToArray();
                            width = width ?? Enumerable.Repeat<float>(1, listview.Items[0].SubItems.Count).ToArray();

                            // Vytvoření tabulky pro záznamy
                            PdfPTable infoTab = new PdfPTable(infoText.Length);
                            infoTab.SpacingAfter = 20f;
                            infoTab.SetWidths(width);
                            infoTab.TotalWidth = PageSize.A4.Width - 80;
                            infoTab.LockedWidth = true;

                            // Vytvoření nadpisů tabulky
                            foreach (string headerInfo in infoText)
                            {
                                Phrase headerInfoText = new Phrase(headerInfo, contentFontBold);
                                PdfPCell tabHeaderCell = new PdfPCell(headerInfoText);
                                tabHeaderCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                tabHeaderCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                tabHeaderCell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                tabHeaderCell.PaddingBottom = 5f;
                                infoTab.AddCell(tabHeaderCell);
                            }

                            int contentRowCount = 1;

                            // Naplnění tabulyk záznamy (každý sudý záznam je barevně odlišen)
                            foreach (ListViewItem item in listview.Items)
                            {
                                for (int i = 0; i < item.SubItems.Count; i++)
                                {
                                    PdfPCell pdfCell = new PdfPCell();

                                    content = new Phrase(item.SubItems[i].Text, contentFont);
                                    pdfCell = new PdfPCell(content);
                                    pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    pdfCell.Border = 0;
                                    pdfCell.MinimumHeight = 15f;
                                    pdfCell.NoWrap = false;
                                    pdfCell.PaddingBottom = 5f;
                                    pdfCell.BackgroundColor = (contentRowCount % 2 == 1) ? BaseColor.WHITE : BaseColor.LIGHT_GRAY;

                                    infoTab.AddCell(pdfCell);
                                }

                                contentRowCount++;
                            }
                            doc.Add(infoTab);
                        }

                        // Vytvoření vertikálně orientované tabulky (informace jsou v řádcích, každý záznam tvoří samostatnou tabulku)
                        else
                        {
                            // Nastavení hodnot informačních popisků, šířek sloupců v případě a indexů pro tvorbu nadpisů tabulek v případě, že nebyly zadány při volání metody
                            headerIndexes = headerIndexes ?? new int[] { 0 };
                            infoText = infoText ?? Enumerable.Repeat<string>(" ", listview.Items[0].SubItems.Count).ToArray();
                            width = width ?? new float[] { 1f, 1f };

                            foreach (ListViewItem item in listview.Items)
                            {
                                // Vytvoření tabulky se záznamy
                                PdfPTable infoTab = new PdfPTable(2);
                                infoTab.SpacingAfter = 40f;

                                // Vytvoření nadpisu s názvem měřicí společnosti
                                string itemNameText = "";

                                foreach (int index in headerIndexes)
                                {
                                    itemNameText += item.SubItems[index].Text + " ";
                                }

                                // Vytvoření nadpisu každé tabulky na základě seznamu se vstupními informacemi a indexů
                                Paragraph itemName = new Paragraph(itemNameText, new iTextSharp.text.Font(bf, 14f, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
                                PdfPCell itemNameCell = new PdfPCell(itemName);
                                itemNameCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                itemNameCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                itemNameCell.Colspan = 2;
                                itemNameCell.PaddingBottom = 5f;
                                itemNameCell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                infoTab.AddCell(itemNameCell);

                                // Naplnění tabulky záznamy
                                for (int i = 0; i < item.SubItems.Count; i++)
                                {
                                    if (!headerIndexes.Contains(i))
                                    {
                                        content = new Phrase(infoText[i], contentFontBold);
                                        PdfPCell infoCell = new PdfPCell(content);
                                        infoCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        infoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        infoCell.Border = 0;
                                        infoCell.MinimumHeight = 15f;
                                        infoCell.NoWrap = false;

                                        infoTab.AddCell(infoCell);

                                        // Ošetření změny textu v případě, že se v záznamu vyskytuje datum ve špatném formátu
                                        if (infoText[i].ToLower().Contains("datum") && item.SubItems[i].Text.Contains("0:00:00"))
                                            content = new Phrase(item.SubItems[i].Text.Split(' ')[0], contentFont);
                                        else
                                            content = new Phrase(item.SubItems[i].Text, contentFont);

                                        PdfPCell pdfCell = new PdfPCell(content);
                                        pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        pdfCell.Border = 0;
                                        pdfCell.MinimumHeight = 15f;
                                        pdfCell.NoWrap = false;

                                        infoTab.AddCell(pdfCell);
                                    }
                                }
                                doc.Add(infoTab);
                            }
                        }

                        // Vytvoření podpisových polí pro činovníky, jejichž podpis je nutný pro ověření výsledků závodu
                        if (jurySign)
                        {
                            PdfPTable juryTab = new PdfPTable(2);
                            juryTab.SpacingBefore = 60f;
                            juryTab.SpacingAfter = 20f;

                            float[] widths = new float[] { 1f, 2f };
                            juryTab.SetWidths(widths);

                            if (juries != null)
                            {
                                foreach (string jury in juries)
                                {
                                    content = new Phrase(jury, contentFontBold);
                                    PdfPCell juryCell = new PdfPCell(content);
                                    juryCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    juryCell.VerticalAlignment = Element.ALIGN_TOP;
                                    juryCell.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                                    juryCell.MinimumHeight = 60f;
                                    juryCell.NoWrap = false;

                                    juryTab.AddCell(juryCell);

                                    content = new Phrase("");
                                    PdfPCell juryCell2 = new PdfPCell(content);
                                    juryCell2.Border = 0;
                                    juryTab.AddCell(juryCell2);
                                }
                                doc.Add(juryTab);
                            }
                        }
                    }

                    // Oznámení o prázdném PDF souboru v případě, že požadovaný seznam je prázdný
                    else
                    {
                        Paragraph noContentInfo = new Paragraph("Žádné informace k vytisknutí", contentFont);
                        noContentInfo.Alignment = Element.ALIGN_CENTER;
                        doc.Add(noContentInfo);
                    }

                    MessageBox.Show("PDF soubor úspěšně vytvořen");
                }
                catch (DocumentException dex)
                {
                    throw (dex);
                }
                catch (IOException ioex)
                {
                    throw (ioex);
                }
                finally
                {
                    doc.Close();
                    fs.Dispose();
                }
            }
        }

        public void Print(PrintType PrintItem)
        {
            switch (PrintItem)
            {
                case PrintType.Races:
                    PrintRaces();
                    break;
                case PrintType.Tournament:
                    PrintTournaments();
                    break;
                case PrintType.Cars:
                    PrintCars();
                    break;
                case PrintType.Category:
                    PrintCategory();
                    break;
                case PrintType.Brands:
                    PrintMeasBrands();
                    break;
                case PrintType.Racers:
                    PrintRacers();
                    break;
                case PrintType.RacesDB:
                    PrintRacesDB();
                    break;
                case PrintType.Teams:
                    PrintTeams();
                    break;
                case PrintType.TournamentsDB:
                    PrintTournamentsDB();
                    break;
                case PrintType.Users:
                    PrintUsers();
                    break;
            }
        }

        Race RaceTracker = null;
        public Race.RaceTime CreateRaceTracker(Int32 race_id)
        {
            RaceTracker = new Race(race_id);
            return RaceTracker.GetRaceTimes();
        }
        public void PrintRaces()
        {
            RaceScore.Clear();
            RaceScore = RaceTracker.GetSummary();

            System.Windows.Forms.ListView lstView_races = new System.Windows.Forms.ListView();
            lstView_races.View = System.Windows.Forms.View.Details;

            string[] columnHeaders = {
                "Position",
                "FullName",
                "StartNumber",
                "Team",
                "RefTime",
                "Time",
                "PenTime",
                "FinalTime",
                "Car",
                "RacerStatusText",
            };

            for (int pos = 0; pos < columnHeaders.Length; pos++)
            {
                lstView_races.Columns.Add(columnHeaders[pos]);
            }

            foreach (Race.RaceSummary dt in RaceScore)
            {
                lstView_races.Items.Add(new System.Windows.Forms.ListViewItem(new string[] {
                    dt.Position.ToString(),
                    dt.FullName,
                    dt.StartNumber.ToString(),
                    dt.Team,
                    dt.RefTime,
                    dt.Time.ToString(),
                    dt.PenTime.ToString(),
                    dt.FinalTime.ToString(),
                    dt.Car,
                    dt.RacerStatusText
                }));
            }

            createPDF(lstView_races, horizontal: true, headerText: "Races", infoText: columnHeaders, width: new float[] { 2.5f, 2.5f, 1f });
        }

        Tournament TClass = null;
        public void CreateTourTracker(Int32 tour_id)
        {
            TClass = new Tournament(tour_id);

        }
        public void PrintTournaments()
        {
            TourScore.Clear();
            TourScore = TClass.GetPositionInTour();

            System.Windows.Forms.ListView lstView_tours = new System.Windows.Forms.ListView();
            lstView_tours.View = System.Windows.Forms.View.Details;

            string[] columnHeaders =
                {
                "Position",
                "Points",
                "StartNum",
                "RacerName",
                "RacerTeam"
                };

            for (int pos = 0; pos < columnHeaders.Length; pos++)
            {
                lstView_tours.Columns.Add(columnHeaders[pos]);
            }

            foreach (Tournament.ScoredRacerInTourByPos dt in TourScore)
            {
                lstView_tours.Items.Add(new System.Windows.Forms.ListViewItem(new string[] {
                    dt.Position.ToString(),
                    dt.Points.ToString(),
                    dt.StartNum.ToString(),
                    dt.RacerName,
                    dt.RacerTeam
                }));
            }

            createPDF(lstView_tours, horizontal: true, headerText: "Tournaments", infoText: columnHeaders, width: new float[] { 2.5f, 2.5f, 1f });
        }

        public void PrintCars()
        {
            List<CarView> _items = new List<CarView>();
            MySqlDataReader rdr = null;
            try
            {
                // load data base on filter
                MySqlCommand command = Database.Database.CreateCommand(Database.Database.QueryStack["GetCarData"]);
                rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    _items.Add(new CarView()
                    {
                        ID = rdr.GetInt32("ID_Car"),
                        Brand = rdr.GetString("Brand"),
                        Model = rdr.GetString("Model"),
                        Year = rdr.GetInt32("Year"),
                        Vin = rdr.GetString("VIN")

                    });
                }
                rdr.Close();
                PrintCarsList.Clear();
                PrintCarsList = _items;
            }
            catch
            {

            }

            System.Windows.Forms.ListView lstView_cars = new System.Windows.Forms.ListView();
            lstView_cars.View = System.Windows.Forms.View.Details;

            string[] columnHeaders =
                {
                "Brand",
                "Model",
                "Year",
                "VIN"
                };

            for (int pos = 0; pos < columnHeaders.Length; pos++)
            {
                lstView_cars.Columns.Add(columnHeaders[pos]);
            }

            foreach (CarView dt in PrintCarsList)
            {
                lstView_cars.Items.Add(new System.Windows.Forms.ListViewItem(new string[] {
                    dt.Brand,
                    dt.Model,
                    dt.Year.ToString(),
                    dt.Vin
                }));
            }

            createPDF(lstView_cars, horizontal: true, headerText: "Cars", infoText: columnHeaders, width: new float[] { 2.5f, 2.5f, 1f });
        }

        public void PrintCategory()
        {
            List<CatView> _items = new List<CatView>();
            MySqlDataReader rdr = null;
            try
            {
                // load data base on filter
                MySqlCommand command = Database.Database.CreateCommand(Database.Database.QueryStack["GetCatData"]);
                rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    _items.Add(new CatView()
                    {
                        ID = rdr.GetInt32("ID_Category"),
                        Name = rdr.GetString("Name"),
                        Limit = rdr.GetInt32("Limit"),
                        Coef = rdr.GetDouble("Coef"),

                    });
                }
                rdr.Close();

                PrintCategoryList.Clear();
                PrintCategoryList = _items;
            }
            catch { }

            System.Windows.Forms.ListView lstView_cat = new System.Windows.Forms.ListView();
            lstView_cat.View = System.Windows.Forms.View.Details;

            string[] columnHeaders =
                {
                "Name",
                "Limit",
                "Coef"
                };

            for (int pos = 0; pos < columnHeaders.Length; pos++)
            {
                lstView_cat.Columns.Add(columnHeaders[pos]);
            }

            foreach (CatView dt in PrintCategoryList)
            {
                lstView_cat.Items.Add(new System.Windows.Forms.ListViewItem(new string[] {
                    dt.Name,
                    dt.Limit.ToString(),
                    dt.Coef.ToString()
                }));
            }

            createPDF(lstView_cat, horizontal: true, headerText: "Categories", infoText: columnHeaders, width: new float[] { 2.5f, 2.5f, 1f });
        }

        public void PrintMeasBrands()
        {
            List<BrandView> _items = new List<BrandView>();
            MySqlDataReader rdr = null;
            try
            {
                // load data base on filter
                MySqlCommand command = Database.Database.CreateCommand(Database.Database.QueryStack["GetBrandData"]);
                rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    _items.Add(new BrandView()
                    {
                        ID = rdr.GetInt32("ID_Meas_Company"),
                        Company = rdr.GetString("Company"),
                        Address = rdr.GetString("Address"),
                        Telephone = rdr.GetString("Telephone"),
                        Email = rdr.GetString("Email"),
                        Web = rdr.GetString("Web")

                    });
                }
                rdr.Close();

                PrintMeasBrandsList.Clear();
                PrintMeasBrandsList = _items;
            }
            catch
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
            }

            System.Windows.Forms.ListView lstView_brands = new System.Windows.Forms.ListView();
            lstView_brands.View = System.Windows.Forms.View.Details;

            string[] columnHeaders =
                {
                "Company",
                "Address",
                "Telephone",
                "Email",
                "Web"
                };

            for (int pos = 0; pos < columnHeaders.Length; pos++)
            {
                lstView_brands.Columns.Add(columnHeaders[pos]);
            }

            foreach (BrandView dt in PrintMeasBrandsList)
            {
                lstView_brands.Items.Add(new System.Windows.Forms.ListViewItem(new string[] {
                    dt.Company,
                    dt.Address,
                    dt.Telephone,
                    dt.Email,
                    dt.Web
                }));
            }

            createPDF(lstView_brands, horizontal: true, headerText: "Brands", infoText: columnHeaders, width: new float[] { 2.5f, 2.5f, 1f });
        }

        public void PrintRacers()
        {
            List<RacerView> _items = new List<RacerView>();
            MySqlDataReader rdr = null;
            try
            {
                // load data base on filter
                MySqlCommand command = Database.Database.CreateCommand(Database.Database.QueryStack["GetRacerData"]);
                rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    _items.Add(new RacerView()
                    {
                        ID = rdr.GetInt32("ID_Racer"),
                        First_name = rdr.GetString("First_name"),
                        Last_name = rdr.GetString("Last_name"),
                        Short_name = rdr.GetString("Short_name"),
                        Gender = rdr.GetString("Gender"),
                        Born = rdr.GetString("Born"),
                        Nationality = rdr.GetString("Nationality"),
                        Address = rdr.GetString("Address"),
                        Tel = rdr.GetString("Telephone"),
                        Mail = rdr.GetString("Email"),
                        Team = rdr.GetString("Team_name"),
                        TeamID = rdr.GetInt32("FK_Team")
                    });
                }
                rdr.Close();



                PrintRacersList.Clear();
                PrintRacersList = _items;
            }
            catch
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
            }

            System.Windows.Forms.ListView lstView_racers = new System.Windows.Forms.ListView();
            lstView_racers.View = System.Windows.Forms.View.Details;

            string[] columnHeaders =
                {
                "First_name",
                "Last_name",
                "Short_name",
                "Gender",
                "Born",
                "Nationality",
                "Address",
                "Tel",
                "Mail",
                "Team",
                };

            for (int pos = 0; pos < columnHeaders.Length; pos++)
            {
                lstView_racers.Columns.Add(columnHeaders[pos]);
            }

            foreach (RacerView dt in PrintRacersList)
            {
                lstView_racers.Items.Add(new System.Windows.Forms.ListViewItem(new string[] {
                    dt.First_name,
                    dt.Last_name,
                    dt.Short_name,
                    dt.Gender,
                    dt.Born,
                    dt.Nationality,
                    dt.Address,
                    dt.Tel,
                    dt.Mail,
                    dt.Team
                }));
            }

            createPDF(lstView_racers, horizontal: true, headerText: "Racers", infoText: columnHeaders, width: new float[] { 2.5f, 2.5f, 1f });
        }

        public void PrintRacesDB()
        {
            List<RacesView> _items = new List<RacesView>();
            ;
            MySqlDataReader rdr = null;
            //Int32 HelpID = 0;

            try
            {
                // load tata to be feeded into selector
                MySqlCommand cmd = Database.Database.CreateCommand(Database.Database.QueryStack["GetRaceDataSelect"]);
                rdr = cmd.ExecuteReader();
                //string TourHelp = "None";
                //int colIndex;

                while (rdr.Read())
                {
                    RacesView DataToList = new RacesView();

                    DataToList.ID = rdr.GetInt32("ID_Race");
                    DataToList.StatusID = rdr.GetInt32("FK_Race_Status");
                    DataToList.RaceNote = rdr.GetInt32("FK_Race_Note");
                    DataToList.Category = rdr.GetInt32("FK_Category");
                    DataToList.MeasID = rdr.GetInt32("FK_Meas_company");
                    DataToList.Title = rdr.GetString("Name");
                    DataToList.Date = rdr.GetDateTime("Date").ToShortDateString();
                    DataToList.Place = rdr.GetString("Place");
                    DataToList.Length = rdr.GetInt32("Length");
                    DataToList.Time = rdr.GetString("Time").ToString();
                    DataToList.StartTime = rdr.GetString("Start_Time");
                    DataToList.NumOfLaps = rdr.GetInt32("NumOfLaps");
                    DataToList.Status = rdr.GetString("Status_text");

                    _items.Add(DataToList);
                }
                rdr.Close();
                PrintRacesDBList.Clear();
                PrintRacesDBList = _items;
            }
            catch
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
            }

            System.Windows.Forms.ListView lstView_races = new System.Windows.Forms.ListView();
            lstView_races.View = System.Windows.Forms.View.Details;

            string[] columnHeaders =
                {
                "Title",
                "Date",
                "Place",
                "Length",
                "Time",
                "NumOfLaps",
                "Status"
                };

            for (int pos = 0; pos < columnHeaders.Length; pos++)
            {
                lstView_races.Columns.Add(columnHeaders[pos]);
            }

            foreach (RacesView dt in PrintRacesDBList)
            {
                lstView_races.Items.Add(new System.Windows.Forms.ListViewItem(new string[] {
                    dt.Title,
                    dt.Date.ToString(),
                    dt.Place,
                    dt.Length.ToString(),
                    dt.Time.ToString(),
                    dt.NumOfLaps.ToString(),
                    dt.Status
                }));
            }

            createPDF(lstView_races, horizontal: true, headerText: "Races", infoText: columnHeaders, width: new float[] { 2.5f, 2.5f, 1f });
        }

        public void PrintTeams()
        {
            List<TeamView> _items = new List<TeamView>();
            MySqlDataReader rdr = null;
            try
            {
                // load data base on filter
                MySqlCommand command = Database.Database.CreateCommand(Database.Database.QueryStack["GetTeamData"]);
                rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    _items.Add(new TeamView()
                    {
                        ID = rdr.GetInt32("ID_Team"),
                        Team_name = rdr.GetString("Team_name"),
                        Short_name = rdr.GetString("Short_name"),
                        E_mail = rdr.GetString("E_mail"),

                    });
                }
                rdr.Close();

                PrintTeamsList.Clear();
                PrintTeamsList = _items;

            }
            catch
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
            }

            System.Windows.Forms.ListView lstView_teams = new System.Windows.Forms.ListView();
            lstView_teams.View = System.Windows.Forms.View.Details;

            string[] columnHeaders =
                {
                "Team name",
                "Short name",
                "Email"
                };

            for (int pos = 0; pos < columnHeaders.Length; pos++)
            {
                lstView_teams.Columns.Add(columnHeaders[pos]);
            }

            foreach (TeamView dt in PrintTeamsList)
            {
                lstView_teams.Items.Add(new System.Windows.Forms.ListViewItem(new string[] {
                    dt.Team_name,
                    dt.Short_name,
                    dt.E_mail
                }));
            }

            createPDF(lstView_teams, horizontal: true, headerText: "Teams", infoText: columnHeaders, width: new float[] { 2.5f, 2.5f, 1f });
        }

        public void PrintTournamentsDB()
        {
            List<TournamentsView> _items = new List<TournamentsView>();
            MySqlCommand command;
            MySqlDataReader rdr = null;
            try
            {
                // load data base on filter
                command = Database.Database.CreateCommand(Database.Database.QueryStack["GetRacesInTourT"]);
                rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    _items.Add(new TournamentsView()
                    {
                        ID = rdr.GetInt32("ID_Tournament"),
                        Name = rdr.GetString("Tournament_name"),
                        Name_Short = rdr.GetString("Tournamen_shortname"),
                        Year = rdr.GetInt32("Year"),
                        CategoryID = rdr.GetInt32("FK_Category"),
                        CategoryName = rdr.GetString("Name")

                    });
                }
                rdr.Close();

                PrintTournamentsDBList.Clear();
                PrintTournamentsDBList = _items;
            }
            catch
            {

            }

            System.Windows.Forms.ListView lstView_tours = new System.Windows.Forms.ListView();
            lstView_tours.View = System.Windows.Forms.View.Details;

            string[] columnHeaders =
                {
                "Name",
                "Short name",
                "Year",
                "Category"
                };

            for (int pos = 0; pos < columnHeaders.Length; pos++)
            {
                lstView_tours.Columns.Add(columnHeaders[pos]);
            }

            foreach (TournamentsView dt in PrintTournamentsDBList)
            {
                lstView_tours.Items.Add(new System.Windows.Forms.ListViewItem(new string[] {
                    dt.Name,
                    dt.Name_Short,
                    dt.Year.ToString(),
                    dt.CategoryName
                }));
            }

            createPDF(lstView_tours, horizontal: true, headerText: "Tournaments", infoText: columnHeaders, width: new float[] { 2.5f, 2.5f, 1f });
        }

        public void PrintUsers()
        {
            List<UserView> _items = new List<UserView>();
            MySqlDataReader rdr = null;
            try
            {
                // load data base on filter
                List<User> userdata = User.GetUserCollection();
                foreach (User user in userdata)
                {
                    _items.Add(user.ToUserView());
                }

                PrintUsersList.Clear();
                PrintUsersList = _items;
            }
            catch
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
            }

            System.Windows.Forms.ListView lstView_users = new System.Windows.Forms.ListView();
            lstView_users.View = System.Windows.Forms.View.Details;

            string[] columnHeaders =
                {
                "Login",
                "Name",
                "Lastname",
                "E_mail",
                "Active"
                };

            for (int pos = 0; pos < columnHeaders.Length; pos++)
            {
                lstView_users.Columns.Add(columnHeaders[pos]);
            }


            foreach (UserView dt in PrintUsersList)
            {
                lstView_users.Items.Add(new System.Windows.Forms.ListViewItem(new string[] {
                    dt.Login,
                    dt.Name,
                    dt.Lastname,
                    dt.E_mail,
                    dt.Active.ToString()
                }));
            }

            createPDF(lstView_users, horizontal: true, headerText: "Users", infoText: columnHeaders, width: new float[] { 2.5f, 2.5f, 1f });
        }
    }


    public class ITextEvents : PdfPageEventHelper
    {

        // This is the contentbyte object of the writer
        PdfContentByte cb;

        // we will put the final number of pages in a template
        PdfTemplate headerTemplate, footerTemplate;

        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf = null;

        // This keeps track of the creation time
        DateTime PrintTime = DateTime.Now;


        #region Fields
        private string _header;
        private string _subHeader;
        private string _font;
        #endregion

        #region Properties
        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }

        public string SubHeader
        {
            get { return _subHeader; }
            set { _subHeader = value; }
        }

        public string Font
        {
            get { return _font; }
            set { _font = value; }
        }
        #endregion


        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                //BaseFont.CP1252,
                //c:/ windows / fonts / verdana.ttf
                PrintTime = DateTime.Now;
                bf = BaseFont.CreateFont(_font, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                cb = writer.DirectContent;
                headerTemplate = cb.CreateTemplate(100, 100);
                footerTemplate = cb.CreateTemplate(50, 50);
            }
            catch (DocumentException de)
            {
                throw (de);
            }
            catch (System.IO.IOException ioe)
            {
                throw (ioe);
            }
        }

        public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {
            base.OnEndPage(writer, document);

            iTextSharp.text.Font baseFontNormalHeader = new iTextSharp.text.Font(bf, 16f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font baseFontNormal = new iTextSharp.text.Font(bf, 14f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

            iTextSharp.text.Font baseFontBig = new iTextSharp.text.Font(bf, 11f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

            Phrase p1Header = new Phrase(_header, baseFontNormalHeader);

            //Create PdfTable object
            PdfPTable pdfTab = new PdfPTable(3);

            //We will have to create separate cells to include image logo and 2 separate strings
            //Row 1
            PdfPCell pdfCell2 = new PdfPCell(p1Header);

            String text = "Strana " + writer.PageNumber + "/";

            //Add paging to footer
            {
                cb.BeginText();
                cb.SetFontAndSize(bf, 12);
                cb.SetTextMatrix(document.PageSize.GetRight(180), document.PageSize.GetBottom(30));
                cb.ShowText(text);
                cb.EndText();
                float len = bf.GetWidthPoint(text, 12);
                cb.AddTemplate(footerTemplate, document.PageSize.GetRight(180) + len, document.PageSize.GetBottom(30));
            }
            //Row 2
            PdfPCell pdfCell4 = new PdfPCell(new Phrase(_subHeader, baseFontNormal));
            //Row 3


            PdfPCell pdfCell5 = new PdfPCell(new Phrase("Vytvořeno: " + PrintTime.ToShortDateString(), baseFontBig));
            PdfPCell pdfCell6 = new PdfPCell();
            PdfPCell pdfCell7 = new PdfPCell(new Phrase("Čas: " + string.Format("{0:t}", DateTime.Now), baseFontBig));


            //set the alignment of all three cells and set border to 0
            pdfCell2.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell4.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell5.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell6.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell7.HorizontalAlignment = Element.ALIGN_CENTER;


            pdfCell2.VerticalAlignment = Element.ALIGN_BOTTOM;
            pdfCell4.VerticalAlignment = Element.ALIGN_TOP;
            pdfCell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell6.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell7.VerticalAlignment = Element.ALIGN_MIDDLE;

            pdfCell2.Colspan = 3;
            pdfCell4.Colspan = 3;


            pdfCell2.Border = 0;
            pdfCell4.Border = 0;
            pdfCell5.Border = 0;
            pdfCell6.Border = 0;
            pdfCell7.Border = 0;


            //add all three cells into PdfTable
            pdfTab.AddCell(pdfCell2);
            pdfTab.AddCell(pdfCell4);
            pdfTab.AddCell(pdfCell5);
            pdfTab.AddCell(pdfCell6);
            pdfTab.AddCell(pdfCell7);

            pdfTab.TotalWidth = document.PageSize.Width - 80f;
            pdfTab.WidthPercentage = 70;
            //pdfTab.HorizontalAlignment = Element.ALIGN_CENTER;


            //call WriteSelectedRows of PdfTable. This writes rows from PdfWriter in PdfTable
            //first param is start row. -1 indicates there is no end row and all the rows to be included to write
            //Third and fourth param is x and y position to start writing
            pdfTab.WriteSelectedRows(0, -1, 40, document.PageSize.Height - 30, writer.DirectContent);
            //set pdfContent value

            //Move the pointer and draw line to separate header section from rest of page
            cb.MoveTo(40, document.PageSize.Height - 90);
            cb.LineTo(document.PageSize.Width - 40, document.PageSize.Height - 90);
            cb.Stroke();

            //Move the pointer and draw line to separate footer section from rest of page
            cb.MoveTo(40, document.PageSize.GetBottom(50));
            cb.LineTo(document.PageSize.Width - 40, document.PageSize.GetBottom(50));
            cb.Stroke();
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            footerTemplate.BeginText();
            footerTemplate.SetFontAndSize(bf, 12);
            footerTemplate.SetTextMatrix(0, 0);
            footerTemplate.ShowText((writer.PageNumber).ToString());
            footerTemplate.EndText();


        }
    }
}

