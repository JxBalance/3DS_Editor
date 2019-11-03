using UnityEngine;
using System;
using System.Diagnostics;
/// <summary>
/// 2017.1.3 By WS
/// 调用/关闭外部进程
/// </summary>
public class CallApplication : MonoBehaviour
{
    private static CallApplication instance;
    private CallApplication() { }

    public static CallApplication GetInstance()
    {
        if (instance == null)
            instance = new CallApplication();
        return instance;
    }

    /// <summary>
    /// 开启应用
    /// </summary>
    /// <param name="ApplicationPath"></param>
    public static void StartProcess(string ApplicationPath)
    {
        if (CheckProcess(ApplicationPath))
        {
            return;
        }
        else 
        {
            UnityEngine.Debug.Log("打开本地应用");
            Process foo = new Process();
            foo.StartInfo.FileName = ApplicationPath;
            foo.Start();
        }
    }

    /// <summary>
    /// 杀死进程
    /// </summary>
    /// <param name="processName">应用程序名</param>
    public static void KillProcess(string processName)
    {
        Process[] processes = Process.GetProcesses();
        foreach (Process process in processes)
        {
            try
            {
                if (!process.HasExited)
                {
                    if (process.ProcessName == processName)
                    {
                        //process.StandardInput.WriteLine("Exit");
                        process.Kill();
                        process.Close();
                        process.Dispose();
                        UnityEngine.Debug.Log("已杀死进程");
                    }
                }
            }
            catch (System.InvalidOperationException)
            {
                //UnityEngine.Debug.Log("Holy batman we've got an exception!");
            }
        }
    }

    /// <summary>
    /// 检查应用是否正在运行
    /// </summary>
    public static bool CheckProcess(string processName)
    {
        bool isRunning = false;
        Process[] processes = Process.GetProcesses();
        int i = 0;
        foreach (Process process in processes)
        {
            try
            {
                i++;
                if (!process.HasExited)
                {
                    if (process.ProcessName.Contains(processName))
                    {
                        UnityEngine.Debug.Log(processName + "正在运行");
                        isRunning = true;
                        continue;
                    }
                    else if (!process.ProcessName.Contains(processName) && i > processes.Length)
                    {
                        UnityEngine.Debug.Log(processName + "没有运行");
                        isRunning = false;
                    }
                }
            }
            catch (Exception ep)
            {
                UnityEngine.Debug.Log(ep);
            }
        }
        return isRunning;
    }

    /// <summary>
    /// 列出已开启的应用
    /// </summary>
    public static void ListAllAppliction()
    {
        Process[] processes = Process.GetProcesses();
        //int i = 0;
        foreach (Process process in processes)
        {
            try
            {
                if (!process.HasExited)
                {
                    UnityEngine.Debug.Log("应用ID:" + process.Id + "应用名:" + process.ProcessName);
                }
            }
            catch (Exception ep)
            {
                UnityEngine.Debug.Log(ep);
            }
        }
    }

}