using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

namespace DiskSpaceUNCPath
{
    class Program
    {
        static void Main(string[] args)
        {


            try
            {
                var searcher = new ManagementObjectSearcher(
                    "root\\CIMV2",
                    "SELECT * FROM Win32_MappedLogicalDisk");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine("Win32_MappedLogicalDisk instance");
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine("Access: {0}", queryObj["Access"]);
                    Console.WriteLine("Availability: {0}", queryObj["Availability"]);
                    Console.WriteLine("BlockSize: {0}", queryObj["BlockSize"]);
                    Console.WriteLine("Caption: {0}", queryObj["Caption"]);
                    Console.WriteLine("Compressed: {0}", queryObj["Compressed"]);
                    Console.WriteLine("ConfigManagerErrorCode: {0}", queryObj["ConfigManagerErrorCode"]);
                    Console.WriteLine("ConfigManagerUserConfig: {0}", queryObj["ConfigManagerUserConfig"]);
                    Console.WriteLine("CreationClassName: {0}", queryObj["CreationClassName"]);
                    Console.WriteLine("Description: {0}", queryObj["Description"]);
                    Console.WriteLine("DeviceID: {0}", queryObj["DeviceID"]);
                    Console.WriteLine("ErrorCleared: {0}", queryObj["ErrorCleared"]);
                    Console.WriteLine("ErrorDescription: {0}", queryObj["ErrorDescription"]);
                    Console.WriteLine("ErrorMethodology: {0}", queryObj["ErrorMethodology"]);
                    Console.WriteLine("FileSystem: {0}", queryObj["FileSystem"]);
                    Console.WriteLine("FreeSpace: {0}", queryObj["FreeSpace"]);
                    Console.WriteLine("InstallDate: {0}", queryObj["InstallDate"]);
                    Console.WriteLine("LastErrorCode: {0}", queryObj["LastErrorCode"]);
                    Console.WriteLine("MaximumComponentLength: {0}", queryObj["MaximumComponentLength"]);
                    Console.WriteLine("Name: {0}", queryObj["Name"]);
                    Console.WriteLine("NumberOfBlocks: {0}", queryObj["NumberOfBlocks"]);
                    Console.WriteLine("PNPDeviceID: {0}", queryObj["PNPDeviceID"]);

                    if (queryObj["PowerManagementCapabilities"] == null)
                        Console.WriteLine("PowerManagementCapabilities: {0}", queryObj["PowerManagementCapabilities"]);
                    else
                    {
                        UInt16[] arrPowerManagementCapabilities = (UInt16[])(queryObj["PowerManagementCapabilities"]);
                        foreach (UInt16 arrValue in arrPowerManagementCapabilities)
                        {
                            Console.WriteLine("PowerManagementCapabilities: {0}", arrValue);
                        }
                    }
                    Console.WriteLine("PowerManagementSupported: {0}", queryObj["PowerManagementSupported"]);
                    Console.WriteLine("ProviderName: {0}", queryObj["ProviderName"]);
                    Console.WriteLine("Purpose: {0}", queryObj["Purpose"]);
                    Console.WriteLine("QuotasDisabled: {0}", queryObj["QuotasDisabled"]);
                    Console.WriteLine("QuotasIncomplete: {0}", queryObj["QuotasIncomplete"]);
                    Console.WriteLine("QuotasRebuilding: {0}", queryObj["QuotasRebuilding"]);
                    Console.WriteLine("SessionID: {0}", queryObj["SessionID"]);
                    Console.WriteLine("Size: {0}", queryObj["Size"]);
                    Console.WriteLine("Status: {0}", queryObj["Status"]);
                    Console.WriteLine("StatusInfo: {0}", queryObj["StatusInfo"]);
                    Console.WriteLine("SupportsDiskQuotas: {0}", queryObj["SupportsDiskQuotas"]);
                    Console.WriteLine("SupportsFileBasedCompression: {0}", queryObj["SupportsFileBasedCompression"]);
                    Console.WriteLine("SystemCreationClassName: {0}", queryObj["SystemCreationClassName"]);
                    Console.WriteLine("SystemName: {0}", queryObj["SystemName"]);
                    Console.WriteLine("VolumeName: {0}", queryObj["VolumeName"]);
                    Console.WriteLine("VolumeSerialNumber: {0}", queryObj["VolumeSerialNumber"]);
                }
            }
            catch (ManagementException ex)
            {
                Console.WriteLine("An error occurred while querying for WMI data: " + ex.Message);
            }











            //GetFreeSpace(@"\\\\Ha-splinter\\Transportation"); // UNC path, Note the double quotes!!!

            //System.IO.DriveInfo di = new System.IO.DriveInfo("Z:");

            //Console.WriteLine(di.AvailableFreeSpace);

    //        try
    //        {
    //    // Connection credentials to the remote computer, not needed if the logged account has access
    //    ConnectionOptions oConn = new ConnectionOptions();

    //    oConn.Username = "svc-art2";
    //    oConn.Password = "Webserver2";
    //    string strNameSpace = @"\\";
    //    string srvname = "Ha-splinter";

    //    if (srvname != "")
    //        strNameSpace += srvname;
    //    else
    //        strNameSpace += ".";

    //    strNameSpace += @"\root\cimv2";

    //    ManagementScope oMs = new ManagementScope(strNameSpace, oConn);

    //    //get Fixed disk state

    //    ObjectQuery oQuery = new ObjectQuery("select FreeSpace,Size,Name from Win32_LogicalDisk where DriveType=3");

    //    //Execute the query
    //    ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oMs, oQuery);

    //    //Get the results
    //    ManagementObjectCollection oReturnCollection = oSearcher.Get();

    //    //loop through found drives and write out info
    //    double D_Freespace = 0;
    //    double D_Totalspace = 0;
    //    foreach (ManagementObject oReturn in oReturnCollection)
    //    {
    //        // Disk name
    //        //MessageBox.Show("Name : " + oReturn["Name"].ToString());
    //        Console.WriteLine("Name : " + oReturn["Name"].ToString());
    //        // Free Space in bytes
    //        string strFreespace = oReturn["FreeSpace"].ToString();
    //        D_Freespace = D_Freespace + System.Convert.ToDouble(strFreespace);
    //        // Size in bytes
    //        string strTotalspace = oReturn["Size"].ToString();
    //        D_Totalspace = D_Totalspace + System.Convert.ToDouble(strTotalspace);
    //    }
    //}


    //catch(Exception ex)
    //{
    //   Console.WriteLine(ex.ToString() + " - Failed to obtain Server Information. The node you are trying to scan can be a Filer or a node which you don't have administrative priviledges. Please use the UNC convention to scan the shared folder in the server", "Server Error");
    //}

        }

        public static void GetFreeSpace(string ProviderName)
        {
            try
            {

                String strSQL = "SELECT FreeSpace, QuotasDisabled ,VolumeName FROM Win32_LogicalDisk WHERE providername='" + ProviderName + "'";
                SelectQuery query = new SelectQuery(strSQL);
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
                foreach (ManagementObject mo in searcher.Get())
                {
                    // Free disk space is irrelevant if per user quota's are enabled!
                    if (mo["QuotasDisabled"].ToString() != "true")
                    {
                        Console.WriteLine("{0} - Free bytes: {1} ", mo["VolumeName"], mo["Freespace"]);
                        Console.Read();
                    }

                    else
                    {
                        Console.WriteLine("{0} - Free bytes: {1} per-user quota's applied!!", mo["VolumeName"], mo["Freespace"]);
                        Console.Read();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.Read();
            }
          
        }

    }
}
