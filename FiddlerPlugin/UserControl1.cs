/***************************************************************************
 *
 * $Author: Turley
 * 
 * "THE BEER-WARE LICENSE"
 * As long as you retain this notice you can do whatever you want with 
 * this stuff. If we meet some day, and you think this stuff is worth it,
 * you can buy me a beer in return.
 *
 ***************************************************************************/

using System;
using System.Globalization;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FiddlerControls;
using Ultima;

namespace FiddlerPlugin
{
	public partial class UserControl1 : UserControl
	{
		public UserControl1()
		{
			InitializeComponent();
		}

		private void onClickExportStatic(object sender, EventArgs e)
		{
			int start=-1, end=-1;
			if(!int.TryParse(text1.Text, out start) || !int.TryParse(text2.Text, out end))
				return;
			string path = FiddlerControls.Options.OutputPath;
			if(!Directory.Exists("MassExport"))
				Directory.CreateDirectory("MassExport");
			path = Path.Combine(path, "MassExport");
			for(int i=start; i<=end; i++)
			{
				if (!Art.IsValidStatic(i))
					continue;
				string FileName = Path.Combine(path, String.Format("{0}.tiff", i));
				Bitmap bit = new Bitmap(Ultima.Art.GetStatic(i));
				if (bit != null)
					bit.Save(FileName, ImageFormat.Tiff);
				bit.Dispose();
			}
		}

