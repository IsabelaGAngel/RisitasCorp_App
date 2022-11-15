using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine.UI;

public class GameManagerRevisionMain : MonoBehaviour
{
    
    private static GameManagerRevisionMain instance;
    //private Thread receiveThread;
    private UdpClient _dataReceiveCiclista;
    private UdpClient _dataReceiveSabotaje;
    private IPEndPoint _receiveEndPointDataCiclista;
    private IPEndPoint _receiveEndPointDataSabotaje;
    public string _ipDataCiclista = "192.168.100.13"; //IP PC
    private string _ipDataSabotaje = "127.0.0.1";//IP Cell2
    private int _receivePortData = 44444;
    private int _sendPortData = 3100;

    private bool isInitialized;
    private Queue receiveQueue;

    private byte _dataReceived;



    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        instance = this;
        _receiveEndPointDataCiclista = new IPEndPoint(IPAddress.Parse(_ipDataCiclista), _sendPortData);
        _receiveEndPointDataSabotaje = new IPEndPoint(IPAddress.Parse(_ipDataSabotaje), _sendPortData);
        _dataReceiveSabotaje = new UdpClient(_receivePortData);
        /*receiveQueue = Queue.Synchronized(new Queue());


         receiveThread = new Thread(new ThreadStart(ReceiveDataListener));
         receiveThread.IsBackground = true;
         receiveThread.Start();*/
        isInitialized = true;
    }

    /*    private void ReceiveDataListener()
    {
        while (true)
        {
            try
            {
                byte[] dataPulse = _dataReceiveClient.Receive(ref _receiveEndPointData);
                receiveQueue.Enqueue(dataPulse);
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }
    }*/
    private void OnDestroy()
    {
        TryKillThread();
    }
    private void OnApplicationQuit()
    {
        TryKillThread();
    }
    private void TryKillThread()
    {
        if (isInitialized)
        {
            /*receiveThread.Abort();
            receiveThread = null;*/

            _dataReceiveSabotaje.Close();
            _dataReceiveSabotaje = null;

            Debug.Log("Thread killed");
            isInitialized = false;
        }
    }

    private void sendStringDataCiclista (byte message)
    {
        try
        {
            byte data = message;
            byte[] dataBytes = new byte[1];
            dataBytes[0] = data;
            _dataReceiveCiclista.Send(dataBytes, 1, _receiveEndPointDataCiclista);
        }
        catch (System.Exception err)
        {
            print(err.ToString());
        }
    }
    private void sendStringDataSabotaje (byte message)
    {
        try
        {
            byte data = message;
            byte[] dataBytes = new byte[1];
            dataBytes[0] = data;
            _dataReceiveSabotaje.Send(dataBytes, 1, _receiveEndPointDataSabotaje);
        }
        catch (System.Exception err)
        {
            print(err.ToString());
        }
    }

    public void ButtonSi()
    {
        sendStringDataSabotaje(0x10);
        TryKillThread();
        SceneManager.LoadScene("ActividadesMaster");
        
    }
    public void ButtonNo()
    {
        sendStringDataCiclista(0x1A);
        sendStringDataSabotaje(0x11);
        TryKillThread();
        SceneManager.LoadScene("ActividadesMaster");
        
    }

    // Update is called once per frame
    void Update()
    {


    }
}
