using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArtiConnect.Hugin
{
    /// <summary>
    /// 事件类型
    /// </summary>
    public enum EventType
    {
        //添加
        ADD,
        //移除
        REMOVE
    }
    /// <summary>
    /// 变化的串口的名称和串口是连接上还是关闭
    /// </summary>
    public class DeviceNotifyEvent : EventArgs
    {


        /// <summary>
        /// 变化的串口的名称和串口是连接上还是关闭
        /// </summary>
        /// <param name="eventType"> 串口变更方式 </param>
        /// <param name="PortName">  串口号       </param>
        public DeviceNotifyEvent(EventType eventType, String PortName)
        {
            this.eventType = eventType;
            this.PortName = PortName;
        }
        /// <summary>
        /// 事件类型
        /// </summary>
        public EventType eventType { private set; get; }
        /// <summary>
        /// 串口名
        /// </summary>
        public String PortName { private set; get; }
    }
    /// <summary>
    /// 监听所有串口设备
    /// </summary>
    public partial class SerialNotify : IDisposable
    {
        public event EventHandler<DeviceNotifyEvent> OnNotify;
        private List<string> filters;
        private List<string> portlist;
        public Boolean m_Plugin = false;
        /// <summary>
        /// 已检测到的设备信息
        /// </summary>
        public static string m_PortName = null;

        /// <summary>
        /// SerialNotify构造函数
        /// </summary>
        public SerialNotify()
        {
            filters = new List<string>();
            filters.Add("VID_0730&PID_DCBA");
            filters.Add("VID_1E0E&PID_902B&MI_00");
            this.portlist = new List<string>();
            //AddUSBEventWatcher(new TimeSpan(0, 0, 0, 1));
            m_Plugin = false;

        }
        #region 外部方法
        /// <summary>
        /// 获得对应设备串口数组
        /// </summary>
        /// <returns></returns>
        public void GetPorts()
        {
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                //Thread.Sleep(1000);
                Thread.Sleep(20);
                string pattern = @"(COM\d{1,3})";
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("select * from Win32_PnPEntity WHERE ");
                    for (int i = 0; i < filters.Count; i++)
                    {
                        if (i == 0)
                            sql.Append(" PNPDeviceID LIKE '%" + filters[i] + "%' ");
                        else
                            sql.Append(" OR PNPDeviceID LIKE '%" + filters[i] + "%' ");
                    }

                    using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(sql.ToString()))
                    {
                        var hardInfos = searcher.Get();
                        foreach (var hardInfo in hardInfos)
                        {
                            string DeviceID = null;
                            string portname = null;
                            DeviceID = hardInfo["DeviceID"] as string;
                            // log.Debug("DeviceID:" + DeviceID+ "   GetPorts");

                            if (DeviceID.IndexOf("VID_0730&PID_DCBA") > 0 || DeviceID.IndexOf("VID_1E0E&PID_902B&MI_00") > 0)
                            {
                                for (int j = 0; j < 10; j++)
                                {
                                    Thread.Sleep(20);
                                    portname = Regex.Match(hardInfo.Properties["NAME"].Value.ToString(), pattern, RegexOptions.IgnoreCase).Value;
                                    //log.Debug(" Getportname:" + portname);
                                    if (portname != null)
                                    {

                                        OnDeviceNotify(portname);
                                    }
                                    else
                                        continue;

                                }

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // log.Warn(ex.Message);
                }

            });
        }
        /// <summary>
        /// 用于监测USB设备状态改变
        /// </summary>
        public void DeviceChange(Form form, Message m)
        {
            try
            {
                //WM_DEVICECHANGE，系统硬件改变发出的系统消息
                if (m.Msg == OnDeviceChange.WM_DEVICECHANGE)
                {
                    //Console.WriteLine(m.WParam.ToInt32());
                    switch (m.WParam.ToInt32())
                    {

                        //DBT_DEVICEARRIVAL，设备检测结束，并且可以使用
                        case OnDeviceChange.DBT_DEVICEARRIVAL:
                            {
                                List<string> portlist2;
                                portlist2 = new List<string>();
                                portlist2.Clear();

                                SerialPort comPort = new SerialPort();
                                foreach (string str in SerialPort.GetPortNames())
                                    portlist2.Add(str);

                                //   string portName = null; 
                                //    portName = GetDevice.GetPortNameFormVidPid(filters);
                                ////   log.Debug("DBT_DEVICEARRIVAL：" + "设备:" + portName );
                                //if (portName != null && !portlist.Contains(portName))
                                if (m_PortName != null && portlist2.Contains(m_PortName) && m_Plugin == false)
                                {
                                    //portlist.Add(portName);
                                    //m_PortName = null;
                                    //m_PortName = portName;
                                    // log.Debug("DBT_DEVICEARRIVAL：  " + "监测到设备_Insert:" + portName );
                                    //OnNotify(this, new DeviceNotifyEvent(EventType.ADD, portName));
                                    OnNotify(this, new DeviceNotifyEvent(EventType.ADD, m_PortName));
                                    m_Plugin = true;
                                }

                                break;
                            }
                        //DBT_DEVICEREMOVECOMPLETE,设备卸载或者拔出
                        case OnDeviceChange.DBT_DEVICEREMOVECOMPLETE:
                            //if (m_PortName != null)
                            {
                                string PortName = "";
                                PortName = null;
                                PortName = GetDevice.GetPortNameFormVidPid(filters);
                                // log.Debug("DBT_DEVICEREMOVECOMPLETE：  " + "设备:" + PortName);
                                if (PortName == null)
                                {
                                    //portlist.Remove(m_PortName);
                                    //  log.Debug("DBT_DEVICEREMOVECOMPLETE：  " + "监测到设备_Remove:" + m_PortName );
                                    OnNotify(this, new DeviceNotifyEvent(EventType.REMOVE, m_PortName));
                                    m_Plugin = false;
                                    //m_PortName = null;
                                }


                            }
                            break;
                        case OnDeviceChange.DBT_DEVNODES_CHANGED:
                            /*  {
                                  if (m_PortName!=null)
                                  {
                                    //  log.Debug("DBT_DEVNODES_CHANGED： " + "m_PortName:" + m_PortName);
                                  }
                                  else
                                    //  log.Debug("DBT_DEVNODES_CHANGED： " + "m_PortName: null");
                                  break;
                              }*/
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                // log.Debug("portName:" + m_PortName + "OnDeviceChange 有异常");
                //MessageBox.Show("The current device cannot be identified correctly. Please unplug it！", "Actation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion
        private void OnDeviceNotify(string DeviceName)
        {
            if (DeviceName != null && !portlist.Contains(DeviceName))
            {
                portlist.Add(DeviceName);
                {
                    try
                    {
                        m_PortName = null;
                        m_PortName = DeviceName;
                        // ThreadPool.QueueUserWorkItem((obj) => {
                        OnNotify(this, new DeviceNotifyEvent(EventType.ADD, DeviceName));

                        // });

                    }
                    catch (Exception ex)
                    {
                        // log.Error(ex.Message);
                    }
                }
            }
            else
                return;
        }

        #region 资源释放模块
        /// <summary>
        /// 是否已经被释放过,默认是false
        /// </summary>
        public bool m_disposed;

        /// <summary>
        /// 析构函数
        /// </summary>
        ~SerialNotify()
        {
            Dispose(false);
        }

        /// <summary>
        /// 实现IDisposable接口
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            //.NET Framework 类库
            // GC..::.SuppressFinalize 方法
            //请求系统不要调用指定对象的终结器。
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 虚方法，可供子类重写
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (!m_disposed)
                {
                    // 释放托管对象
                    if (disposing)
                    {

                    }
                    // RemoveUSBEventWatcher();
                    m_disposed = true;
                }
            }
            catch (Exception ex)
            {
                //logger.Warn(ex.Message);
            }
        }
        #endregion
    }
}
