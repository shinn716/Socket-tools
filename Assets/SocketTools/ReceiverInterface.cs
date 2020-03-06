// Data Receiver Sample - UDP Socket
// Author : Shinn

using Shinn.Common;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ReceiverInterface : MonoBehaviour
{
    #region Declare
    [Serializable]
    public class RawData
    {
        public RawData(int _id)
        {
            id = _id;
        }
        public string mac = string.Empty;
        public string sn = string.Empty;
        public int id = 0;
        public float x, y, z;
        public float xy, xz, yz;
        public float ax, ay, az;
        public float rawyz;
        public Quaternion rawq = Quaternion.identity;
        public Vector3 gyro = Vector3.zero;
        public Vector3 accel = Vector3.zero;
        public int battery;
        public string HW;
        public string FW;
    }
    
    [Header("Raw data")]
    public List<RawData> rawDataList = new List<RawData>();
    public string Result { get; set; }

    SenderInterface sender;
    UDPServer server;
    Thread thread;
    #endregion

    #region Public 
    // Connect 
    public bool SetConnect { get; set; }
    public bool GetConnectStatus { get; set; }

    // Get data mode
    public DataMode GetDataMode { get; set; } = DataMode.Null;

    // Clear list and status
    public void Clear()
    {
        rawDataList.Clear();
    }

    // Set SetRawDefault
    public void SetRawDefault()
    {
        foreach (RawData raw in rawDataList)
        {
            raw.x = 0;
            raw.y = 0;
            raw.z = 0;
            raw.xy = 0;
            raw.xz = 0;
            raw.yz = 0;
            raw.rawyz = 0;
            raw.rawq = Quaternion.identity;
            raw.gyro = Vector3.zero;
            raw.accel = Vector3.zero;
            raw.ax = 0;
            raw.ay = 0;
            raw.az = 0;
        }
    }

    // Dispose thread
    public void Dispose()
    {
        SetConnect = false;
        GetConnectStatus = false;
        Clear();
        if (server != null)
        {
            server.callback -= GetRawData;
            server.Dispose();
            server = null;
        }
        thread.Join();
    }

    #endregion

    #region Main
    public enum DataMode
    {
        G_sensor,
        Quaternion,
        Raw,
        Both,
        Null
    }

    private void Start()
    {
        sender = GetComponent<SenderInterface>();
        server = new UDPServer();
        server.callback += GetRawData;
        //thread = new Thread(Call);
        //thread.IsBackground = true;
        thread = new Thread(new ThreadStart(server.ReceiveData));
        thread.Start();
    }

    private void OnApplicationQuit()
    {
        Dispose();
    }
    #endregion

    #region Private
    private void GetRawData()
    {
        //if (SetConnect)
        //{
        if (!GetConnectStatus)
        {
            GetConnectStatus = true;
        }

        string data = server.CallbackEvent();
        //擷取0,1 byte
        string[] splitType = data.Split(char.Parse(" "));
        var GetType = splitType[0];
        //Debug.Log(GetType);
        Debug.Log("Socket CallbackEvent: " + data);
        
        // 顧問方法 未來修正成此方法
        //byte[] response = { 0x00 };
        //try
        //{
        //    response = StringToBytes(splitType[4].Trim());
        //}
        //catch (Exception) { }

        //if (SetConnect)
        //{
        //    // ver 2
        //    if (GetType.Trim() == "[Q]")
        //    {
        //        string[] splitData = data.Split(char.Parse(","));
        //        var idsp = splitData[0].Trim().Split(char.Parse(" "));
        //        var id = idsp[2];
        //        var mac = splitData[1].Trim();
        //        var sn = splitData[2].Trim();
        //        var qw = splitData[3].Trim();
        //        var qx = splitData[4].Trim();
        //        var qy = splitData[5].Trim();
        //        var qz = splitData[6].Trim();

        //        int idtemp = Convert.ToInt16(id);
        //        if (rawDataList.Count == 0)
        //            rawDataList.Add(new RawData(idtemp));
        //        else
        //        {
        //            int[] tindex = rawDataList.Select(x => x.id).ToArray();
        //            bool exists = tindex.Contains(idtemp);
        //            if (!exists)
        //                rawDataList.Add(new RawData(idtemp));
        //            else
        //            {
        //                for (int i = 0; i < rawDataList.Count; i++)
        //                {
        //                    if (idtemp == rawDataList[i].id)
        //                    {
        //                        rawDataList[i].mac = mac;
        //                        rawDataList[i].sn = sn;
        //                        rawDataList[i].rawq = new Quaternion(float.Parse(qx), float.Parse(qy), float.Parse(qz), float.Parse(qw));
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    if (GetType.Trim() == "[G]")
        //    {
        //        string[] splitData = data.Split(char.Parse(","));
        //        var idsp = splitData[0].Trim().Split(char.Parse(" "));
        //        var id = idsp[2];
        //        var mac = splitData[1].Trim();
        //        var sn = splitData[2].Trim();
        //        var _x = splitData[3].Trim();
        //        var _y = splitData[4].Trim();
        //        var _z = splitData[5].Trim();
        //        var _xy = splitData[6].Trim();
        //        var _xz = splitData[7].Trim();
        //        var _yz = splitData[8].Trim();

        //        int idtemp = Convert.ToInt16(id);
        //        if (rawDataList.Count == 0)
        //            rawDataList.Add(new RawData(idtemp));
        //        else
        //        {
        //            int[] tindex = rawDataList.Select(x => x.id).ToArray();
        //            bool exists = tindex.Contains(idtemp);
        //            if (!exists)
        //                rawDataList.Add(new RawData(idtemp));
        //            else
        //            {
        //                for (int i = 0; i < rawDataList.Count; i++)
        //                {
        //                    if (idtemp == rawDataList[i].id)
        //                    {
        //                        rawDataList[i].mac = mac;
        //                        rawDataList[i].sn = sn;
        //                        rawDataList[i].x = AngleTo360(float.Parse(_x));
        //                        rawDataList[i].y = AngleTo360(float.Parse(_y));
        //                        rawDataList[i].z = AngleTo360(float.Parse(_z));
        //                        rawDataList[i].xy = AngleTo360(float.Parse(_xy));
        //                        rawDataList[i].xz = AngleTo360(float.Parse(_xz));
        //                        rawDataList[i].yz = AngleTo360(float.Parse(_yz));

        //                        rawDataList[i].rawyz = float.Parse(_yz);
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    if (GetType.Trim() == "[R]")
        //    {
        //        GetDataMode = DataMode.Raw;
        //        string[] splitArray = data.Split(char.Parse(","));
        //        var id = splitArray[0].Split(char.Parse(" "))[2].Trim();
        //        var mac = splitArray[1].Trim();
        //        var sn = splitArray[2].Trim();
        //        var ax = splitArray[3].Trim();
        //        var ay = splitArray[4].Trim();
        //        var az = splitArray[5].Trim();
        //        //print("[Q] " + id + " " + mac + " " + sn + " " + qw + " " + qx + " " + qy + " " + qz);

        //        int idtemp = Convert.ToInt16(id);
        //        if (rawDataList.Count == 0)
        //            rawDataList.Add(new RawData(idtemp));
        //        else
        //        {
        //            int[] tindex = rawDataList.Select(x => x.id).ToArray();
        //            bool exists = tindex.Contains(idtemp);
        //            if (!exists)
        //                rawDataList.Add(new RawData(idtemp));
        //            else
        //            {
        //                for (int i = 0; i < rawDataList.Count; i++)
        //                {
        //                    if (idtemp == rawDataList[i].id)
        //                    {
        //                        rawDataList[i].mac = mac;
        //                        rawDataList[i].sn = sn;
        //                        rawDataList[i].ax = float.Parse(ax);
        //                        rawDataList[i].ay = float.Parse(ay);
        //                        rawDataList[i].az = float.Parse(az);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //if (GetType.Trim() == "[D]")
        //{
        //    string status = splitType[4].Substring(0, 4);
        //    // Get Sensor version
        //    //if (status == "1000")
        //    if (response[0] == 0x10 && response[1] == 0x00)
        //    {
        //        string header = ByteToString(response[0]) + ByteToString(response[1]);
        //        //string bitmask = BytesToString(response[2] + response[3] + response[4] + response[5]);
        //        string hw = ByteToString(response[4]);
        //        string fw_5 = ByteToString(response[5]);
        //        string fw_6 = ByteToString(response[6]);
        //        string fw_7 = ByteToString(response[7]);
        //        //string po_10 = ByteToString(response[10]);
        //        //string po_11 = ByteToString(response[11]);
        //        //string po_12 = ByteToString(response[12]);

        //        var splitData = data.Split(char.Parse(","));
        //        var idsp = splitData[0].Trim().Split(char.Parse(" "));
        //        var id = idsp[2];

        //        for (int i = 0; i < rawDataList.Count; i++)
        //        {
        //            if (rawDataList[i].id.Equals(Convert.ToInt16(id)))
        //            {
        //                //print("hwver " + hw);
        //                //print("fwver " + fw_5 + fw_6 + fw_7);

        //                if (hw == "00")
        //                    rawDataList[i].HW = "EVT1";
        //                else if (hw == "01")
        //                    rawDataList[i].HW = "EVT2";
        //                else if (hw == "10")
        //                    rawDataList[i].HW = "DVT1";
        //                else if (hw == "20")
        //                    rawDataList[i].HW = "PVT1";
        //                else if (hw == "30")
        //                    rawDataList[i].HW = "preMP";
        //                else if (hw == "40")
        //                    rawDataList[i].HW = "MP";

        //                rawDataList[i].FW = fw_5.Replace("0", "") + "." + fw_6.Replace("0", "") + "." + fw_7.Replace("0", "");
        //            }
        //        }
        //    }

        //    // Get Connecting Status
        //    if (status == "6000")
        //    {
        //        string[] splitData = data.Split(char.Parse(","));
        //        var idsp = splitData[0].Trim().Split(char.Parse(" "));
        //        var id = idsp[2];
        //        var mac = splitData[1].Trim();
        //        int idtemp = Convert.ToInt16(id);
        //        string enable = splitType[4].Substring(4, 2);
        //        //print("6000 " + enable + " " + id + " " + mac + " " + idtemp);
        //        // Sensor Enable
        //        if (enable == "00")
        //        {
        //            if (rawDataList.Count == 0)
        //            {
        //                rawDataList.Add(new RawData(idtemp));
        //                for (int i = 0; i < rawDataList.Count; i++)
        //                {
        //                    if (idtemp == rawDataList[i].id)
        //                        rawDataList[i].mac = mac;
        //                }
        //            }
        //            else
        //            {
        //                int[] tindex = rawDataList.Select(x => x.id).ToArray();
        //                bool exists = tindex.Contains(idtemp);
        //                if (!exists)
        //                {
        //                    rawDataList.Add(new RawData(idtemp));
        //                    rawDataList[rawDataList.Count - 1].mac = mac;
        //                }
        //            }
        //            sender.GetBatteryInfo();
        //        }

        //        // Sensor Disconnect
        //        if (enable == "01")
        //        {
        //            int[] tindex = rawDataList.Select(x => x.id).ToArray();
        //            int currentSelectionIndex = tindex.ToList().IndexOf(idtemp);
        //            if (currentSelectionIndex >= 0)
        //            {
        //                //print("Remove " + currentSelectionIndex);
        //                rawDataList.RemoveAt(currentSelectionIndex);
        //            }
        //        }
        //    }

        //    if (response[0] == 0x54 && response[1] == 0x00)
        //    {
        //        string header = ByteToString(response[0]) + ByteToString(response[1]);
        //        //string bitmask = BytesToString(response[2] + response[3] + response[4] + response[5]);
        //        string Battery = ByteToString(response[6]);

        //        var splitData = data.Split(char.Parse(","));
        //        var idsp = splitData[0].Trim().Split(char.Parse(" "));
        //        var id = idsp[2];

        //        //print(id + " " + ByteToString(response[0]) + ByteToString(response[1]) + " " + Battery + " " + Convert.ToInt32(Battery, 16));
        //        for (int i = 0; i < rawDataList.Count; i++)
        //        {
        //            if (rawDataList[i].id.Equals(Convert.ToInt16(id)))
        //                rawDataList[i].battery = Convert.ToInt32(Battery, 16);
        //        }
        //    }
        //}
    }

    private void PrintArray(string[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            print(i + " " + arr[i]);
        }
    }

    private float AngleTo360(float raw)
    {
        if (raw < 0)
            raw += 180;
        return raw;
    }

    private int HexToInt(string str)
    {
        string hex = "" + str;
        switch (hex)
        {
            default: return 0;
            case "0": return 0;
            case "1": return 1;
            case "2": return 2;
            case "3": return 3;
            case "4": return 4;
            case "5": return 5;
            case "6": return 6;
            case "7": return 7;
            case "8": return 8;
            case "9": return 9;
            case "A": return 10;
            case "B": return 11;
            case "C": return 12;
            case "D": return 13;
            case "E": return 14;
            case "F": return 15;
        }
    }

    private byte[] StringToBytes(string s)
    {
        byte[] s_bytes = new byte[s.Length / 2];
        for (int i = 0; i < s.Length; i = i + 2)
        {
            //每2位16進位數字轉換為一個10進位整數
            s_bytes[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
        }
        return s_bytes;
    }

    private string ByteToString(byte _byte)
    {
        string result = "";
        result = _byte.ToString("X2");
        return result;
    }

    private string BytesToString(byte[] bytes)
    {
        string result = "";

        foreach (var b in bytes)
            result += b.ToString("X2");

        return result;
    }
    #endregion
}