		private void onClickImportStatic(object sender, EventArgs e)
		{
			if(!Directory.Exists("MassImport"))
			{
				MessageBox.Show(string.Format("Devi creare la directory -> MassImport <- in {0}", FiddlerControls.Options.OutputPath), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
				return;
			}
			string path = FiddlerControls.Options.OutputPath;
			Dictionary<int, string> ItemDataCSV_OLD=null;
			Dictionary<int, string> ItemDataCSV_NEW=null;
			path = Path.Combine(path, "MassImport");
			if(File.Exists("ItemDataOLD.csv") && File.Exists("ItemDataNEW.csv"))
			{
				ItemDataCSV_OLD = new Dictionary<int, string>();
				ItemDataCSV_NEW = new Dictionary<int, string>();
				string line;
				using(StreamReader reader = new StreamReader("ItemDataOLD.csv"))
				{
					while((line=reader.ReadLine())!=null)
					{
						int i;
						string[] splitted = line.Split(';');
						if(int.TryParse(splitted[0].Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out i))
							ItemDataCSV_OLD[i]=line;
					}
				}
				using(StreamReader reader = new StreamReader("ItemDataNEW.csv"))
				{
					while((line=reader.ReadLine())!=null)
					{
						int i;
						string[] splitted = line.Split(';');
						if(int.TryParse(splitted[0].Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out i))
							ItemDataCSV_NEW[i]=line;
					}
				}
			}
			int start=-1, end=-1;
			if(!int.TryParse(text1i.Text, out start) || !int.TryParse(text2i.Text, out end))
				return;

			StreamWriter writer = new StreamWriter("ItemData.csv", false);
			for(int i=start; i<=end; i++)
			{
				string FileName = Path.Combine(path, string.Format("{0}.tiff", i));
				if(!File.Exists(FileName))
				{
					if(ItemDataCSV_NEW!=null)
					{
						writer.WriteLine(ItemDataCSV_NEW[i]);
						writer.Flush();
					}
					continue;
				}
				if(ItemDataCSV_OLD!=null)
				{
					writer.WriteLine(ItemDataCSV_OLD[i]);
					writer.Flush();
				}
				Bitmap bmp = new Bitmap(FileName);
				//bmp = Utils.ConvertBmp(bmp);
				Art.ReplaceStatic(i, bmp);
				FiddlerControls.Events.FireItemChangeEvent(this, i);
				Options.ChangedUltimaClass["Art"] = true;
			}
			writer.Close();
		}

		private void onClickExportLand(object sender, EventArgs e)
		{
			int start=-1, end=-1;
			if(!int.TryParse(text3.Text, out start) || !int.TryParse(text4.Text, out end))
				return;
			string path = FiddlerControls.Options.OutputPath;
			if(!Directory.Exists("MassExportLand"))
				Directory.CreateDirectory("MassExportLand");
			path = Path.Combine(path, "MassExportLand");
			if(!Directory.Exists("MassExportLand/Textures"))
				Directory.CreateDirectory("MassExportLand/Textures");
			for(int i=start; i<=end; i++)
			{
				string FileName = Path.Combine(path, String.Format("Textures/{0}.tiff", i));
				Bitmap bitt = Textures.GetTexture(i);
            	if (bitt != null)
            	{
                	bitt.Save(FileName, ImageFormat.Tiff);
            		bitt.Dispose();
            	}
				if (!Art.IsValidLand(i))
					continue;
				FileName = Path.Combine(path, String.Format("{0}.tiff", i));
				Bitmap bit = new Bitmap(Ultima.Art.GetLand(i));
				if (bit != null)
					bit.Save(FileName, ImageFormat.Tiff);
				bit.Dispose();
			}
		}

		private void onClickImportLand(object sender, EventArgs e)
		{
			if(!Directory.Exists("MassImportLand"))
			{
				MessageBox.Show(string.Format("Devi creare la directory -> MassImportLand <- in {0}", FiddlerControls.Options.OutputPath), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
				return;
			}
			string path = FiddlerControls.Options.OutputPath;
			path = Path.Combine(path, "MassImportLand");
			Dictionary<int, string> LandDataCSV_OLD=null;
			Dictionary<int, string> LandDataCSV_NEW=null;
			if(File.Exists("LandDataOLD.csv") && File.Exists("LandDataNEW.csv"))
			{
				LandDataCSV_OLD = new Dictionary<int, string>();
				LandDataCSV_NEW = new Dictionary<int, string>();
				string line;
				using(StreamReader reader = new StreamReader("LandDataOLD.csv"))
				{
					while((line=reader.ReadLine())!=null)
					{
						int i;
						string[] splitted = line.Split(';');
						if(int.TryParse(splitted[0].Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out i))
							LandDataCSV_OLD[i]=line;
					}
				}
				using(StreamReader reader = new StreamReader("LandDataNEW.csv"))
				{
					while((line=reader.ReadLine())!=null)
					{
						int i;
						string[] splitted = line.Split(';');
						if(int.TryParse(splitted[0].Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out i))
							LandDataCSV_NEW[i]=line;
					}
				}
			}
			int start=-1, end=-1;
			if(!int.TryParse(text3i.Text, out start) || !int.TryParse(text4i.Text, out end))
				return;

			StreamWriter writer = new StreamWriter("LandData.csv", false);
			for(int i=start; i<=end; i++)
			{
				string FileName = Path.Combine(path, string.Format("Textures/{0}.tiff", i));
				if(File.Exists(FileName))
				{
					Bitmap tex = new Bitmap(FileName);
					Textures.Replace(i, tex);
					FiddlerControls.Events.FireTextureChangeEvent(this, i);
                    Options.ChangedUltimaClass["Texture"] = true;
				}
				FileName = Path.Combine(path, string.Format("{0}.tiff", i));
				if(!File.Exists(FileName))
				{
					if(LandDataCSV_NEW!=null)
					{
						writer.WriteLine(LandDataCSV_NEW[i]);
						writer.Flush();
					}
					continue;
				}
				if(LandDataCSV_OLD!=null)
				{
					writer.WriteLine(LandDataCSV_OLD[i]);
					writer.Flush();
				}
				Bitmap bmp = new Bitmap(FileName);
				//bmp = Utils.ConvertBmp(bmp);
				Art.ReplaceLand(i, bmp);
				FiddlerControls.Events.FireLandTileChangeEvent(this, i);
				Options.ChangedUltimaClass["Land"] = true;
			}
			writer.Close();
		}
	}
}
