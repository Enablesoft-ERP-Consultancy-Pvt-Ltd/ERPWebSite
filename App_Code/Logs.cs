using System;
using System.IO;
using System.Configuration;
/// <summary>
/// Created By: Pawan Kumar Saini
/// Created On: 01-MAR-2011
/// Function:   Write Logs
/// </summary>
public class Logs
{
		
	public static void WriteErrorLog(string message)
	{
		try
		{
			string strFileName;
			string strLine;			
			string strLogPath;	
			StreamWriter fw;
		
			strLine=DateTime.Now.ToString("dd-MMM-yyyy")+"|"+message;  
			strLogPath=ConfigurationManager.AppSettings["ErrorPath"].ToString();
            strFileName = strLogPath + @"\" + DateTime.Now.ToString("dd-MM-yyyy") + "_erpErr.log";	
	
			if(!File.Exists(strFileName))
				fw = File.CreateText(strFileName);
			else
				fw = File.AppendText(strFileName);

			fw.Close();
			fw=new StreamWriter(strFileName,true);
			fw.WriteLine(strLine); 
			fw.Close();
		}
		catch
		{}
	}

	public static void WriteLog(string message)
	{		
		StreamWriter fw;
		try
		{
			string strFileName;
			string strLine;			
			string strLogPath;			
		
			strLine=DateTime.Now.ToString("dd-MMM-yyyy")+"|"+message;  
			strLogPath=ConfigurationManager.AppSettings["LogPath"].ToString();
            strFileName = strLogPath + @"\" + DateTime.Now.ToString("dd-MM-yyyy") + "_erp.log";

			if(!File.Exists(strFileName))
				fw = File.CreateText(strFileName);
			else
				fw = File.AppendText(strFileName);

			fw.Close();
			fw=new StreamWriter(strFileName,true);
			fw.WriteLine(strLine); 
			fw.Close();				
		}
		catch
		{
		}
	}
}
