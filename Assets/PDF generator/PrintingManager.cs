using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using UnityEditor;

public class PrintingManager : MonoBehaviour
{
    [SerializeField] Sprite temp;

    string path = "C:/Users/Abirk/Downloads/PDF/1.pdf";

    private void Start()
    {
        string dirPtah = "C:/Users/Abirk/Downloads/PDF";
        if (!System.IO.Directory.Exists(dirPtah))
        {
            System.IO.Directory.CreateDirectory(dirPtah);
        }

        GenerateFile();
    }

    public void GenerateFile()
    {

        //int index = HUDManager.instance.printablePerfomanceValueIndex;
        //path = Application.persistentDataPath+ "/PDF/Performance " + CsvHandler.instance.alldetails[index].simulationNo+".pdf";

        using (var fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
        {
            var document = new Document(PageSize.A4, 10f, 10f, 30f, 0f);
            var writer = PdfWriter.GetInstance(document, fileStream);

            document.Open();

            document.NewPage();

            //browser title
            document.AddTitle("Test");
            var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

           

        #region  customize font
            BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.NORMAL);

            BaseFont bf2 = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font2 = new iTextSharp.text.Font(bf2, 12, iTextSharp.text.Font.BOLDITALIC);
        #endregion

        #region logo area

           // string imageURL = Application.dataPath + "/Resources/Logo.png";

            //iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(imageURL);


            //Resize image depend upon your need
            //logo.ScaleToFit(350f, 250f);

            //Give some space after the image
            //logo.SpacingAfter = 50f;

            //logo.Alignment = Element.ALIGN_CENTER;

            //document.Add(logo);
        #endregion


        #region headline
            Paragraph headline = new Paragraph("Welding Performance Report");
            headline.Font.Size = 25;
            headline.Alignment = Element.ALIGN_CENTER;
            headline.SpacingAfter = 8;
            document.Add(headline);
        #endregion


        #region  simulation no
            Paragraph simulationNo = new Paragraph("Shanto");
            simulationNo.Font.Size = 35;
            simulationNo.Alignment = Element.ALIGN_CENTER;
            simulationNo.SpacingAfter = 10;
            document.Add(simulationNo);
        #endregion


        #region data table

            PdfPTable table = new PdfPTable(2);

            //change some table default settings
            table.DefaultCell.Padding = 8;
            table.DefaultCell.BackgroundColor = (new BaseColor(152, 152, 152, 40));

            table.AddCell(new Phrase("English", font));  
            table.AddCell(new Phrase("70", font2));

            table.AddCell(new Phrase("Bangla", font));  
            table.AddCell(new Phrase("20", font2)); 

            table.AddCell(new Phrase("ICT", font));
            table.AddCell(new Phrase("60", font2));

            document.Add(table);

        #endregion



        #region suggestion image area

            string suggImagePath = "C:/Users/Abirk/Downloads/q.png";
            

            iTextSharp.text.Image suggImage = iTextSharp.text.Image.GetInstance(suggImagePath);

            //Resize image depend upon your need
            suggImage.ScaleToFit(420f, 750f);

           

            suggImage.Alignment = Element.ALIGN_CENTER;

            document.Add(suggImage);
        #endregion

            document.Close();
            writer.Close();
        }

        PrintFiles();
    }

    private void PrintFiles()
    {
        Debug.Log(path);
        if (path == null)
            return;

        System.Diagnostics.Process process = new System.Diagnostics.Process();
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
        process.StartInfo.UseShellExecute = true;
        process.StartInfo.FileName = path;

        process.Start();
        
        System.Diagnostics.Process.Start(Application.persistentDataPath);
    }
}
